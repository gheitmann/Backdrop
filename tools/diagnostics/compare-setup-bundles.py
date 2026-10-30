"""Compare original setup bytes and extract their embedded bundles for AV triage.

Does not execute, modify, repackage or publish an installer.
"""
from pathlib import Path
import hashlib
import io
import json
import struct
import sys
import zipfile


def digest(data):
    return hashlib.sha256(data).hexdigest()


def embedded_bundle(data):
    pe = struct.unpack_from('<I', data, 0x3c)[0]
    count = struct.unpack_from('<H', data, pe + 6)[0]
    optional_size = struct.unpack_from('<H', data, pe + 20)[0]
    optional = pe + 24
    magic = struct.unpack_from('<H', data, optional)[0]
    directories = optional + (96 if magic == 0x10b else 112)
    sections = optional + optional_size

    def offset(rva):
        for index in range(count):
            section = sections + index * 40
            virtual_size, address, raw_size, raw = struct.unpack_from('<IIII', data, section + 8)
            if address <= rva < address + max(virtual_size, raw_size):
                return raw + rva - address
        raise ValueError('RVA outside PE sections')

    cli = offset(struct.unpack_from('<I', data, directories + 14 * 8)[0])
    resource = offset(struct.unpack_from('<I', data, cli + 24)[0])
    size = struct.unpack_from('<I', data, resource)[0]
    archive = data[resource + 4:resource + 4 + size]
    if len(archive) != size or not archive.startswith(b'PK\x03\x04'):
        raise ValueError('Expected the original single Backdrop.Bundle resource')
    return archive


def normalized(name):
    if name.startswith('Backdrop-MP139-AnyCPU-ALPHA'):
        return 'skin.mpe1.sha256' if name.endswith('.sha256') else 'skin.mpe1'
    return name


def main():
    root = Path(__file__).resolve().parents[2]
    output = Path(sys.argv[1]).resolve()
    output.mkdir(parents=True, exist_ok=True)
    records = {}
    for revision in (22, 23):
        name = f'ALPHA{revision}'
        setup = root / f'dist/Backdrop-Setup-{name}.exe'
        data = setup.read_bytes()
        bundle = embedded_bundle(data)
        expected = (root / f'dist/Backdrop-Installer-{name}.zip').read_bytes()
        if bundle != expected:
            raise ValueError(f'{name}: private ZIP differs from the actual EXE resource')
        entries = {}
        target = output / name
        with zipfile.ZipFile(io.BytesIO(bundle)) as archive:
            for item in archive.infolist():
                path = (target / item.filename).resolve()
                if not path.is_relative_to(target):
                    raise ValueError('Unsafe archive path')
                content = archive.read(item)
                path.parent.mkdir(parents=True, exist_ok=True)
                path.write_bytes(content)
                entries[normalized(item.filename)] = {'name': item.filename, 'sha256': digest(content), 'bytes': len(content)}
        records[name] = {'setup_sha256': digest(data), 'bundle_sha256': digest(bundle), 'entries': entries}
    old, new = (records[name]['entries'] for name in ('ALPHA22', 'ALPHA23'))
    changes = []
    for name in sorted(old.keys() | new.keys()):
        before, after = old.get(name), new.get(name)
        if before is None or after is None or before['sha256'] != after['sha256']:
            changes.append(name)
    records['changed_entries'] = changes
    (output / 'bundle-comparison.json').write_text(json.dumps(records, indent=2) + '\n', encoding='utf-8')
    print(f'Both original EXE resources verified byte-for-byte; {len(old)} / {len(new)} bundle entries.')
    print('Changed entries: ' + ', '.join(changes))
    print('Evidence: ' + str(output))


if __name__ == '__main__':
    main()
