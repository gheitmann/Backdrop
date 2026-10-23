# ALPHA11 validation

Target: MediaPortal 1.39 x64, Windows 11, 23 October 2026. AnyCPU menu editor 1.39.0.11.

Historical checklist: **36/37 passed (97.3%), one blocked**, carried forward from ALPHA10/ALPHA9, not rerun in full. It is not an overall compatibility percentage. See REGRESSION-ALPHA11.json and VALIDATION-ALPHA10.md. New evidence below has a separate scope.

## Topbar pointer handling

The plugin observes its own application's left-button messages, hit-tests the native visible enabled topbar button, and dispatches that button's existing action on release over the same button/window/receiver. Both halves are consumed to avoid duplicate native clicks. Routed dialogs, other skins, fullscreen video, disabled/hidden topbars, inactive forms and cancelled gestures are excluded. Hidden-bar wake-up still uses native behavior.

Six command cases passed in the isolated runtime during this development session: Home (26), Pause (25), Resume (24), Next (29), Previous (22), and player/Now Playing (30). Pause held position at 0:50, Play resumed without restarting; Next/Previous changed the synthetic track exactly once. Each produced one pointer activation entry. The final rebuild adds action/speed/position logging, restricts input to the MediaPortalApp renderer form and clears pending input on application deactivation. These six runtime cases were checked before those final guard/logging additions; they are not a fresh runtime certification of the final binary.

Forward/Rewind produced the intended native control activation in the initial run but speed/result verification remains partial. Do not count this as a complete transport pass. Later repeated clicks after the bar/cursor had auto-hidden did not consistently produce new activations. Final binary smoke checks additionally passed Home (one control 26/action 115 activation), Exit (one control 31/action 97 activation and clean process termination), and cross-button Home-to-Exit drag cancellation (no activation or exit). The command matrix therefore has 7/11 passed (63.6%), two partial (Forward/Rewind), two untested (Back/Stop); evidence before/after final guards is distinguished above. Modal guard behavior still needs targeted runtime certification. No claim that all 11 commands or every mouse action is fixed. The plugin retains native action IDs rather than implementing a second playback engine.

## Music information

Button 5 remains the native toggle target, with two mutually exclusive translated label controls following the same skin-setting visibility conditions as the body. A label expression using skin.hassetting did not refresh after toggling and was replaced before release. Artist-only fixture: F9 information dialog displays seeded metadata, Unicode text and long biography; switching changes biography to discography and caption Alben to Bio. without clipping. Album uses the equivalent structure for Tracks/Review; a fresh final-layout album runtime check remains pending. Native refresh button 6 remains unchanged. No online metadata refresh was triggered.

## Installer presentation

Separate optional helper; **not part of the MPE1 payload**. The installed MpeCore remains unchanged (SHA256 124cd3074e92c3dcc53f20f0ec6a1ab7d62eb3b53d5182ae0ae3ec9fa6ba670c). Original installation/dependency/registration methods are not patched.

- Native preview visually shows readable standard Windows buttons.
- Three of three button style checks pass; form double buffering is enabled.
- All 2,393/2,393 synthetic native file events are preserved: **100% event accounting**. The short burst performs two targeted progress repaints and flushes completion. This is not an installation-speed measurement.
- Assembly binding probe confirms the original MpeInstaller binds to isolated MpeCore without invoking its entry point.
- Built ZIP was extracted and Initialize.ps1 successfully reproduced the local patch and both probes. No MediaPortal binaries are distributed in that ZIP.
- Full real package installation through the helper, elevation behavior, other Windows themes/DPI and x86 are unverified. Ordinary MPE installation remains available. See INSTALLER-UI.md.

## Structural checks and remaining work

139 native windows audited: zero incompatible bound control types, 69 absent fields unchanged. Pointer seek geometry: 12/12 passed. No fixtures, databases, personal SkinSettings.xml or credentials are packaged. Payload and metadata are validated separately at packaging.

The second EPG background remains unconfirmed; ALPHA10 populated TV guide evidence is retained. TuneIn is user-confirmed on V6 but absent in this isolated runtime. Offline TVSeries rating, ARD HLS, full x86, all locale renderings and additional themes retain their earlier limits. Only the default theme exists. Existing 37 translation files are statically audited; they are not newly runtime-certified.

Normal installed skin/binaries were not overwritten during the owner's V10 installation. The candidate skin and plugin were staged only in the isolated test runtime. Temporary test profile input overrides are restored after testing.

Delivery checks: validated 2,393 payload files and 557 skin XML files, AnyCPU flags, the installed MP 1.39 metadata serializer and byte-for-byte reopened installer payload. Translation inventory: 37 files, no missing English fallback properties.
