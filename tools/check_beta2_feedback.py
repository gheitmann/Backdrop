"""Protect the photo filmstrip and radio-guide regressions reported against BETA2."""
from pathlib import Path
import xml.etree.ElementTree as ET

SKIN = Path(__file__).resolve().parent.parent / 'skin/Backdrop'
tv = ET.parse(SKIN / 'mytvguide.xml').getroot()
for name in ('myradioguide.xml', 'mp139.myradioguide.xml'):
    root = ET.parse(SKIN / name).getroot()
    assert root.findtext('id') == '762'
    assert not list(root.iter('include')), 'DWHD guide includes must not override the active layout'
    for control_id in ('2', '6', '8', '20', '40', '100'):
        original = next(c for c in tv.iter('control') if c.findtext('id') == control_id)
        current = next(c for c in root.iter('control') if c.findtext('id') == control_id)
        assert current.findtext('type') == original.findtext('type')
        for key in ('posX', 'posY', 'width', 'height'):
            assert current.findtext(key) == original.findtext(key), (name, control_id, key)
    assert all('#TV.' not in (n.text or '') for n in root.iter())
    assert 'common.time.xml' in [n.text for n in root.iter('import')]

film = next(c for c in ET.parse(SKIN / 'common.facade.pictures.xml').getroot().iter('control')
            if c.findtext('type') == 'filmstrip')
assert film.findtext('showFrame') == 'no', 'Fixed-size plates must not show behind aspect-fitted photographs'
assert film.findtext('showFolder') == 'no' and film.findtext('keepaspectratio') == 'yes'
assert film.findtext('onright') == '50'
for name in ('RadioPlayingNow.xml', 'mp139.RadioPlayingNow.xml'):
    assert not any(c.findtext('type') == 'button' for c in ET.parse(SKIN / name).getroot().iter('control'))
for name in ('mymusicgenres.xml', 'mymusicsongs.xml', 'mymusicplaylist.xml'):
    controls = list(ET.parse(SKIN / name).getroot().iter('control'))
    assert not any(c.findtext('description') == 'Now Playing Button Animation' for c in controls)
    assert any(c.findtext('id') == '991899' and c.findtext('hyperlink') == '510' for c in controls)
print('PASS: classic lower radio-guide geometry, native guide IDs, clean photo filmstrip and omitted radio channel arrows.')
