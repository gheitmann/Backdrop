// Preserve cul8er's actual Backdrop topbar artwork, not a redrawn approximation.
const fs = require('node:fs'), path = require('node:path'), crypto = require('node:crypto');
const {execFileSync} = require('node:child_process');
const root = path.resolve(__dirname, '..');
const fidelity = require('../Source/Artwork/native-fidelity.json');
const names = Object.keys(fidelity.files).filter(n => n.startsWith('top_') || n === 'overlay_topbar.png');
if (names.length !== 31) throw new Error('Expected all 30 original topbar states and their backing strip.');
for (const name of names) {
  const target = path.join(root, 'skin/Backdrop/Media', name);
  const digest = data => crypto.createHash('sha256').update(data).digest('hex');
  if (fs.existsSync(target) && digest(fs.readFileSync(target)) === fidelity.files[name].sha256) continue;
  // Shallow checkouts already contain the reviewed assets. Recovery of an
  // overwritten file needs the provenance commit; fail rather than redraw it.
  const original = execFileSync('git', ['show', `${fidelity.source_commit}:skin/Backdrop/Media/${name}`], {cwd: root});
  if (digest(original) !== fidelity.files[name].sha256) throw new Error('Original asset checksum differs: ' + name);
  fs.writeFileSync(target, original);
}
console.log(`PASS: ${names.length} original Backdrop topbar textures preserved byte-for-byte.`);
