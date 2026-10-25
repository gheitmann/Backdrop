"""Fetch pinned upstream icons and provider-rendered skin backgrounds; never modify personal fanart."""
from concurrent.futures import ThreadPoolExecutor
from pathlib import Path
import hashlib
import json
import urllib.request

ROOT = Path(__file__).resolve().parent.parent
REVISION = '7faf87628a1ad6175975bc595344337ce14a7db4'
ICONS = {
    'back': 'arrow-left', 'exit': 'x', 'fastforward': 'fast-forward',
    'full_screen': 'maximize', 'home': 'house', 'lyrics': 'mic-vocal',
    'nowplaying': 'disc-3', 'pause': 'pause', 'play': 'play',
    'playlist': 'list-music', 'rew': 'rewind', 'search': 'search',
    'skipback': 'skip-back', 'skipforward': 'skip-forward', 'stop': 'square',
}
PHOTOS = {
    'home': ('1765766638330-a180e8a5a990', 'Marina Nazina',
             'https://unsplash.com/photos/modern-living-room-with-dark-sofa-and-bookshelf-XAyj4ATVE9g'),
    'studio': ('1708791783497-ad3ae07f9e69', 'Shoham Avisrur',
               'https://unsplash.com/photos/a-recording-studio-with-a-keyboard-and-a-guitar-lVtrIFIKNZw'),
    'abstract': ('1532896374032-fdffb11e2b8e', 'Jack B',
                 'https://unsplash.com/photos/blue-and-black-abstract-artwork-qnBMlkav-j8'),
}
CONTROL_ICONS = ['check', 'circle-check', 'circle', 'chevron-left',
                 'chevron-right', 'chevron-up', 'chevron-down', 'power', 'monitor',
                 'video', 'headphones', 'captions', 'menu', 'layout-grid', 'volume-2',
                 'volume-x', 'bookmark', 'sliders-horizontal', 'repeat', 'repeat-1',
                 'shuffle', 'star', 'chart-no-axes-column-increasing', 'settings-2',
                 'lock', 'clock', 'plus', 'bell', 'download', 'users', 'folder',
                 'film', 'globe', 'radio', 'image', 'disc-3']

def fetch(item):
    url, path = item
    try:
        with urllib.request.urlopen(urllib.request.Request(url, headers={'User-Agent': 'Backdrop-artwork-build/1.0'}), timeout=60) as response:
            data = response.read()
    except Exception as error:
        raise RuntimeError(f'Failed to fetch {url}: {error}') from error
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_bytes(data)
    return {'file': path.relative_to(ROOT).as_posix(), 'source': url, 'sha256': hashlib.sha256(data).hexdigest()}

def main():
    if (ROOT / 'Source/Artwork/cinematic-night.json').is_file():
        raise SystemExit('Cinematic artwork is pinned. Use build-cinematic-artwork.cjs; legacy mixed icon/photo fetch would overwrite it.')
    source = ROOT / 'Source/Artwork/lucide'
    tasks = [(f'https://raw.githubusercontent.com/lucide-icons/lucide/{REVISION}/icons/{name}.svg', source / f'{name}.svg') for name in sorted(set(ICONS.values()) | set(CONTROL_ICONS))]
    tasks.append((f'https://raw.githubusercontent.com/lucide-icons/lucide/{REVISION}/LICENSE', ROOT / 'skin/Backdrop/Media/Modern/LUCIDE-LICENSE.txt'))
    for name, (identifier, _, _) in PHOTOS.items():
        # Request a 16:9 rendition from the photo provider, not a local image edit.
        crop = 'focalpoint&fp-x=0.5&fp-y=0.65' if name == 'home' else 'entropy'
        tasks.append((f'https://images.unsplash.com/photo-{identifier}?fm=jpg&fit=crop&crop={crop}&w=1920&h=1080&q=90', ROOT / f'skin/Backdrop/Media/Modern/{name}.jpg'))
    with ThreadPoolExecutor(max_workers=5) as pool:
        fetched = list(pool.map(fetch, tasks))
    manifest = {'lucide_revision': REVISION, 'icons': ICONS,
                'photos': {name: {'author': author, 'page': page, 'license': 'https://unsplash.com/license'} for name, (_, author, page) in PHOTOS.items()},
                'files': fetched}
    (ROOT / 'Source/Artwork/sources.json').write_text(json.dumps(manifest, indent=2) + '\n', encoding='utf-8')
    print(f'Fetched {len(set(ICONS.values()) | set(CONTROL_ICONS))} Lucide icons, license and {len(PHOTOS)} backgrounds.')

if __name__ == '__main__':
    main()
