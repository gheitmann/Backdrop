# Optional background lighting — unreleased prototype

The owner authorized ALPHA27 publication and a separate plugin repository.
Plugin version 0.2.0.0 was previously installed and verified locally.
Canonical plugin source: https://github.com/gheitmann/MediaPortal-DayNight
Snapshot provenance: Source/DayNight/UPSTREAM.json.

## Controls

In MediaPortal: Settings > GUI > Skin > Background lighting.
In German: Einstellungen > GUI > Skin > Hintergrundbeleuchtung.
The native menu dialogs work with keyboard, remote control and mouse.
Changes are staged until Save; Cancel or Escape discards them.
The separate MediaPortal Configuration process-plugin dialog remains available.
The Backdrop menu editor is not required.

Six modes are available: Automatic, Morning, Noon, Day / afternoon, Night,
and Off (original). Automatic uses local Windows time with four configurable
minute-precision starts: 06:00, 12:00, 15:00 and 19:00 respectively.
Rotated schedules crossing midnight are supported; duplicate or out-of-order
starts are rejected. Existing version-1 preferences preserve their bright
interval endpoints when migrated to version 2. The two internal boundaries
are interpolated; the original 06:00–19:00 schedule becomes 06/12/15/19.
Preferences live in the separate MediaPortal configuration file DayNight.xml.
Saving from the native menu applies immediately; external edits are picked
up on minute boundaries or window activation.

The plugin embeds 37 languages with 18 UI strings each and English fallback.
The language follows MediaPortal. Translation completeness was checked;
linguistic review by speakers of every language has not been performed.

## Skin integration

The independent AnyCPU process plugin publishes #DayNight.Period (morning,
noon, day, night), #DayNight.Mode, #DayNight.Supported, #DayNight.Available,
and #DayNight.Label.Title. A minute-aligned timer coalesces GUI-thread updates.
No artwork is decoded by the timer, and properties are written only on changes.

Media/DayNight/skin-support.xml declares contract 1 and supported periods.
All declared assets must exist inside the current skin Media directory.
Unsupported skins retain their own rendering. Missing assets or invalid
preferences fall back to original backgrounds. Older two-period integrations
receive day for morning/noon. Other skins need their own bindings and artwork;
installing the plugin cannot automatically adapt arbitrary skins.

The optional native configuration window uses ID 87950, registered only when
DayNight.xml exists and the ID is free. Backdrop exposes button 87900 only
when the window is available. The plugin never replaces another window.

## Artwork

Ten themes each have distinct Morning, Noon and Day PNGs. Morning uses warm
low sunlight; Noon uses clearer overhead light. The 20 new images preserve
the scene themes and broadly matching composition, rather than pixel-identical
geometry. Generated dimensions and exact prompts are recorded in
Source/Artwork/day-night.json. All 30 daylight PNGs decode successfully.

There are 42 phase expressions across 33 root XML files, including the native
configuration landing page. Both editor output paths preserve the same stock
mapping. Custom images, fanart, original controls and menu layout are retained.
All 425 protected original artwork hashes and dimensions remain unchanged.
The startup splash remains static because it loads before these expressions.

## Verification

Seven selected static commands passed (7/7). Native x86 and x64 probes each
passed 1,364/1,364 assertions, including schedule boundaries, migration,
settings persistence, fallback, all expressions, languages, editor output,
configuration rendering and GUI-thread lifecycle. These hosts emit existing
log4net configuration warnings; no core logging binaries were modified.

The earlier private ALPHA26 candidate verified 2,441/2,441 payload files.
The final ALPHA27 package adds the plugin license and verifies 2,442/2,442
payload files and 560 skin XML files. See ALPHA27-BUILD-REPORT.md.
Local deployment verified 69/69 written files with a reversible backup.
The full x64 client opened the native dialog, displayed all six modes and
four starts, and applied Morning and Noon immediately without restarting.
See DAY-NIGHT-LOCAL-TEST.md for the local record and remaining limits.

Full x86 client interaction, suspend/resume, prolonged playback and visual
review of every translated dialog remain outside this scoped verification.
