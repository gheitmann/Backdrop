"""Protect menu row framing, native text alignment and plugin padding."""
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
count = 0
for name in ('mymusicsongs.xml', 'mymusicgenres.xml', 'mymusicplaylist.xml'):
    controls = ET.parse(SKIN / name).getroot().findall('.//control')
    for c in controls:
        if (c.findtext('type') not in ('button', 'menubutton', 'sortbutton', 'checkbutton')
                or int(c.findtext('width', '0')) < 350
                or c.findtext('font') not in ('font14BC', 'font16BC')):
            continue
        assert c.findtext('textvalign') == 'middle', (name, c.findtext('id'))
        assert c.findtext('height') == '42'
        assert int(c.findtext('textXOff')) >= 16
        for field, texture in (('textureFocus', 'hiddenmenu_button_fo.png'),
                               ('textureNoFocus', 'hiddenmenu_button_no_fo.png')):
            assert c.findtext(field) == texture
            assert (SKIN / 'Media' / texture).is_file()
        assert not any(c.findtext('id') and 'Control.HasFocus(' + c.findtext('id') + ')' in image.findtext('visible', '')
                       for image in controls if image.findtext('type') == 'image'
                       and image.findtext('texture') == 'hiddenmenu_button_fo.png')
        count += 1
    assert not any(c.findtext('type') == 'image' and c.findtext('texture') == 'hiddenmenu_button_fo.png'
                   and 'hasfocus' in c.findtext('visible', '').lower() for c in controls)
plugin = next(c for c in ET.parse(SKIN / 'myHomePlugIns.xml').getroot().findall('.//control')
              if c.findtext('id') == '50')
assert plugin.findtext('type') == 'menu'  # Retain native plugin routing/binding.
assert int(plugin.findtext('buttonTextXOff')) >= 24
assert int(plugin.findtext('buttonOffset')) == 0
print(f'PASS: {count} music menu rows with native vertical centering and consistent frames; plugin text inset 24px.')
