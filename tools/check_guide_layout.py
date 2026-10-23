"""Protect the original TV guide regions and MP 1.39 bindings from fallback drift."""
import json
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin' / 'Backdrop'


def require(condition, message):
    if not condition:
        raise ValueError(message)


def check():
    reference = json.loads((ROOT / 'docs/ORIGINAL-TVGUIDE-LAYOUT.json').read_text())
    for name in ('mytvguide.xml', 'mp139.mytvguide.xml'):
        root = ET.parse(SKIN / name).getroot()
        require(root.findtext('id') == '600', f'{name}: wrong native window')
        controls = root.findall('.//control')
        require(not root.findall('.//include'), f'{name}: replacement guide template included')
        imports = [e.text for e in root.findall('.//import')]
        require(imports == ['common.time.xml'], f'{name}: unexpected guide imports {imports}')
        clock = ET.parse(SKIN / 'common.time.xml').getroot()
        for anchor in reference['anchors']:
            candidates = clock.findall('.//control') if anchor['name'] == 'clock' else controls
            if anchor['name'] == 'clock':
                candidates = [c for c in candidates if c.findtext('label') == '#date | #time']
            expected_id = '99' if anchor['name'] == 'video_preview' else anchor['id']
            matches = [c for c in candidates if c.findtext('id') == expected_id
                       and c.findtext('type') == anchor['type']]
            require(len(matches) == 1, f'{name}: missing/duplicate {anchor["name"]}')
            for field, expected in anchor['coordinates'].items():
                require(matches[0].findtext(field) == str(expected),
                        f'{name}: original {anchor["name"]}.{field} drifted')
            if anchor.get('align'):
                require(matches[0].findtext('align') == anchor['align'], f'{name}: clock alignment')
        require(sum(c.findtext('type') == 'videowindow' for c in controls) == 1,
                f'{name}: duplicate video area')
        backgrounds = [c for c in controls if c.findtext('type') == 'image'
                       and c.findtext('width') == '1920' and c.findtext('height') == '1080']
        require(len(backgrounds) == 1 and backgrounds[0].findtext('texture') == 'background.tvguide.png',
                f'{name}: duplicate or foreign full-screen background')
        groups = [c for c in controls if c.findtext('id') == '100']
        require(len(groups) == 1 and groups[0].findtext('type') == 'button3part',
                f'{name}: incompatible native group binding')
    require((SKIN / 'mytvguide.xml').read_bytes() == (SKIN / 'mp139.mytvguide.xml').read_bytes(),
            'Active guide definitions differ')
    print('Original TV guide regions, single background/preview and native group binding: 2/2 definitions passed.')


if __name__ == '__main__':
    check()
