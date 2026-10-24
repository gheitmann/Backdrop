"""Inventory control textures and classify UI artwork separately from media/game content."""
from pathlib import Path
import json
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parent.parent
MEDIA = ROOT / 'skin/Backdrop/Media'

def classify(relative):
    name = Path(relative.replace('\\', '/')).name.lower()
    if Path(name).suffix not in ('.png', '.gif') or name in ('default.png','defaultfanart.gif'):
        return {'kind': 'content'}
    if re.search(r'/settings/|/clickmania/|/hexxagon/|/minesweeper/|/pipes/|/yahtzee/|mask|border|drop_frame', '/'+relative.lower()):
        return {'kind': 'content'}
    if name.startswith('top_') or name == 'overlay_topbar.png':
        return {'kind': 'topbar'}
    if name.startswith('overlay') or name.startswith('black_transp') or name in ('playing.overlay.png','list_background_focus_transp.png'):
        return {'kind': 'content'}
    if name == 'tvguide_slider.png':
        return {'kind': 'timeline'}
    focus = bool(re.search(r'(?<!no)focus|_fo(?:\.|_)|fo\.png|_enabled|3\.png|_selected_|^selected_', name)) and not bool(re.search(r'nofocus|no_fo|nof_|_nf|nf\.|_disabled|^unselected_', name))
    if name.startswith(('hiddenmenu_togglebutton_', 'dropdown_togglebutton_')):
        focus = '_nof_' not in name
    on = not bool(re.search(r'off|check-boxnf|ledoff', name))
    common = {'focus': focus}
    if name in ('default.line.separator.png', 'line.listview.png'):
        return {'kind': 'content'}
    if name == 'uncheckbox_bhe.png':
        return dict(common, kind='mark', icon='square', active=False, checkbox=True)
    if 'check-box' in name:
        return dict(common, kind='mark', icon='check' if on else 'square', active=on, checkbox=True)
    if name in ('ledon.png','ledoff.png','led.png'):
        return dict(common, kind='mark', icon='circle-check' if on else 'circle', active=on)
    if 'userratingstar' in name:
        return dict(common, kind='mark', icon='star', active='on' in name)
    if name in ('star_yellow.png',):
        return dict(common, kind='mark', icon='star', active=True)
    if 'arrow' in name or name.startswith(('page_', 'scroll-', 'playlist_item_', 'playlist_up_', 'playlist_down_')):
        for direction in ('left','right','up','down'):
            if direction in name:
                return dict(common, kind='icon', icon='chevron-'+direction)
    if 'playlist' in name and 'delete' in name:
        return dict(common, kind='icon', icon='x')
    if name in ('warning-icon.png', 'rec.png', 'pause_white_transparent.png', 'video-pause.png'):
        return dict(common, kind='icon', icon='pause' if 'pause' in name else 'circle' if name=='rec.png' else 'bell', active=name=='rec.png')
    speed = re.match(r'(ff_|rew_|video([+-]))(\d+x)\.png', name)
    if speed:
        return dict(common, kind='speed', icon='rewind' if name.startswith(('rew_', 'video-')) else 'fast-forward', text=speed[3])
    if name.startswith(('osdprogress', 'osd_progress', 'osd_slider_bg', 'now_playing_progress', 'progressback')):
        return {'kind': 'content'}
    if name.startswith('osd_slider_nib'):
        return dict(common, kind='mark', icon='circle', active=focus)
    semantic = [('shutdown','power'),('exit','x'),('close','x'),('tvdesktop','monitor'),
                ('dialogue_ok','check'),('tb_extend','maximize'),('tb_menu','menu'),
                ('tb_subtitles','captions'),('tb_playlist','list-music'),('tb_video','video'),
                ('tb_music','headphones'),('tb_record','circle'),('tb_cf','layout-grid'),('basic_focus','house'),('basic_nofocus','house')]
    semantic += [('lock','lock'),('clock','clock'),('logo_fastforward','fast-forward'),
                 ('logo_rewind','rewind'),('logo_pause','pause'),('next.arrow','chevron-right'),
                 ('previous.arrow','chevron-left'),('new_icon','plus'),('rec_indicator','circle'),
                 ('default.recording','circle'),('slidernib','circle'),('tvguide_slider','circle'),
                 ('tvseries_unwatched','circle'),('tvseries_watched','check'),('overlayunwatched','circle'),
                 ('overlaywatched','check'),('myfilms/download','download'),('volume.states.mute','volume-x'),
                 ('volume.states','volume-2'),('playing.overlay','play')]
    for token, icon in semantic:
        if token in name or token in relative.lower():
            return dict(common, kind='icon', icon=icon, active='record' in token)
    if name.startswith('osd'):
        osd = [('audiomute','volume-x'),('audio','volume-2'),('dvd','disc-3'),('bookmarks','bookmark'),
               ('nexttrack','skip-forward'),('prevtrack','skip-back'),('forward','fast-forward'),
               ('rewind','rewind'),('seekforward','fast-forward'),('seekrewind','rewind'),
               ('pause','pause'),('playlist','list-music'),('play','play'),('stop','square'),
               ('presetsettings','sliders-horizontal'),('prefo','sliders-horizontal'),('prenf','sliders-horizontal'),
               ('random','shuffle'),('repeatone','repeat-1'),('repeat','repeat'),('record','circle'),
               ('video','video'),('viz','chart-no-axes-column-increasing')]
        for token, icon in osd:
            if name.startswith('osd'+token):
                return dict(common, kind='icon', icon=icon, active='on' in name or ('record' in name and 'off' not in name))
    if name.startswith(('osd2x','osd4x','osd8x','osd16x','osd32x')):
        return dict(common, kind='speed', icon='fast-forward', text=name[3:-4])
    if name.startswith(('default','albumno','nothumb', 'missing_coverart', 'myfilms_latest_nocover')) or name in ('persons.png','collection.png','compactdisc.png','picture_cover.png','trailer.png','online.png'):
        icon = 'folder' if 'folder' in name else 'users' if ('person' in name or 'director' in name) else 'radio' if 'radio' in name else 'headphones' if ('music' in name or 'album' in name or 'disc' in name) else 'image' if ('picture' in name or 'thumb' in name) else 'film'
        return dict(common, kind='placeholder', icon=icon)
    if name in ('led_green.png','led_grey.png','led_red.png','led_yellow.png'):
        return dict(common, kind='mark', icon='circle', active='grey' not in name, status=name[4:-4])
    # These assets are program badges, media frames or board-game graphics, not commands.
    if re.search(r'tvguide_hd_|episode\.frame|poster_frame|music_frame|tetris|yahtzee|marker_|no99|rss_image', relative.lower()):
        return {'kind': 'content'}
    if re.search(r'list|button|focus|nofocus|selection|border|drop_frame|bar_hor|tv_group_bg|basic_', name):
        return dict(common, kind='surface', active=('ledon' in name or 'togglebutton_on' in name), toggle=('togglebutton' in name or 'ledon' in name), status='red' if 'recbutton' in name else 'green' if 'buttongreen' in name else None)
    if re.search(r'^dialog_|dialogmenu|menu_bg|menu_selected|black_transp|glasstitlebar|time.header|tvguide_time_bg|statusbar_back|sliderbackground', name):
        return dict(common, kind='surface')
    if name.startswith(('overlay.', 'overlay_', 'osdbackground','osd_settings','osdmenubg','osdbg')):
        # Existing opacity gradients define readable content regions; they are not icons.
        return {'kind': 'content'}
    # Remaining textures are background scenes, codec/logo badges, calibration or gameplay artwork.
    return {'kind': 'content'}

def main():
    refs = {}
    for path in (ROOT / 'skin/Backdrop').glob('*.xml'):
        for node in ET.parse(path).iter():
            tag = node.tag.lower()
            if not (tag.startswith('texture') or tag.startswith('alttexture')):
                continue
            value = (node.text or '').strip().replace('\\','/')
            if value and '#' not in value and value != '-' and (MEDIA / value).is_file():
                refs.setdefault(value, set()).add(tag)
    # OSD and alternate checkbutton states can be loaded natively, beyond explicit XML tags.
    for path in MEDIA.rglob('*.png'):
        relative = path.relative_to(MEDIA).as_posix()
        name = path.name.lower()
        if classify(relative)['kind'] in ('icon','mark','speed','placeholder','surface','topbar','timeline'):
            # Include alternate native states even when no current page names them.
            if not re.search(r'/animations/|/weather/', '/'+relative.lower()):
                refs.setdefault(relative, set()).add('native-or-alternate-state')
    # Windows references are case-insensitive; record the real on-disk name once.
    canonical = {p.relative_to(MEDIA).as_posix().lower(): p.relative_to(MEDIA).as_posix() for p in MEDIA.rglob('*') if p.is_file()}
    merged = {}
    for name, tags in refs.items():
        merged.setdefault(canonical[name.lower()], set()).update(tags)
    records = {name: dict(classify(name), tags=sorted(tags)) for name,tags in sorted(merged.items())}
    (ROOT / 'Source/Artwork/control-textures.json').write_text(json.dumps(records, indent=2)+'\n',encoding='utf-8')
    counts = {kind:sum(r['kind']==kind for r in records.values()) for kind in sorted({r['kind'] for r in records.values()})}
    print(json.dumps(counts))
    print('Unclassified:', [name for name,r in records.items() if r['kind']=='unclassified'])

if __name__ == '__main__':
    main()
