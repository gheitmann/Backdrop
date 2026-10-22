# ALPHA6 validation

Target: MediaPortal 1.39. Runtime checks used the authorized isolated x64 installation. The menu editor is unchanged at 1.39.0.5, MSIL/AnyCPU; full x86 runtime testing remains pending.

## Latest reported regression areas

| Area | Result | Evidence and limits |
| --- | --- | --- |
| Settings content | Verified for the changed pages | Music Now Playing and GUI/Skin opened in MP. Unsupported DWHD-only choices are absent; native lookup, VU, skin, language, theme, and prefix controls remain. This is not an exhaustive test of every settings page. |
| Topbar previous-track artwork | Verified visually | Existing Backdrop skip-back icon appears in the running topbar; native action 15 and control 22 remain unchanged. Playback skip behavior was not retested in this pass. |
| TV/radio guide styling | Partial | Backdrop time-header graphics/type inspected without channels. Original group textures replace white/overlay textures in XML, but the group control is not shown in the empty-guide test. Populated EPG, tuning, recordings, and reminders remain unverified. |
| TuneIn/RadioTime freeze | Unresolved | Actual plugin version is unknown and the plugin is absent from the isolated runtime. The screenshot is insufficient to identify a deadlock, network stall, or player failure. No TuneIn binary changes are shipped. |
| Artist/album information | Verified for reported empty content | F9 on an artist-only tagged fixture opened the native artist dialog with name, artwork, all metadata fields, biography, and a working albums toggle. F9 on an album-tagged fixture displayed album title, artist, year, tones/styles, and review. Online scraping/refresh was not tested. Album track-list fixture serialization was corrected; its final populated toggle was not visually retested. |

**3 of 5 areas verified (60% of this specific regression checklist), one partially verified, and one unresolved.** This is not an overall compatibility percentage. The earlier ALPHA5 6/7 and historical 84/100 checklists are separate denominators; see [the ALPHA5 record](VALIDATION-ALPHA5.md).

## Static and package checks

- 139 windows audited; zero incompatible control types; 69 absent optional/required fields remain in the local audit.
- 557 shipped skin XML files and 2,393 installer payload files validated with hashes and XML parsing.
- Editor PE flags remain ILOnly/AnyCPU, without Required32Bit or Preferred32Bit.
- Installer packaging verifies every archived payload against its source. Installed MP 1.39 metadata serialization is checked separately before delivery.
- All 62 immutable v1 media fixture hashes remain unchanged. Additional artist-only audio and two synthetic artist/album info records are retained outside v1 and outside GitHub.
- Test application closed normally between runs. Separate local BASS/radio test fixes remain confined to the isolated runtime and are not part of this package. The independent HideVolumeOSD startup warning remains.

## Implementation evidence

Current MP code publishes properties rather than updating the legacy label/image IDs:

- [GUIMusicArtistInfo.cs](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/WindowPlugins/Common.GUIPlugins/Music/GUIMusicArtistInfo.cs)
- [GUIMusicInfo.cs](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/WindowPlugins/Common.GUIPlugins/Music/GUIMusicInfo.cs)
- [GUISettingsMusic_NowPlaying.cs](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/WindowPlugins/GUISettings/MyMusic/GUISettingsMusic_NowPlaying.cs)

The [legacy RadioTime source](https://github.com/yoavain/mediaportal-tunein) contains synchronous network and player calls. That is a diagnostic lead only; it does not establish which plugin build or failure occurred on the reporting installation. A fresh MediaPortal log archive immediately after the freeze, plugin DLL version, and a same-station comparison with DefaultWideHD are still needed.
