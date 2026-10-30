# ALPHA27 build verification — 26 October 2026

- Package/editor/setup version: 1.39.0.27. Independent DayNight plugin: 0.2.0.0,
  released as v0.2.0-alpha.1 in gheitmann/MediaPortal-DayNight.
- Seven selected static checks passed (7/7): payload, integration, guide,
  playback, menu, secondary layout and Day/Night validation.
- Native integration probes passed 1,364/1,364 assertions on each of x86/x64.
  Standalone extracted-plugin probes passed 2,201/2,201 on each architecture.
- Verified 30 daylight asset hashes/dimensions and 42 expressions in 33 XML files;
  decoded all 30/30 daylight PNGs. All 425/425 protected native artwork hashes
  and dimensions remain unchanged. There are 37 plugin languages x 18 strings.
- Package reopened and byte-compared 2,442/2,442 payload files, with 560 skin XML
  definitions. The additional plugin license is included in the skin Media folder.
- Private setup bundle reopened and byte-compared 21/21 entries. No MediaPortal
  core binaries are redistributed.
- Final single-EXE VerifyOnly passed against the installed x64 client and extracted
  x86 client (2/2). Each presentation probe checked 3/3 readable buttons and
  preserved 2,393/2,393 synthetic events. No installation was performed by these
  final EXE checks. The first sandboxed invocation failed in its restricted context;
  the normal Windows-context reruns passed on both architectures.
- Final EXE size: 167,951,360 bytes. SHA256:
  `2551605322aa52392be486f1a7339c99c24ff2181bb1a5e7545c6a147920b25a`.
- The independent plugin ZIP re-read and byte-compared 8/8 files. It contains
  the AnyCPU DLL, English documentation, license and skin integration examples.
  Backdrop bundles exactly the same DLL. Source snapshot recorded in
  Source/DayNight/UPSTREAM.json; the separate repository is canonical.

The preceding local x64 deployment verified 69/69 files and exercised the native
configuration entry, six mode options, Morning/Noon live switching, time picker,
Cancel and restoration of Automatic. That local prototype test is carried forward;
the newly versioned full ALPHA27 installer has not been run in install mode.
See DAY-NIGHT-LOCAL-TEST.md. Full x86 client interaction, suspend/resume, prolonged
playback and every translated screen remain outside this scoped verification.
No recordings-pagination-flash or TV-progress fix is claimed. Personal fixtures,
local core/audio changes and third-party experiments remain excluded.

Official upstream review: MediaPortal's release page lists 1.39 Endurance in
x86/x64 form. This is context, not evidence of compatibility with every build:
https://www.team-mediaportal.com/news/mediaportal-139-release
