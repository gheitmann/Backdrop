// Generate MP1 control textures directly from Lucide vectors and original matte SVG surfaces.
const fs = require('node:fs'), path = require('node:path'), sharp = require('sharp');
const {execFileSync} = require('node:child_process');
const root = path.resolve(__dirname, '..'), media = path.join(root, 'skin/Backdrop/Media');
const inventory = JSON.parse(fs.readFileSync(path.join(root,'Source/Artwork/control-textures.json'),'utf8'));
const reportPath = path.join(root,'Source/Artwork/control-render-report.json');
const previous = fs.existsSync(reportPath) ? JSON.parse(fs.readFileSync(reportPath,'utf8')) : {};
const fidelityPath = path.join(root, 'Source/Artwork/native-fidelity.json');
const fidelity = fs.existsSync(fidelityPath) ? JSON.parse(fs.readFileSync(fidelityPath, 'utf8')) : {files:{}};
const menuOnly = process.argv.includes('--menu-only');
const report = menuOnly ? {...previous} : {};
const isMenu = name => /^(?:hiddenmenu_|dropdown_|menu[._]|dialog_)/i.test(name);
const tracked = new Map(execFileSync('git',['ls-tree','-r','--name-only','HEAD'],{cwd:root}).toString().trim().split(/\r?\n/).map(name=>[name.toLowerCase(),name]));
function body(icon) {
  const svg = fs.readFileSync(path.join(root,'Source/Artwork/lucide',icon+'.svg'),'utf8');
  return svg.slice(svg.indexOf('>',svg.indexOf('<svg'))+1,svg.lastIndexOf('</svg>'));
}
async function main() {
  for (const [name, spec] of Object.entries(inventory)) {
    // Texture names are not semantic icon assignments: e.g. nowplaying.overlay
    // is a full-screen composition, and clock.png is a clock backing plate.
    if (fidelity.files[name]) { report[name] = previous[name]; continue; }
    if (['content','topbar'].includes(spec.kind)) continue;
    if (menuOnly && !isMenu(name)) continue;
    const file = path.join(media,name);
    const sourceName = tracked.get(('skin/Backdrop/Media/'+name).toLowerCase());
    if (!sourceName) throw new Error('No original texture for '+name);
    const original = execFileSync('git',['show','HEAD:'+sourceName],{cwd:root,maxBuffer:32*1024*1024});
    const metadata = await sharp(original).metadata();
    const native = previous[name]?.native || [metadata.width,metadata.height];
    const [nw,nh] = native;
    let width=nw, height=nh;
    if (['icon','mark','speed','placeholder'].includes(spec.kind)) {
      const scale=Math.max(2,96/Math.min(nw,nh));
      width=Math.round(nw*scale); height=Math.round(nh*scale);
    } else {
      // Preserve aspect ratio: SVG's default meet transform otherwise letterboxes rows.
      const scale=Math.max(1,Math.min(4,96/Math.min(nw,nh),4096/Math.max(nw,nh)));
      width=Math.round(nw*scale); height=Math.round(nh*scale);
    }
    const focus=spec.focus, active=spec.active;
    let color=active?'#74d5ff':focus?'#d0f3ff':'#c9d5e2';
    if (spec.status) color={red:'#ef6f7b',green:'#68d6a6',yellow:'#e8c36c',grey:'#738291'}[spec.status];
    const stroke=focus?'#69cfff':'#3a5064';
    let canvas='';
    if(spec.kind==='timeline') {
      // Native guide control 25 has no explicit width; retain its intrinsic size.
      width=nw; height=nh;
      canvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}"><path d="M${width/2} 1V${height-1}" stroke="#74d5ff" stroke-width="2"/><path d="M${width/2-3} 1h6M${width/2-3} ${height-1}h6" stroke="#74d5ff" stroke-width="2"/></svg>`;
    } else if (spec.kind==='surface') {
      // Fixed-size border/radius in the native coordinate space prevents enlarged bevel pixels.
      const nativeCanvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}" viewBox="0 0 ${nw} ${nh}"><rect x=".75" y=".75" width="${Math.max(1,nw-1.5)}" height="${Math.max(1,nh-1.5)}" rx="${Math.min(5,nh/8,nw/8)}" fill="${focus?'#1b3b53':'#101c2a'}" fill-opacity="${focus?'.98':'.88'}" stroke="${focus?stroke:'#263748'}" stroke-width="${focus?'1.5':'.7'}"/>${focus?`<path d="M2 3V${nh-3}" stroke="#72d5ff" stroke-width="2"/>`:''}</svg>`;
      // MP draws checkbutton state through textureCheckmark separately. An embedded
      // tick in an "on"-named background would appear even while unchecked.
      canvas=nativeCanvas;
      if(active) canvas=canvas.replace(/#101c2a/g,'#132d3f').replace(/#1b3b53/g,'#214760');
      if(spec.status==='red') canvas=canvas.replace(/#1b3b53|#101c2a/g,focus?'#59313f':'#35232e').replace(/#69cfff|#263748/g,'#ef6f7b');
      if(isMenu(name)) {
        // Backdrop's original menus are near-black with restrained grey focus,
        // not cyan panels. Keep the new scalable artwork and separate state icons.
        const panel=/menu_selected_bg|menu_bg|dialogmenu|dialog_background/i.test(name);
        const selected=focus&&!panel;
        const fill=selected?'url(#menuFocus)':active?'#181a1d':'#08090b';
        canvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}" viewBox="0 0 ${nw} ${nh}"><defs><linearGradient id="menuFocus" x2="0" y2="1"><stop stop-color="${active?'#41464c':'#363b41'}"/><stop offset=".48" stop-color="#24282d"/><stop offset="1" stop-color="#141619"/></linearGradient></defs><rect x=".6" y=".6" width="${Math.max(1,nw-1.2)}" height="${Math.max(1,nh-1.2)}" rx="${Math.min(1,nh/12,nw/12)}" fill="${fill}" fill-opacity="${selected?'.98':'.94'}" stroke="${selected?'#92989f':panel?'#555a60':'#25272a'}" stroke-width="${selected?'1':'.65'}"/></svg>`;
      }
    } else if(spec.kind==='track') {
      canvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}"><rect width="100%" height="100%" rx="2" fill="#142334" stroke="#3a5064"/></svg>`;
    } else if(spec.kind==='overlay') {
      canvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}"><rect width="100%" height="100%" fill="#080f1a" fill-opacity=".60"/></svg>`;
    } else {
      const size=Math.min(width,height)*.72, x=(width-size)/2, y=(height-size)/2;
      const isMark=spec.kind==='mark';
      const back=isMark?'':`<rect x="1" y="1" width="${width-2}" height="${height-2}" rx="${Math.min(18,height*.18,width*.18)}" fill="#102033" fill-opacity="${spec.kind==='placeholder'?'.88':'.96'}" stroke="${stroke}" stroke-width="${focus?3:1.5}"/>`;
      let glyph=body(spec.icon);
      if(spec.checkbox) glyph=(active?'<rect x="3" y="3" width="18" height="18" rx="3"/>':'')+glyph;
      const fill=spec.icon==='star'&&active?color:'none';
      const text=spec.text?`<text x="${width/2}" y="${height*.93}" fill="${color}" text-anchor="middle" font-family="Arial" font-size="${height*.19}">${spec.text}</text>`:'';
      canvas=`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}">${back}<g transform="translate(${x},${y}) scale(${size/24})" fill="${fill}" stroke="${color}" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">${glyph}</g>${text}</svg>`;
    }
    const renderer=sharp(Buffer.from(canvas));
    const png=await (name.toLowerCase().endsWith('.gif')?renderer.gif():renderer.png()).toBuffer();
    fs.writeFileSync(file,png);
    report[name]={...spec,native,rendered:[width,height]};
  }
  fs.writeFileSync(reportPath,JSON.stringify(report,null,2)+'\n');
  console.log(`Rendered ${Object.keys(report).length} control textures from SVG; topbar handled separately.`);
}
main().catch(error=>{console.error(error);process.exit(1);});
