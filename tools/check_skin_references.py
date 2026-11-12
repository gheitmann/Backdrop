"""Check skin texture, font, import and core-window references without starting MediaPortal.

MediaPortal silently ignores a texture that does not exist, falls back to the
debug font for an unknown font name, and logs only a warning for a missing
import. These defects therefore survive visual review. This check resolves every
static reference (texture elements and `mask` attributes) against the shipped
files and the MediaPortal 1.39 window IDs.
"""
from pathlib import Path
import json
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin' / 'Backdrop'
MEDIA = SKIN / 'Media'
BASELINE = ROOT / 'Source' / 'Artwork' / 'skin-reference-baseline.json'
WINDOWS = ROOT / 'Source' / 'Artwork' / 'mp139-window-ids.json'

TEXTURE_TAGS = {
    'texture', 'texturefocus', 'texturenofocus', 'textureup', 'texturedown',
    'textureupfocus', 'texturedownfocus', 'image', 'imagepath', 'texturecheckmark',
    'texturecheckmarknofocus', 'lefttexture', 'midtexture', 'righttexture',
    'texturebg', 'scrollbarbackground', 'scrollbarleft', 'scrollbarright',
    'scrollbartop', 'scrollbarbottom', 'scrollbarbg',
}
TEXTURE_ATTRIBUTES = {'mask'}
IMAGE_SUFFIXES = ('.png', '.jpg', '.jpeg', '.gif', '.bmp')


def skin_files():
    return sorted(SKIN.glob('*.xml'))


def skin_tree(path):
    try:
        return ET.parse(path)
    except ET.ParseError as error:
        raise ValueError(f'Invalid skin XML {path.name}: {error}') from error


def media_index():
    return {path.relative_to(MEDIA).as_posix().lower() for path in MEDIA.rglob('*') if path.is_file()}


def defined_fonts():
    names = set()
    for node in ET.parse(SKIN / 'fonts.xml').getroot():
        if node.tag in ('font', 'alias'):
            name = node.findtext('name')
            if name:
                names.add(name.strip())
    return names


def texture_references(node):
    if isinstance(node.tag, str) and node.tag.lower() in TEXTURE_TAGS:
        yield (node.text or '').strip()
    for name, value in node.attrib.items():
        if name.lower() in TEXTURE_ATTRIBUTES:
            yield (value or '').strip()


def check_textures(index, allowed):
    unresolved = {}
    for path in skin_files():
        for node in skin_tree(path).iter():
            for reference in texture_references(node):
                lowered = reference.lower()
                if '#' in reference or reference.startswith('..') or ':' in reference:
                    continue
                if not lowered.endswith(IMAGE_SUFFIXES):
                    continue
                if lowered.replace('\\', '/') in index:
                    continue
                unresolved.setdefault(reference, set()).add(path.name)
    unexpected = sorted(set(unresolved) - set(allowed))
    if unexpected:
        details = '; '.join(f'{name} ({", ".join(sorted(unresolved[name])[:3])})' for name in unexpected)
        raise ValueError(f'Unresolved texture references: {details}')
    stale = sorted(set(allowed) - set(unresolved))
    return len(unresolved), stale


def check_fonts():
    known = defined_fonts()
    unknown = {}
    for path in skin_files():
        for node in skin_tree(path).iter('font'):
            name = (node.text or '').strip()
            if not name or name == '-' or name in known:
                continue
            unknown.setdefault(name, set()).add(path.name)
    if unknown:
        details = '; '.join(f'{name} ({", ".join(sorted(files)[:3])})' for name, files in sorted(unknown.items()))
        raise ValueError(f'Unknown font names: {details}')
    return len(known)


def check_imports():
    missing = {}
    for path in skin_files():
        for node in skin_tree(path).iter():
            if not isinstance(node.tag, str) or node.tag.lower() not in ('import', 'include'):
                continue
            name = (node.text or '').strip()
            if not (SKIN / name).is_file():
                missing.setdefault(name, set()).add(path.name)
    if missing:
        details = '; '.join(f'{name} ({", ".join(sorted(files)[:3])})' for name, files in sorted(missing.items()))
        raise ValueError(f'Missing imports or includes: {details}')


def check_window_ids():
    expected = json.loads(WINDOWS.read_text(encoding='utf-8'))['windows']
    checked = 0
    for name, identifier in sorted(expected.items()):
        path = SKIN / name
        if not path.is_file():
            raise ValueError(f'Missing MediaPortal 1.39 window file: {name}')
        window = skin_tree(path).getroot()
        if window.tag != 'window':
            raise ValueError(f'Not a window definition: {name}')
        actual = (window.findtext('id') or '').strip()
        if actual != identifier:
            raise ValueError(f'Window ID mismatch in {name}: {actual} != {identifier}')
        checked += 1
    return checked


def main():
    baseline = json.loads(BASELINE.read_text(encoding='utf-8'))
    allowed = baseline['unresolved_textures']
    index = media_index()
    textures, stale = check_textures(index, allowed)
    fonts = check_fonts()
    check_imports()
    windows = check_window_ids()
    for name in stale:
        print(f'NOTE: baseline entry now resolves, remove it: {name}')
    print(f'Checked {textures} baseline textures, {fonts} fonts, {windows} MediaPortal 1.39 window IDs and all imports.')


if __name__ == '__main__':
    main()
