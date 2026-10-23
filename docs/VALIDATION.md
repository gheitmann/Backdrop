# ALPHA10 validation

Target: MediaPortal 1.39 x64, 23 October 2026. Skin-only follow-up; the AnyCPU menu editor remains 1.39.0.9.

The existing checklist remains **36/37 passed (97.3%) and one blocked**, carried forward from [ALPHA9](VALIDATION-ALPHA9.md), not rerun in full. This measures the historical finite checklist, not overall compatibility. See [case evidence](REGRESSION-ALPHA10.json). The targeted settings/guide checks are separate.

## Targeted V6 feedback checks

- Viewed all 13 supplied full-size forum screenshots. See the [review matrix](V6-FEEDBACK-REVIEW.md).
- Removed row-count captions from all 20 TV/radio guide variants and both Skin settings definitions. Guide geometry and native bindings are retained.
- Removed inherited General settings controls 8–11 and their shortcut/DriveFreeSpace panels. Native buttons 2–7 remain, with first/last focus wrapping. Removed obsolete visibility conditions so saved legacy flags cannot hide the six buttons.
- Isolated runtime: General displays six options, with Up from the first option focusing the last; Skin settings displays its four options without the guide-count caption.
- Populated 12-row TV guide renders fictional channels, time headings and programme details without the upper-right row-count label. Only the original sofa/room background is visible in this run; the reported second image was not reproduced.
- Binding audit: 139 windows inspected, zero incompatible control types; 69 absent fields remain unchanged.

## Remaining limits

TuneIn playback, station search and virtual keyboard are confirmed by the user's V6 report/screenshots. The plugin is absent locally, so this is not a local runtime certification or a new ALPHA10 plugin fix. Filled music information dialogs and Backdrop guide timeline artwork are also confirmed by the supplied V6 report.

The reported second EPG background remains unconfirmed: current TV guide XML has one opaque full-screen background matching the original Backdrop asset. Do not remove original artwork without reproducing a second layer. Topbar immediate focus/click timing, offline TVSeries rating, ARD HLS playback, full x86 runtime, all locale renderings and additional themes remain limited as documented in ALPHA9.

The normal Program Files editor DLL could not be updated during ALPHA9 because Windows denied the copy. Installing the full package remains necessary there for ALPHA9 pointer seeking; ALPHA10 does not change the DLL.

## Delivery checks

- Validated 2,393 payload files, 557 skin XML files and unchanged AnyCPU editor flags. All installer payload bytes were reopened and compared after packaging.
- Installed MP 1.39 MPE serializer accepts the installer metadata and AnyCPU platform declaration.
- Static inventory: 37 translation files, no missing English fallback properties, no additional themes, 11 topbar command definitions. This is not a fresh runtime check of all languages or actions.
- All 24 changed XML files were backed up and deployed to the normal skin folder; no foreign edits were skipped. Candidate copies were tested in the isolated runtime.
