(() => {
 const fa=document.documentElement.lang==='fa';
 const T=(faText,enText)=>fa?faText:enText;
 const commands=[
  [T('نامه جدید','New letter'),'/Letters/Create','✉'],[T('کارتابل من','My inbox'),'/Inbox','▣'],
  [T('وظایف','Tasks'),'/Tasks','✓'],[T('جلسات','Meetings'),'/Meetings','▦'],
  [T('جست‌وجوی سراسری','Global search'),'/GlobalSearch','⌕'],[T('دستیار هوشمند','AI Copilot'),'/AiCopilot','✦'],
  [T('اعلان‌ها','Notifications'),'/Notifications','◉'],[T('مرکز فرماندهی','Command Center'),'/CommandCenter','⌁'],
  [T('مرکز دانش','Knowledge base'),'/Knowledge','◇'],[T('امنیت حساب','Account security'),'/Security','⚿']
 ];
 let overlay;
 const esc=s=>String(s).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
 function close(){overlay?.remove();overlay=null;}
 function open(){if(overlay)return;overlay=document.createElement('div');overlay.className='cmd-overlay';
  overlay.innerHTML=`<div class="cmd-box" role="dialog" aria-modal="true" aria-label="${T('فرمان سریع','Quick command')}"><div class="cmd-input"><span>⌕</span><input aria-label="${T('فرمان سریع','Quick command')}" placeholder="${T('جست‌وجو یا اجرای فرمان...','Search or run a command...')}"><kbd>Esc</kbd></div><div class="cmd-list"></div><div class="cmd-foot">${T('Enter اجرا · Esc بستن · Ctrl+K باز کردن','Enter run · Esc close · Ctrl+K open')}</div></div>`;
  document.body.appendChild(overlay);const input=overlay.querySelector('input'),list=overlay.querySelector('.cmd-list');
  const render=()=>{const raw=input.value.trim(),q=raw.toLowerCase();const rows=commands.filter(x=>x[0].toLowerCase().includes(q));list.innerHTML=rows.map(x=>`<a href="${x[1]}"><i>${x[2]}</i><span>${esc(x[0])}</span><small>${T('باز کردن','Open')}</small></a>`).join('')+(raw?`<a class="cmd-search" href="/GlobalSearch?q=${encodeURIComponent(raw)}"><i>⌕</i><span>${T('جست‌وجوی','Search')} «${esc(raw)}»</span><small>${T('همه ماژول‌ها','All modules')}</small></a>`:'');};
  input.addEventListener('input',render);input.addEventListener('keydown',e=>{if(e.key==='Enter'&&input.value.trim())location.href='/GlobalSearch?q='+encodeURIComponent(input.value.trim())});overlay.addEventListener('click',e=>{if(e.target===overlay)close()});render();setTimeout(()=>input.focus(),0);
 }
 document.addEventListener('keydown',e=>{if((e.ctrlKey||e.metaKey)&&e.key.toLowerCase()==='k'){e.preventDefault();open()}else if(e.key==='Escape')close()});
 document.addEventListener('click',e=>{if(e.target.closest('[data-command-palette]')){e.preventDefault();open()}});
})();