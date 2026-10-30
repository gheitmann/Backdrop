"""Guard the reported Beta 1 focus, keyboard, photo and radio regressions."""
from pathlib import Path
import hashlib,json
import xml.etree.ElementTree as ET

ROOT=Path(__file__).resolve().parent.parent
SKIN=ROOT/'skin/Backdrop'
def controls(name):return list(ET.parse(SKIN/name).getroot().iter('control'))

# All original control artwork restored from the supplied archive is protected.
manifest=json.loads((ROOT/'Source/Artwork/native-fidelity.json').read_text())
restored=manifest['additional_source']['restored_files']
assert len(restored)==76
for name in restored:
    assert hashlib.sha256((SKIN/'Media'/name).read_bytes()).hexdigest()==manifest['files'][name]['sha256']
for page in SKIN.glob('*.xml'):
    assert not any(c.findtext('description')=='Animated menu focus background' for c in controls(page.name))
for name in ('settings.xml','mp139.settings.xml','mp139.common.settings.xml'):
    for c in controls(name):
        if '#currentmodule' in c.findtext('label',''):
            assert '!Window.IsVisible(1002)+!Window.IsVisible(1004)' in c.findtext('visible',''),name
            assert 'L(101002)' in c.findtext('label','') and 'L(101004)' in c.findtext('label',''),name
photo=next(c.findtext('texture') for c in controls('mypics.xml') if '#DayNight.Period' in c.findtext('texture',''))
assert all(path in photo for path in ('Modern/Morning/photos.png','Modern/Noon/photos.png','Modern/Day/photos.png','Modern/photos.jpg'))
for name in ('common.facade.pictures.xml',):
    thumb=next(c for c in controls(name) if c.findtext('type')=='thumbnailpanel')
    assert thumb.findtext('frameFocus')=='thumb.frame.black.focus.png'
for name in ('myradio.xml','mp139.myradio.xml','myradiosearch.xml','mp139.myradiosearch.xml'):
    for c in controls(name):
        if c.findtext('type') not in ('listcontrol','thumbnailpanel','filmstrip','coverflow','playlistcontrol'):continue
        assert c.findtext('showScrollbar')=='no',name
        assert all(not n.tag.lower().startswith('scrollbar') for n in c),name
        assert int(c.findtext('spinPosX'))+int(c.findtext('spinWidth'))<=1920,name
        if c.findtext('type')=='thumbnailpanel':
            assert c.findtext('imageFolder')==c.findtext('imageFolderFocus')=='-'
            assert c.findtext('hideUnfocusTexture')=='no'
            assert c.findtext('renderFocusText')==c.findtext('renderUnfocusText')=='yes'
            assert c.findtext('textureMask','')==''
            assert int(c.findtext('itemHeight'))>=int(c.findtext('thumbHeight'))+32
            assert int(c.findtext('itemHeightBig'))>=int(c.findtext('thumbHeightBig'))+32
for name in ('RadioPlayingNow.xml','mp139.RadioPlayingNow.xml'):
    page=controls(name)
    buttons={c.findtext('id'):c for c in page if c.findtext('type')=='button'}
    assert not buttons, 'Channel-change arrows intentionally omitted after BETA2 feedback'
    assert 'common.time.xml' in [n.text for n in ET.parse(SKIN/name).getroot().iter('import')]
    for c in page:
        for key in ('texture','texturebg','midtexture','textureFocus','textureNoFocus'):
            value=c.findtext(key,'')
            assert 'MP139' not in value,(name,value)
            if value and not value.startswith(('#','-')):assert (SKIN/'Media'/value).is_file(),value
print('PASS: 76 original textures; native focus ownership; modal keyboard guards; four Photos backgrounds; radio fallback icons, paging and omitted channel arrows.')
