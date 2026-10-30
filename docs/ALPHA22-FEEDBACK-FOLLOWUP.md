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
- TV and radio recording action-menu headings now use an explicit lower-left
  rotation anchor and a zero-duration 90-degree transform, matching the repaired
  dialog captions. The previous centered, implicit-pivot one-millisecond
  transform left MENU horizontal in the supplied TV Recordings screenshot.
  Caption containment passed for both pages at 1920x1080 and 3440x1440; the
  installed XMLs were backed up and replaced. Live appearance needs a recheck.

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
The normal icon grid was exercised across multiple pages with 72 demo recordings.
Page Down and Page Up work, including reaching page 5/5. This establishes basic
pagination usability; it does not establish the absence of a transient flash.
The large-icon layout loaded with six items and twelve pages, but its pagination
was not established in this check. A temporal capture of the flash remains open.

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
- Populated Latest Recordings captions checked in all three slots (3/3): heading,
  blue title and genre are separate and readable, including a long title.
- Local demo library contains 72 synthetic recordings. The normal icon layout
  displayed five pages; pages 1, 2, 3 and 5 were observed and navigation worked
  in both directions. Standard fallback disc artwork was displayed; generated
  numbered thumbnail sidecars were not established as working in the client.
- Fixed a local TV database fixture prerequisite: missing FirstDayOfWeekend
  caused the native recordings screen to fail while building date groups.
  Added value 0, which produces the native Monday working-day fallback. No
  recording rows were removed and no MediaPortal binary was modified.
- Navigation experiments were reverted: the four-second idle transition and
  mouse hover explained the focus changes. No speculative animation change is
  included. Temporary debug logging was restored to its previous level (2).
- Large icons loaded on the restored, unmodified animation layout (page 1/12).
  Its pagination, temporal flash verification and full x86 runtime
  coverage remain unverified; the above checks are not a 100% runtime claim.
- No new Alpha installer was built or uploaded for these follow-up changes.
