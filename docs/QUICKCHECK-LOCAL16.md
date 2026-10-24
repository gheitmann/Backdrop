# LOCAL16 integration quick check — 24 October 2026

Working source only; published ALPHA15 and its InstallerFix asset are unchanged. Isolated MP 1.39 x64, German application language, default Backdrop theme, 3440 x 1440. Existing synthetic media remain available for future ports.

## Fresh runtime scope

**5 of 6 scoped cases passed (83.3%); one incomplete.** This is not overall compatibility or a full plugin/transport certification.

| Case | Result | Observed evidence |
| --- | --- | --- |
| Lower BasicHome navigation | Passed | Mouse selects Music & Radio and Plugins at their visible bottom captions; their submenus appear. Music opens the vertical native menu. A cursor wake-up click can focus rather than activate; keyboard Enter was used after focus where needed. Immediate one-click behavior after auto-hide is not certified. |
| Sudoku | Passed | Opened from Plugins, New Game populates the grid; German command labels remain within the menu and music overlays do not cover the board. |
| Tetris | Passed | Opened from Plugins, New Game shows falling pieces and updates score/level; Escape pauses and subsequently returns to Plugins. No music overlay covers gameplay. |
| Topbar Back | Passed | Revealed bar in Sudoku and clicked Back. Returned to Plugins. Exactly one pointer activation log: control 21, window 7777, action 10. This is fresh evidence for the working binary. |
| Moving Pictures | Passed | Opened through Plugins and All Movies; three retained synthetic titles populate the list. Details display title, year, rating, summary and media flags. List/detail smoke test only; thumbnail/filmstrip modes were not rerun. |
| Topbar Stop during playback | Incomplete | The retained 20-second movie starts according to the native player log, but the quick session did not establish a topbar Stop activation and resulting playback state. No pass claimed. |

Screens were inspected through the Computer Use tool. Intermediate animation frames were allowed to settle before assessing layout. No online metadata update, plugin installation or normal installation was performed. The isolated session was closed after the checks; startup is restored to BasicHome (35).

## Static checks and correction

- Added the missing Danish `HighFive` credit key using the existing English fallback wording; no claim of a new Danish translation.
- Translation inventory covers nested skin XML as well as root pages. All 37 locale files now contain all 50 referenced keys, with no empty values or duplicate keys. Many values intentionally remain English; this is structural coverage, not translation completeness or font rendering certification.
- The integration audit now checks all **11/11 topbar commands (100%)** for exact single action mappings, left/right navigation around the complete bar and both focused/unfocused artwork files. Dynamic Resume/Play is retained. This is static evidence only.
- Fresh isolated-runtime native binding audit: **139 windows**, zero incompatible bound control types; **69 absent optional/required fields** unchanged. The separate normal client inventory contains 125 windows and 57 absent fields because fewer plugins are installed; these denominators must not be mixed.
- Pointer seek geometry: **12/12 passed (100%)**. This does not certify runtime drag seeking.
- Payload validation: **2,393 files**, 557 skin XML files, AnyCPU editor flags. Original guide and rotated-caption containment checks pass.

## Remaining scope

Topbar Stop, Forward/Rewind playback outcomes, modal guards, immediate pointer timing, further optional plugins/dialog variants, x86 runtime and full locale/DPI/aspect-ratio rendering remain open. TuneIn is owner-confirmed and absent locally. Only the default theme is supplied; there are no additional themes to test. Live TV progress behavior remains deferred. Latest Music focused-caption runtime evidence is in LATEST-MUSIC-DETAILS.md; ALPHA15 installer completion is in INSTALLER-PATH-FIX.md. Neither is still blocked as suggested by older historical reports.
