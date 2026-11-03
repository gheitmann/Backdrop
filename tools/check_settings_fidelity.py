"""Check native Settings contracts, scoped original artwork and supplied slideshow."""
from pathlib import Path
import hashlib
import json
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
audit = json.loads((ROOT / 'Source/Artwork/settings-originality-audit.json').read_text())
tags = ('id', 'type', 'hyperlink', 'action', 'onclick', 'onleft', 'onright', 'onup', 'ondown')
pages = {p.name for p in SKIN.glob('*.xml') if 'settings' in p.name.lower()}
assert pages == {r['file'] for r in audit['files']}, 'Update the complete Settings inventory'
for record in audit['files']:
    page = ET.parse(SKIN / record['file']).getroot()
    controls = list(page.iter('control'))
    bindings = sorted([[(t, c.findtext(t)) for t in tags] for c in controls
                       if c.findtext('id') not in (None, '0')], key=repr)
    signature = hashlib.sha256(json.dumps({'window': page.findtext('id'),
        'default': page.findtext('defaultcontrol'), 'bindings': bindings}, sort_keys=True).encode()).hexdigest()
    assert signature == record['binding_signature'], record['file']
    assert 'mp139.common.time.xml' not in [n.text for n in page.iter('import')], record['file']
    for control in controls:
        for node in control:
            value = node.text or ''
            if value.startswith('MP139\\'):
                assert control.findtext('visible') == 'no' or value == 'MP139\\Blu-ray_regions.png', (record['file'], value)
            if control.findtext('visible') != 'no' and node.tag.lower() in ('texture', 'texturefocus', 'texturenofocus', 'textureup', 'texturedown',
                                    'textureupfocus', 'texturedownfocus', 'texturebg',
                                    'alttexturefocus', 'alttexturenofocus') and value and not value.startswith(('#', '-')):
                assert (SKIN / 'Media' / value.replace('\\', '/')).is_file(), (record['file'], value)
slide = SKIN / 'slideshow.xml'
assert hashlib.sha256(slide.read_bytes()).hexdigest() == audit['slideshow']['sha256']
root = ET.parse(slide).getroot()
title = next(c for c in root.iter('control') if c.findtext('type') == 'textbox')
count = next(c for c in root.iter('control') if c.findtext('id') == '11')
assert int(title.findtext('posX')) + int(title.findtext('width')) < int(count.findtext('posX'))
assert title.findtext('label') == '#selecteditem'
print(f'PASS: {len(pages)} Settings binding contracts, Backdrop clocks/control artwork and supplied non-scrolling slideshow title.')

shared = ET.parse(SKIN / 'mp139.common.settings.xml').getroot()
assert all(c.findtext('texture') != 'dialog_743_336.png' for c in shared.iter('control'))
heading = next(c for c in shared.iter('control') if c.findtext('font') == 'font32')
assert 'window.isvisible(4)' not in heading.findtext('label')
assert any(c.findtext('label') == '897' and c.findtext('visible') == 'window.isvisible(4)' for c in shared.iter('control'))
for name in ('settings.xml', 'mp139.settings.xml'):
    root = ET.parse(SKIN / name).getroot()
    assert any(c.findtext('type') == 'label' and c.findtext('label') == '897' for c in root.iter('control'))
