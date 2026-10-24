"""Protect dialog focus states and the bounded five-tile music menu."""
from pathlib import Path
import xml.etree.ElementTree as ET

SKIN = Path(__file__).resolve().parent.parent / 'skin/Backdrop'
PAGES = {
    'DialogAlbumInfo.xml': '5 6',
    'DialogArtistInfo.xml': '5 6',
    'DialogCheckList.xml': '10 11',
    'DialogFile.xml': '11 10 12 13 14',
    'DialogIncompatiblePlugins.xml': '3',
    'DialogNotify.xml': '4',
    'DialogNotifyLastFM.xml': '4',
    'dialogOK.xml': '10 11',
    'dialogOldSkin.xml': '3',
    'dialogPlayStop.xml': '10 11',
    'dialogProgress.xml': '10',
    'DialogShareDrives.xml': '3 4 5 6 8 9',
    'DialogTVConflict.xml': '11 12 13 14',
    'dialogTVNotifyYesNo.xml': '10 11',
    'dialogYesNo.xml': '11 10',
    'SettingsMyMusic.xml': '8 13 10',
    'settingsScreen.xml': '2 3 4',
    'settingsSkipSteps.xml': '19 20 18',
    'settings_movies.xml': '24 29 25 27 28 21',
    'settings_tvEpg.xml': '2 3',
    'settings_tvepg_select.xml': '27',
}


def controls(name):
    return ET.parse(SKIN / name).getroot().findall('.//control')


count = states = 0
for name, identifiers in PAGES.items():
    page = controls(name)
    for identifier in identifiers.split():
        matches = [c for c in page if c.findtext('type') == 'button'
                   and c.findtext('id') == identifier]
        assert len(matches) == 1, (name, identifier)
        c = matches[0]
        assert c.findtext('textvalign') == 'middle', (name, identifier)
        assert len(c.findall('textYOff')) == 1 and c.findtext('textYOff') == '0'
        if name.lower().startswith('dialog'):
            assert c.findtext('textalign') == 'center'
            assert c.findtext('textXOff') == '0'
        else:
            assert int(c.findtext('textXOff')) >= 16
        if c.findtext('textureFocus') == 'button.focus.png':
            focus = SKIN / 'Media' / c.findtext('textureFocus')
            neutral = SKIN / 'Media' / c.findtext('textureNoFocus')
            assert focus.is_file() and neutral.is_file()
            assert focus.read_bytes() != neutral.read_bytes(), (name, identifier)
            states += 1
        count += 1
assert count == 52 and states == 19, (count, states)

# File confirmation buttons share one frame size, so the first button cannot
# extend below its row or crowd the action beside it.
file_buttons = [c for c in controls('DialogFile.xml') if c.findtext('type') == 'button']
assert len(file_buttons) == 5
assert {(c.findtext('width'), c.findtext('height')) for c in file_buttons} == {('150', '52')}

# Check the finished native hit rectangles, icon rectangles, caption anchors
# and horizontal navigation together, independently of the generator values.
page = controls('MusicMenu.xml')
order = ('4', '5', '6', '7', '3')
tiles = {c.findtext('id'): c for c in page if c.findtext('type') == 'button'}
assert set(tiles) == set(order)
rectangles = []
for index, identifier in enumerate(order):
    tile = tiles[identifier]
    x, y, width, height = [int(tile.findtext(k)) for k in ('posX', 'posY', 'width', 'height')]
    assert 0 <= x and 0 <= y and x + width <= 1920 and y + height <= 1080
    assert tile.findtext('onleft') == order[index - 1]
    assert tile.findtext('onright') == order[(index + 1) % len(order)]
    description = tile.findtext('description')
    icon = next(c for c in page if c.findtext('description') == description + ' Image')
    label = next(c for c in page if c.findtext('description') == description + ' Label')
    ix, iy, iw, ih = [int(icon.findtext(k)) for k in ('posX', 'posY', 'width', 'height')]
    assert x <= ix and y <= iy and ix + iw <= x + width and iy + ih <= y + height
    assert ix + iw / 2 == int(label.findtext('posX')) == x + width / 2
    assert y <= int(label.findtext('posY')) < y + height
    rectangles.append((x, x + width))
for left, right in zip(rectangles, rectangles[1:]):
    assert left[1] + 20 <= right[0], (left, right)
assert rectangles[0][0] == 1920 - rectangles[-1][1]
print(f'PASS: {count} dialog/settings buttons, {states} distinct focus states, five bounded music tiles and closed horizontal navigation.')
