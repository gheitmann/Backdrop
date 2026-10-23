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
    for path in ROOT.glob('*.xml'):
        used.update(re.findall(r'#SkinTranslation\.Translations\.([A-Za-z0-9_]+)\.Label', path.read_text(encoding='utf-8-sig')))
    missing = sorted(used - english.keys())
    if missing:
        raise ValueError(f'Missing English fallback properties: {missing}')
    buttons = []
    for control in ET.parse(ROOT / 'topbar.xml').findall('.//control'):
        if control.findtext('type') == 'button':
            buttons.append({'id': control.findtext('id'), 'description': control.findtext('description'), 'actions': [node.text for node in control.findall('action')]})
    print(json.dumps({
        'scope': 'Static inventory; runtime results are recorded separately.',
        'translation_files': len(locales),
        'used_properties': len(used),
        'missing_english_properties': missing,
        'locale_coverage': {name: {'missing_used_keys': sorted(used - strings.keys()), 'empty_keys': sorted(key for key, value in strings.items() if not value), 'used_values_equal_to_english': sum(strings.get(key) == english[key] for key in used)} for name, strings in locales.items()},
        'additional_themes': [p.name for p in (ROOT / 'Themes').glob('*') if p.is_dir()],
        'topbar': buttons,
    }, indent=2, ensure_ascii=False))

if __name__ == '__main__':
    main()
