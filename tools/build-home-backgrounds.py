"""Fetch licensed category photographs and bind the six BasicHome menu slots."""
from concurrent.futures import ThreadPoolExecutor
from pathlib import Path
import hashlib
import json
import subprocess
import urllib.request
import uuid
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
PHOTOS = {
    'photos': (35598858, 'Caleb Oquendo', 'high-tech-camera-on-office-desk'),
    'cinema': (35120842, 'Stephen Leonardi', 'iconic-theater-at-night-with-neon-lights'),
    'tv': (11701160, 'Max Vakhtbovych', 'big-screen-television'),
    'gaming': (5303957, 'Peter Laskowski', 'black-gaming-controller-on-a-black-surface'),
    'plugins': (6636476, 'Sergei Starostin', 'blue-and-silver-circuit-board'),
}

def write(path, value):
    original = subprocess.run(['git', 'show', 'HEAD:' + path.relative_to(ROOT).as_posix()], cwd=ROOT, capture_output=True)
    newline = '\r\n' if b'\r\n' in original.stdout else '\n'
    path.write_text(value, encoding='utf-8', newline=newline)

def fetch(item):
    name, (number, author, slug) = item
    url = f'https://images.pexels.com/photos/{number}/pexels-photo-{number}.jpeg?auto=compress&cs=tinysrgb&fit=crop&w=1920&h=1080'
    request = urllib.request.Request(url, headers={'User-Agent': 'Backdrop-artwork-build/1.0'})
    with urllib.request.urlopen(request, timeout=60) as response:
        data = response.read()
    if not data.startswith(b'\xff\xd8'):
        raise ValueError(f'Expected JPEG for {name}')
    path = SKIN / f'Media/Modern/{name}.jpg'
    path.write_bytes(data)
    return name, {'author': author, 'page': f'https://www.pexels.com/photo/{slug}-{number}/',
                  'license': 'https://www.pexels.com/license/', 'source': url,
                  'sha256': hashlib.sha256(data).hexdigest()}

def main():
    if (ROOT / 'Source/Artwork/cinematic-night.json').is_file():
        print('Cinematic backgrounds are pinned. Use tools/build-cinematic-artwork.cjs; legacy downloads were skipped.')
        return
    with ThreadPoolExecutor(max_workers=5) as pool:
        photos = dict(pool.map(fetch, PHOTOS.items()))
    write(ROOT / 'Source/Artwork/home-backgrounds.json', json.dumps(photos, indent=2) + '\n')
    credits = SKIN / 'Media/Modern/ARTWORK-CREDITS.txt'
    text = credits.read_text(encoding='utf-8')
    text = text.split('\nHome menu category photographs (Pexels)')[0]
    text += '\nHome menu category photographs (Pexels)\n'
    for name, photo in photos.items():
        text += f"\n{name.title()}: {photo['author']}\n{photo['page']}\n"
    text += '\nLicense: https://www.pexels.com/license/\nProvider-rendered 1920x1080 crops. Home category photographs use full brightness.\n'
    write(credits, text)
    controls = []
    for slot, name in enumerate(('photos', 'studio', 'cinema', 'tv', 'gaming', 'plugins'), 1):
        ids = range(77002 + slot * 10, 77010 + slot * 10)
        condition = '|'.join(f'Control.HasFocus({control})' for control in ids)
        controls.append(f'''    <control>
      <description>Home category {slot}: {name}</description>
      <type>image</type><id>0</id>
      <posX>0</posX><posY>0</posY><width>1920</width><height>1080</height>
      <texture>Modern/{name}.jpg</texture>
      <colordiffuse>ffffffff</colordiffuse>
      <shouldCache>true</shouldCache>
      <visible>{condition}</visible>
    </control>''')
    write(SKIN / 'common.basichome.categories.xml', '<?xml version="1.0" encoding="utf-8"?>\n<window>\n  <controls>\n' + '\n'.join(controls) + '\n  </controls>\n</window>\n')
    home = SKIN / 'BasicHome.xml'
    text = home.read_text(encoding='utf-8-sig')
    if '<import>common.basichome.categories.xml</import>' not in text:
        text = text.replace('<import>common.basichome.latest.xml</import>', '<import>common.basichome.categories.xml</import>\n    <import>common.basichome.latest.xml</import>')
    write(home, text)
    source = ROOT / 'Source/BackdropMenuEditor/BackdropMenuEditorConfig.cs'
    text = source.read_text(encoding='utf-8-sig')
    line = '                    rawXML.AppendLine("<import>common.basichome.latest.xml</import> ");'
    if 'rawXML.AppendLine("<import>common.basichome.categories.xml</import>");' not in text:
        assert line in text
        text = text.replace(line, '                    rawXML.AppendLine("<import>common.basichome.categories.xml</import>");\n' + line)
    write(source, text)
    clock = SKIN / 'common.time.xml'
    text = clock.read_text(encoding='utf-8-sig').replace('<texture>time.header.png</texture>\n      <visible>!control.isvisible(93761232)</visible>', '<texture>time.header.png</texture>\n      <visible>!control.isvisible(93761232)+!window.isactive(35)</visible>')
    write(clock, text)
    # This is a transparent clock gradient, never a selectable button surface.
    original = subprocess.check_output(['git', 'show', 'ad461879d4e377a93f3355efd2f7bd3a9105bc5f:skin/Backdrop/Media/time.header.png'], cwd=ROOT)
    (SKIN / 'Media/time.header.png').write_bytes(original)
    template = ROOT / 'packaging/MediaPortalExtension.template.xml'
    text = template.read_text(encoding='utf-8')
    doc = ET.fromstring(text)
    indexes = [doc.find('./Groups/Items/GroupItem/Files/Items'), doc.find('./UniqueFileList/Items')]
    additions = []
    for index in indexes:
        known = {node.text for node in index.findall('./FileItem/LocalFileName')}
        records = []
        for path in [SKIN / 'common.basichome.categories.xml', *sorted((SKIN / 'Media/Modern').glob('*.jpg'))]:
            relative = path.relative_to(ROOT).as_posix()
            if relative in known:
                continue
            item = ET.Element('FileItem', InstallType='CopyFile', SystemFile='false', Modified='true')
            for tag, value in [('Param1', ''), ('UpdateOption', 'AlwaysOverwrite'), ('LocalFileName', relative),
                               ('ZipFileName', 'Installer{CopyFile}\\{' + str(uuid.uuid5(uuid.NAMESPACE_URL, relative)) + '}-' + path.name),
                               ('DestinationFilename', '%Skin%\\Backdrop\\' + str(path.relative_to(SKIN)).replace('/', '\\'))]:
                ET.SubElement(item, tag).text = value
            records.append(ET.tostring(item, encoding='unicode'))
        additions.append('\n'.join(records))
    import re
    text = re.sub(r'(<Files>\s*<Items>)(.*?)(</Items>)', lambda m: m[1] + m[2] + additions[0] + '\n' + m[3], text, count=1, flags=re.S)
    text = re.sub(r'(<UniqueFileList>\s*<Items>)(.*?)(</Items>)', lambda m: m[1] + m[2] + additions[1] + '\n' + m[3], text, count=1, flags=re.S)
    write(template, text)
    print('Fetched five category photographs; music uses the existing licensed studio photograph. Bound six home menu slots.')

if __name__ == '__main__':
    main()
