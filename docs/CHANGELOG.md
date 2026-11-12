# Changelog since the 1.39.0.34 import

This list covers every change made after the pristine import of the upstream
1.39.0.34 tree (`68bccc7`), including the released versions 1.39.0.36 to 1.39.0.39
and the repository tooling added alongside them.

## 1.39.0.36

Static reference work and the first maintenance release.

- Added `tools/check_skin_references.py`: resolves textures, `mask` attributes,
  fonts, imports and the MediaPortal 1.39 window IDs against the shipped files;
  parses the XML with ElementTree so commented controls are ignored.
- Added `Source/Artwork/skin-reference-baseline.json` (90 intentional legacy
  references with reasons) and `Source/Artwork/mp139-window-ids.json` (198 pinned
  window IDs); the check runs in GitHub Actions.
- Fixed the `topbarhome.xml` window ID (3004 volume overlay -> 3005 topbar).
- Fixed the paging-arrow and scrollbar texture paths in `references.xml`.
- Repaired remaining static texture references: video facade progress templates,
  no-signal progress background, file-dialog and cover-art-grabber progress
  backgrounds, and the doubled extension in the SubCentral default poster path.
- Merged the 1.39.0.35 tester work that had been released from a feature branch:
  TV Home, TV overlay, DialogMenu, InfoService feeds and Latest Movies changes,
  including the rebuilt editor, Day/Night plugin and setup sources.
- Aligned README, maintenance notes, installer documentation and the in-skin
  version label with the package version.
- `tools/validate.py` writes the hash manifest with LF endings on Windows.

## 1.39.0.37

Runtime fix and installer polish.

- Fixed the menu editor idle refresh: timer ticks are coalesced and window/control
  updates run on the GUI thread through `GUIWindowManager.SendThreadCallback`.
  This removes the `LoadSkin: Running on wrong thread` errors and the follow-up
  `GUIMultiImage` exception from `BackdropMenuEditor`.
- Replaced the installer welcome text that still called the package an experimental
  test build and pointed to a German readme.
- Rebuilt the AnyCPU menu editor and the Day/Night plugin at 1.39.0.37 and stamped
  the setup executable with the release version.
- Verified in the isolated client and installed on the real client
  (2,443 of 2,444 payload entries byte-identical; the difference is the personal
  `BackdropMenuEditor.xml`).

## 1.39.0.38

Plugin page release.

- Rebuilt the InfoService feeds page with a content panel, larger typography and a
  full-height feed list.
- Re-laid out the Wikipedia page: 640x360 image, separate 1085-wide text column,
  DayNight background instead of a black page. Verified with a real search
  ("Berlin").
- Ported the WorldWeatherLite window set (11 XML files), `Media\WorldWeather` and
  20 support images from DefaultWideHD, mapped to Backdrop fonts, DayNight
  background and static separators instead of unsupported `#scroll.wrap` tokens.
  The package payload grew to 2,574 files.
- Hid the static Extensions placeholder labels.
- Documented the Extensions limit: the list itself is drawn by the plugin.

## 1.39.0.39

Installer correctness release with the same plugin page content.

- Regenerated the serialized `UniqueFileList` with MpeCore. The 1.39.0.38 package
  contained all 2,574 files but the index still listed 2,444, so the native
  installer skipped the 130 new weather files.
- Verified the package reports 2,574 installable and 2,574 indexed files.
- Installed on the real client: 2,573 of 2,574 payload entries byte-identical,
  zero missing; editor and skin report 1.39.0.39.
- Added the index-regeneration rule to `docs/MAINTENANCE.md`.

## Repository, validation and release tooling

- Initialized Git, imported the upstream tree and connected
  `https://github.com/gheitmann/Backdrop.git`.
- Added a self-contained runtime test client
  (`Documents/Codex/Testmedien/MediaPortal/runtime-test`) with its own
  `MediaPortalDirs.xml`, plus a portable x86 instance, so checks no longer touch
  the installed client.
- Recorded runtime evidence in `docs/VALIDATION.md`: scoped x64 start, verified
  real installation, x86 runtime and setup VerifyOnly, and the open limits.
- Documented the checks, the runtime client and the package rules in
  `docs/MAINTENANCE.md`.
- `tools/package.py` names packages by version (`Backdrop-MP139-<version>.mpe1`);
  the `AnyCPU` suffix was dropped from package and CI artifact names.
- Published releases 1.39.0.36 to 1.39.0.39 with the setup EXE and MPE1 plus
  SHA256 sidecars; tags `v1.39.0.36` to `v1.39.0.39` are pushed.
- Added release notes for 1.39.0.36, 1.39.0.37, 1.39.0.38 and 1.39.0.39.

## Known limitations

- The Extensions list is drawn by the plugin and stays empty until it indexes
  extensions; the skin only supplies the frame.
- WorldWeatherLite shows placeholder values until a location is configured.
- The external LatestMediaHandler plugin still loads skin resources off the GUI
  thread; those log entries are not caused by Backdrop.
- No complete x86 installation run is claimed.
