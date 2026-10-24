"""Report translation coverage, installed themes, and topbar mappings without claiming runtime compatibility."""
from pathlib import Path
import json
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent / 'skin' / 'Backdrop'

def main():
    locales = {}
    for path in sorted((ROOT / 'Translations').glob('*.xml')):
        strings = ET.parse(path).findall('.//string')
        keys = [node.get('Field') for node in strings]
        duplicates = sorted({key for key in keys if keys.count(key) > 1})
        if duplicates:
            raise ValueError(f'Duplicate translation keys in {path.name}: {duplicates}')
        locales[path.stem] = {node.get('Field'): ''.join(node.itertext()).strip() for node in strings}
    english = locales['en']
    used = set()
    for path in ROOT.rglob('*.xml'):
        if 'Translations' in path.parts:
            continue
        used.update(re.findall(r'#SkinTranslation\.Translations\.([A-Za-z0-9_]+)\.Label', path.read_text(encoding='utf-8-sig')))
    missing = sorted(used - english.keys())
    if missing:
        raise ValueError(f'Missing English fallback properties: {missing}')
    buttons = []
    for control in ET.parse(ROOT / 'topbar.xml').findall('.//control'):
        if control.findtext('type') == 'button':
            buttons.append({'id': control.findtext('id'), 'description': control.findtext('description'), 'actions': [node.text for node in control.findall('action')]})
    # Check command identity, keyboard navigation and artwork separately from runtime behavior.
    controls = {c.findtext('id'): c for c in ET.parse(ROOT / 'topbar.xml').findall('.//control') if c.findtext('type') == 'button'}
    order = ['21', '26', '22', '23', '24', '25', '27', '28', '29', '30', '31']
    actions = ['10', '115', '15', '17', "#(iif(eq(#playlogo,'logo_pause.png'),12,68))", '12', '13', '16', '14', '18', '97']
    if len(buttons) != len(order) or set(controls) != set(order):
        raise ValueError('Unexpected or duplicate topbar command IDs')
    for index, (identifier, action) in enumerate(zip(order, actions)):
        control = controls[identifier]
        if [node.text for node in control.findall('action')] != [action]:
            raise ValueError(f'Unexpected topbar action: {identifier}')
        if control.findtext('onleft') != order[index - 1] or control.findtext('onright') != order[(index + 1) % len(order)]:
            raise ValueError(f'Broken topbar navigation: {identifier}')
        for tag in ('textureFocus', 'textureNoFocus'):
            texture = control.findtext(tag)
            if not texture or not (ROOT / 'Media' / texture).is_file():
                raise ValueError(f'Missing topbar artwork: {identifier}/{tag}')
    print(json.dumps({
        'scope': 'Static inventory; runtime results are recorded separately.',
        'translation_files': len(locales),
        'used_properties': len(used),
        'missing_english_properties': missing,
        'locale_coverage': {name: {'missing_used_keys': sorted(used - strings.keys()), 'empty_keys': sorted(key for key, value in strings.items() if not value), 'used_values_equal_to_english': sum(strings.get(key) == english[key] for key in used)} for name, strings in locales.items()},
        'additional_themes': [p.name for p in (ROOT / 'Themes').glob('*') if p.is_dir()],
        'topbar': buttons,
        'topbar_static_commands_passed': len(order),
        'topbar_static_commands_total': len(order),
    }, indent=2, ensure_ascii=False))

if __name__ == '__main__':
    main()
