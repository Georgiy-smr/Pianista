# Pianista — Project Rules and Conventions

## About the project

Pianista is a free, open-source Avalonia (.NET) desktop app that teaches you to play
a MIDI keyboard: falling notes, wait mode, accompaniment played through the user's
own instrument. The vision, scope and decisions are in [docs/CONCEPT.md](docs/CONCEPT.md) —
read it before proposing features or architecture changes.

**Current state:** solution skeleton only — an empty Avalonia window wired to the
Generic Host; no features yet.

## Communication

- Talk to the maintainer in **Russian**.
- Everything in the repository is in **English**: code, docs, commit messages, issues.
- User-facing UI strings live in localization resources, never hard-coded.

## Architecture

The solution file is `Pianista.slnx` (the XML `.slnx` format, not a classic `.sln`).
`bin/`, `obj/` and `.vs/` are generated — ignore them when exploring the codebase.

```
Pianista/
├── Pianista.slnx
├── global.json                ← pinned .NET SDK, Microsoft.Testing.Platform test runner
├── Directory.Build.props      ← shared build settings (net10.0, Nullable, warnings as errors)
├── Directory.Packages.props   ← all NuGet versions (Central Package Management)
├── .github/workflows/         ← CI, see docs/CI.md
├── src/
│   ├── Pianista.Core/         ← song model, lesson logic, note evaluation, feature commands
│   │                             + handlers, domain exceptions, interfaces for external interaction
│   ├── Pianista.Midi/         ← DryWetMidi implementations: MIDI devices and reading .mid files
│   └── Pianista.Desktop/      ← Avalonia + ReactiveUI: views, view models, rendering,
│                                 DI composition, error presentation, localization
└── tests/
    └── Pianista.Tests/        ← xUnit v3 tests for Core and Midi
```

Rules:

- `Pianista.Core` must not reference Avalonia, DryWetMidi, or anything platform-specific.
- Interfaces for external interaction (MIDI input/output, song files, clock, settings)
  are declared in `Core`; real implementations live in `Midi` or `Desktop`.
- Every package version goes into `Directory.Packages.props`; `PackageReference` items
  never carry a `Version`.
- Game logic never reads time directly (`DateTime.Now`, `Stopwatch`) — time comes from
  an injected clock interface.
- Playback, timing and note evaluation run locally in-process; latency is the top priority.
- Sound comes from the user's keyboard via MIDI Out. No software synthesizer.

## Code style

The style is inspired by Elegant Objects principles, but applied pragmatically
rather than dogmatically — follow the spirit of the principles, not the letter,
where strict adherence would needlessly complicate the code.

**Static methods** are allowed for two purposes only:
- dependency registration (`IServiceCollection` extension methods for DI);
- a `private static` helper that computes an argument for a constructor chain
  (`: this(...)` / `: base(...)`), where an instance method cannot be called yet.

In all other cases, static methods should not be used.

**Service decoration** is done using the Scrutor library.

**Don't restate a default interface member's value.** When an interface member has
a default implementation (`string Foo => "bar";`), only override it in an implementing
type when the value actually needs to differ from the default.

**No comments in code** — this rule is strict, not pragmatic. Never add comments
(including XML doc comments and `TODO`s) to new or changed code; express intent
through names and structure instead. Do not propose, ask about, or mention comments
in plans, task descriptions or reports.

General:

- .NET 10, C# latest, `Nullable` and `ImplicitUsings` enabled, file-scoped namespaces.
- Logging via `ILogger<T>`.

## Testing conventions

Each test method (`[Fact]`/`[Theory]`) should contain exactly **one** `Assert.*` call.
If a test needs to verify more than one condition, do not stack multiple `Assert.*`
calls — write a small custom assertion (a private static helper, e.g.
`AssertNoteJudgedAs(...)`, or a shared helper if reused across test classes) that
performs all the checks and reports a single, well-described failure.

Logic in `Core` comes with unit tests. Timing logic is tested with the fake clock and
fake MIDI input, never with real devices.

## Fake implementations inside interfaces

Whenever you declare an interface that represents a side effect or external
interaction (a MIDI device, the clock, the file system, etc.), add a nested `Fake`
class — a simple stub implementation. Its methods record that they were called via
`System.Diagnostics.Trace.WriteLine` (do not inject `ILogger` — fakes stay lightweight
and dependency-free). This lets the interface be used in tests and as a null-object
without mocks, while keeping call visibility in debug output.

```csharp
public interface IMidiOutput
{
    void Send(NotePlayed note);

    public class Fake : IMidiOutput
    {
        public void Send(NotePlayed note)
        {
            Trace.WriteLine($"{nameof(Fake)}.{nameof(Send)} called with {note}");
        }
    }
}
```

## Code organization: by topic

Inside each project, folders are organized by topic, not by technical layer. A topic
folder holds everything that belongs to it: models, commands and handlers, interfaces
with their `Fake`s, exceptions, route stages.

```
Pianista.Core/
├── Songs/        ← Song model, ISongFile, LoadSong + handler, InvalidSongFileException
├── Lessons/      ← practice modes, hand selection, tempo, A–B loop
├── Evaluation/   ← judging played notes against expected ones
├── Playback/     ← scheduling the accompaniment
├── Devices/      ← IMidiInput, IMidiOutput, device selection
├── Time/         ← IClock
└── Routes/       ← IRoute<T>
```

`Pianista.Midi`, `Pianista.Desktop` and `Pianista.Tests` mirror the Core topics where it
makes sense (`Pianista.Midi/Songs/DryWetSongFile.cs`, `Pianista.Tests/Songs/...`).
The tree above is the intended shape; create a folder when its first type appears.

## Feature pattern: Mediator

Every feature is implemented as a "Command + Handler" pair via
[Mediator](https://github.com/martinothamar/Mediator) (source-generated, MIT).
Do not use MediatR — its current versions are commercially licensed.

- a `sealed record` implementing `IRequest<TResult>` describes the feature's input data;
- a `sealed class` implementing `IRequestHandler<TCommand, TResult>` contains the
  handling logic and returns the result directly.

Illustrative example (these types do not exist yet):

```csharp
public sealed record LoadSong(string FilePath) : IRequest<Song>;

public sealed class ParseAndOpenSong(
    ISongFile file,
    IRoute<Song> route,
    ILogger<ParseAndOpenSong> logger) : IRequestHandler<LoadSong, Song>
{
    public async ValueTask<Song> Handle(LoadSong request, CancellationToken cancellationToken)
    {
        Song song = route.Passed(await file.Parsed(request.FilePath, cancellationToken));
        logger.LogInformation("Song loaded: {Song}", song);
        return song;
    }
}
```

`IRoute<T>` is a generic interface designed for decoration. Each route stage is a
Scrutor decorator that first passes the value through the inner route and then does
its own work on the result, so the registration order in DI is the order in which the
stages run. For example, the song route can get separate stages for track-to-hand
assignment, tempo scaling or making the song current, without modifying the handler.

**The hot path bypasses Mediator.** Commands are for user intent: load a song, change
the practice mode, change the tempo, select a device. The real-time path — MIDI note
received → note evaluated → key highlighted, and scheduled playback — calls its
services directly, without commands or handlers, and avoids allocations per note.

## Error handling

Expected failures are reported with exceptions and presented to the user in `Desktop`.

- Every domain exception derives from the abstract `PianistaException` in `Core`.
  One type per failure the user can act on (`InvalidSongFileException`,
  `MidiDeviceUnavailableException`); it carries the relevant data as properties and an
  English message for logs.
- `Midi` implementations translate DryWetMidi exceptions into domain exceptions, so
  nothing above them depends on third-party exception types.
- Handlers do not catch domain exceptions — they let them propagate.
- `Desktop` handles them in one place: view-model commands are `ReactiveCommand`s, and
  their `ThrownExceptions` are routed to a single error presenter. It looks up the
  localized message in `Strings.resx` under a key equal to the exception type name
  (`InvalidSongFileException`). Any other exception is logged as critical and shown
  with a generic message.
- Exceptions are never used for normal game flow: a wrong note is a judgement result,
  not an exception.

## Localization

User-facing strings live in `Pianista.Desktop` resources (`Strings.resx`, plus
`Strings.<culture>.resx` per language). `Core` and `Midi` contain no user-facing text.

## Commands

```bash
dotnet build
dotnet test
dotnet run --project src/Pianista.Desktop
```

Tests run on Microsoft.Testing.Platform (configured in `global.json`); xUnit v3 test
projects are executables.

## CI

GitHub Actions builds and tests every push and pull request to `main` on Windows —
see [docs/CI.md](docs/CI.md). Warnings are errors, so a change is done only when
`dotnet build` reports zero warnings and `dotnet test` passes.

## Git and dependencies

- Commit messages: Conventional Commits (`feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`).
- Base stack: Avalonia, ReactiveUI.Avalonia, Melanchall.DryWetMidi, Mediator (martinothamar),
  Scrutor, Microsoft.Extensions.* (DI, Hosting, Logging), xUnit v3.
  Ask before adding any other NuGet dependency.

## Hardware

Claude cannot play the keyboard. Anything that needs a real device (latency, device
detection, how the keyboard sounds) is verified by the maintainer on a **Casio CT-X700**
(61 keys, C2–C7, MIDI notes 36–96). When a change needs hardware testing, say so and
describe what to check.
