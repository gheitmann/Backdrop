# ALPHA5 validation

Baseline date: 2026-10-22. Target: MediaPortal 1.39, tested on x64. Editor assembly: 1.39.0.5, MSIL/AnyCPU, without Required32Bit or Preferred32Bit flags. Full x86 runtime testing remains pending.

## Reported UI regressions

| Area | Result | Evidence and limits |
| --- | --- | --- |
| Settings | Verified | Main, GUI, and Skin pages inspected in the running application; Backdrop styling and vertical navigation retained. |
| TV/radio guide | Partial | Guide opened without channels. Populated EPG, tuning, recording, and reminder states still need testing. |
| Music Now Playing | Verified | Standard, analog VU, and LED VU layouts inspected during synthetic music playback. |
| Topbar | Verified in Music | Topbar appeared and Home worked. Other actions and pages are not exhaustively covered. |
| Lower menu mouse navigation | Verified | Category hit areas and submenu wake-up checked. All mouse gestures are not exhaustively covered. |
| Separator placeholders | Verified | No remaining visible `#scroll.wrap` tokens in the checked skin XML. |
| Vertical side-menu caption | Verified | Blue side caption observed vertically in the running playlist action menu. |

**6 of 7 areas verified (85.7% of this specific regression checklist).** This denominator does not represent overall skin or plugin compatibility.

The standard MP3 to FLAC to M4A playlist ran to its natural end and returned to the playlist. The analog and LED runs verified presentation. The test application closed normally. Separate local core/radio test fixes were present in the isolated test environment; they are not included in this repository or installer.

## Static baseline

- 139 audited windows, zero control-type mismatches.
- 69 missing optional/required fields remain documented in the local audit; zero type mismatches does not mean every binding is complete.
- 557 shipped skin XML files; the runtime-generated personal settings file is excluded.
- 617 imports/includes checked in the local audit.
- 2,393 installer payload files: 2,388 skin files, one editor DLL, one default editor configuration, and three fonts.
- All baseline payload bytes matched the verified ALPHA5 delivery before import.
- 62 persistent local media fixtures retained with unchanged SHA256 hashes; fixtures are not uploaded.

The repository validator checks payload hashes, XML syntax, forbidden personal settings, unresolved separator tokens, the editor's basic AnyCPU PE flags, and installer mappings. It does not reproduce the GUI control-binding audit or runtime tests.

## Remaining checks

- Populated TV/radio guide, channel switching, recordings, and reminders.
- Full x86 runtime and plugin interoperability.
- Additional plugin pages, languages, themes, and navigation paths.
- OnlineVideos playback retest after successful isolated source/decoder checks.
- Independent HideVolumeOSD startup warning in the test installation.

The historical 84/100 runtime checklist belongs to an earlier build and is not a newly measured ALPHA5 score.
