# LOCAL18: category backgrounds and dark menus

BasicHome now switches its full-size background as soon as a bottom menu slot or its submenu receives focus. The six stock assignments are photography, recording studio, illuminated Los Angeles theatre entrance, large television close-up, games controller and circuit board. They are local 1920x1080 JPEGs; no scraper, account or network connection is required at runtime. Latest-item fanart remains above this layer when a latest-item card receives focus.

The category import is retained when the menu editor regenerates BasicHome. It follows the six standard Backdrop menu slots and their control-ID ranges. Reassigning a slot to a different topic also requires updating `common.basichome.categories.xml`.

The clock header's full-screen image is suppressed on BasicHome. Its original transparent gradient is restored for other pages, and `time.header.png` is classified as content so the control-artwork generator cannot turn it into a blue button surface again. Category images use full brightness, while menu surfaces provide their own local contrast.

Menu, dropdown and dialog surfaces now use near-black charcoal with thin grey borders, subdued grey focus and a one-unit corner radius in native texture coordinates. Lucide glyphs remain intact. Toggle backgrounds retain distinguishable on/off shades while the native controls draw their actual state markers separately. The music Radio control paints its own focus beneath its label and mark; the two external animated focus images are removed. A scoped pointer handler dispatches one native selection for a complete click on control 18 in music windows 501/504, retaining the required native checkbutton type.

## Validation

- Six provider-rendered category images decode at 1920x1080. Credits, source URLs, license links and SHA256 values are retained in `Media/Modern/ARTWORK-CREDITS.txt` and `Source/Artwork/home-backgrounds.json`.
- General texture validation: 518 outputs decode; seven critical state pairs differ. Restoring the clock gradient removes one incorrectly classified UI asset from the previous 519 outputs.
- Payload validation: 2404 files, 558 skin XML files and AnyCPU editor flags pass. Native audit: 139 windows, zero incompatible bindings; 69 pre-existing missing fields remain.
- All six category backgrounds have been observed in the normal installed MediaPortal x64 instance. The revised Movies image shows the illuminated Tower Theatre entrance and street; TV now uses the large-screen close-up. Darker menus and reduced corner rounding are visible on BasicHome and the music context menu. A single click directly on the Radio state mark in music window 504 opens the native Radio page; keyboard activation also works. Window 501 shares the same XML repair and scoped pointer handler but has not been separately exercised.
- The normal installation plays the retained `Input test 1` audio fixture through BASS. Movie and series plugin initialization completes, and Latest Media Handler publishes seeded movie/music metadata. This does not establish end-to-end playback for every optional plugin.

## Retained local demo

The ordinary local profile was populated from the preserved fictional test-media collection. Its configuration and affected files were backed up before deployment. Existing MediaPortal core binaries were preserved.

The demo includes picture shares, an additional eight-image gallery, three library music tracks plus the retained audio-folder collections, three movies and one series with three episodes. Fanart Handler 4.0.5.442, Latest Media Handler 2.5.0.100, Moving Pictures 1.9.7.249, MP-TVSeries 4.5.1.764 and OnlineVideos 2.9.2 were installed from the retained official packages. Moving Pictures and TVSeries use pre-seeded fictional metadata. OnlineVideos initializes its nine configured sites; service availability and streaming have not been fully tested. TuneIn/RadioTime is not installed in this local profile. The native Radio page opens and shows fixture groups, but its TV plugin logs a pre-existing `RadioHelper.SetRadioProperties` null-reference with no tuned radio station; this skin change does not claim to repair that backend behaviour.

The retained EPG refresh produced 4200 programmes across 16 fictional TV and four fictional radio channels, covering 23Ã¢â‚¬â€œ30 October 2026. These stations have no tuner mappings and do not supply live TV. The maintenance script now explicitly loads the TV Server dependency assemblies before opening its database connection.

Personal profiles, media databases, server credentials, deployment logs and fixture backups remain outside the public skin package. Keep the immutable fixture versions for later ports; extend the collection in a new versioned directory instead of replacing them. The setup distribution remains a single EXE. LOCAL18 is a local preview, not a new published Alpha.

## Reproduction

Run `tools/build-home-backgrounds.py` to fetch the five Pexels photographs and rebuild the category import; Music reuses the licensed studio photograph. Run `tools/build-control-artwork.cjs --menu-only` to regenerate just the menu surfaces, or omit the flag for the complete control-artwork build. Preserve the recorded native dimensions and refresh only the Backdrop cache after replacing textures.
