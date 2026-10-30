// Decode every generated UI asset and preview the states visible in Settings/menus/EPG.
const fs=require('node:fs'), path=require('node:path'), sharp=require('sharp');
const root=path.resolve(__dirname,'..'), media=path.join(root,'skin/Backdrop/Media');
const report=JSON.parse(fs.readFileSync(path.join(root,'Source/Artwork/control-render-report.json')));
const fidelity=JSON.parse(fs.readFileSync(path.join(root,'Source/Artwork/native-fidelity.json')));
async function main(){
  for(const [name,entry] of Object.entries(report)){
    const data=fs.readFileSync(path.join(media,name)), metadata=await sharp(data).metadata();
    if(metadata.width>4096 || metadata.height>4096) throw new Error('Oversized texture: '+name);
    if(metadata.width!==entry.rendered[0] || metadata.height!==entry.rendered[1]) throw new Error('Unexpected dimensions: '+name);
    if(name.endsWith('.gif') && metadata.format!=='gif') throw new Error('GIF signature mismatch: '+name);
    if(entry.kind==='timeline' && (metadata.width!==entry.native[0] || metadata.height!==entry.native[1])) throw new Error('Native guide timeline size changed: '+name);
  }
  const topbar=JSON.parse(fs.readFileSync(path.join(root,'Source/Artwork/topbar-native-sizes.json')));
  for(const [name,[width,height]] of Object.entries(topbar)){
    const metadata=await sharp(path.join(media,'top_'+name+'.png')).metadata();
    const native = fidelity.files['top_'+name+'.png'];
    const expected = native ? native.dimensions : [width*4,height*4];
    if(metadata.width!==expected[0] || metadata.height!==expected[1]) throw new Error('Wrong topbar raster size: '+name);
  }
  const pairs=[['check-box.png','check-boxNF.png'],['togglebutton_on.png','togglebutton_off.png'],
    ['hiddenmenu_togglebutton_on.png','hiddenmenu_togglebutton_off.png'],
    ['listbg_togglebutton_on_fo.png','listbg_togglebutton_off_fo.png'],
    ['top_skipback_enabled.png','top_back_enabled.png'],
    ['hiddenmenu_button_no_fo.png','hiddenmenu_button_fo.png'],
    ['tvguide_button_middle.png','tvguide_button_selected_middle.png']];
  for(const [a,b] of pairs) if(fs.readFileSync(path.join(media,a)).equals(fs.readFileSync(path.join(media,b)))) throw new Error('Indistinguishable states: '+a+' / '+b);
  const crypto = require('node:crypto');
  for (const [name, spec] of Object.entries(fidelity.files)) {
    const data = fs.readFileSync(path.join(media, name));
    if (crypto.createHash('sha256').update(data).digest('hex') !== spec.sha256) throw new Error('Native semantic artwork overwritten: ' + name);
    const metadata = await sharp(data).metadata();
    if (metadata.width !== spec.dimensions[0] || metadata.height !== spec.dimensions[1]) throw new Error('Native artwork dimensions changed: ' + name);
  }
  if (process.argv.includes('--verify-only')) {
    console.log(`PASS: ${Object.keys(report).length} decoded textures, ${Object.keys(fidelity.files).length} preserved native assets; no preview generated.`);
    return;
  }
  const names=['check-boxNF.png','check-box.png','top_skipback_disabled.png','top_back_disabled.png',
    'hiddenmenu_togglebutton_off.png','hiddenmenu_togglebutton_on.png',
    'hiddenmenu_button_no_fo.png','hiddenmenu_button_fo.png',
    'tv_group_bg.png','tv_group_bg_focus.png','tvguide_button_middle.png','tvguide_button_selected_middle.png'];
  const width=1200,height=650, layers=[];
  for(let i=0;i<names.length;i++){
    const left=(i%2)*600,top=35+Math.floor(i/2)*100;
    const label=Buffer.from(`<svg width="590" height="28"><text x="16" y="21" fill="#c9d5e2" font-family="Arial" font-size="17">${names[i]}</text></svg>`);
    const asset=await sharp(path.join(media,names[i])).resize(550,58,{fit:'inside'}).png().toBuffer();
    layers.push({input:label,left,top},{input:asset,left:left+20,top:top+32});
  }
  fs.mkdirSync(path.join(root,'dist'),{recursive:true});
  await sharp({create:{width,height,channels:4,background:'#080f1a'}}).composite(layers).png().toFile(path.join(root,'dist/control-artwork-preview.png'));
  console.log(`PASS: ${Object.keys(report).length} generated textures decoded; ${pairs.length} critical state pairs differ. Preview: dist/control-artwork-preview.png`);
}
main().catch(e=>{console.error(e);process.exit(1)});
