"""Create an MPE installer from the portable, versioned payload template."""
from pathlib import Path
import argparse
import hashlib
import xml.etree.ElementTree as ET
import zipfile

ROOT = Path(__file__).resolve().parent.parent

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output', type=Path, default=ROOT / 'dist' / 'Backdrop-MP139-AnyCPU-ALPHA5.mpe1')
    args = parser.parse_args()
    root = ET.parse(ROOT / 'packaging' / 'MediaPortalExtension.template.xml').getroot()
    entries = root.findall('./Groups/Items/GroupItem/Files/Items/FileItem')
    args.output.parent.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(args.output, 'w', compression=zipfile.ZIP_DEFLATED) as archive:
        for entry in entries:
            relative = Path(entry.findtext('LocalFileName'))
            source = (ROOT / relative).resolve()
            if relative.is_absolute() or not source.is_relative_to(ROOT):
                raise ValueError(f'Unsafe payload path: {relative}')
            name = entry.findtext('ZipFileName').replace('\\', '/')
            archive.writestr(name, source.read_bytes())
        archive.writestr('MediaPortalExtension.xml', ET.tostring(root, encoding='utf-8', xml_declaration=True))
    # Reopen and verify every byte, not just the ZIP directory.
    with zipfile.ZipFile(args.output) as archive:
        for entry in entries:
            actual = archive.read(entry.findtext('ZipFileName').replace('\\', '/'))
            expected = (ROOT / entry.findtext('LocalFileName')).read_bytes()
            if actual != expected:
                raise ValueError(f'Payload mismatch: {entry.findtext("LocalFileName")}')
        assert len(archive.namelist()) == len(entries) + 1
    digest = hashlib.sha256(args.output.read_bytes()).hexdigest()
    args.output.with_suffix(args.output.suffix + '.sha256').write_text(digest + '  ' + args.output.name + '\n', encoding='utf-8')
    print(f'Packaged and verified {len(entries)} payload files: {args.output.name}')
    print(f'SHA256: {digest}')

if __name__ == '__main__':
    main()
