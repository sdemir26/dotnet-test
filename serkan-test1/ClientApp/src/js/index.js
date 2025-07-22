// src/index.js

// CSS dosyasını içe aktararak Webpack'in onu işlemesini sağlayın

import '../styles/tailwind.css';
import '../styles/bootstrap.scss';

// Bootstrap JavaScript'ini import et
import 'bootstrap';
// Bootstrap JavaScript bileşenlerini dahil edin (isteğe bağlı, sadece CSS kullanıyorsanız gerekmez)
// import 'bootstrap'; // Tüm Bootstrap JS'i dahil eder
// import 'bootstrap/dist/js/bootstrap.bundle.min.js'; // Sadece bundle olanı dahil eder (önerilir)

// 4. Kendi JavaScript kodlarınızı buraya ekleyebilirsiniz
console.log('Webpack + Bootstrap + Tailwind başarıyla yüklendi!');



document.addEventListener('DOMContentLoaded', () => {
    const ltrBtn = document.getElementById('ltrBtn');
    const rtlBtn = document.getElementById('rtlBtn');
    const htmlElement = document.documentElement; // html etiketi

    // Başlangıçta dir niteliğini ayarla
    // htmlElement.setAttribute('dir', 'ltr'); // Varsayılanı 'ltr' olarak ayarla

    if (ltrBtn) {
        ltrBtn.addEventListener('click', () => {
            htmlElement.setAttribute('dir', 'ltr');
            document.body.classList.remove('rtl'); // Varsa rtl sınıfını kaldır
            console.log('Switched to LTR');
        });
    }

    if (rtlBtn) {
        rtlBtn.addEventListener('click', () => {
            htmlElement.setAttribute('dir', 'rtl');
            document.body.classList.add('rtl'); // rtl sınıfını ekle
            console.log('Switched to RTL');
        });
    }

    // Konsol logu sadece geliştirme amaçlıdır, ESLint uyarısını gidermek için kaldırılabilir
    console.log('App loaded successfully!');
});