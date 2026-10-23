# ALPHA9 validation

Target: MediaPortal 1.39 x64, 23 October 2026. The AnyCPU menu editor is rebuilt as 1.39.0.9. No core or third-party plugin binary is bundled.

## Follow-up results

The existing 37-case checklist contains **36 passed (97.3%) and one blocked**. P01 and M09 were revisited; the remaining 35 cases are explicitly carried forward from [ALPHA8](VALIDATION-ALPHA8.md), not rerun or newly certified. This percentage measures that finite checklist, not overall skin/plugin compatibility. See [case evidence](REGRESSION-ALPHA9.json).

| ID | Check | Result | Evidence |
| --- | --- | --- | --- |
| P01 | Moving Pictures views | passed | List and both thumbnail checks carried forward. Filmstrip first-cover zoom now fits inside the left edge; Right changes selection/title. Coverflow displays three synthetic covers and Right changes its centered cover/title. |
| M09 | Progress click/drag seeking | passed | A paused 180-second local music fixture seeks forward to 147.86s (2:27), backward to 45.03s (0:45), and by midpoint click to 89.95s (1:29). Runtime logs record seek start and release positions. Twelve geometry checks pass. |
| P06 | TVSeries rating | blocked, inherited | Synthetic offline series ID -1 does not expose online rating; no online account action was attempted. |

## Implementation

Filmstrip retains native facade/control 50 and its existing cover sizes/zoom. X=60 and width=1800 reserve horizontal zoom margins; `onrigth` is corrected to `onright`.

Required native GUIProgressControl 118 remains unchanged. The existing process plugin installs an application-local WinForms message filter. It maps client coordinates to MediaPortal render coordinates, accepts only the visible Now Playing progress rectangle with eight pixels vertical padding, and requires seekable music on window 510 with Backdrop active and no routed dialog. It arms on left-button down inside the bar, seeks during held movement with a 200ms throttle, and commits the release inside the bar. Leaving the bar does not seek outside its bounds. The right edge stops 0.25 seconds before duration to avoid accidental end-of-track. Input exceptions are contained, and native messages continue through the application. No global hooks or OS input changes are installed.

Run `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tools/test-pointer-seek.ps1` with the installed MP references to reproduce the 12 geometry/boundary checks.

## Populated guide supplement

A separate fixture pass now verifies the 12-row TV guide with 16 fictional TV channels, programme cells/time headings/current-time marker, selected programme descriptions and TV group selection. Four radio channels and a total of 4,200 programmes are retained locally. See [EPG fixtures](EPG-FIXTURES.md). This supplement is not silently added to the historical 37-case denominator. Live tuning, recordings, reminders and radio guide rendering are not claimed tested.

## Remaining limits

Immediate topbar mouse focus/click timing remains an input limitation; functional command results from earlier passes do not certify every rapid pointer gesture. TuneIn/RadioTime remains absent and its reported freeze is unresolved. ARD HLS playback, full x86 runtime, all locale renderings and additional themes remain unverified. Only the default theme is shipped. Local BASS/radio/Latest Media Handler experiments remain outside this installer.

## Delivery checks

- Built AnyCPU editor 1.39.0.9; geometry checks 12/12 pass. The final guarded build also seeks during playback from 89.95s to 45.03s by drag.
- Validated 2,393 payload files, 557 skin XML files and AnyCPU assembly flags. Installed MPE serializer accepts the package and platform metadata.
- Binding audit: 139 windows inspected, zero incompatible bound control types; 69 absent optional/required fields remain as before.
- All 62 immutable v1 media hashes remain unchanged. Locale/theme/topbar static inventory passes with the previously documented fallback limitations.
- Reviewed filmstrip XML is backed up and deployed to the normal skin folder. The isolated runtime contains the new editor DLL. Copying the DLL to the normal Program Files plugin folder was denied by Windows permissions, so that installation needs the ALPHA9 installer to enable pointer seeking. This local deployment limit does not change the isolated runtime evidence.
