# ALPHA26 owner-feedback follow-up

This is source work after the published ALPHA26 installer. It is not a new
release or a locally installed update.

## Reviewed changes

- TV HomeServer now uses the existing `Modern/tv.jpg` living-room background
  instead of the cinema image inherited through `Modern/home.jpg`.
- Restored the two original TV-guide group textures and all four original
  hidden-menu toggle textures from provenance commit
  `90f1911ef2f56e78ce265054a542539045afd00b`. Their hashes and native
  dimensions are protected in the fidelity manifest.
- The Settings root page imports Backdrop's native right-aligned clock/header
  in both active definitions. Other settings pages retain their current
  header and need their own visual review.
- Raised the shared fast-forward/rewind seek cluster by 210 canvas pixels.
  Its lowest edge is now Y=739, above the expanded OSD that starts at Y=765.
  The shared include also serves TV and Online Videos, so runtime inspection
  remains necessary on those screens.

## Confirmed owner results retained

Latest Recordings description, Music Now Playing menu edge, Music list-row
artwork, Live TV frame, and the TV Recordings list separator are reported
correct. The focused icon caption in TV Recordings is intentionally hidden by
the facade XML; no change was made to its focus scale or caption behavior.

## Still open

- Artist and album information dialogs already bind the native thumb, Genre,
  Styles and review/track properties. The previously tested artist fixture
  showed a populated thumb. Reproduce the new missing-thumb case with the
  actual album/artist and metadata before changing bindings or F9 buttons.
- Lyrics' top menu texture is byte-identical to the original asset. Search
  Music uses its own dialog/menu layout. Both need a runtime comparison before
  altering icons solely from visual inference.
- A more general Settings background was generated for review. It has not
  replaced the ALPHA26 artwork or entered the package.
- Seek placement, TV-group artwork, toggle appearance, and Settings header
  need live confirmation. Recording icon pagination flash and full x86 runtime
  coverage remain separate open checks.

## Validation

All 425/425 protected original artwork files match their recorded SHA256 and
dimensions. Payload validation passes for 2,408 files, including 558 skin XML
definitions. Guide, playback, menu and secondary layout checks pass. The
optional Node artwork decoder did not run because `sharp` is absent in the
current shell; its image-hash coverage was checked directly with Pillow.
