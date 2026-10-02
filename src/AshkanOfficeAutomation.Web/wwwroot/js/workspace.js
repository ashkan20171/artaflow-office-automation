(() => {
  const fa=document.documentElement.lang==='fa';
  const $=(s)=>document.querySelector(s);
  const store={
    get(k,d){try{return JSON.parse(localStorage.getItem(k))??d}catch{return d}},
    set(k,v){localStorage.setItem(k,JSON.stringify(v))}
  };
  const path=location.pathname+location.search;
  const title=()=>document.querySelector('h1,h2,.page-title')?.textContent?.trim()||document.title.split('|')[0].trim();
  if(location.pathname!=='/Account/Login'){
    let recent=store.get('ashkan.recent',[]).filter(x=>x.path!==path);
    recent.unshift({path,title:title(),at:Date.now()});
    store.set('ashkan.recent',recent.slice(0,8));
  }
  const favorites=()=>store.get('ashkan.favorites',[]);
  function render(){
    const fav=favorites(), recent=store.get('ashkan.recent',[]);
    const f=$('#workspaceFavorites'),r=$('#workspaceRecent');
    if(f) f.innerHTML=fav.length?fav.map(x=>`<a href="${esc(x.path)}">${esc(x.title)}</a>`).join(''):`<span class="workspace-empty">${fa?'هنوز صفحه‌ای ذخیره نشده':'No favorites yet'}</span>`;
    if(r) r.innerHTML=recent.length?recent.map(x=>`<a href="${esc(x.path)}">${esc(x.title)}</a>`).join(''):`<span class="workspace-empty">${fa?'هنوز سابقه‌ای وجود ندارد':'No recent pages'}</span>`;
    const b=$('#favoriteCurrent'); if(b){const on=fav.some(x=>x.path===path);b.classList.toggle('active',on);b.textContent=on?'★':'☆';b.title=on?(fa?'حذف از علاقه‌مندی‌ها':'Remove favorite'):(fa?'افزودن به علاقه‌مندی‌ها':'Add favorite')}
  }
  function esc(s){return String(s).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]))}
  $('#favoriteCurrent')?.addEventListener('click',()=>{
    let a=favorites(),i=a.findIndex(x=>x.path===path);
    if(i>=0)a.splice(i,1);else a.unshift({path,title:title()});
    store.set('ashkan.favorites',a.slice(0,12));render();
  });
  $('#workspaceToggle')?.addEventListener('click',()=>$('#workspacePanel')?.classList.toggle('open'));
  $('#workspaceClose')?.addEventListener('click',()=>$('#workspacePanel')?.classList.remove('open'));
  $('#focusToggle')?.addEventListener('click',()=>{document.body.classList.toggle('focus-mode');store.set('ashkan.focus',document.body.classList.contains('focus-mode'))});
  $('#densityToggle')?.addEventListener('click',()=>{document.body.classList.toggle('compact-ui');store.set('ashkan.compact',document.body.classList.contains('compact-ui'))});
  if(store.get('ashkan.focus',false))document.body.classList.add('focus-mode');
  if(store.get('ashkan.compact',false))document.body.classList.add('compact-ui');
  document.addEventListener('keydown',e=>{
    if((e.ctrlKey||e.metaKey)&&e.shiftKey&&e.key.toLowerCase()==='f'){e.preventDefault();$('#favoriteCurrent')?.click()}
    if((e.ctrlKey||e.metaKey)&&e.shiftKey&&e.key.toLowerCase()==='b'){e.preventDefault();$('#workspaceToggle')?.click()}
    if(e.key==='Escape')$('#workspacePanel')?.classList.remove('open');
  });
  render();
})();