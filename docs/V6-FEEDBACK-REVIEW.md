# V6 feedback review

Reviewed on 23 October 2026 against the current ALPHA9-based worktree, then addressed in ALPHA10. All 13 full-size images were visible in the in-app browser. User-reported successes are distinguished from local tests.

| Report | Finding and action |
| --- | --- |
| TuneIn, playback/search/keyboard (images 1–4) | User confirms working in V6; screenshots show playback, station results and the virtual keyboard. Retain existing skin integration. No local plugin version or runtime test is available. |
| Filled album/artist information (5–6) | Existing fix confirmed by the user; album review and tracks are populated. No replacement with DWHD dialogs. |
| Guide timeline (7–8) | Existing Backdrop artwork confirmed. Retain it. |
| Guide row count (7–8) | Remove the top-right row-count label in all five row layouts for TV/radio, including mp139 variants (20 files). |
| Two guide backgrounds (7–8) | Not reproduced or confirmed in the current XML. The single opaque background.tvguide.png already contains the sofa/room fading to black and matches the original asset. Leave this item open pending reproduction. |
| Music Now Playing settings (9) | Right-side native settings are shown correctly. Keep the vertical MP 1.39 settings categories on the left; these provide current window navigation. |
| Skin settings final EPG line (10) | Remove the static guide-row caption from both page definitions. Retain the hidden native binding required by MP. |
| General extras and follow-up panels (11–13) | Remove Keyboard Shortcuts, DriveFreeSpace and their help/enlarge panels. Keep the six native General options and correct cyclic focus navigation. Clear obsolete visibility dependencies, including when saved flags remain enabled. |

Source images: [1](https://forum.team-mediaportal.com/download/2026/10/351632_1.png), [2](https://forum.team-mediaportal.com/download/2026/10/351633_2.png), [3](https://forum.team-mediaportal.com/download/2026/10/351634_3.png), [4](https://forum.team-mediaportal.com/download/2026/10/351636_4.png), [5](https://forum.team-mediaportal.com/download/2026/10/351639_5.png), [6](https://forum.team-mediaportal.com/download/2026/10/351640_6.png), [7](https://forum.team-mediaportal.com/download/2026/10/351643_7.png), [8](https://forum.team-mediaportal.com/download/2026/10/351644_8.png), [9](https://forum.team-mediaportal.com/download/2026/10/351647_9.png), [10](https://forum.team-mediaportal.com/download/2026/10/351648_10.png), [11](https://forum.team-mediaportal.com/download/2026/10/351650_11.png), [12](https://forum.team-mediaportal.com/download/2026/10/351653_12.png), [13](https://forum.team-mediaportal.com/download/2026/10/351654_13.png).
