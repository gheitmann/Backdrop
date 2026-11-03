# Backdrop BETA5 for MediaPortal 1.39

Version **1.39.0.32** incorporates the BETA4 tester feedback.

## Main changes

- Adopt the supplied slideshow XML unchanged. Picture titles use a non-scrolling textbox, separated from the item count.
- Audit all Settings definitions and restore Backdrop clock placement, original list selection graphics, paging arrows, white button text and blue section headings. Preserve current MediaPortal controls and navigation rather than reverting to incompatible historical window IDs.
- Remove stretched dialog frames that crossed Settings headings and bottom rows. Use the existing Backdrop dark panels.
- Correct the Settings root heading so returning from a subpage cannot leave its previous title behind. Use native visibility controls for compatibility with the skin expression engine.
- Restyle extension settings with a scoped Backdrop facade and the existing four-period Settings background. Correct missing legacy Settings texture references.

## Installation and verification

Download **Backdrop-Setup-BETA5.exe**, close MediaPortal and run the single executable. The optional SHA256 file is not required for installation.

All 110 Settings binding contracts passed. Eight Python checks passed; the package was reopened and all 2,444 payload files compared. Final EXE preparation checks passed for x64 and isolated x86, without installation. A focused live x64 review confirmed the corrected root Settings, Pictures and Pictures–Slideshow pages, including the bottom rows and upper-right clock. The test client closed normally.

See [build verification](BETA5-BUILD-REPORT.md) for scope. Not every Settings page or device-specific option was exercised. Full x86 interaction, recording-icon pagination flashes and Live TV progress behaviour remain outside this update. No MediaPortal core/audio fix or third-party plugin binary is included.
