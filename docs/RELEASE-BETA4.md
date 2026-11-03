# Backdrop BETA4 for MediaPortal 1.39

Version **1.39.0.31** incorporates the BETA3 tester corrections.

## Main changes

- Restore the previous Photos filmstrip definition from BETA2, including its
  translucent backgrounds and original folder presentation. Keep all four
  approved sharpened Photos backgrounds unchanged.
- Adopt the tester's corrected MovingPictures XML files and all 44 supplied
  logos. Separate video resolution from aspect ratio and codec categories,
  fixing the missing logo in MovingPictures details. Add the VideoResolution
  folder to the installer payload.
- Keep the slideshow Properties heading static, with a smaller font and fixed
  bounds inside the dialog. No marquee/fade label is used for the heading.

## Installation and validation

Download the single `Backdrop-Setup-BETA4.exe`, close MediaPortal and run it.
The optional `.sha256` file is a checksum, not an installation dependency.

The source checks and package byte comparisons passed. The supplied XML files
and 44 logos match the attachment exactly; all logos decoded successfully.
The Photos facade matches BETA2 byte-for-byte. See
[build verification](BETA4-BUILD-REPORT.md) for installer checks and scope.

No fresh client visual review or full installer wizard run was performed for
these corrections. MovingPictures logos with actual media metadata and long
slideshow captions still need practical confirmation. Earlier limitations,
including full x86 interaction, recording-icon pagination flashes and Live TV
progress behaviour, remain unchanged. No MediaPortal core/audio workaround or
third-party plugin binaries are included.
