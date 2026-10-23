# ALPHA7 validation

Target: MediaPortal 1.39 x64, authorized isolated installation, 23 October 2026. The unchanged menu editor is 1.39.0.5, MSIL/AnyCPU. Full x86 runtime testing remains pending.

## Measured scope

**32 of 37 named checks passed (86.5%), four are partial, and one is blocked.** Every case was attempted or assessed. This is a regression checklist, not an overall compatibility score or a certification of every plugin, language, theme, input device, or playback mode. Machine-readable evidence is in [REGRESSION-ALPHA7.json](REGRESSION-ALPHA7.json); the previous reported defects remain documented in [ALPHA6](VALIDATION-ALPHA6.md).

| ID | Check | Result | Evidence and limits |
| --- | --- | --- | --- |
| P01 | Moving Pictures views | partial | List and thumbnail views show all three synthetic movies. The grid now starts below the title; selected first-cover zoom still clips at the left edge. Filmstrip and other layouts were not certified. |
| P02 | Moving Pictures details | passed | Movie summary/details tabs switch by mouse; title, synopsis, score, watched flag, genre and media icons render from synthetic metadata. Blank cast/director match unseeded fixture fields. |
| P03 | Moving Pictures rating | passed | Rate Movie dialog opens; Right changes 3/5 to 4/5 with matching stars/caption. Cancelled without online submission. |
| P04 | TVSeries seasons | passed | With SkipSeasonViewOnSingleSeason disabled in the isolated profile, the Season 1 row, three episode count and series-season title render and open correctly. |
| P05 | TVSeries episode information | passed | After standard Home-to-TVSeries activation and four-second idle delay, episode info shows synthetic plot, thumbnail, aired date and media icon. Empty director/writer/rating are unseeded fixture fields. |
| P06 | TVSeries rating | blocked | The local synthetic series uses offline ID -1; the episode context and Actions menus do not expose a rating command. Online rating/account behavior remains unverified. |
| P07 | OnlineVideos category navigation | passed | OnlineVideos opens language groups, German site list and ARD Live TV/Sendungen A-Z/Programm categories with icons and description. Existing definitions retained; playback and automatic site updates are outside this check. |
| P08 | Sudoku page | passed | New Game populates all nine Sudoku columns while synthetic music continues. Removing the shared now-playing overlay prevents it covering the right-hand cells; title spelling corrected. |
| P09 | Tetris page | passed | Tetris New Game starts at level 1 with visible falling blocks and results. Widened New Game/Sounds captions fit; music overlay no longer covers gameplay. |
| L01 | German UI | passed | German music files, playlist and topbar render during input regression. |
| L02 | English UI | passed | English native Settings labels, date and navigation render correctly; fixture titles retain Unicode. |
| L03 | French UI | passed | French native labels render accents and fit Settings navigation; custom untranslated labels use English fallback. |
| L04 | Russian UI | passed | Russian Settings, date, navigation and translated GuideRows caption render Cyrillic correctly. Long native prefix caption scrolls/clips within its button. |
| L05 | Chinese UI | passed | Simplified Chinese Settings navigation and GuideRows caption render CJK characters using runtime fallback. Untranslated native/custom strings retain English. |
| T01 | Default theme render | passed | Default Backdrop assets render in the isolated MP 1.39 x64 runtime. |
| T02 | Installed theme selector inventory | passed | Theme selector opens and offers only Skin default, matching the absence of additional theme directories. |
| TB01 | Back | passed | Mouse Back returned from the playlist to BasicHome. |
| TB02 | Home | passed | Topbar Home returned from GUI/Skin settings to BasicHome after live language switching. |
| TB03 | Previous track | passed | Topbar Previous moved Input test 3 back to Input test 2, with the corrected skip-back artwork. |
| TB04 | Rewind | passed | Focused Rewind action 17 sets BASS speed -2; displayed position falls from 0:33 to 0:30. Immediate mouse focus and activation are not reliable in this automation session. |
| TB05 | Play | partial | General Play action 68 is bound and starts the selected playlist track, but the playlist handler restarted it at 0:00 instead of resuming at 0:28. Resume behavior on other screens is unverified. |
| TB06 | Pause | passed | Topbar mouse pause held the playback position at 1:01 across later observations. |
| TB07 | Stop | passed | Focused Stop action ends playback and removes the now-playing overlay; the playlist and selected track remain visible. |
| TB08 | Fast forward | passed | Focused Fast forward action 16 produces BASS playback speed 2 and accelerated progress. Mouse focusing and keyboard activation were both exercised; video playback speed remains a separate check. |
| TB09 | Next track | passed | Topbar Next advanced the running multi-track fixture from Input test 2 to Input test 3. |
| TB10 | Fullscreen player toggle | passed | Topbar action 18 opened Music Now Playing from BasicHome during audio playback; Escape returned to the previous navigation screen. |
| TB11 | Exit button | passed | Native action 97 closed the isolated MediaPortal process normally. This is Exit, not operating-system shutdown; no OS power action was invoked. |
| M01 | Photos category mouse area | passed | Clicking the Photos caption focuses its category and opens its submenu. |
| M02 | Music category mouse area | passed | Clicking Music and Radio opens its vertical submenu at the visible caption. |
| M03 | Movies category mouse area | passed | Movies caption opens its category submenu; synthetic movie fanart and title render. |
| M04 | TV category mouse area | passed | TV caption click opens TV guide, recording and series submenu without entering a tuner page. |
| M05 | Games category mouse area | passed | Games and Sports mouse caption opens Sudoku and Tetris submenu. |
| M06 | Plugins category mouse area | passed | Plugins caption opens its vertical submenu at the visible label. |
| M07 | List mouse wheel | passed | Mouse wheel advances the selection and scroll position in the native language list. |
| M08 | Right-click context menu | partial | F9 context menus work. Separate right clicks did not open the movie context menu in this automation session; native first-click/cursor gating remains to be distinguished from plugin behavior. |
| M09 | Progress drag/seek | partial | Now Playing progress renders and advances. Dragging across its bound progress control did not seek in this session; control 118 retains the native required progress type. |
| M10 | List double click | passed | Mouse double-click entered the input fixture folder and started the first track after pointer wake-up. |

## Input limitations

All eleven topbar commands were exercised. Several used mouse focus followed by Enter: immediate pointer movement and click sometimes activated the previously focused button in this automation session. Native cursor wake-up and render/focus timing are possible causes, not an established diagnosis. Therefore this pass does not establish reliable single-click operation for every transport control. Do not modify MediaPortal core merely to conceal this limitation.

Play uses general action 68, rewind 17, and forward 16. The playlist still restarts its selected track when Play is invoked; reliable resume there is unresolved. Right-click menus and mouse progress seeking remain unconfirmed. F9 context menus, category caption hit areas, list double-click and wheel navigation worked. Required progress control 118 is preserved rather than replaced with an incompatible slider.

## Languages and themes

Five representative locales were rendered: German, English, French, Russian and Simplified Chinese. These checks cover observed Settings/music navigation and script rendering, not every page or full translation completeness. All 37 translation XML files are parsed by `tools/audit_integrations.py`; the tool reports missing/empty keys and English-equivalent values per locale. Many legacy strings remain English fallback. The new short GuideRows caption has explicit German, French, Russian and Simplified Chinese text, with English fallback elsewhere.

Only the default theme is shipped; the theme selector correctly lists only Skin default. Additional themes cannot be certified without their assets. Switching from Chinese to English left stale generated glyphs during one restart; archiving the isolated generated font cache allowed correct regeneration. No personal cache is shipped, and the font definitions were not changed.

## Static and package checks

- Payload validator: 557 shipped skin XML files and 2,393 installer payload files, XML parsing and SHA256 checks.
- Binding audit: 139 windows, zero incompatible control types, 69 absent optional/required fields retained for follow-up.
- Installer metadata is separately checked with the installed MediaPortal serializer; package generation verifies every archived payload against its source.
- Editor flags remain ILOnly/AnyCPU, with neither Required32Bit nor Preferred32Bit. No editor rebuild was necessary.
- All 62 immutable v1 fixture hashes were checked unchanged. The retained v3 input fixtures add three synthetic 180-second songs, a playlist and a synthetic video outside v1 and outside GitHub.
- Moving Pictures fixture cover lists were corrected in the isolated database, not in the installer: selected covers must also appear in AlternateCovers. TVSeries season visibility and four-second detail activation were exercised with isolated test configuration.

## Remaining external limits

TuneIn/RadioTime is absent from the isolated installation and its reported version is unknown; its freeze remains unresolved. OnlineVideos category navigation passes, but the earlier ARD HLS playback COM failure is not claimed fixed. Populated TV/radio EPG operations still require configured channels. Online accounts, stream availability, every plugin version, x86 runtime and additional themes remain outside the verified scope. Local BASS/radio/Latest Media Handler experiments are not included in this skin installer.
