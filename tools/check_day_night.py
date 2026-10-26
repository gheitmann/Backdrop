"""Check paired artwork, native property bindings and the optional plugin payload."""
from pathlib import Path
import hashlib
import json
import struct
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'


def check():
    manifest = json.loads((ROOT / 'Source/Artwork/day-night.json').read_text())
    media = SKIN / 'Media'
    for relative, record in manifest['files'].items():
        data = (media / relative).read_bytes()
        assert hashlib.sha256(data).hexdigest() == record['sha256'], relative
        assert data[:8] == b'\x89PNG\r\n\x1a\n', relative
        assert list(struct.unpack('>II', data[16:24])) == record['dimensions'], relative
    pairs = manifest['pairs']
    declared = {n.get('path') for n in ET.parse(media / 'DayNight/skin-support.xml').getroot().findall('asset')}
    assert declared == set(pairs) | set(manifest['files']), 'Incomplete skin support declaration'
    for relative in declared:
        assert (media / relative).is_file(), relative
    expected = {
        "#(iif(string.equals(#DayNight.Period,'morning'),'" + day.replace('Modern/Day/', 'Modern/Morning/')
        + "',iif(string.equals(#DayNight.Period,'noon'),'" + day.replace('Modern/Day/', 'Modern/Noon/')
        + "',iif(string.equals(#DayNight.Period,'day'),'" + day + "','" + night + "'))))"
        for night, day in pairs.items()
    }
    bindings = {}
    for page in SKIN.glob('*.xml'):
        for node in ET.parse(page).getroot().iter('texture'):
            value = node.text or ''
            assert value.replace('\\', '/') not in pairs, f'Unconverted stock background: {page.name}'
            if '#DayNight.' in value:
                assert value in expected, f'Unknown day/night binding in {page.name}'
                bindings[page.name] = bindings.get(page.name, 0) + 1
    assert bindings == manifest['changed_xml'], 'Binding inventory changed'
    editor = (ROOT / 'Source/DayNightBackgrounds.cs').read_text()
    for night, day in pairs.items():
        assert '{ "' + night + '", "' + day + '" }' in editor, 'Editor mapping differs: ' + night
    generator = (ROOT / 'Source/BackdropMenuEditor/BackdropMenuEditorConfig.cs').read_text(encoding='utf-8-sig')
    assert generator.count('DayNightBackgrounds.Apply(rawXML.ToString())') == 2, 'Both editor output paths must transform stock textures'
    languages = ET.parse(ROOT / 'Source/DayNight/Languages.xml').getroot().findall('language')
    expected_languages = {p.stem.lower() for p in (SKIN / 'Translations').glob('*.xml')}
    assert {n.get('code').lower() for n in languages} == expected_languages
    keys = {n.get('key') for n in languages[0].findall('text')}
    for language in languages:
        entries = language.findall('text')
        assert len(entries) == len(keys) and {n.get('key') for n in entries} == keys
        assert all(n.text and n.text.strip() for n in entries), language.get('code')
    for name in ('settings_GUI_Skin.xml', 'mp139.settings_GUI_Skin.xml'):
        buttons = [n for n in ET.parse(SKIN / name).getroot().iter('control') if n.findtext('id') == '87900']
        assert len(buttons) == 1 and buttons[0].findtext('hyperlink') == '87950'
        assert buttons[0].findtext('label') == '#DayNight.Label.Title'
    assert ET.parse(SKIN / 'DayNight.xml').getroot().findtext('id') == '87950'
    # Check the standalone plugin's actual PE/CLI architecture, not its filename.
    data = (ROOT / 'Source/DayNight.Backgrounds.dll').read_bytes()
    pe = struct.unpack_from('<I', data, 0x3c)[0]
    assert data[pe:pe+4] == b'PE\0\0'
    assert struct.unpack_from('<H', data, pe + 4)[0] == 0x14c
    optional_size = struct.unpack_from('<H', data, pe + 20)[0]
    optional = pe + 24
    assert struct.unpack_from('<H', data, optional)[0] == 0x10b
    cli = struct.unpack_from('<I', data, optional + 96 + 14 * 8)[0]
    flags = None
    for index in range(struct.unpack_from('<H', data, pe + 6)[0]):
        section = optional + optional_size + index * 40
        size, address, raw_size, offset = struct.unpack_from('<IIII', data, section + 8)
        if address <= cli < address + max(size, raw_size):
            flags = struct.unpack_from('<I', data, offset + cli - address + 16)[0]
            break
    assert flags is not None and flags & 1 and not flags & (2 | 0x20000), 'Expected ILOnly AnyCPU plugin'
    template = ET.parse(ROOT / 'packaging/MediaPortalExtension.template.xml').getroot()
    for group in ('./Groups/Items/GroupItem/Files/Items', './UniqueFileList/Items'):
        records = template.findall(group + '/FileItem')
        selected = [r for r in records if r.findtext('LocalFileName') == 'Source/DayNight.Backgrounds.dll']
        assert len(selected) == 1 and selected[0].findtext('DestinationFilename') == r'%Plugins%\process\DayNight.Backgrounds.dll'
    print(f"Day/Night: {len(manifest['files'])} artwork hashes/dimensions, {sum(bindings.values())} bindings in {len(bindings)} XML files, {len(languages)} languages x {len(keys)} strings, in-skin entry, editor mappings and AnyCPU plugin verified.")


if __name__ == '__main__':
    check()
