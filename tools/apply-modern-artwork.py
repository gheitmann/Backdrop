"""Apply the reviewed artwork mappings without changing native controls or user fanart."""
from pathlib import Path
import re
import shutil
import uuid
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
SKIN = ROOT / 'skin/Backdrop'
MEDIA = SKIN / 'Media'

def main():
    copies = {
        'Animations/music/default.jpg': 'studio',
        'Animations/settings/default.jpg': 'abstract',
        'Animations/games/default.jpg': 'abstract',
        'Animations/shutdownbg/default.jpg': 'abstract',
        **{f'Animations/tvguide/tvguide{n}_default.jpg': 'home' for n in (1, 2, 3)},
    }
    for target, source in copies.items():
        shutil.copyfile(MEDIA / f'Modern/{source}.jpg', MEDIA / target)
    count = 0
    for path in SKIN.glob('*.xml'):
        text = path.read_text(encoding='utf-8-sig')
        updated = text.replace('<texture>default.png</texture>', '<texture>Modern/home.jpg</texture>')
        # Keep existing dimmer guide/overlay settings; cap bright static images only.
        def dim(match):
            block = match.group(0)
            if not re.search(r'<texture>(?:Modern/home.jpg|Animations[\\/](?:settings|games|shutdownbg|music|tvguide)[\\/][^<]+\.jpg)</texture>', block):
                return block
            color = re.search(r'<colordiffuse>([0-9a-fA-F]{8})</colordiffuse>', block)
            if color:
                value = color.group(1)
                rgb = ''.join(f'{min(int(value[i:i+2],16),112):02x}' for i in (2,4,6))
                return block[:color.start(1)] + value[:2] + rgb + block[color.end(1):]
            return block.replace('</texture>', '</texture>\n      <colordiffuse>ff707070</colordiffuse>', 1)
        updated = re.sub(r'<control\b[^>]*>(?:(?!<control\b).)*?</control>', dim, updated, flags=re.S)
        if updated != text:
            path.write_text(updated, encoding='utf-8', newline='\r\n')
            count += 1
    source = ROOT / 'Source/BackdropMenuEditor/BackdropMenuEditorConfig.cs'
    text = source.read_text(encoding='utf-8-sig')
    text = text.replace('<texture>default.png</texture>', '<texture>Modern/home.jpg</texture>')
    # The editor regenerates the three Latest Media fallback definitions.
    text = re.sub(r'rawXML.AppendLine\("        <texture>Modern/home.jpg</texture>"\);(?!\s*rawXML.AppendLine\("        <colordiffuse>)',
                  'rawXML.AppendLine("        <texture>Modern/home.jpg</texture>");\n                rawXML.AppendLine("        <colordiffuse>ff707070</colordiffuse>");', text)
    source.write_text(text, encoding='utf-8', newline='\r\n')
    template = ROOT / 'packaging/MediaPortalExtension.template.xml'
    text = template.read_text(encoding='utf-8')
    doc = ET.fromstring(text)
    indexes = [doc.find('./Groups/Items/GroupItem/Files/Items'), doc.find('./UniqueFileList/Items')]
    additions = [[], []]
    for path in sorted((MEDIA / 'Modern').glob('*')):
        relative = path.relative_to(ROOT).as_posix()
        item = ET.Element('FileItem', InstallType='CopyFile', SystemFile='false', Modified='true')
        for tag, value in [('Param1', ''), ('UpdateOption','AlwaysOverwrite'), ('LocalFileName',relative),
                           ('ZipFileName','Installer{CopyFile}\\{'+str(uuid.uuid5(uuid.NAMESPACE_URL, relative))+'}-'+path.name),
                           ('DestinationFilename','%Skin%\\Backdrop\\Media\\Modern\\'+path.name)]:
            ET.SubElement(item, tag).text = value
        for index, records in enumerate(indexes):
            if relative not in {node.text for node in records.findall('./FileItem/LocalFileName')}:
                additions[index].append(ET.tostring(item, encoding='unicode'))
    # Both native file indexes must contain the same added payload records.
    text = re.sub(r'(<Files>\s*<Items>)(.*?)(</Items>)',
                  lambda m: m[1] + m[2] + '\n'.join(additions[0]) + '\n' + m[3], text, count=1, flags=re.S)
    text = re.sub(r'(<UniqueFileList>\s*<Items>)(.*?)(</Items>)',
                  lambda m: m[1] + m[2] + '\n'.join(additions[1]) + '\n' + m[3], text, count=1, flags=re.S)
    template.write_text(text, encoding='utf-8', newline='\r\n')
    print(f'Updated {count} skin XML definitions, seven stock background mappings and {len(additions[0])} new payload files.')

if __name__ == '__main__':
    main()
