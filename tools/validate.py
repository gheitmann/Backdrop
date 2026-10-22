"""Validate Backdrop payload integrity and selected structural invariants."""
from pathlib import Path
import argparse
import hashlib
import json
import struct
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--refresh', action='store_true', help='Intentionally refresh hashes after reviewed changes')
    args = parser.parse_args()
    root = ET.parse(ROOT / 'packaging' / 'MediaPortalExtension.template.xml').getroot()
    for record in root.findall('.//LocalFileName'):
        value = record.text or ''
        if ':/' in value.replace('\\', '/') or value.startswith(('\\', '/')):
            raise ValueError('Machine-specific path in installer metadata')
    entries = root.findall('./Groups/Items/GroupItem/Files/Items/FileItem')
    manifest_path = ROOT / 'packaging' / 'baseline-sha256.json'
    baseline = json.loads(manifest_path.read_text(encoding='utf-8'))
    actual = {}
    xml_count = 0
    destinations = set()
    archive_names = set()
    for entry in entries:
        relative = entry.findtext('LocalFileName')
        path = (ROOT / relative).resolve()
        if not path.is_relative_to(ROOT) or Path(relative).is_absolute():
            raise ValueError(f'Unsafe payload path: {relative}')
        if path.name.lower() in ('skinsettings.xml', 'skinsettings.xml.bak', 'thumbs.db'):
            raise ValueError(f'Personal or generated file: {relative}')
        actual[relative] = hashlib.sha256(path.read_bytes()).hexdigest()
        destination = entry.findtext('DestinationFilename')
        name = entry.findtext('ZipFileName')
        if destination in destinations or name in archive_names or relative in ('', None):
            raise ValueError(f'Duplicate or invalid installer mapping: {relative}')
        destinations.add(destination)
        archive_names.add(name)
        if path.suffix.lower() == '.xml':
            ET.parse(path)
            if relative.startswith('skin/'):
                xml_count += 1
                if '#scroll.wrap' in path.read_text(encoding='utf-8-sig').lower():
                    raise ValueError(f'Unresolved separator: {relative}')
    shipped = {p.resolve().relative_to(ROOT).as_posix() for p in (ROOT / 'skin' / 'Backdrop').rglob('*') if p.is_file()}
    mapped = {p for p in actual if p.startswith('skin/')}
    if shipped != mapped:
        raise ValueError(f'Unmapped or missing skin files: {sorted(shipped ^ mapped)}')
    dll = (ROOT / 'Source' / 'BackdropMenuEditor.dll').read_bytes()
    pe = struct.unpack_from('<I', dll, 0x3c)[0]
    assert dll[pe:pe+4] == b'PE\0\0', 'Invalid editor PE signature'
    machine, section_count = struct.unpack_from('<HH', dll, pe + 4)
    optional_size = struct.unpack_from('<H', dll, pe + 20)[0]
    optional = pe + 24
    assert machine == 0x14c and struct.unpack_from('<H', dll, optional)[0] == 0x10b, 'Expected I386 PE32 AnyCPU editor'
    cli_rva = struct.unpack_from('<I', dll, optional + 96 + 14 * 8)[0]
    sections = optional + optional_size
    cli_offset = None
    for index in range(section_count):
        section = sections + index * 40
        size, address, raw_size, raw_offset = struct.unpack_from('<IIII', dll, section + 8)
        if address <= cli_rva < address + max(size, raw_size):
            cli_offset = raw_offset + cli_rva - address
            break
    assert cli_offset is not None, 'Missing CLI header'
    flags = struct.unpack_from('<I', dll, cli_offset + 16)[0]
    assert flags & 1 and not flags & (2 | 0x20000), 'Editor must be ILOnly without 32-bit required/preferred flags'
    if args.refresh:
        manifest_path.write_text(json.dumps(dict(sorted(actual.items())), indent=2) + '\n', encoding='utf-8')
        print('Refreshed reviewed payload checksums.')
    elif actual != baseline:
        changes = sorted(p for p in actual.keys() | baseline.keys() if actual.get(p) != baseline.get(p))
        raise ValueError(f'Payload differs from baseline: {changes}')
    print(f'Validated {len(actual)} payload files, {xml_count} skin XML files, and AnyCPU editor flags.')

if __name__ == '__main__':
    main()
