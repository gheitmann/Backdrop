// Deterministic export of image_gen photographs and code-native vector branding.
// NODE_PATH must expose sharp. No network fetching and no generated UI text.
const fs = require('fs');
const path = require('path');
const crypto = require('crypto');
const sharp = require('sharp');
const root = path.resolve(__dirname, '..');
const args = process.argv.slice(2);
function arg(name, fallback) {
  const index = args.indexOf(name);
  return index < 0 ? fallback : args[index + 1];
}
const width = Number(arg('--width', '1920'));
const height = Number(arg('--height', '1080'));
const output = path.resolve(arg('--output', path.join(root, 'skin/Backdrop/Media')));
if (!Number.isInteger(width) || !Number.isInteger(height) || width < height || width < 1280 || height < 720 || width > 7680 || height > 4320)
  throw new Error('Use a supported landscape target resolution.');
const master = path.join(root, 'Source/Artwork/cinematic-night');
const hash = data => crypto.createHash('sha256').update(data).digest('hex');
async function main() {
  fs.mkdirSync(path.join(output, 'Modern'), {recursive:true});
  const photos = {};
  for (const name of ['cinema', 'tv', 'studio', 'photos', 'gaming', 'plugins']) {
    const source = path.join(master, name + '.png');
    const metadata = await sharp(source).metadata();
    // No stretching: cover-fit with a fixed central crop. Masters remain untouched.
    const jpg = await sharp(source).resize(width, height, {fit:'cover', position:'centre'}).jpeg({quality:94, chromaSubsampling:'4:4:4'}).toBuffer();
    fs.writeFileSync(path.join(output, 'Modern', name + '.jpg'), jpg);
    photos[name] = {file:'Modern/'+name+'.jpg', sha256:hash(jpg), source:'Source/Artwork/cinematic-night/'+name+'.png', source_sha256:hash(fs.readFileSync(source)), source_dimensions:[metadata.width,metadata.height], export_dimensions:[width,height]};
  }
  // Generic/home fallback follows the approved cinema scene as well.
  fs.copyFileSync(path.join(output, 'Modern/cinema.jpg'), path.join(output, 'Modern/home.jpg'));
  const background = await sharp(path.join(master, 'cinema.png')).resize(width,height,{fit:'cover',position:'centre'}).toBuffer();
  const logoWidth = Math.round(height * (1920 * .34 / 1080));
  const logo = await sharp(path.join(master,'backdrop-logo.svg'), {density:Math.max(96, 72*logoWidth/700)}).resize(logoWidth).png().toBuffer();
  const splash = await sharp(background).composite([{input:logo, left:Math.round(width*.11), top:Math.round(height*.32)}]).png().toBuffer();
  fs.writeFileSync(path.join(output,'splashscreen.png'),splash);
  const manifest = {collection:'cinematic-night',generated_with:'OpenAI image_gen',date:'2026-10-25',notes:'Photographic masters are 1672x941 from the built-in image tool; larger exports are resampled, not native 4K. Wordmark is rasterized from SVG at target resolution. No baked loading percentage.',photos,splash:{sha256:hash(splash),dimensions:[width,height],logo_source:'Source/Artwork/cinematic-night/backdrop-logo.svg'}};
  if (output === path.join(root,'skin/Backdrop/Media')) {
    fs.writeFileSync(path.join(root,'Source/Artwork/cinematic-night.json'),JSON.stringify(manifest,null,2)+'\n');
    fs.writeFileSync(path.join(root,'Source/Artwork/home-backgrounds.json'),JSON.stringify(manifest,null,2)+'\n');
  } else fs.writeFileSync(path.join(output,'cinematic-night-export.json'),JSON.stringify(manifest,null,2)+'\n');
  console.log('Exported six categories, home fallback and branded splash at '+width+'x'+height+' to '+output);
}
main().catch(error=>{console.error(error);process.exitCode=1;});
