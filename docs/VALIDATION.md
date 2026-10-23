# ALPHA12 validation

Target: MediaPortal 1.39 x64, Windows 11, 23 October 2026. Unchanged AnyCPU editor 1.39.0.11. Skin-only update based on ALPHA11.

## Original TV guide restoration

The two active TV guide definitions now use the verified original 1920 x 1080 coordinates. They no longer include the DWHD 12-row replacement. Grid Y=612 and row height=64 produce seven visible channel rows. Programme details are above the grid on the left, the clock is right-aligned at X=1912/Y=6, and the sole native video window 99 occupies X=1537/Y=162, 359 x 197.

Targeted runtime checklist: **8/9 passed (88.9%), one blocked**. This is the following finite TV guide checklist, not overall skin compatibility:

| Check | Result | Evidence |
| --- | --- | --- |
| Populated lower guide | Passed | Seven rows render at the bottom with fictional programmes and original time-header artwork. |
| Upper-left programme details | Passed | Title, time, genre and wrapped long description update between Underwater Cities, Quiet Roads and Orbit Nine without crossing into the grid. |
| Upper-right clock | Passed | Original date/time string renders at the right edge. |
| Mouse programme selection | Passed | Clicking the second-row programme focuses it and updates details. Clicking an already selected current programme invokes native tuning; the expected no-free-card notice occurs because fixtures have no tuner. |
| Keyboard selection | Passed | Down selects the next channel programme and refreshes title, time, genre and description. |
| Channel pagination | Passed | Next advances the channel list beyond the first seven channels, including Summit Sports, Lantern Mystery and Horizon Travel. |
| Group switching | Passed | Group control 100 opens the native selector; choosing Backdrop Test Highlights displays its four channels and updates the vertical group caption. |
| Single-channel schedule | Passed | Selecting Studio One HD opens its chronological seven-row schedule, including programmes after midnight. |
| Active Live TV preview | Blocked | The fictional channels have no tuner mappings. Placement and native control binding pass static checks; actual live video is not runtime-certified. |

Observed at 3440 x 1440 fullscreen using the native skin scaler; no separate 16:9 output or DPI matrix was rerun. Cursor auto-hide can consume the first mouse click. No immediate single-click certification is claimed. The guide shows one original sofa-to-black background when playback is stopped. Duplicate background under active playback remains unverified. Empty guide, channel logos, day/interval changes and other aspect ratios need separate follow-up.

Compatibility adaptations preserve native group button3part 100 and keyed-channel label 34. The preview is inlined once and bound as 99. Channel labels are widened to fit current names; the single-channel logo is moved into the header; timeline height is clipped to the original grid bottom. These are intentional departures from legacy overflow geometry. Radio guide layout is unchanged.

## Structural and delivery checks

`python tools/check_guide_layout.py` checks all nine original anchors in both active definitions, including the imported right-aligned clock, one full-screen background, one preview and modern group binding. CI runs this check to prevent fallback layout drift. Native MP 1.39 binding audit: 139 windows, zero incompatible bound types, 69 absent fields unchanged. Payload validation: 2,393 files, 557 skin XML files and valid AnyCPU editor flags. The native metadata serializer and reopened MPE1 payload are checked separately.

Historical 37-case checklist remains **36/37 passed (97.3%), one blocked**, carried forward without a full rerun. See REGRESSION-ALPHA12.json and VALIDATION-ALPHA11.md for inherited evidence and topbar/music/installer limitations. ALPHA12 does not rebuild the editor or change the separate ALPHA11 installer helper. No fixtures, databases, personal SkinSettings.xml or credentials are packaged.

Only the isolated candidate skin was changed during runtime testing. The test application is closed and its startup screen restored after testing. Normal installed skin files are delivered through the versioned MPE1.
