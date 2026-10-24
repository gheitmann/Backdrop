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
)
count = 0
for name in MENU_PAGES:
    controls = ET.parse(SKIN / name).getroot().findall('.//control')
    for c in controls:
        if (c.findtext('type') not in ('button', 'menubutton', 'sortbutton', 'checkbutton')
                or int(c.findtext('width', '0')) < 350
                or c.findtext('font') not in ('font14BC', 'font16BC')):
            continue
        assert c.findtext('textvalign') == 'middle', (name, c.findtext('id'))
        assert c.findtext('height') == '42'
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
        assert not any(c.findtext('id') and 'Control.HasFocus(' + c.findtext('id') + ')' in image.findtext('visible', '')
                       for image in controls if image.findtext('type') == 'image'
                       and image.findtext('texture') == 'hiddenmenu_button_fo.png')
        count += 1
assert count == 184, count
plugin = next(c for c in ET.parse(SKIN / 'myHomePlugIns.xml').getroot().findall('.//control')
              if c.findtext('id') == '50')
assert plugin.findtext('type') == 'menu'  # Retain native plugin routing/binding.
assert int(plugin.findtext('buttonTextXOff')) >= 24
assert int(plugin.findtext('buttonOffset')) == 0
print(f'PASS: {count} menu rows across {len(MENU_PAGES)} pages with native centering and consistent frames; plugin text inset 24px.')
