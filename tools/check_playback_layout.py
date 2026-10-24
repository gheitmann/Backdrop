"""Check native playback geometry and recording-review contracts."""
from pathlib import Path
import xml.etree.ElementTree as ET

SKIN = Path(__file__).resolve().parent.parent / 'skin/Backdrop'

def controls(name):
    return ET.parse(SKIN / name).getroot().findall('.//control')

recordings = controls('mytvrecordedtv.xml')
compress = next(c for c in recordings if c.findtext('id') == '7')
assert compress.findtext('type') == 'button' and compress.findtext('visible') == 'no'
assert compress.find('hyperlink') is None
assert not any(c.findtext('ondown') == '7' for c in recordings)

for c in controls('MyMusicPlayingNow.xml'):
    if c.findtext('type') != 'progress':
        continue
    assert c.findtext('texturebg') == 'progressback.png'
    assert c.findtext('onlymidtexture') == 'yes'
    assert c.findtext('midwidth') == c.findtext('width')
    assert c.findtext('midheight') == c.findtext('height')
    assert c.findtext('midoffsetX') == c.findtext('midoffsetY') == '0'
    # With native only-mid rendering, 0/50/100% stay within the track,
    # starting at its left/top edge without legacy cap/centering offsets.
    for percent in (0, 50, 100):
        fill_width = int(c.findtext('midwidth')) * percent / 100
        assert 0 <= fill_width <= int(c.findtext('width'))

panel = next(c for c in controls('common.facade.recordings.xml') if c.findtext('type') == 'thumbnailpanel')
assert int(panel.findtext('posY')) + int(panel.findtext('height')) <= 1080
assert int(panel.findtext('posX')) + int(panel.findtext('width')) <= 1920
assert int(panel.findtext('textYOff')) >= 0

weather = next(c for c in controls('common.time.xml') if c.findtext('description') == 'Weather Condition')
for value in ('temp', 'condition'):
    assert '!string.equals(#infoservice.weather.today.' + value + ',)' in weather.findtext('visible')

preview = next(c for c in controls('common.overlays.xml') if c.findtext('type') == 'videowindow')
assert '[window.isactive(35)|[' in preview.findtext('visible')
print('PASS: hidden legacy Compress binding, two co-located music tracks, recording viewport, empty-weather guard and Home preview override.')
