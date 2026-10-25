# ALPHA22 visual feedback follow-up — 25 October 2026

This work follows the ALPHA24 draft. It does not replace its uploaded installer.
The user confirmed that actual startup looks correct, closing that visual item.

## Corrections

- Restored all 30 original topbar icon states and the original backing strip.
  These are cul8er's actual PNG files, not the previous vector reconstruction.
- Restored original Home list focus/unfocused textures, Settings list textures,
  the bookmark/menu dialog frame and default Now Playing separator texture.
  Total: 37 assets compared byte-for-byte against the original source and the
  provenance commit recorded in native-fidelity.json. The existing Home text
  padding, fixed dialog-caption anchors and current native actions are retained.
- DialogMenu already retained the original panel/list geometry. Restoring its
  original dialog_290_400.png returns the framed appearance without restoring
  the old out-of-frame rotated title coordinates.
- Focused Latest Recordings used the same x=1554 column for the heading and blue
  title. The title and genre now follow the heading at x=1602 and x=1640, ending
  before thumbnails at x=1678. All three recording slots and editor-generated
  output use the same layout. With the 90-degree rotation, these are consecutive
  lines in the original reading orientation.
- The actual original separator PNG is restored for the two Now Playing lines;
  the corrected progress-track geometry is retained.

## Recording icon-row flash

The active recording grid disabled automatic focus zoom but still requested
65-pixel manual X/Y zoom. The native GUIThumbnailPanel renderer applies these
manual values independently and expands the selected item's clip rectangle.
Both recording grids now explicitly use zero manual zoom. This removes the
skin's above-viewport expansion and is a targeted mitigation, not a confirmed
resolution of every pagination flash.

Official source inspected:
https://github.com/MediaPortal/MediaPortal-1/blob/master/mediaportal/Core/guilib/GUIThumbnailPanel.cs
RenderItem and its manual zoom/clip handling; OnPageUp/OnPageDown and texture
release are implemented in MediaPortal itself. No core binary is modified here.
A multi-page recording-library retest is still needed to confirm the reported
transient flash. A static image cannot establish that temporal behavior.

## Verification

- Editor DLL and standalone EXE rebuilt successfully, including its x86 worker.
- Six repository check commands passed (6/6, 100% of that static command scope).
- Artwork verification decoded 518 tracked textures and verified hashes/dimensions
  of 363 preserved original assets. The topbar build command preserves all 31
  original icon/strip files rather than regenerating them.
- Local deployment: 42 files backed up, copied and SHA256 compared, including
  the standalone editor and process plugin. Receipt: project workspace
  outputs/ALPHA22-Feedback-Backup-20261025-181618/deployment.json.
- Live x64 check: Settings shows original transparent rows, silver focus and
  readable inset text; the topbar shows the restored original transport icons.
- Populated recording-caption and multi-page icon-grid behavior are not certified
  by this brief live check. The pagination flash remains subject to retest.
- No new Alpha installer was built or uploaded for these follow-up changes.
