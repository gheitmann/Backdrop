# Backdrop BETA3 for MediaPortal 1.39

Version 1.39.0.30. This beta addresses the BETA2 tester feedback while preserving
the classic Backdrop controls and current layouts.

## Main changes

- Repair stray building fragments in all four Photos background variants and
  sharpen the central detail in the daytime variants.
- Remove oversized background plates behind Photos filmstrip thumbnails while
  retaining the correct image proportions, reflections and focus zoom.
- Rebuild the active radio guide with Backdrop's lower seven-row TV Guide layout,
  radio programme details, original group control and clock.
- Improve the Background Lighting and Artist Info headings and radio list contrast.
- Remove auxiliary pulsing music-menu images while retaining the working Now
  Playing shortcuts. Omit the newer radio channel-change arrows for now.

## Validation and remaining limits

The changed skin files were installed locally. A scoped x64 review confirmed a
populated radio guide with working row navigation and programme descriptions,
and the Photos filmstrip with landscape, portrait and square fixtures. Static
checks, package comparisons and the native Day/Night probe passed. See
[build verification](BETA3-BUILD-REPORT.md) for the precise scope.

Final live confirmation of the heading adjustments, radio list panel and arrow
removal remains open. Full x86 interaction, live radio playback, radio-guide
group/single-channel switching and recording-icon pagination flashes are not
certified by this release. No MediaPortal core/audio workaround or third-party
plugin changes are bundled.

Download the single `Backdrop-Setup-BETA3.exe`, close MediaPortal and run it.
The `.sha256` file is an optional checksum, not an installation dependency.
