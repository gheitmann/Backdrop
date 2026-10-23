# ALPHA8 validation

Target: MediaPortal 1.39 x64, 23 October 2026. The AnyCPU menu editor remains unchanged at 1.39.0.5; no DLL rebuild or core/plugin replacement is included.

## Follow-up results

The existing 37-case checklist now contains **34 passed (91.9%), two partial and one blocked**. This pass revisited the four cases below; other results are explicitly carried forward from [ALPHA7](VALIDATION-ALPHA7.md), not rerun or newly certified. See [REGRESSION-ALPHA8.json](REGRESSION-ALPHA8.json) for all cases. The percentage is specific to that checklist, not overall compatibility.

| ID | Check | Result | Evidence and limits |
| --- | --- | --- | --- |
| P01 | Moving Pictures views | partial | List, normal thumbnail and large thumbnail views render the three synthetic movies. The corrected 60-pixel grid margin keeps the first zoomed cover fully visible. Filmstrip was also opened and still clips its first cover at the left edge; coverflow remains unverified. |
| TB05 | Play | passed | With the synthetic track paused at 1:28 using the native shortcut, a mouse click on Play resolved its dynamic action to 12 and BASS resumed the same stream. The position advanced to 1:30 and 1:48 without restarting. When not paused, Play retains action 68; existing playlist start/restart semantics remain. |
| M08 | Right-click context menu | passed | With general/autohidemouse temporarily disabled only in the isolated profile, right-click opened the populated Moving Pictures context menu from list and thumbnail views. This verifies context-menu support with a visible cursor, not every first click after native cursor auto-hide. The temporary setting was restored. |
| M09 | Progress drag/seek | partial | Now Playing progress displays and advances. MP 1.39 GUIProgressControl.CanFocus explicitly returns false and has no seek input handler. Required bound progress control 118 is retained; drag seeking is not implemented by this skin-only update. |

## Changes and source evidence

Play uses `#(iif(eq(#playlogo,'logo_pause.png'),12,68))`. MP publishes `#playlogo` as `logo_pause.png` while paused; GUIButtonControl supports property/expression-based action IDs. Choosing native Pause/Resume action 12 in that state avoids the playlist's global Play handler, which starts the selected item again. Cold Play and Play during ordinary playback retain action 68. The running application logged both resolved values, and the resume check advanced from the held position without a new stream start.

- [MP 1.39 playback state publication](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/MediaPortal.Application/MediaPortal.cs)
- [GUIButtonControl action parsing](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/Core/guilib/GUIButtonControl.cs)
- [Playlist global Play handler](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/WindowPlugins/Common.GUIPlugins/Music/GUIMusicPlaylist.cs)
- [Native progress control](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/Core/guilib/GUIProgressControl.cs)

The thumbnail panel retains Y=210/height=810 and now uses X=60/width=1800, reserving space for focus zoom at both edges. Normal and large thumbnails were inspected with three synthetic covers. Filmstrip edge clipping remains a separate layout issue; it is not silently declared fixed.

## Mouse diagnosis and limits

The right-click test succeeds when the cursor remains visible. MP's native MouseClickEvent deliberately suppresses a first click when its cursor is hidden. This explains a reproducible test precondition; it does not prove every previous-focus topbar activation has the same cause. Immediate topbar focus/click timing remains a limitation from ALPHA7. No global mouse setting is changed by the package; the isolated temporary change was restored and startup returned to BasicHome.

Dragging the existing progress display cannot be enabled by changing its skin rectangle: its native type is non-focusable. Replacing required control 118 with a slider would break the native binding. A future interactive seek control needs its own input implementation and separate runtime validation.

TVSeries online rating remains blocked for the offline test fixture. TuneIn/RadioTime DLLs are absent from both the isolated and normal plugin directories inspected here; the reported build and freeze remain unknown. Populated EPG, ARD HLS playback, additional themes and full x86 runtime remain unverified. Full locale coverage is unchanged from the ALPHA7 static inventory and five representative rendering checks.

## Delivery checks

- Payload XML/hashes and AnyCPU flags validated; 2,393 payload files and 557 skin XML files.
- Integration audit checks all 37 translation files, English property fallback, installed themes and topbar mappings.
- Installed MP metadata serializer and 139-window binding audit checked; zero incompatible control types, 69 absent optional/required fields remain.
- All 62 immutable v1 media hashes checked unchanged. Synthetic fixtures and diagnostic knowledge remain outside the repository.
- Packaging verifies archived payload bytes against their sources. Only the reviewed topbar and thumbnail XML are deployed to the normal skin directory after an ALPHA7 comparison and backup.
