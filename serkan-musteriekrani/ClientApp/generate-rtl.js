// generate-rtl.js
const fs = require('fs');
const path = require('path');
const postcss = require('postcss');
const rtlcss = require('rtlcss');

// Bu betiğin ClientApp içinde olduğunu varsayıyoruz
const WWWROOT_DIR = path.resolve(__dirname, '../wwwroot'); // wwwroot klasörü ClientApp'in bir üst seviyesinde

const inputCssFile = path.resolve(WWWROOT_DIR, 'static/css/app.css'); // LTR CSS dosyası
const outputRtlCssFile = path.resolve(WWWROOT_DIR, 'static/css/app.rtl.css'); // RTL CSS dosyası

// RTL CSS oluşturma
async function generateRTL() {
  try {
    // CSS dosyasını oku
    const css = await fs.promises.readFile(inputCssFile, 'utf8');
    
    // RTLCSS ile dönüştür
    const result = await postcss([
      rtlcss({
        // RTLCSS ayarları
        autoRename: false,
        autoRenameStrict: false,
        blacklist: {
          // Bu sınıfları RTL'e çevirme
          'tw-': true, // Tailwind sınıflarını korur
        },
        plugins: [
          {
            'name': 'Bootstrap RTL Support',
            'priority': 100,
            'directives': {
              'control': {},
              'value': []
            },
            'processors': [
              {
                'name': 'Bootstrap Direction Fix',
                'expr': /direction\s*:\s*(ltr|rtl)/im,
                'action': function(prop, value) {
                  if (value === 'ltr') {
                    return { prop: prop, value: 'rtl' };
                  } else if (value === 'rtl') {
                    return { prop: prop, value: 'ltr' };
                  }
                  return { prop: prop, value: value };
                }
              }
            ]
          }
        ]
      })
    ]).process(css, { 
      from: inputCssFile, 
      to: outputRtlCssFile 
    });

    // RTL CSS'i yaz
    await fs.promises.writeFile(outputRtlCssFile, result.css);
    
    // Source map varsa onu da yaz
    if (result.map) {
      await fs.promises.writeFile(outputRtlCssFile + '.map', result.map.toString());
    }
    
    console.log('✅ RTL CSS dosyası başarıyla oluşturuldu:', outputRtlCssFile);
    
    // RTL CSS'e ek düzeltmeler
    await addRTLFixes();
    
  } catch (error) {
    console.error('❌ RTL CSS oluşturulurken hata:', error);
    process.exit(1);
  }
}

// RTL CSS'e ek düzeltmeler ekle
async function addRTLFixes() {
  try {
    let rtlCss = await fs.promises.readFile(outputRtlCssFile, 'utf8');
    
    // Bootstrap ve Tailwind uyumluluğu için ek kurallar
    const additionalRTLRules = `
/* Additional RTL fixes */
[dir="rtl"] .tw-text-left { text-align: right !important; }
[dir="rtl"] .tw-text-right { text-align: left !important; }
[dir="rtl"] .tw-float-left { float: right !important; }
[dir="rtl"] .tw-float-right { float: left !important; }

/* Bootstrap RTL improvements */
[dir="rtl"] .navbar-toggler {
  margin-left: 0;
  margin-right: auto;
}

[dir="rtl"] .breadcrumb-item + .breadcrumb-item::before {
  float: right;
  padding-right: 0;
  padding-left: 0.5rem;
}

/* Tailwind margin/padding RTL fixes */
[dir="rtl"] .tw-ml-1 { margin-left: 0; margin-right: 0.25rem; }
[dir="rtl"] .tw-mr-1 { margin-right: 0; margin-left: 0.25rem; }
[dir="rtl"] .tw-ml-2 { margin-left: 0; margin-right: 0.5rem; }
[dir="rtl"] .tw-mr-2 { margin-right: 0; margin-left: 0.5rem; }
[dir="rtl"] .tw-ml-3 { margin-left: 0; margin-right: 0.75rem; }
[dir="rtl"] .tw-mr-3 { margin-right: 0; margin-left: 0.75rem; }
[dir="rtl"] .tw-ml-4 { margin-left: 0; margin-right: 1rem; }
[dir="rtl"] .tw-mr-4 { margin-right: 0; margin-left: 1rem; }

[dir="rtl"] .tw-pl-1 { padding-left: 0; padding-right: 0.25rem; }
[dir="rtl"] .tw-pr-1 { padding-right: 0; padding-left: 0.25rem; }
[dir="rtl"] .tw-pl-2 { padding-left: 0; padding-right: 0.5rem; }
[dir="rtl"] .tw-pr-2 { padding-right: 0; padding-left: 0.5rem; }
[dir="rtl"] .tw-pl-3 { padding-left: 0; padding-right: 0.75rem; }
[dir="rtl"] .tw-pr-3 { padding-right: 0; padding-left: 0.75rem; }
[dir="rtl"] .tw-pl-4 { padding-left: 0; padding-right: 1rem; }
[dir="rtl"] .tw-pr-4 { padding-right: 0; padding-left: 1rem; }
`;
    
    rtlCss += additionalRTLRules;
    
    await fs.promises.writeFile(outputRtlCssFile, rtlCss);
    console.log('✅ RTL CSS düzeltmeleri eklendi');
    
  } catch (error) {
    console.error('❌ RTL düzeltmeleri eklenirken hata:', error);
  }
}

// Betiği çalıştır
generateRTL();