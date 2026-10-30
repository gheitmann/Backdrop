# ALPHA25 artwork feedback follow-up - 26 October 2026

This source/local correction follows the published ALPHA25. It is not a new
installer release and does not modify the assets already uploaded to GitHub.

## Feedback addressed

- Restored original MENU and Live TV edge tabs, including their baked lettering.
  The modern surface renderer had replaced these semantic images with empty
  blue plates. This covers Music Now Playing and the TV screens sharing them.
- Restored original playlist focus/background/end textures used by TV Home and
  Music. Restored selected/unselected list rows used by the single-channel guide.
- Restored the original root TV guide texture family, including normal, light,
  selected and recording-state segments, time backing and timeline graphic.
- Restored the artist/album F9 information-dialog frame and its original button
  focus texture; retained the current populated artist/album properties and
  readable button labels.
- Classic Home and Plugins Home use the original room background and original
  narrow vertical list separator again. Their current padding and native menu
  bindings remain intact.
- Settings uses its original mixing-console photograph again. Its list focus,
  unfocused textures and black translucent backing are original assets; the
  latter was already unchanged and was verified rather than redrawn.
- tv.bg.png and tv_green_border.png already match the original source; retained
  byte-for-byte. No speculative resizing of the live TV viewport was performed.
- The user-confirmed Latest Recordings captions, Bookmark dialog, topbar and
  Music Now Playing progress/separator fixes are retained.
- New cinematic BasicHome backgrounds and startup artwork are retained.

## Original and modern variants

The installed/source baseline favors original control artwork. The previously
modernized versions of the 51 restored textures remain reproducible from commit
7fbaddb, with their SHA256 hashes and dimensions in
Source/Artwork/modern-variant-provenance.json. This is preparation for separate
variants, not a claim that two selectable themes or installers already exist.
Do not mix those assets into the classic baseline by filename guessing.

The native-fidelity manifest now protects 419 original assets. Control artwork
generation respects this manifest; the background mapping tool also preserves
the original Settings background and classic Home mappings.

## Verification scope

- 51 changed textures restored from the recorded original provenance commit.
- 64 relevant textures checked/protected, including 13 already original files.
- Artwork decoder: 518 textures checked, 419 original hashes/dimensions checked.
- Five Python checks passed: payload validation, guide layout, playback layout,
  menu layout and secondary layout. Together with the artwork decoder this is
  6/6 passing commands (100% of that static command scope).
- Local deployment: 53 skin files backed up, copied and SHA256 compared. The
  previous texture cache was retained under a dated sibling name. Receipt:
  project outputs/ALPHA25-Artwork-Feedback-20261026-092515/deployment.json.
- Geometry and native bindings are unchanged except the two classic Home
  background texture mappings. No editor rebuild is needed for these changes.
- Live visual confirmation of this entire follow-up remains pending. Original
  artwork hashes establish provenance, not a complete runtime rendering result.
- The earlier recording pagination flash and full x86 runtime coverage remain
  open. No new security conclusion is claimed.
