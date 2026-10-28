"""Protect the original TV guide regions and MP 1.39 bindings from fallback drift."""
import json
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin' / 'Backdrop'


def require(condition, message):
    if not condition:
        raise ValueError(message)


def check_caption(control, region, name):
    """Rotate the scaled text viewport, then check containment in its visible strip."""
    x, y, width, height = [float(control.findtext(f)) for f in ('posX', 'posY', 'width', 'height')]
    rotations = [a for a in control.findall('animation') if a.get('effect') == 'rotate']
    require(len(rotations) == 1, f'{name}: ambiguous caption rotation')
    rotation = rotations[0]
    require(rotation.get('center') == f'{int(x)},{int(y)}' and x > 0 and y > 0,
            f'{name}: rotation must use the absolute lower-left text anchor; zero means automatic centre')
    require(rotation.text == 'Conditional' and rotation.get('start') == rotation.get('end') == '90'
            and control.findtext('align') == 'left', f'{name}: caption must remain bottom anchored')
    for output in ((1920, 1080), (3440, 1440)):
        sx, sy = output[0] / 1920, output[1] / 1080
        left, top, right, bottom = region
        require(left * sx <= x * sx and x * sx + height * sy <= right * sx
                and top * sy <= y * sy - width * sx and y * sy <= bottom * sy,
                f'{name}: rotated caption escapes its strip at {output}')


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
        require(len(backgrounds) == 1 and backgrounds[0].findtext('texture') == "#(iif(string.equals(#DayNight.Period,'morning'),'Modern/Morning/cinema.png',iif(string.equals(#DayNight.Period,'noon'),'Modern/Noon/cinema.png',iif(string.equals(#DayNight.Period,'day'),'Modern/Day/cinema.png','Modern/home.jpg'))))"
                and backgrounds[0].findtext('colordiffuse') == 'ff505050',
                f'{name}: duplicate or foreign full-screen background')
        groups = [c for c in controls if c.findtext('id') == '100']
        require(len(groups) == 1 and groups[0].findtext('type') == 'button3part',
                f'{name}: incompatible native group binding')
        caption = [c for c in controls if c.findtext('description') == 'TvGroup button label']
        require(len(caption) == 1, f'{name}: missing guide caption')
        require(caption[0].findtext('type') == 'label' and caption[0].find('scrollStartDelaySec') is None,
                f'{name}: rotated marquee reintroduced; group caption must remain static')
        check_caption(caption[0], (7, 612, 59, 1060), name)
    require((SKIN / 'mytvguide.xml').read_bytes() == (SKIN / 'mp139.mytvguide.xml').read_bytes(),
            'Active guide definitions differ')
    for name in ('DialogMenu.xml', 'DialogSelect.xml', 'DialogSelect2.xml', 'DialogMultiSelect.xml'):
        root = ET.parse(SKIN / name).getroot()
        controls = root.findall('.//control')
        panel = next(c for c in controls if c.findtext('texture') == 'dialog_290_400.png')
        caption = next(c for c in controls if c.findtext('description') == 'Heading text label')
        x, y, height = [float(panel.findtext(f)) for f in ('posX', 'posY', 'height')]
        check_caption(caption, (x, y, x + 100, y + height), name)
    print('Original TV guide regions: 2/2 definitions passed. Rotated caption containment: 6 definitions x 2 output sizes passed.')
    latest = ET.parse(SKIN / 'common.basichome.latest.xml').getroot()
    captions = [c for c in latest.findall('.//control') if c.findtext('description') == 'Latest Label']
    require(len(captions) == 9, 'Missing latest-media captions')
    for c in captions:
        x = float(c.findtext('posX'))
        check_caption(c, (x, 400, x + 100, 980), c.findtext('label'))
    details = [c for c in latest.findall('.//control') if c.findtext('description') in ('Artist Label', 'Album Label')
               and (c.findtext('label') or '').startswith('#latestMediaHandler.music.')]
    require(len(details) == 6, 'Missing music metadata captions')
    for c in details:
        x = float(c.findtext('posX'))
        check_caption(c, (x, 400, x + 100, 980), c.findtext('label'))
    recording_details = [c for c in latest.findall('.//control')
                         if c.findtext('description') in ('Series Label', 'Episode Label')
                         and c.findtext('label', '').startswith('#latestMediaHandler.tvrecordings.')]
    require(len(recording_details) == 6, 'Missing TV recording metadata captions')
    for c in recording_details:
        require(c.findtext('posX') == ('1602' if c.findtext('description') == 'Series Label' else '1640'),
                'TV title/genre strips overlap')
        check_caption(c, (float(c.findtext('posX')), 400, float(c.findtext('posX')) + 30, 980), c.findtext('label'))
    # Focused heading occupies x=1554..1594; details follow it and must end
    # before thumbnails at x=1678. Check all three focus states explicitly.
    for slot in (1, 2, 3):
        title, genre = [next(c for c in recording_details if c.findtext('label') ==
                            f'#latestMediaHandler.tvrecordings.latest{slot}.{field}')
                        for field in ('title', 'genre')]
        require(1594 + 8 <= float(title.findtext('posX')) and
                float(title.findtext('posX')) + float(title.findtext('height')) + 8 <= float(genre.findtext('posX')) and
                float(genre.findtext('posX')) + float(genre.findtext('height')) + 8 <= 1678,
                'Focused Latest Recordings heading/details/thumbnails overlap')
    source = (ROOT / 'Source/BackdropMenuEditor/BackdropMenuEditorConfig.cs').read_text(encoding='utf-8-sig')
    import re
    generated = re.findall(r'<description>Latest Label</description>.*?<label>(#SkinTranslation[^<]+)</label>', source, re.S)
    require(len(generated) == 9, 'Latest caption generator count drifted')
    # The generator uses literal geometry for each definition; protect its persisted output too.
    blocks = re.findall(r'<description>Latest Label</description>.*?<label>#SkinTranslation[^<]+</label>.*?>Conditional</animation>', source, re.S)
    require(len(blocks) == 9, 'Latest caption generator rotation drifted')
    native_blocks = re.findall(r'rawXML.AppendLine\("    <control>"\);(?:(?!rawXML.AppendLine\("    <control>"\);).)*?rawXML.AppendLine\("    </control>[^\n]*', source, re.S)
    recording_blocks = [b for b in native_blocks if re.search(r'<description>(?:Series|Episode) Label</description>', b)
                        and '#latestMediaHandler.tvrecordings.latest' in b]
    require(len(recording_blocks) == 6, 'TV recording caption generator drifted')
    for c, block in zip(recording_details, recording_blocks):
        for field in ('posX', 'posY', 'width', 'height'):
            require(f'<{field}>{c.findtext(field)}</{field}>' in block, 'TV recording caption generator geometry differs')
        require('center=\\"' + c.findtext('posX') + ',970\\"' in block, 'TV recording caption generator pivot differs')
        require(block.count('>Conditional</animation>') == 1 and 'end=\\"90\\" time=\\"0\\">Hidden' not in block,
                'TV recording captions must not interpolate their rotation')
    for c, block in zip(captions, blocks):
        for field in ('posX', 'posY', 'width', 'height'):
            require(f'<{field}>{c.findtext(field)}</{field}>' in block, 'Latest caption generator geometry differs')
        require('center=\\"' + c.findtext('posX') + ',970\\"' in block, 'Latest caption generator pivot differs')
    for name in ('mymusicgenres.xml', 'mymusicsongs.xml'):
        radio = next(c for c in ET.parse(SKIN / name).getroot().findall('.//control') if c.findtext('id') == '18')
        require(radio.findtext('type') == 'checkbutton' and radio.findtext('hyperlink') == '30'
                and radio.findtext('textureNoFocus') == 'hiddenmenu_button_no_fo.png', f'{name}: radio must use the same neutral row frame as other music controls')
        require(radio.findtext('textureFocus') == 'hiddenmenu_button_fo.png',
                f'{name}: radio checkbutton must own its native focus texture')
        require(not any('Button 18 - Navigating' in c.findtext('description', '')
                        for c in ET.parse(SKIN / name).getroot().findall('.//control')),
                f'{name}: external radio focus images obscure the state mark')
    print('Latest captions: 9 definitions x 2 output sizes passed; generator geometry and 2 radio-menu overrides passed.')
    print('Focused music metadata captions: 6 definitions x 2 output sizes passed.')
    print('Focused TV recording metadata: 6 definitions x 2 output sizes passed.')


if __name__ == '__main__':
    check()
