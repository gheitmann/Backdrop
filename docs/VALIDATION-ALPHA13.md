# ALPHA13 validation

Target: MediaPortal 1.39 x64, Windows 11, 23 October 2026. Skin-only update; AnyCPU editor 1.39.0.11 unchanged.

## Vertical captions

The original rotation used an automatic control centre, while the ALPHA12 guide used a mixed absolute/automatic centre. Left-aligned text then extended below its frame, and the centred guide caption extended above its strip. Local inspection of the installed MP 1.39 Core.dll VisualEffect.Create/SetCenter confirms that `center` is absolute and a zero component falls back to the automatically supplied centre. Nonzero absolute text anchors are required here.

Four dialog definitions now use persistent 90-degree rotation about the lower-left text anchor. DialogMenu uses X=664/Y=730, while DialogSelect, DialogSelect2 and DialogMultiSelect use X=664/Y=775. Each has a 400 x 38 text viewport. The two active TV guides use X=12/Y=1050, width=320, height=34 and left alignment. Long group names scroll after three seconds within that bounded viewport. No group prefix or translation was removed. Grid, channel names, programme cells and their original coordinate anchors are unchanged.

Targeted runtime caption checklist: **3/3 passed (100%)**, limited to these checks:

| Check | Evidence |
| --- | --- |
| Lower-left EPG group caption | The long Backdrop Test Highlights caption remains inside the lower left group strip while scrolling; it no longer overlaps the date selector/time-heading band. The text origin is near the grid bottom. |
| Group selection dialog | Opening native DialogSelect 2000 shows GRUPPE fully inside the left frame strip, above its bottom border. Selection remains populated. |
| Shared action-menu dialog | Guide F9 opens native GUIDialogMenu 2012; the vertical MENÜ title stays inside the frame. The log confirms route 600 -> GUIDialogMenu 2012. This exercises the same DialogMenu.xml/control 4 used by the owner's music Actions dialog, but does not newly certify the music-specific command list or every caption language. |

Runtime observation was at 3440 x 1440 fullscreen. The shared music side-menu and playlist remained accessible, but their F9 paths did not expose the owner's Actions list in this test session; the shared menu was therefore checked through TV guide F9. No recording, tuning, deletion or online metadata action was selected. DialogSelect2 and MultiSelect received the equivalent correction and static containment checks; their runtime variants remain untested.

`python tools/check_guide_layout.py` now checks rotated text viewport containment in the intended left strip for six definitions at 1920 x 1080 and 3440 x 1440: **12/12 static geometry cases passed (100%)**. It also preserves all nine original guide anchors in both active definitions, one guide background, one preview and native group binding. Static geometry is not a second runtime certification.

## Delivery and inherited limits

Native binding audit: 139 windows, zero incompatible bound types, 69 absent fields unchanged. Payload checks cover 2,393 files, 557 skin XML files and AnyCPU editor flags. Native MP metadata serialization and byte-for-byte reopened MPE1 payload are checked during packaging. No test media, databases, personal SkinSettings.xml or credentials are shipped.

The ALPHA12 nine-case TV guide checklist remains 8/9 passed (88.9%), carried forward without a complete rerun; actual Live TV preview remains blocked by missing tuner mappings. The historical 37-case checklist remains 36/37 passed (97.3%), also carried forward. See VALIDATION-ALPHA12.md and VALIDATION-ALPHA11.md for complete inherited evidence and topbar/music/installer limitations. Full x86, locale/DPI/aspect-ratio matrices and active-playback duplicate backgrounds remain unverified. Radio guide captions and other dialog families are outside this update.

The isolated test application is closed and its startup screen restored. Normal installed skin files are delivered through the MPE1; they were not overwritten during this session. The separate optional InstallerUI helper remains ALPHA11.

## Owner follow-up after publication

The owner's subsequent report confirms the intended EPG position, working Live TV in the upper-right preview and channel changes by clicking the guide. These are owner observations on their installation, not new local tuner tests; the precise installed alpha was not restated. Retain the local missing-tuner limitation separately.

The owner reports a progress bar moving backwards in small steps while paused and advancing unevenly during normal Live TV. Investigation was explicitly deferred; no playback or progress implementation is changed here. The optional installer helper has not yet been assessed by the owner.

The reported quarter-screen room background is present in the artwork itself: Media/background.tvguide.png is byte-identical to the retained original package resource (SHA256 e1acee2327d6a8f12953f5e5c51ca451c9a5bac1f81d15dcfde856c336962721). It is a 1920 x 1080 image containing a room in the upper-left area fading to black, and the guide draws it at 0,0 with width1920/height1080. This source inspection does not certify every playback overlay state. No artwork was replaced.

An original mytvguide.xml is not self-contained: it imports common.time.xml and common.overlays.guide.xml. Original group control100 is a plain button; the current native binding requires button3part. The old preview99 block in the main XML is commented out and the visible preview comes from the imported file. Copying or renaming the main XML to an mp139 filename does not adapt these bindings or dependencies. Without the earlier failed run's logs, its exact failure cannot be assigned retroactively.
