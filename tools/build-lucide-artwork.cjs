// Rasterize upstream Lucide SVGs for MP1's PNG texture renderer.
// Usage: NODE_PATH=<directory containing sharp> node tools/build-lucide-artwork.cjs
const fs = require('node:fs');
const path = require('node:path');
const sharp = require('sharp');
const {execFileSync} = require('node:child_process');
const root = path.resolve(__dirname, '..');
const media = path.join(root, 'skin/Backdrop/Media');
const sources = JSON.parse(fs.readFileSync(path.join(root, 'Source/Artwork/sources.json'), 'utf8'));
const sizesPath=path.join(root,'Source/Artwork/topbar-native-sizes.json');
const nativeSizes=fs.existsSync(sizesPath)?JSON.parse(fs.readFileSync(sizesPath,'utf8')):{};

async function main() {
  let count = 0;
  for (const [texture, icon] of Object.entries(sources.icons)) {
    const svg = fs.readFileSync(path.join(root, 'Source/Artwork/lucide', icon + '.svg'), 'utf8');
    const body = svg.slice(svg.indexOf('>', svg.indexOf('<svg')) + 1, svg.lastIndexOf('</svg>'));
    for (const state of ['disabled', 'enabled']) {
      const target = path.join(media, `top_${texture}_${state}.png`);
      const original=execFileSync('git',['show',`HEAD:skin/Backdrop/Media/top_${texture}_${state}.png`],{cwd:root});
      const metadata = await sharp(original).metadata();
      const [width,height]=nativeSizes[texture+'_'+state] || [metadata.width,metadata.height];
      nativeSizes[texture+'_'+state]=[width,height];
      // Four-times raster resolution keeps vectors crisp on 1440p/4K outputs.
      // The viewBox retains the original icon proportions and stroke weights.
      const focus = state === 'enabled'; // Native filename means focus, not IsEnabled.
      const size = 38;
      const x = (width - size) / 2, y = (height - size) / 2;
      const background = focus ? '#15364a' : '#111b27';
      const border = focus ? '#61c9f3' : '#576474';
      const color = focus ? '#c6efff' : '#e0e8ef';
      const canvas = `<svg xmlns="http://www.w3.org/2000/svg" width="${width*4}" height="${height*4}" viewBox="0 0 ${width} ${height}">
        <rect x="4" y="4" width="${width-8}" height="${height-8}" rx="14" fill="${background}" fill-opacity=".95" stroke="${border}" stroke-width="${focus ? 2 : 1}"/>
        <g transform="translate(${x},${y}) scale(${size/24})" fill="none" stroke="${color}" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">${body}</g></svg>`;
      const png = await sharp(Buffer.from(canvas)).png().toBuffer();
      fs.writeFileSync(target, png);
      count++;
    }
  }
  // Replace the chrome strip with a quiet, translucent surface; retain its dimensions.
  const strip = path.join(media, 'overlay_topbar.png');
  const {width, height} = await sharp(strip).metadata();
  fs.writeFileSync(strip, await sharp(Buffer.from(`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}"><rect width="100%" height="100%" fill="#0b111b" fill-opacity=".96"/><path d="M0 ${height-1}H${width}" stroke="#344c60"/></svg>`)).png().toBuffer());
  fs.writeFileSync(sizesPath,JSON.stringify(nativeSizes,null,2)+'\n');
  console.log(`Rendered ${count} Lucide focus-state PNGs at 4x resolution and topbar surface; original proportions retained.`);
}
main().catch(error => { console.error(error); process.exit(1); });
