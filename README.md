# Pianista

[![Build](https://github.com/Georgiy-smr/Pianista/actions/workflows/build.yml/badge.svg)](https://github.com/Georgiy-smr/Pianista/actions/workflows/build.yml)

**A free, open-source app that teaches you to play a MIDI keyboard.**

Notes scroll toward an on-screen keyboard, the accompaniment plays through your own
instrument, and Pianista listens to what you play and helps you get it right.

> **Status: early development.** There is nothing to download yet.
> See [docs/CONCEPT.md](docs/CONCEPT.md) for the vision and plan.

## Why

Most apps that teach piano with a MIDI keyboard are paid or subscription-based.
Pianista aims to be a modern, free alternative focused on beginners:

- **Free and open** — MIT license, no accounts, no paywalls, no telemetry.
- **Low latency** — everything runs locally on the computer your keyboard is plugged into.
- **Your instrument makes the sound** — the accompaniment is played through your keyboard via MIDI.
- **Any keyboard** — works with class-compliant USB-MIDI keyboards (61, 76 or 88 keys).

## Planned features

First release (v0.1):

- [ ] Choose MIDI input and output devices
- [ ] Load songs from standard MIDI files (`.mid`)
- [ ] Assign tracks to right hand, left hand and accompaniment
- [ ] Falling-notes view with an on-screen keyboard
- [ ] **Listen** mode — hear the song played on your keyboard
- [ ] **Wait** mode — the song waits until you press the right notes
- [ ] Practice one hand at a time, slow down the tempo, loop a section

Later: play-along mode with scoring, progress tracking, metronome, sheet-music view,
UI localization, macOS and Linux builds. See the [roadmap](docs/CONCEPT.md#7-roadmap-after-mvp).

## Requirements

- Windows 10/11 (macOS and Linux are planned)
- A keyboard or synthesizer with USB-MIDI (developed and tested on a Casio CT-X700)

## Building from source

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/Georgiy-smr/Pianista.git
cd Pianista
dotnet build
dotnet test
dotnet run --project src/Pianista.Desktop
```

## Tech stack

- .NET 10, C#
- [Avalonia](https://avaloniaui.net/) and [ReactiveUI](https://www.reactiveui.net/) for the UI
- [DryWetMidi](https://github.com/melanchall/drywetmidi) for MIDI devices and files
- [Mediator](https://github.com/martinothamar/Mediator) and [Scrutor](https://github.com/khellang/Scrutor) for application structure
- xUnit v3 for tests

## Contributing

The project is at a very early stage. Ideas and feedback are welcome — open an issue.
If you have a MIDI keyboard other than the Casio CT-X700, reports about how Pianista
works with it will be especially valuable once the first build is out.

## License

[MIT](LICENSE)
