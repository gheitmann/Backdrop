# BETA4 feedback follow-up â€” 3 November 2026

Source/local follow-up included in BETA5. The published BETA4 release is unchanged. See BETA5-BUILD-REPORT.md for the final title correction and completed client review.

## Slideshow

Replace `slideshow.xml` byte-for-byte with the tester's file from
`352122_slideshow.7z`. It uses a native non-scrolling textbox bound to
`#selecteditem`, retains the item count on the right, and leaves a 100-pixel
horizontal gap between their regions. No generated artwork is involved.

## Complete Settings inventory

Inspect all 109 existing root XML definitions whose names contain Settings,
including compatibility aliases, shared imports and plugin settings. Compare
them against the tester-supplied Backdrop v1.0.1 MPE1. Add one scoped extension
facade, yielding 110 audited definitions. There are 34 namesake historical
matches among these 110 definitions, including aliases; this is not 34 distinct
historical windows.

The machine-readable inventory in `Source/Artwork/settings-originality-audit.json`
records original/current window IDs, changed status, retained foreign resources
and native binding signatures. Old Settings pages often use different window IDs
from their current MediaPortal counterparts, so wholesale historical XML copying
would lose current functionality. Preserve every native window/default-control,
control type/ID, command, hyperlink and directional navigation contract. All 110
binding signatures are checked against their preceding implementation.

- Replace the DWHD clock/weather imports with Backdrop's upper-right clock.
- Restore Backdrop list selection textures and original paging arrows. Remove
  the foreign hand-shaped list-navigation indicators.
- Use the original white unfocused Settings button text and blue section headings.
- Restyle the extension settings page and a scoped copy of its facade, preserving
  native list/control layout without changing the shared DWHD fallback facade.
  The extension settings background now participates in all four lighting periods.
- Replace missing UI-calibration images with Backdrop's original
  `settingsUICalibration.png` and an existing dark backing. Correct missing legacy
  Settings overlays and TVCropSettings dialog/close-button image references.
- Retain the existing Backdrop dark panel graphics. A stretched dialog-frame
  experiment was rejected during live review because it crossed headings and rows.
- Deliberately retain the Blu-ray region diagram, which conveys functional
  information with no historical replacement, and 22 explicitly hidden skin
  preview resources in each of the two skin-settings aliases. Do not expose the
  unsupported historical DWHD layout choices.

## Verification

- Eight Python checks passed: payload validation, BETA1/BETA2/BETA3 feedback,
  menu layout, secondary layout, Day/Night and the complete Settings contract check.
  Final payload validation covers 2,444 mapped files and 561 skin XML definitions.
- The focused check verifies all 110 native binding signatures, active texture
  references, the foreign-resource exceptions, the supplied slideshow hash and
  separation of title/item-count regions.
- Native x64 Day/Night probe passed 1,476 assertions for 50 expressions in 41 XML
  files. The independent plugin binary is unchanged. Inherited log4net warnings
  did not fail assertions.
- Scoped live x64 review opened Settings, Pictures and Picturesâ€“Slideshow through
  the actual BasicHome menu. Navigation worked, the original selection graphic
  appeared and the clock was at the upper right. No Settings values were changed.
  The final dark-panel restoration reuses the preceding Backdrop graphics; this
  review does not certify every Settings page or device-specific option.
- Backed up, copied and byte-compared 89 local skin files. Final receipt: project
  outputs `Runtime-review-d054b7c5/deployment.json`. The separate review profile was
  used; the main Apollo configuration was preserved. The client was left running
  after Computer Use detected user input. Reopening Settings/restarting the client
  is needed to load the final background-file revision into already loaded windows.
- The internal review MPE1 was reopened and all 2,444 mapped files byte-compared.

No full x86 client review, every-Settings-page visual certification or fresh
slideshow playback is claimed. The supplied slideshow is adopted as requested.
No MediaPortal core, audio workaround or third-party plugin binary is included,
and BETA5-BUILD-REPORT.md records the subsequent EXE build and release.
