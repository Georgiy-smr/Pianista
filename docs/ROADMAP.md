# Roadmap

The ordered plan of work up to the first release. Each item becomes a GitHub issue when
work on it starts; the issue title starts with the item ID (`R3: MIDI devices`), and the
item is done when that issue is closed. Items are worked on in order: an item starts
only when everything it depends on is done.

The plan favors getting a visible result on a real keyboard early: pressing a key on the
instrument lights it up on screen before any song features are built.

## M1 — Foundation

Pure logic, fully covered by tests.

| ID | Area | Item | Depends on |
|---|---|---|---|
| R1 | core | **Song model.** `Song` with notes (pitch, start, duration, velocity, track, channel) in absolute time (`TimeSpan`); `ISongFile` interface, `PianistaException` and `InvalidSongFileException`. First real tests; remove `--ignore-exit-code 8` from the test project. | — |
| R2 | midi | **Reading `.mid` files.** DryWetMidi implementation of `ISongFile` that produces a `Song`; DryWetMidi errors become `InvalidSongFileException`. Test files are built in code. | R1 |

## M2 — Hear the keyboard

First result on the real instrument.

| ID | Area | Item | Depends on |
|---|---|---|---|
| R3 | core, midi | **MIDI devices.** `IMidiInput`, `IMidiOutput` and device listing in Core with `Fake`s; DryWetMidi implementations. | R1 |
| R4 | desktop | **UI infrastructure.** Localization (`Strings.resx`), single error presenter for `ReactiveCommand.ThrownExceptions`, settings storage. | R1 |
| R5 | desktop, hardware | **Device setup screen.** Choose MIDI input and output; the choice is remembered. | R3, R4 |
| R6 | desktop, hardware | **On-screen keyboard.** 61/76/88-key range; keys light up in real time as they are pressed on the instrument. | R5 |
| R7 | hardware, docs | **Latency measurement.** Measure key press → highlight latency on Windows and document the result. | R6 |

## M3 — Listen mode

A song plays through the instrument while notes are shown.

| ID | Area | Item | Depends on |
|---|---|---|---|
| R8 | core | **Track assignment.** Mark tracks as right hand, left hand or accompaniment; defaults from track names, channel 10 as drums, pitch split for single-track files. | R2 |
| R9 | desktop | **Open a song.** File dialog, `LoadSong` command, track assignment screen. | R4, R8 |
| R10 | core, midi, hardware | **Playback scheduler.** `IClock` with `Fake`; sends the song to MIDI Out driven by `IClock`; tracks can be muted. | R3, R8 |
| R11 | desktop, hardware | **Falling notes.** Custom-drawn view synced to playback at a steady 60 fps. | R6, R9, R10 |
| R12 | desktop | **Transport controls.** Play, pause, seek, tempo 25–100 %. | R11 |

## M4 — Wait mode

The learning part.

| ID | Area | Item | Depends on |
|---|---|---|---|
| R13 | core | **Wait mode logic.** Playback stops at each note or chord until all expected keys are pressed. | R10 |
| R14 | desktop, hardware | **Wait mode UI.** Correct and wrong presses shown differently. | R12, R13 |
| R15 | core, desktop | **Hand selection.** Right / left / both; the player's part is not sent to the instrument. | R14 |
| R16 | core, desktop | **A–B loop.** Repeat a section of the song. | R14 |

## M5 — Release v0.1

| ID | Area | Item | Depends on |
|---|---|---|---|
| R17 | docs | **Sample songs.** A few public-domain songs bundled with the app. | R9 |
| R18 | infra | **Release build.** Self-contained Windows x64 zip published by GitHub Actions on a `v*` tag. | R16 |
| R19 | docs | **User docs.** README with screenshots and usage, `CONTRIBUTING.md`. | R18 |
