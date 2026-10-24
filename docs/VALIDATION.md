# ALPHA15 validation

Completed real-install follow-up: INSTALLER-PATH-FIX.md records the corrected starter, owner-confirmed absence of flicker, **2,393/2,393 installed payload files matching**, native plugin destination/uninstall record and registered version 1.39.0.15. This closes the original setup-path defect; broader x86/display coverage remains open.

Follow-up on 24 October: QUICKCHECK-ALPHA15.md records 3/4 scoped checks passed (75%): Radio nonfocused appearance, real isolated copy/progress I/O, and setup VerifyOnly. Latest Music runtime visibility remains blocked. This adds evidence without changing the published ALPHA15 payload.

Target: MediaPortal 1.39 x64, Windows 11, 23 October 2026. Package, setup and AnyCPU editor version 1.39.0.15. Revision 14 was an unpublished local delivery.

## New evidence since ALPHA13

| Scope | Result | Evidence and limits |
| --- | --- | --- |
| Guide background runtime | 3/3 passed (100%) | Complete original room fills the canvas, programme details update/read clearly, populated F9 menu remains readable. Observed on the same skin changes in the local working build at 3440 x 1440; carried into ALPHA15. See UNRELEASED-GUIDE-BACKGROUND.md. Actual Live TV was not tuned locally. |
| Single EXE and filename repaint preparation | 4/4 passed (100%) | Fixed opaque filename viewport, long-to-short rendering, embedded bundle identity, end-to-end VerifyOnly and lone-EXE operation with cleanup. Originally checked in LOCAL14; assembly/package versions are advanced and final ALPHA15 EXE binding/initialization is verified again before upload. See SETUP-EXE-VALIDATION.md. These probes do not perform a real installation. |
| Latest Media caption geometry | 18/18 passed (100%) | Nine captions at 1920 x 1080 and 3440 x 1440, bounded above the home bar. Generator positions, dimensions and explicit rotation pivots match. This is static evidence. |
| Music Radio background override | 2/2 definitions passed | Songs/genre views explicitly suppress the inherited yellow nonfocused texture; native checkbutton 18 and hyperlink 30 remain. Fresh controlled runtime visual confirmation is pending. |

All nine original guide anchors in both active files and the earlier six caption definitions at two output sizes still pass. Native binding audit: 139 windows, zero incompatible bound types, 69 absent fields unchanged. Payload: 2,393 files, 557 skin XML files, AnyCPU flags verified. The native MP package serializer accepts the metadata; packaging reopens all payload files and compares every byte.

## Inherited results and open checks

ALPHA13 caption runtime results 3/3, ALPHA12 local guide 8/9 and historical regression 36/37 are carried forward without full reruns. The owner separately confirmed working upper-right Live TV and channel changes by clicking the EPG; local fixture channels still have no tuner mappings. See VALIDATION-ALPHA13.md, VALIDATION-ALPHA12.md and VALIDATION-ALPHA11.md for exact evidence and topbar/music/plugin limitations. Do not combine these denominators into a compatibility percentage.

Open: fresh runtime confirmation of latest/radio overlays, real installation/elevation and file-copy repainting, complete x86/locale/DPI/aspect-ratio coverage, and deferred Live TV progress irregularities. The exact music Actions command list and some dialog variants remain unverified as described in ALPHA13 validation. TuneIn is owner-confirmed but absent in the isolated test installation.

The installed MpeCore hash remains 124cd3074e92c3dcc53f20f0ec6a1ab7d62eb3b53d5182ae0ae3ec9fa6ba670c. Setup extracts only into its private run directory, uses the guarded local presentation copy, and cleans up after the child exits. No credentials, personal settings, test media or databases are packaged. Normal installed skin files were not overwritten by development checks.
