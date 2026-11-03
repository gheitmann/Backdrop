# BETA3 tester feedback — 3 November 2026

Unreleased source follow-up. The published BETA3 installer is unchanged.

## Changes

- Restore `common.facade.pictures.xml` byte-for-byte from BETA2, commit
  `f9c85d5`, as explicitly requested. This restores the translucent filmstrip
  backgrounds, including the original folder presentation. The earlier attempt
  to remove the larger plates was rejected by the tester. Keep all four approved
  sharpened Photos backgrounds unchanged.
- Replace both MovingPictures XML files with the supplied corrected versions.
  Their only semantic difference is the resolution-logo path, which now uses
  `Logos/VideoResolution` instead of `Logos/VideoAspectRatio`.
- Replace the three logo directories with the supplied categories: four aspect
  ratios, 22 codecs and 18 resolution resources. Remove 20 misfiled/obsolete
  source files and map every resulting asset into both installer inventories.
  The new payload has 2,443 files, a net increase of one over BETA3.
- Keep the slideshow Properties title as a static native label. Give control 20
  a fixed width of 1,245 at X=345, height 65 and the smaller font16BC. Remove
  its duplicate font declaration. Long captions are constrained to the dialog
  heading area rather than scrolling across the right border.

## Evidence

The supplied archive contains 46 files. Both XML definitions and all 44 logos
match it byte-for-byte. All 44 logos were decoded successfully. Archive SHA256,
per-logo hashes and dimensions are recorded in
`Source/Artwork/tester-logo-corrections.json`. The original archive and comparison
reports are retained outside the repository under project outputs
`Beta3-feedback`.

Payload validation passed for 2,443 mapped files and 560 skin XML definitions.
Seven Python commands passed: payload validation, BETA1/BETA2/BETA3 feedback,
menu layout, secondary layout and Day/Night contracts. The focused BETA3 check
also protects the supplied logo hashes and the static bounded heading.

The internal review MPE1 was reopened and every mapped file byte-compared.
No client GUI test or local installation was performed for this source follow-up.
The remaining practical check is the long slideshow title in the running client
and the MovingPictures logo strip with actual media metadata. No new EXE or
release has been published by this change.
