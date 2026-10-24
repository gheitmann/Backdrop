// Original vector transport silhouettes on Backdrop-style charcoal/silver discs.
// No opaque icon cards are introduced into the glass OSD artwork.
const fs = require('node:fs'), path = require('node:path'), sharp = require('sharp');
const root = path.resolve(__dirname, '..'), media = path.join(root, 'skin/Backdrop/Media');
const sizes = require('../Source/Artwork/topbar-native-sizes.json');
const glyphs = {
  back: '<path d="M3 12l9-9v6h9v6h-9v6z"/>',
  exit: '<path d="M5 3l7 7 7-7 2 2-7 7 7 7-2 2-7-7-7 7-2-2 7-7-7-7z"/>',
  fastforward: '<path d="M2 4l10 8L2 20zm10 0l10 8-10 8z"/>',
  full_screen: '<path d="M2 2h8v3H5v5H2zm12 0h8v8h-3V5h-5zM2 14h3v5h5v3H2zm17 0h3v8h-8v-3h5z"/>',
  home: '<path fill-rule="evenodd" d="M1 11L12 1l11 10h-4v11h-5v-8h-4v8H5V11z"/>',
  lyrics: '<rect x="8" y="2" width="8" height="13" rx="4"/><path d="M5 10h2v2a5 5 0 0010 0v-2h2v2a7 7 0 01-6 7v2h4v2H7v-2h4v-2a7 7 0 01-6-7z"/>',
  nowplaying: '<path fill-rule="evenodd" d="M12 1a11 11 0 100 22 11 11 0 000-22zm0 7a4 4 0 110 8 4 4 0 010-8z"/>',
  pause: '<path d="M5 3h5v18H5zm9 0h5v18h-5z"/>',
  play: '<path d="M5 2l16 10L5 22z"/>',
  playlist: '<path d="M2 3h13v3H2zm0 6h10v3H2zm0 6h7v3H2zm15-7h5v3h-3v8a4 4 0 11-2-3z"/>',
  rew: '<path d="M12 4L2 12l10 8zm10 0l-10 8 10 8z"/>',
  search: '<path fill-rule="evenodd" d="M10 1a9 9 0 106 16l5 6 3-3-6-5A9 9 0 0010 1zm0 3a6 6 0 110 12 6 6 0 010-12z"/>',
  skipback: '<path d="M2 3h3v18H2zm19 0L6 12l15 9z"/>',
  skipforward: '<path d="M19 3h3v18h-3zM3 3l15 9-15 9z"/>',
  stop: '<rect x="3" y="3" width="18" height="18" rx="1"/>',
};

async function main() {
  let count = 0;
  for (const [name, glyph] of Object.entries(glyphs)) {
    for (const state of ['disabled', 'enabled']) {
      const [w, h] = sizes[name + '_' + state];
      const focused = state === 'enabled', size = 27;
      const svg = `<svg xmlns="http://www.w3.org/2000/svg" width="${w*4}" height="${h*4}" viewBox="0 0 ${w} ${h}">
        <defs><linearGradient id="disc" x2="0" y2="1"><stop stop-color="#41454a"/><stop offset=".48" stop-color="#181a1d"/><stop offset="1" stop-color="#090a0c"/></linearGradient>
        <linearGradient id="metal" x2="0" y2="1"><stop stop-color="${focused?'#f5fbff':'#f2f2f2'}"/><stop offset="1" stop-color="${focused?'#7fb1e5':'#90979e'}"/></linearGradient></defs>
        <ellipse cx="${w/2}" cy="${h/2}" rx="${w/2-3}" ry="${h/2-3}" fill="url(#disc)" stroke="${focused?'#7fb1e5':'#65696e'}" stroke-width="${focused?1.5:.7}"/>
        <g transform="translate(${(w-size)/2},${(h-size)/2}) scale(${size/24})" fill="url(#metal)">${glyph}</g></svg>`;
      fs.writeFileSync(path.join(media, `top_${name}_${state}.png`), await sharp(Buffer.from(svg)).png().toBuffer());
      count++;
    }
  }
  const strip = path.join(media, 'overlay_topbar.png');
  const {width, height} = await sharp(strip).metadata();
  fs.writeFileSync(strip, await sharp(Buffer.from(`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}"><defs><linearGradient id="glass" x2="0" y2="1"><stop stop-color="#25282c" stop-opacity=".94"/><stop offset="1" stop-color="#090a0c" stop-opacity=".94"/></linearGradient></defs><rect width="100%" height="100%" fill="url(#glass)"/><path d="M0 ${height-1}H${width}" stroke="#65696e"/></svg>`)).png().toBuffer());
  console.log(`Rendered ${count} classic Backdrop topbar states at 4x native size.`);
}
main().catch(error => {console.error(error); process.exit(1);});
