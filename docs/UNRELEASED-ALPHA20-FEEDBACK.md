# Unreleased response to the ALPHA20 field report

This local code/artwork block addresses the user's ALPHA20 screenshots. No new
Alpha, installation or runtime visual acceptance pass is included. Modern category
photographs and the preceding menu geometry fixes remain in place.

## Standalone editor on x86

The ALPHA20 AnyCPU editor reproduced the reported BadImageFormatException while
loading Utils.dll from official MediaPortal 1.39 x86 files on 64-bit Windows.
The corrected launcher reads Utils metadata before loading MediaPortal code and
starts its embedded x86 worker when required. The public editor remains one EXE;
no manual architecture selection or separate worker download is necessary. The
worker is extracted to a content-addressed local application cache. Both processes
embed the editor DLL and use the selected installation's own dependencies.

Verification: ALPHA20 failed with the exact Utils.dll error (exit 1). The corrected
single EXE creates and loads the editor in x86 and x64 processes (2/2, exit 0),
without showing or saving the form. The x86 test includes a directory name with
spaces and a trailing separator. Interactive editing/saving on the HTPC is still
untested. No installed MediaPortal files were changed.

## Artwork direction

The topbar now uses 30 newly generated states with filled transport silhouettes,
silver shading and restrained Backdrop-style charcoal discs. Focus uses the
skin's pale blue accent. This replaces the outlined Lucide cards on that surface.
The glyphs are code-authored vectors; PNGs retain the native footprint at 4x
raster resolution. Tools honor the selected classic topbar style on regeneration.

326 semantic/native-layout assets are restored from the repository's original
Backdrop import (90f1911ef2f56e78ce265054a542539045afd00b). This includes OSD
status artwork, speed indicators, marks, slider nibs, the clock backing plate,
NowPlaying composition and matching menu highlight/arrow variants. Filename-based
icon inference had incorrectly turned nowplaying.overlay.png into a giant Play
symbol, clock.png into a lock and slider nibs into opaque icon cards. Native
geometry and transparency are retained. Menu-selected backing art restores the
original opening silhouette. No new background photographs were introduced.

The fidelity manifest records the source revision, dimensions and SHA256 values.
The control generator skips these semantic assets; payload validation and the
artwork decoder check their hashes to prevent accidental regeneration. Other
existing redesigned surfaces remain; this is a targeted return toward the
original visual language, not a claim of a wholly new accepted icon collection.

## Home and Latest Media

- Six focused main-menu buttons now obey the same idle-hide flag as the menu
  artwork. Hidden focus remains available for waking navigation. Generated menus
  receive this rule too. Submenu buttons explicitly suppress an inherited neutral
  texture, preventing unintended empty surfaces.
- Original right-arrow highlight variants are restored, including the matching
  neutral states. Latest Media navigation commands remain unchanged.
- Six TV recording title/genre captions use separate vertical strips, an explicit
  lower anchor and a constant 90-degree pivot. Previously Visible/Hidden rotations
  had an implicit center and could leave both lines horizontally overlapping.
  Saved skin XML and editor-generated XML are changed together.
- The reported NowPlaying track/fill offset was already addressed after ALPHA20
  in c2838df across its main, VU and overlay variants. Those changes remain. This
  block also removes the erroneous giant Play background via native artwork.

## Verification scope

- 518 control textures decode; 326 fidelity hashes and native dimensions pass.
  Thirty topbar states have the expected native footprint at 4x resolution.
  No generated artwork was visually previewed in this block.
- Six idle guards and transparent submenu defaults pass structural checks.
- Six focused TV recording metadata captions pass bounds checks at 1920x1080
  and 3440x1440, with generator geometry/pivots checked against saved XML.
- Existing checks for 265 menu rows, ten selection rows, 52 secondary action
  buttons, five music tiles and 15 playback tracks pass.
- Native binding audit: 139 windows, zero incompatible controls; the same 69
  pre-existing absent fields remain. Payload: 2405 files and 558 skin XMLs.

The supplied reference images were read in the browser. The local MediaPortal UI
was not opened or inspected. Runtime appearance, idle wake behavior and playback
still need a later visual pass. Live-TV progress and renderer image-effect slider
support remain separate open issues. Existing dist files are the old ALPHA21
release and are not rebuilt by this work.
