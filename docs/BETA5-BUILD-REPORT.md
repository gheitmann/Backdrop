# BETA5 build verification — 3 November 2026

Version **1.39.0.32**, tag `v1.39.0-beta.5`. Includes [BETA4 feedback follow-up](BETA4-FEEDBACK-FOLLOWUP.md) and the final Settings overlap/title correction. Editor, Day/Night plugin and installer helper binaries are unchanged; the single EXE is rebuilt with the reviewed payload and new version.

- Eight Python checks passed: payload, BETA1/BETA2/BETA3 feedback, menus, secondary layout, Day/Night and Settings fidelity. Final payload validation covers 2,444 files and 561 skin XML definitions. The Settings check verifies all 110 native binding contracts, active textures and the tester slideshow hash.
- Native x64 Day/Night probe passed 1,476 assertions for 50 expressions in 41 XML files during this feedback pass. No plugin binary changed.
- Final MPE1 reopened and all 2,444/2,444 payload files byte-compared. Private setup bundle passed 21/21 entry comparisons and excludes MediaPortal core binaries.
- Final EXE VerifyOnly passed on x64 and isolated x86 (2/2 architectures), without installation. Reports: project outputs `Backdrop-BETA5/setup-x64.txt` and `setup-x86.txt`.
- Focused live x64 review confirmed the root Settings title, Pictures page and Pictures–Slideshow. All displayed rows fit inside the dark panels; no stretched dialog frame crossed headings or bottom controls. The upper-right Backdrop clock remained visible. No settings values were changed. The separate review profile preserved the main Apollo configuration. The client exited normally through Alt+F4.
- An unsupported window visibility expression discovered during review was replaced by separate native visible labels and rechecked in the client. The final package contains the corrected labels.
- Local deployment copied and hash-checked 89 skin files, with backup at project outputs `Runtime-review-2d1d7f8b`; its shared Settings XML was then updated for the final native title correction.

Final EXE: **169,006,080 bytes**. SHA256:
`7c770f03b952acce76aabb2dcb809f6b86b72264a100738c597370d529ec3830`.

No full installer wizard run, fresh slideshow playback, every-Settings-page certification or full x86 client review is claimed. Existing recording pagination and Live TV progress limitations remain. No MediaPortal core or separate audio workaround is bundled.
