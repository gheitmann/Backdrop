# Backdrop ALPHA19 for MediaPortal 1.39

This prerelease brings the LOCAL16–18 Lucide redesign to a downloadable single
Setup EXE, and adds a standalone menu editor. It also includes targeted changes
from the user's ALPHA15 recordings/playback report.

## Included

- Dark charcoal menus with restrained borders and reduced rounding, Lucide icons,
  and six category backgrounds that follow BasicHome focus. Movies uses an
  illuminated Los Angeles cinema entrance; TV uses a large-screen close-up.
- Standalone `BackdropMenuEditor.exe`, installed in the MediaPortal application
  folder. It opens the editor directly and flushes saved settings without going
  through MP Configuration. Close Configuration before use and restart MediaPortal
  after changing its menu. The runtime plugin DLL remains installed.
- Hide the obsolete TV recordings Compress entry and skip it in navigation. The
  native control binding is retained; MediaPortal's TV plugin itself hides this
  control and exposes no active compression handler there.
- Music NowPlaying track and fill share a native progress control, eliminating
  separate vertical positions and legacy end-cap offsets in both progress bars.
- Bound the lower recordings thumbnail viewport to the screen, remove four long
  focus/visibility slide animations, and remove the negative text offset.
- Suppress the weather caption when its values are empty, preventing an orphaned
  separator below the clock.
- Make BasicHome video previews independent of the previous page's icon/list view
  state, including their associated frame, caption and progress information.
- Retain the previous pointer fixes, installer presentation repairs and native
  installation-path correction. No MediaPortal core binaries are distributed.

## Verification and remaining limits

All six reported topics have been inspected. This is not a claim that all six
runtime problems are resolved. XML/geometry assertions and native bindings are
checked separately from playback verification.

The changed local skin starts, displays the category backgrounds and clock, and
does not show an empty-weather separator. The standalone editor constructs and
loads the installed menu with the correct plugin/settings/skin paths. Its verify
mode does not save or modify the production menu.

Runtime confirmation is still needed for multi-page recording thumbnail flicker,
the final NowPlaying progress appearance and the exact Latest Movies fullscreen
return sequence. Local Latest Music cards did not reliably start the seeded title
in this run, so playback-dependent checks are not reported as passed.

Brightness (704), contrast (705) and gamma (706) match MediaPortal's native OSD
controls and handlers. Their reported lack of image effect remains unresolved;
renderer/driver support must be investigated. The skin does not implement image
processing. Audio/subtitle selection has not been tested. Live TV progress remains
a separate known issue.

The user confirmed successful ALPHA15 rewind/forward 2x–32x, pause, interval seek,
bookmarks and recordings views/filters. These are user test results, not newly
repeated automated tests. The local demo contains no tuner-mapped live stations
or populated real TV recording library. Personal demo data is excluded.

Internal revision is 1.39.0.19. LOCAL16–18 were local previews; the last public tag
was ALPHA15. Windows may display a publisher warning because the EXE is unsigned.

## Release checks

- Payload checks: 2405 files and 558 skin XML documents pass.
- Native binding audit: 139 windows, zero incompatible control bindings; 69
  pre-existing absent optional/required fields remain in the audit.
- Playback layout contracts and original guide/caption checks pass.
- Single Setup EXE `--verify` exits 0: isolated native installer binding and
  presentation probes pass; installed MediaPortal binaries remain unchanged.
  The presentation probe preserves 2393/2393 synthetic progress events and
  verifies three readable buttons and long-to-short filename repaint.
- Standalone editor `--verify` exits 0 and loads the installed menu. Interactive
  saving and a fresh full ALPHA19 installation have not been repeated.
