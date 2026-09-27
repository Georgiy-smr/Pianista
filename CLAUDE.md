# Pianista — Project Rules and Conventions

## About the project

Pianista is a free, open-source Avalonia (.NET) desktop app that teaches you to play
a MIDI keyboard: falling notes, wait mode, accompaniment played through the user's
own instrument. The vision, scope and decisions are in [docs/CONCEPT.md](docs/CONCEPT.md) —
read it before proposing features or architecture changes.

**Current state:** documentation only; no solution or code yet.

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
├── src/
│   ├── Pianista.Core/      ← song model, MIDI-file parsing, lesson logic, note evaluation,
│   │                          feature commands + handlers, interfaces for external interaction
│   ├── Pianista.Midi/      ← MIDI device implementations of Core interfaces (DryWetMidi)
│   └── Pianista.Desktop/   ← Avalonia app: views, view models, rendering, DI composition
└── tests/
    └── Pianista.Tests/     ← xUnit tests
```

Rules:

- `Pianista.Core` must not reference Avalonia, DryWetMidi device APIs, or anything
  platform-specific.
- Interfaces for external interaction (MIDI input/output, clock, file system, settings)
  are declared in `Core`; real implementations live in `Midi` or `Desktop`.
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

## Feature pattern: Mediator

Every feature is implemented as a "Command + Handler" pair via
[Mediator](https://github.com/martinothamar/Mediator) (source-generated, MIT).
Do not use MediatR — its current versions are commercially licensed.

- a `record` inheriting from a base command type (`BaseCommandWithStatus`) describes
  the feature's input data;
- a `sealed class` implementing `IRequestHandler<TCommand, TResult>` contains the
  handling logic.

Illustrative example (these types do not exist yet):

```csharp
public record LoadSong(string FilePath) : BaseCommandWithStatus;

public sealed class ParseAndOpenSong(
    ISongFile file,
    IRoute<Song> route,
    ILogger<ParseAndOpenSong> logger) : IRequestHandler<LoadSong, IStatusGeneric>
{
    public async ValueTask<IStatusGeneric> Handle(LoadSong request, CancellationToken cancellationToken)
    {
        StatusGenericHandler handler = new StatusGenericHandler(request.ToString());
        try
        {
            Song song = route.Passed(await file.Parsed(request.FilePath, cancellationToken));
            logger.LogInformation("Song loaded: {song}", song);
        }
        catch (InvalidSongFileException e)
        {
            logger.LogWarning(e.Message);
            handler.AddError(Strings.SongInvalidFile);
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
            throw;
        }
        return handler;
    }
}
```

`IRoute<T>` is a generic interface designed for decoration. Each route stage is a
Scrutor decorator that first passes the value through the inner route and then does
its own work on the result, so the registration order in DI is the order in which the
stages run. For example, the song route can get separate stages for track-to-hand
assignment, tempo scaling or making the song current, without modifying the handler.

When writing new features, follow this pattern: command + handler, error handling via
`IStatusGeneric`/`StatusGenericHandler`, logging via `ILogger`, and generic interfaces
wherever decorable behavior is needed.

**The hot path bypasses Mediator.** Commands are for user intent: load a song, change
the practice mode, change the tempo, select a device. The real-time path — MIDI note
received → note evaluated → key highlighted, and scheduled playback — calls its
services directly, without commands or handlers, and avoids allocations per note.

## Commands

```bash
dotnet build
dotnet test
dotnet run --project src/Pianista.Desktop
```

## Git and dependencies

- Commit messages: Conventional Commits (`feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`).
- Base stack: Avalonia, Melanchall.DryWetMidi, Mediator (martinothamar), Scrutor,
  GenericServices.StatusGeneric, Microsoft.Extensions.* (DI, Hosting, Logging), xUnit.
  Ask before adding any other NuGet dependency.

## Hardware

Claude cannot play the keyboard. Anything that needs a real device (latency, device
detection, how the keyboard sounds) is verified by the maintainer on a **Casio CT-X700**
(61 keys, C2–C7, MIDI notes 36–96). When a change needs hardware testing, say so and
describe what to check.
