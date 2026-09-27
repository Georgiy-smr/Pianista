# Pianista — Concept

> Status: draft. This document captures the vision and the key decisions behind Pianista.
> README.md and CLAUDE.md are derived from it.

## 1. Vision

**Pianista is a free, open-source app that teaches you to play a MIDI keyboard.**
Notes scroll toward an on-screen keyboard, the accompaniment plays through your own
instrument, and Pianista listens to what you play and helps you get it right.

## 2. Problem

Most "learn piano with your MIDI keyboard" apps are paid or subscription-based.
The free alternatives that exist (Neothesia, PianoBooster, Linthesia) are either
focused on visualization rather than teaching, or no longer actively developed.
There is room for a modern, free, beginner-friendly trainer built on .NET.

## 3. Audience

- Beginners who own a digital keyboard or synthesizer with USB-MIDI and want to learn songs.
- People who can't read sheet music yet and want a visual, game-like way to start.
- Developed and tested on a **Casio CT-X700** (61 keys), but designed for any
  class-compliant USB-MIDI keyboard.

## 4. Principles

1. **Free and open.** MIT license, no accounts, no paywalls, no telemetry.
2. **Local and low-latency.** All game logic, timing and note evaluation run on the
   machine the keyboard is plugged into. Judging whether a note was on time needs
   stable latency in the ~10–20 ms range.
3. **Your instrument makes the sound.** The accompaniment is sent to the keyboard via
   MIDI Out, so it sounds like a real instrument and Pianista needs no software synth.
4. **Any keyboard.** No vendor lock-in; the keyboard range (61/76/88 keys) is configurable.
5. **Beginner first.** Waiting for the player is more important than scoring them.

## 5. MVP scope (v0.1)

1. **Device setup** — choose MIDI input (keyboard → app) and output (app → keyboard);
   remember the choice.
2. **Load a song** from a standard MIDI file (`.mid`).
3. **Track/hand assignment** — mark which tracks are the right hand, the left hand,
   and the accompaniment. Fallback when a file has a single track: split by pitch.
4. **Falling-notes view** above an on-screen keyboard; keys light up when pressed
   (correct vs. wrong shown differently).
5. **Wait mode** — the song stops at each note (or chord) until the player presses
   the right key(s), then continues.
6. **Practice controls** — right hand / left hand / both, tempo 25–100 %,
   loop a section (A–B), play/pause/seek.

## 6. Practice modes

| Mode | Behavior | Release |
|---|---|---|
| **Listen** | Plays the whole song through the keyboard, notes are shown | MVP |
| **Wait** | Stops until the correct notes are pressed | MVP |
| **Play-along** | Song keeps going; each note is judged (perfect / good / miss) | v0.2 |

## 7. Roadmap (after MVP)

- Play-along mode with accuracy scoring and a results screen.
- Progress tracking per song (best score, time practiced).
- Metronome and count-in.
- Sheet-music (staff) view as an alternative to falling notes.
- Song library screen with folders and search.
- UI localization (English first, then Russian and others).
- macOS and Linux builds.
- MusicXML import.

## 8. Non-goals

- A software synthesizer (sound comes from the user's instrument).
- A notation or MIDI editor.
- Online features: accounts, cloud sync, multiplayer, song store.

## 9. Architecture

```
Pianista.Core      Song model, MIDI-file parsing, lesson logic, note evaluation.
                   No UI and no device access — fully unit-testable.
Pianista.Midi      MIDI device input/output: DryWetMidi implementations of the
                   IMidiInput / IMidiOutput interfaces declared in Core.
Pianista.Desktop   Avalonia application: views, view models, rendering.
Pianista.Tests     Unit tests for Core (and Midi via fakes).
```

Key rules:

- `Core` depends on nothing platform-specific. Timing is driven by an injectable clock,
  so evaluation logic can be tested deterministically without a keyboard.
- `Desktop` talks to devices only through the interfaces declared in `Core`, so a fake keyboard
  can be plugged in for development and tests.
- Porting to macOS/Linux should only require verifying the `Midi` layer and packaging.

## 10. Technology

| Area | Choice |
|---|---|
| Runtime | .NET 10 (LTS), C# |
| UI | Avalonia (MVVM) |
| MIDI devices and files | Melanchall.DryWetMidi |
| Application structure | Microsoft.Extensions DI/Hosting/Logging, Mediator — source-generated, MIT (command + handler), Scrutor (decorators), StatusGeneric |
| Tests | xUnit |
| Target platform | Windows first; macOS and Linux later |
| License | MIT |
| Repository language | English (code, docs, commits, issues) |

## 11. Technical risks and open questions

- **Latency and timing accuracy** — measure end-to-end latency (key press → on-screen
  highlight) on Windows early; decide on hit windows for play-along based on real data.
- **Rendering smoothness** — falling notes need a steady 60 fps in Avalonia; prototype
  a custom-drawn control early.
- **Messy MIDI files** — tracks without names, both hands on one track, drums on
  channel 10, tempo changes. Needs sensible defaults and manual override.
- **Muting the player's part** — in wait/play-along mode the notes the player must
  play are not sent to the keyboard; only the accompaniment is.
- **Keyboard quirks** — test with the CT-X700 first; collect reports for other models.
- **Song content** — the repo ships only public-domain / self-made MIDI files;
  users bring their own.

## 12. Prior art

- Synthesia (paid) — the reference for the falling-notes experience.
- Neothesia (Rust, open source), PianoBooster (C++, open source), Linthesia (C++, open source).
- Built-in lesson functions on keyboards (e.g. Casio step-up lessons) — limited to built-in songs.
