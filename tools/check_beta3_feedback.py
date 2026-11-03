"""Guard corrected MovingPictures logo categories and a bounded static photo title."""
from pathlib import Path
import hashlib
import json
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
logos = json.loads((ROOT / 'Source/Artwork/tester-logo-corrections.json').read_text())['files']
for name, record in logos.items():
    assert hashlib.sha256((SKIN / name).read_bytes()).hexdigest() == record['sha256'], name
for name in ('movingpictures.xml', 'movingpictures.details.xml'):
    textures = [n.text or '' for n in ET.parse(SKIN / name).getroot().iter('texture')]
    resolution = [t for t in textures if '#MovingPictures.LocalMedia.videoresolution' in t]
    assert resolution and all('Logos\\VideoResolution\\' in t for t in resolution), name
    assert any('Logos\\VideoAspectRatio\\' in t for t in textures), name
for name in ('1.33.png', '1.78.png', 'fullscreen.png', 'widescreen.png'):
    assert (SKIN / 'Media/Logos/VideoAspectRatio' / name).is_file()
assert (SKIN / 'Media/Logos/VideoCodecs/mpg4.png').is_file()
assert len(list((SKIN / 'Media/Logos/VideoResolution').glob('*.png'))) == 18
title = next(c for c in ET.parse(SKIN / 'DialogPictureInfo.xml').getroot().iter('control')
             if c.findtext('id') == '20')
assert title.findtext('type') == 'label'
assert len(title.findall('font')) == 1 and title.findtext('font') == 'font16BC'
assert int(title.findtext('posX')) + int(title.findtext('width')) <= 1590
assert int(title.findtext('height')) <= 70
print('PASS: MovingPictures resolution/aspect logo separation and bounded static slideshow-properties title.')
