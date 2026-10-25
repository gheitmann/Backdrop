# Backdrop ALPHA22 for MediaPortal 1.39

This release collects the menu, dialog and editor corrections made since ALPHA21.
Download **Backdrop-Setup-ALPHA22.exe**, close MediaPortal and run it. This is one
self-contained setup executable; no companion scripts or DLL downloads are needed.
The skin, editor and setup revision is 1.39.0.22.

## Changes

- Extended consistent row frames, label alignment and focus handling across plugin,
  settings and dialog menus; removed external focus artwork that covered controls.
- Standard Home and Plugins display each enabled entry once, with a 24px text inset
  and vertically centered labels. Native scrolling and activation are retained.
- Restored classic icon styling closer to the original Backdrop appearance, while
  retaining the newer category backgrounds.
- Corrected BasicHome idle hiding, submenu arrows and several playback captions.
- Standalone menu editor supports the client architecture and correctly handles
  Save, Discard and Cancel. Open BackdropMenuEditor.exe directly from the client folder.
- Retained the architecture-aware installer and buffered filename/status presentation.

## Verification and limits

The compact installed x64 pass covered editor Save/Discard, category backgrounds,
BasicHome idle hiding and wake-up, submenu arrows, lower-screen EPG navigation,
plugin rows, Now Playing metadata and basic music playback. This is practical
coverage, not an exhaustive certification of every plugin and playback mode.
Build, payload, layout and single-EXE preparation results are recorded in the
release verification document.

The Now Playing progress-track alignment and text contrast on bright fanart still
need refinement. Album Artists population, recording thumbnail paging and Latest
Recordings captions were not fully confirmed with a real recording library.
Live TV/timeshift needs configured tuners; audio/subtitle and renderer-dependent
video sliders remain unverified. The native recording Compress action remains
unconfirmed. Full x86 runtime coverage is pending.

A separate local BASS playlist-end correction allowed MediaPortal to exit normally
in the test installation. **That MediaPortal Core.dll change is not included in
this skin release**, so ALPHA22 does not claim to fix the upstream audio shutdown
hang on other installations. Diagnostic source and evidence are kept separately.

Personal demo data, credentials, caches, databases and modified MediaPortal core
binaries are excluded. The setup patches only its private installer presentation
copy and preserves installed MediaPortal core files.
