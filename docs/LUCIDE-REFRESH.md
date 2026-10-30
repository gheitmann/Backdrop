# LOCAL17 artwork refresh

Local preview version: skin/editor/setup 1.39.0.17. Published ALPHA15 remains unchanged.

## Visible changes

- Replace bevelled, low-resolution controls with Lucide outlines and matte blue-grey surfaces. Checked state uses a cyan check; unchecked state uses an empty square. Focus has a cyan border and left accent. Native checkbuttons draw their state separately, so an always-visible tick is never baked into a background.
- Refresh navigation, transport, OSD, rating, arrow and missing-artwork symbols, including alternate native states. Previous track and navigation Back remain different icons. Native command IDs, hit areas and bindings are retained.
- Rasterize topbar vectors at four times their original resolution. Preserve original proportions for every surface to prevent thin, letterboxed selection rows.
- Replace seven stock slideshow backgrounds with three licensed modern images: living room, recording studio and blue abstract. Thirty XML definitions and the matching editor fallback generator use the new stock artwork. Personal fanart remains property-driven. Background gradients, media masks and preview screenshots retain their original functions.
- Replace the guide's rotated marquee with a static, smaller label in both active definitions. Long group names can be clipped; the group chooser still provides their full names. This avoids the reported vertical scrolling flicker without changing the bound group button.

## Scope and evidence

The inventory contains 819 texture names: 550 UI textures and 269 content/gradient/preview assets. All 550 UI textures are generated: 519 general textures plus 30 topbar states and one topbar surface. This is asset coverage, not a claim that every optional plugin was exercised.

- 519/519 general output textures decode and match their recorded dimensions; seven critical state pairs differ. Topbar source vectors and native dimensions are retained separately.
- Native binding audit: 139 windows, zero incompatible bound types; the 69 previously absent fields remain unchanged.
- Both original TV guide definitions retain their anchors. Six rotated captions at two output sizes, nine Latest captions, six focused music-detail captions, generator geometry and both radio texture overrides pass the layout check. The guide check also rejects reintroduced marquee scrolling.
- Payload validation: 2,398 files, 557 skin XML files and AnyCPU editor flags pass. Packaging reopens and byte-compares every mapped payload file.
- Isolated MP 1.39 x64 visual checks at 3440 x 1440: Home selection rows, Settings root, General panel and main settings render readable matte controls at full height. A checkbox was switched off/on/off and displayed the correct separate outline/check each time; its initial test setting was restored. New abstract background and outlined spin arrows were visible. Topbar displays the Lucide glyphs and a distinct Home focus state; the final asset render increases raster resolution without changing its geometry.
- Single-file EXE VerifyOnly completes with exit code 0 and native binding/presentation probes. This performs no installation. A fresh real installation of LOCAL17 is not claimed.
- Final populated guide check: the modern room fills the canvas, programme selection is cyan, arrows and time headers render as matte controls, the current-time marker is a thin line at its native intrinsic width, and the rotated group caption remains static. The retained Highlights group intentionally has four fixture channels. Native progress textures retain their original implementation.

Final setup SHA256: `94ebf03c4019ff9d4a9f07c21b994acb6f8ac5a29e7d68eea48bd99dbfb51df0`.

The isolated test runtime uses a separate LOCAL17 texture cache; the prior cache and retained media/EPG fixtures are preserved. Inspect a regenerated cache after changing raster dimensions. Initial test iterations exposed content misclassification and aspect-ratio padding; these were corrected before packaging.

## Sources and reproduction

Lucide is pinned to commit `7faf87628a1ad6175975bc595344337ce14a7db4`. The shipped `Media/Modern/LUCIDE-LICENSE.txt` includes ISC and inherited notices. Photo author links and license attribution are in `Media/Modern/ARTWORK-CREDITS.txt`; upstream URLs and checksums are in `Source/Artwork/sources.json`.

Run `tools/fetch-modern-artwork.py` only when refreshing upstream assets. Use `tools/apply-modern-artwork.py` for the stock-background mappings. Build UI artwork with `tools/inventory-control-artwork.py`, `tools/build-control-artwork.cjs` and `tools/build-lucide-artwork.cjs` (Node with Sharp). Run `tools/check-control-artwork.cjs` for decoding/state checks and the contact sheet, then the regular layout, binding, payload and packaging checks.

Keep the recorded native dimensions when rebuilding: reusing rendered dimensions would enlarge the assets again. Classify photos, masks, backgrounds, game art and settings previews before matching generic names such as "default" or "button". Do not change the native control type to implement a cosmetic refresh.

Outstanding functional transport cases, optional-plugin runtime coverage, x86 and the previously deferred Live TV progress behaviour are outside this visual refresh.
