# Original Backdrop TV guide geometry

Feedback applies to **ALPHA11**. The owner confirms the unwanted row-count/EPG captions and General settings extras are removed. The remaining guide problem is the inherited DWHD layout, despite the Backdrop textures.

The supplied original MPE1 contains a mytvguide.xml that byte-matches the retained original reference. Its full-screen background defines a **1920 x 1080 coordinate canvas**. These particular values must not be multiplied by 1.5 again. Raw screenshot pixel positions are not source skin coordinates.

## Verified original anchors

| Element | X | Y | Width | Height | Meaning |
| --- | ---: | ---: | ---: | ---: | --- |
| Guide panel, ID 2 | 30 | 612 | 1882 | 453 | Lower screen area |
| Channel row template, ID 20 | 63 | 612 | 398 | 64 | Native row template; do not assume 12 rows |
| Time header background, ID 90 | 356 | 555 | 244 | 51 | Immediately above the grid |
| Programme title, ID 13 | 30 | 45 | 1350 | inherited | Upper-left title |
| Programme time, ID 14 | 150 | 120 | 450 | inherited | Above description |
| Programme genre, ID 27 | 150 | 165 | 405 | inherited | Above description |
| Description, ID 15 | 15 | 225 | 1050 | 300 | Upper left; leaves space on the right |
| Video preview, imported | 1537 | 162 | 359 | 197 | Upper-right preview area |
| Date/time, imported | 1912 | 6 | 900 | inherited | Right-aligned anchor; X is the right edge |

Full machine-readable anchors and source SHA256 values: ORIGINAL-TVGUIDE-LAYOUT.json. Channel logo/label coordinates are offsets within generated row templates, not independent screen positions. Hidden dummy controls and off-screen placeholders are not visible layout defects by themselves.

## Confirmed ALPHA11 mismatch

Both active TV guide wrappers explicitly include mp139.mytvguide.12lines.xml. That template places channel rows at Y=205 (height 45), time headings near Y=153 and the description near Y=850. It also places preview video in the lower-right area, rather than the original upper-right area. The main wrapper provides an imported DWHD-style date/weather bar at the top left rather than using Backdrop's original common.time.xml clock. Multiple include files are valid; the problem is retaining their replacement geometry as the final design.

## Rules for the correction

1. Preserve the original four regions: grid below, description above left, preview above right, date/time above right. Treat these as fixed acceptance criteria, not optional visual inspiration in a prompt.
2. Resolve all includes/imports and relative/template coordinates before comparing layout. Track absolute controls, group offsets, row-relative offsets, right-aligned anchors and animation centres separately.
3. Preserve MP 1.39 required IDs/control types and modern bindings. In particular, group control 100 requires button3part; the original plain button cannot simply be restored unchanged.
4. Start with the verified native-coordinate geometry. For a real 1280 x 720 source, apply a uniform factor 1.5 to pixel dimensions/positions/animation geometry, including imported definitions, exactly once. Font sizes require separate visual verification. Do not scale from the dimensions of a forum thumbnail.
5. Do not retain a forced 12-row layout merely because it is currently selected. Derive visible row geometry from the original layout and verify channel pagination, single-channel view and group switching with the fictional EPG fixtures.
6. Use one guide background and one preview video area. Diagnose full-screen imports and playback overlays before removing artwork. This coordinate comparison does not yet prove the reported duplicate background is fixed.
7. Test empty and populated guides, long descriptions, no playback and active video, clock alignment, keyboard/mouse navigation and aspect-ratio scaling. Synthetic EPG channels have no tuner mappings; an actual live TV preview requires a separate playback test.

## ALPHA12 implementation

Both active definitions now use these nine anchors. The preview is inlined once with required native ID 99. The original group position retains MP 1.39 button3part type 100. Channel label width is increased, the single-channel logo moved into the header and the timeline clipped at the grid bottom. Seven visible rows follow the original template geometry. Radio guide variants remain unchanged. See VALIDATION.md for the bounded runtime results and active-preview limitation.

Run `python tools/check_guide_layout.py` before future guide updates. CI compares the active definitions with this reference rather than accepting replacement fallback coordinates.
