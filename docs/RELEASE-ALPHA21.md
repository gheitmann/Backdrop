# Backdrop ALPHA21: menu layout consistency

Music Files, Music Library and Music Playlist now use the same framed row surface
for buttons, sort controls and checkbuttons. All 33 affected rows have a 42px
native height, 16px left inset and native vertical text centering. Removed 62
external animated focus images so each native control paints its focus beneath
its label, sort indicator and checkmark. Control IDs, types, commands and plugin
routing remain intact.

The Plugins page has a 24px left text inset and vertically centered labels.
MediaPortal's native home menu repeats entries to fill its configured eleven
rows, so five enabled plugins used to appear more than twice. A Backdrop-scoped
adapter caps the native viewport at min(11, actual plugin count), retaining
native scrolling and plugin activation. It rebuilds native menu resources only
when the visible row count changes. No MediaPortal core files are modified.

## Technical verification

- 33/33 affected music row definitions pass framing/alignment checks, including
  real texture-file existence and removal of external focus overlays.
- Original guide/caption layout and playback layout checks pass.
- Native binding audit: 139 windows, zero incompatible controls. The same 69
  pre-existing absent fields remain in the audit.
- Payload: 2405 files and 558 skin XMLs pass validation and byte-verified packing.
- Completed single-file Setup verification passes against both official extracted
  x86 client files and the installed x64 client (2/2). No native installation was
  performed by these probes. The ALPHA20 architecture correction is retained.

Personal demo data, credentials and machine-specific logs are excluded. Installer,
skin and editor internal revision is 1.39.0.21. ALPHA19 playback limitations remain
open; this release addresses the reported menu layout problems.

## Final visual pass

The installed x64 demo was inspected after the implementation block. The Plugins
page shows five enabled entries in five rows, each once, before and after Down
navigation; its labels have visible left padding and vertical centering. Music
Library and Playlist menus show complete neutral row frames and centered labels,
including Sort by, Radio, Shuffle and Repeat. Sort focus renders beneath its label
and arrow. Music Files shares the checked row definitions but was not separately
visited in this visual pass. This is menu coverage, not a full plugin or playback
acceptance test.
