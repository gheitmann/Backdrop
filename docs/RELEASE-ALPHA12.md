# Backdrop ALPHA12 for MediaPortal 1.39

Restores the original Backdrop TV guide layout using verified source XML coordinates: seven visible channel rows below, programme information above left, preview area above right, and the original right-aligned date/time. Both active guide definitions stop loading the DWHD 12-row replacement. One guide background and one native preview are retained.

Preserves current MP 1.39 bindings, including button3part group control 100 and preview 99. Widened channel names, header logo placement and a bounded timeline avoid legacy overflow. Radio guide layout is unchanged. The AnyCPU editor remains 1.39.0.11.

Targeted x64 TV guide checklist: 8/9 passed (88.9%); active Live TV preview remains blocked by the test fixture's missing tuner mapping. Populated grid, long descriptions, clock, mouse selection, keyboard navigation, pagination, group switching and single-channel view passed. No duplicate stopped-playback background was observed; active-playback background behavior remains unverified. CI protects nine original coordinate anchors in both active definitions. Native binding audit: zero incompatible types in 139 windows.

Historical checklist remains 36/37 (97.3%), carried forward without a full rerun. This release does not change the separate optional ALPHA11 InstallerUI helper; that ZIP remains available on the ALPHA11 release. Ordinary MPE1 installation does not alter the Extension Installer appearance. Close MediaPortal before installing. Full x86 runtime remains unverified.
