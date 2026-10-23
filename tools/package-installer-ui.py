"""Package the optional helper, without redistributing MediaPortal binaries."""
from pathlib import Path
import argparse
import hashlib
import zipfile

ROOT = Path(__file__).resolve().parent.parent

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--build', required=True, type=Path)
    parser.add_argument('--cecil-license', required=True, type=Path)
    parser.add_argument('--output', type=Path, default=ROOT / 'dist/Backdrop-InstallerUI-ALPHA11.zip')
    args = parser.parse_args()
    files = {name: args.build / name for name in (
        'BackdropInstaller.exe', 'Backdrop.InstallerPresentation.dll', 'PresentationProbe.exe',
        'PatchInstaller.exe', 'Mono.Cecil.dll')}
    files['Initialize.ps1'] = ROOT / 'Source/InstallerUi/Initialize.ps1'
    files['README.md'] = ROOT / 'docs/INSTALLER-UI.md'
    files['Mono.Cecil-LICENSE.txt'] = args.cecil_license
    for path in (ROOT / 'Source/InstallerUi').glob('*.cs'):
        files['Source/' + path.name] = path
    for name in ('presentation-probe.txt', 'binding-probe.txt'):
        if not (args.build / name).read_text().startswith('PASS:'):
            raise ValueError('Build probe did not pass: ' + name)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(args.output, 'w', compression=zipfile.ZIP_DEFLATED) as archive:
        for name, source in files.items():
            archive.writestr(name, source.read_bytes())
    with zipfile.ZipFile(args.output) as archive:
        assert 'MpeCore.dll' not in archive.namelist()
        for name, source in files.items():
            assert archive.read(name) == source.read_bytes(), name
    digest = hashlib.sha256(args.output.read_bytes()).hexdigest()
    args.output.with_suffix('.zip.sha256').write_text(digest + '  ' + args.output.name + '\n')
    print(f'Verified {len(files)} helper files; no MediaPortal binaries: {args.output.name}')

if __name__ == '__main__':
    main()
