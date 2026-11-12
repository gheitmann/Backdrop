# Current validation and historical evidence

1.39.0.37 (12 November 2026): fixes the menu editor refresh so it runs window and control updates on MediaPortal's GUI thread, and replaces the installer welcome text. The rebuilt editor was checked in the isolated client: BasicHome navigation with the refresh timer active produced no `BackdropMenuEditor` thread errors; the remaining `wrong thread` entries came from the external LatestMediaHandler plugin. The published setup was then installed on the real client through the native wizard: 2,443 of 2,444 payload entries match the repository byte for byte, the only difference being the personal `%Config%\BackdropMenuEditor.xml`; MPE registers 1.39.0.37 and the installed editor and skin report 1.39.0.37. Twelve Python checks pass, packaging verifies 2,444/2,444 payload files, and the setup VerifyOnly probes pass. No complete x86 coverage is claimed for this release; see [release notes](RELEASE-1.39.0.37.md).

x86 check (12 November 2026): a portable x86 MediaPortal 1.39 client rendered the Backdrop BasicHome with its rotating category backgrounds, submenus and the My Videos share list. The AnyCPU menu editor 1.39.0.37 loaded there, and stress navigation produced no thread or texture errors. The only log errors were the first-start cache cleanup and an empty RSS feed in the fixture configuration. The setup's VerifyOnly probes also passed against that x86 client: presentation 2,393/2,393 events, native binding and a plugin destination inside the x86 tree.

1.39.0.36 (12 November 2026): package version advanced after merging the 1.39.0.35 tester work into `main`. Twelve Python checks pass, packaging verifies 2,444/2,444 payload files, and a scoped start of the isolated client copy reported no texture-loading errors. The AnyCPU menu editor and the optional Day/Night plugin were rebuilt from the merged sources. No full runtime, installer wizard or x86 review is claimed for this release; see [release notes](RELEASE-1.39.0.36.md).

Installation and runtime check (12 November 2026, same package): the published setup was installed through the patched native installer, wizard welcome page to completion page. All 2,444 payload entries were verified afterwards: 2,443 match the repository byte for byte; the only difference is the personal `%Config%\BackdropMenuEditor.xml`, which the installer correctly left untouched. The installed skin (4,874 file entries) reports 1.39.0.36, the AnyCPU menu editor 1.39.0.36 and the Day/Night plugin 0.2.0.0. An earlier unelevated trial used a temporary directory override and a moved extension registration, both restored before the elevated run. The isolated client started BasicHome, the Latest Movies panel and Moving Pictures without texture-loading errors.

Findings from this check, fixed in 1.39.0.37: `BackdropMenuEditor.UpdateImageTimer` called `GUIWindowManager.GetWindow` from a timer thread; MediaPortal then logged `LoadSkin: Running on wrong thread`, skipped the load and raised an `ArgumentOutOfRangeException` in a `GUIMultiImage` allocation while another window loaded. The installer welcome page also showed "Experimental MP 1.39 AnyCPU test build. Read README-DE.md ..." from the package template.

Current working tree (12 November 2026, package version 1.39.0.34): static-only review of the imported 1.39.0.34 tree. Twelve Python checks pass; `tools/check_skin_references.py` resolves every static texture, mask, font, import and MediaPortal 1.39 window ID against the shipped files, and packaging reproduces 2,444/2,444 payload files. No MediaPortal runtime review is claimed for these changes, and the BETA5 runtime evidence below still describes the 1.39.0.32 build.

Runtime spot check of the same working tree (12 November 2026): the eleven reviewed skin files were deployed to the installed Backdrop skin and MediaPortal 1.39 x64 was started with a copied configuration (`/config`, skin set to Backdrop, `/windowed`). The classic Home window, the video shares root, a populated synthetic share folder and a movie detail page rendered correctly, MediaPortal exited normally, and `MediaPortal.log` contained no texture-loading errors and no error entries. `gui/startbasichome=yes` in the copied configuration did not activate BasicHome, so the screenshots show the classic Home window. Not exercised: TV no-signal, file-dialog progress, cover-art grabber progress and SubCentral search. The installed skin was restored byte-identically afterwards.

The runtime check also showed that `common.facade.video.Title.xml`, `common.facade.video.shares.xml` and `myvideo.facade.fanartwall.xml` (and their `mp139.` twins) are unused templates: they contain a window fragment without a window id and no skin file imports them, so MediaPortal never loads them. Correcting their texture paths removes broken static references but has no runtime effect; the active video list is defined in `common.facade.video.xml`.

BETA5 (3 November 2026): final Settings root/Pictures/Slideshow live review passed; overlapping frames removed and native titles verified. All 110 Settings contracts, 2,444/2,444 package files and 21/21 bundle entries checked. Final EXE VerifyOnly passed x64 and isolated x86 (2/2 architectures). See [build verification](BETA5-BUILD-REPORT.md).

BETA4 feedback follow-up (included in BETA5) (3 November 2026): all 110 Settings native
binding contracts checked, supplied slideshow adopted, eight Python checks passed
and native x64 Day/Night probe passed 1,476 assertions for 50 expressions. Scoped
live Settings navigation and clock review completed. See
[feedback verification](BETA4-FEEDBACK-FOLLOWUP.md) for final revision and runtime limits.

BETA4 (3 November 2026): 2,443/2,443 package files and 21/21 private bundle
entries matched. Final single EXE VerifyOnly passed for x64 and x86 (2/2
architectures), without installation. See [build verification](BETA4-BUILD-REPORT.md).

BETA3 tester follow-up (3 November 2026; included in BETA4): previous Photos filmstrip
restored, supplied MovingPictures XML/logo categories adopted and slideshow
title bounded without marquee. Seven static commands passed; 44/44 logos
decoded and matched the archive. See [feedback details](BETA3-FEEDBACK-FOLLOWUP.md).

BETA3 (30 October 2026): 2,442/2,442 package files and 21/21 private bundle
entries matched. Final single EXE VerifyOnly passed on x64 and x86 (2/2
architectures). See [BETA3 build verification](BETA3-BUILD-REPORT.md) and its
explicit live-review limits.

Unreleased BETA2 follow-up (30 October 2026): eight Python commands and the
native x64 Day/Night probe (1,448 assertions, 48 bindings) passed. Scoped live
review confirmed populated radio-guide navigation and the Photos filmstrip.
See [BETA2 feedback follow-up](BETA2-FEEDBACK-FOLLOWUP.md) for limits; this is
not a new release or full runtime certification.

BETA2 (29 October 2026): eight static checks, 2,442/2,442 package and isolated
native-copy comparisons, 21/21 setup bundle entries and final EXE VerifyOnly
on 2/2 architectures passed. See [BETA2 build report](BETA2-BUILD-REPORT.md).

Unreleased BETA1 source/local feedback follow-up (28 October 2026): 76 original
control textures restored; eight Python checks and the artwork decoder passed.
Native x64 Day/Night probe passed 1,420 assertions for 46 bindings; 92 local skin
files were backed up, copied and hash-compared. Scoped live review covered music focus navigation, Photos and radio layouts.
Final x64 visual confirmation passed for keyboard captions and nonduplicated radio icons. See
[Beta 1 feedback follow-up](BETA1-FEEDBACK-FOLLOWUP.md) for scopes and open checks.

BETA1 (28 October 2026): scoped local x64 UI review, native package update,
2,441 matching installed payload files plus one preserved personal configuration,
and final EXE VerifyOnly on x64/x86. See [BETA1 build report](BETA1-BUILD-REPORT.md)
for exact scopes and remaining limitations.

ALPHA27 (26 October 2026): four-period backgrounds and an independent optional
plugin. Seven static checks passed (7/7), native integration probes each passed
1,364/1,364 assertions on x86/x64, and independent plugin probes each passed
2,201/2,201. Final package verified 2,442/2,442 files; final EXE VerifyOnly passed
2/2 architectures without installation. Earlier local x64 interaction is carried
forward. See [ALPHA27 build report](ALPHA27-BUILD-REPORT.md).

ALPHA26 (26 October 2026): three thematic Home/Settings/Plugins backgrounds are mapped
across eight XML definitions. Six selected static commands passed (6/6); 2,408
package files and 21 private bundle files were byte-compared. The single EXE
passed VerifyOnly preparation on x64 and x86 (2/2). Live visual confirmation
and full x86 runtime coverage remain open. See
[ALPHA26 build verification](ALPHA26-BUILD-REPORT.md).

Earlier source/local follow-up (26 October 2026): see
[ALPHA25 artwork feedback](ALPHA25-FEEDBACK-FOLLOWUP.md). The reviewed changes
restore 51 original textures while preserving cinematic BasicHome artwork.
Six static commands passed (6/6); 518 textures decoded and 419 original hashes
and dimensions checked. Local deployment verified 53/53 copied files. Live
visual confirmation, recording pagination flash and full x86 runtime coverage
remain open. These reviewed changes are included in ALPHA26.
Scheduled maintenance also revalidated and byte-compared all 2405 package files;
see [26 October maintenance](MAINTENANCE-2026-10-26.md).

The entries below are historical evidence, not the current release identity.

# ALPHA15 validation

LOCAL17 visual follow-up: `LUCIDE-REFRESH.md` records 550 generated UI textures, modern stock backgrounds, static TV group caption, full-height selection rows and native checked/unchecked visual confirmation. Payload validation covers 2,398 files and 557 XML definitions; the single EXE passes VerifyOnly. These working changes do not replace the published ALPHA15 release.

Current working follow-up: QUICKCHECK-LOCAL16.md records 5/6 fresh runtime cases passed (83.3%), complete static 11-command topbar coverage, a repaired Danish fallback key and the remaining transport/locale/x86 limits. LATEST-MUSIC-DETAILS.md supersedes the earlier Latest Music fixture blocker with 2/2 runtime layout passes. Published ALPHA15 remains unchanged.

Completed real-install follow-up: INSTALLER-PATH-FIX.md records the corrected starter, owner-confirmed absence of flicker, **2,393/2,393 installed payload files matching**, native plugin destination/uninstall record and registered version 1.39.0.15. This closes the original setup-path defect; broader x86/display coverage remains open.

Historical follow-up on 24 October: QUICKCHECK-ALPHA15.md recorded 3/4 scoped checks passed (75%): Radio nonfocused appearance, real isolated copy/progress I/O, and setup VerifyOnly. Its Latest Music fixture blocker was subsequently resolved as documented above. Published ALPHA15 payload bytes are unchanged.

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

Open: complete topbar transport outcomes and immediate pointer/modal behavior, broader plugin/dialog coverage, complete x86/locale/DPI/aspect-ratio rendering, and deferred Live TV progress irregularities. Latest/radio overlay checks and the real installer follow-up have since been completed within their documented scopes. The exact music Actions command list and some dialog variants remain unverified as described in ALPHA13 validation. TuneIn is owner-confirmed but absent in the isolated test installation.

The installed MpeCore hash remains 124cd3074e92c3dcc53f20f0ec6a1ab7d62eb3b53d5182ae0ae3ec9fa6ba670c. Setup extracts only into its private run directory, uses the guarded local presentation copy, and cleans up after the child exits. No credentials, personal settings, test media or databases are packaged. Normal installed skin files were not overwritten by development checks.
