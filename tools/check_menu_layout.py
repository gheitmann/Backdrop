"""Protect menu row framing, native text alignment and plugin padding."""
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
MENU_PAGES = (
    'mymusicsongs.xml', 'mymusicgenres.xml', 'mymusicplaylist.xml',
    'mypics.xml', 'myvideo.xml', 'myvideoTitle.xml', 'myvideoplaylist.xml',
    'mytvrecordedtv.xml', 'myradiorecorded.xml', 'mytvschedulerServer.xml',
    'mytvsearch.xml', 'mytvprogram.xml', 'movingpictures.xml', 'TVSeries.xml',
    'TVSeries.Playlist.xml', 'TVSeries.Actors.xml', 'TVSeries.FanArt.xml',
    'myonlinevideos.xml', 'RadioTime.xml', 'MyRadioLastFM.xml', 'myLyrics.xml',
    'MyMusicPlayingNow.xml', 'MyMusicPlayingNowAnVU.xml',
    'MyMusicPlayingNowLedVU.xml', 'GlobalSearch.Music.xml',
    'GlobalSearch.Music.Details.xml', 'ShowTimesGUI.xml', 'SubCentral.xml',
    '4TR_ProgramInfo.xml', '4TR_RadioGuideSearch.xml', '4TR_RecordedRadio.xml',
    '4TR_RecordedTv.xml', '4TR_RecordedTvInfo.xml', '4TR_TvGuideSearch.xml',
    '4TR_Upcoming.xml', 'infoservice.feeds.xml', 'infoservice.twitter.xml',
    'MusicVids.xml', 'myburner.xml', 'mynumberplace.xml',
    'myonlinevideosUpdater.xml', 'myrss.xml', 'mytetris.xml',
    'mytrailers.index.xml', 'rockstar.xml', 'SleepControl.xml', 'wikipedia.xml',
    'common.settings1.xml',
)
count = 0
for name in MENU_PAGES:
    controls = ET.parse(SKIN / name).getroot().findall('.//control')
    for c in controls:
        if (c.findtext('type') not in ('button', 'menubutton', 'sortbutton', 'checkbutton')
                or int(c.findtext('width', '0')) < (150 if c.get('clrtype') == 'Menu' else 350)
                or c.findtext('font') not in ('font14BC', 'font16BC')):
            continue
        assert c.findtext('textvalign') == 'middle', (name, c.findtext('id'))
        height = '38' if name == 'mynumberplace.xml' else '52' if name == 'common.settings1.xml' else '42'
        assert c.findtext('height') == height
        assert int(c.findtext('textXOff')) >= 16
        assert len(c.findall('textYOff')) == 1 and c.findtext('textYOff') == '0'
        assert len(c.findall('textureFocus')) == len(c.findall('textureNoFocus')) == 1
        if c.findtext('type') == 'sortbutton':
            assert int(c.findtext('offsetSortButtonX')) + 30 + 12 == int(c.findtext('width'))
            assert c.findtext('offsetSortButtonY') == '6'
        assert not any(a.get('effect') == 'zoom' and (a.text or '').lower() in ('focus', 'unfocus')
                       for a in c.findall('animation'))
        for field, texture in (('textureFocus', 'hiddenmenu_button_fo.png'),
                               ('textureNoFocus', 'hiddenmenu_button_no_fo.png')):
            assert c.findtext(field) == texture
            assert (SKIN / 'Media' / texture).is_file()
        assert not any(c.findtext('id') and 'control.hasfocus(' + c.findtext('id') + ')' in image.findtext('visible', '').lower()
                       for image in controls if image.findtext('type') == 'image'
                       and image.findtext('texture') == 'hiddenmenu_button_fo.png')
        count += 1
assert count == 265, count

# Selection controls draw texturebg and use different caption offsets while
# focused. They do not implement native button textvalign. Keep their native
# type and selection handlers; verify both states and stable arrow dimensions.
selector_count = 0
for name, identifiers in (
    ('GlobalSearch.Music.xml', {'5'}),
    ('myonlinevideos.xml', {'5', '6', '7', '9'}),
    ('myonlinevideosUpdater.xml', {'503', '504', '506', '507'}),
    ('mytvsearch.xml', {'7'}),
):
    controls = ET.parse(SKIN / name).getroot().findall('.//control')
    found = set()
    for c in controls:
        if c.findtext('id') not in identifiers:
            continue
        found.add(c.findtext('id'))
        assert c.findtext('type') == 'selectbutton'
        assert c.findtext('height') == '42'
        assert c.findtext('texturebg') == c.findtext('textureFocus') == 'hiddenmenu_button_fo.png'
        assert c.findtext('textureNoFocus') == 'hiddenmenu_button_no_fo.png'
        assert c.findtext('textYOff') == c.findtext('textYOff2') == '8'
        assert int(c.findtext('textXOff')) + 20 == int(c.findtext('textXOff2')) == 36
        for stem in ('textureLeft', 'textureLeftFocus', 'textureRight', 'textureRightFocus'):
            assert c.findtext(stem + 'Width') == c.findtext(stem + 'Height') == '20'
        assert not any(image.findtext('type') == 'image'
                       and image.findtext('texture') == 'hiddenmenu_button_fo.png'
                       and 'control.hasfocus(' + c.findtext('id') + ')' in image.findtext('visible', '').lower()
                       for image in controls)
        selector_count += 1
    assert found == identifiers, (name, found)
assert selector_count == 10

# Fixed-position Sudoku rows have a 40px pitch. Their full framed hit regions
# must not intersect, including the unfocused row beneath the selection.
sudoku = [c for c in ET.parse(SKIN / 'mynumberplace.xml').getroot().findall('.//control')
          if c.findtext('type') in ('button', 'checkbutton') and c.findtext('font') == 'font14BC']
sudoku.sort(key=lambda c: int(c.findtext('posY')))
assert len(sudoku) == 8
for top, bottom in zip(sudoku, sudoku[1:]):
    assert int(top.findtext('posY')) + int(top.findtext('height')) + 2 <= int(bottom.findtext('posY'))
plugin = next(c for c in ET.parse(SKIN / 'myHomePlugIns.xml').getroot().findall('.//control')
              if c.findtext('id') == '50')
assert plugin.findtext('type') == 'menu'  # Retain native plugin routing/binding.
assert int(plugin.findtext('buttonTextXOff')) >= 24
assert int(plugin.findtext('buttonOffset')) == 0
print(f'PASS: {count} menu rows across {len(MENU_PAGES)} pages; {selector_count} native selection rows; non-overlapping Sudoku rows; plugin text inset 24px.')
