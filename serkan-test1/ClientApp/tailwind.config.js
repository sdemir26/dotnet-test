// tailwind.config.js
const path = require("path");
const glob = require("glob"); // glob'u dahil ediyoruz

/** @type {import('tailwindcss').Config} */
module.exports = {
  // Tailwind'in hangi dosyalarda sınıf isimlerini arayacağını belirtin
  content: [
    path.resolve(__dirname, "../wwwroot/**/*.html"), // wwwroot klasöründeki tüm HTML dosyaları (index.html dahil)
    path.resolve(__dirname, "./src/**/*.{html,js,ts,jsx,tsx}"), // src klasöründeki tüm HTML, JS vb. dosyalar
    // node_modules'ü hariç tutarak daha spesifik yollar
    // Bu, performans uyarılarını gidermeye yardımcı olacaktır.
    path.resolve(__dirname, "../Views/**/*.{html,cshtml,razor}"),
    // Aşağıdaki satırın node_modules'ü taramadığından emin olmak için daha dikkatli olun.
    // Eğer projenizin kök dizininde başka HTML/Razor dosyaları varsa ve node_modules'ü içermiyorsa bu kalabilir.
    // Aksi takdirde, bu satırı kaldırabilir veya daha spesifik hale getirebilirsiniz.
    // path.resolve(__dirname, "../**/*.{html,cshtml,razor}"), // Bu satır uyarıya neden olabilir
    
    // Alternatif olarak, glob ile node_modules'ü hariç tutabilirsiniz:
    ...glob.sync(path.join(__dirname, '..', '**', '*.{html,cshtml,razor}'), {
      ignore: [
        path.join(__dirname, '..', 'node_modules', '**', '*')
      ]
    }),
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: "#eff6ff",
          100: "#dbeafe",
          200: "#bfdbfe",
          300: "#93c5fd",
          400: "#60a5fa",
          500: "#3b82f6",
          600: "#2563eb",
          700: "#1d4ed8",
          800: "#1e40af",
          900: "#1e3a8a",
          950: "#172554",
        },
      },
      // RTL desteği için spacing ve margin ayarları
      spacing: {
        rtl: "1rem",
      },
    },
  },
  // Tailwind v3 eklentilerini buraya ekliyoruz
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
    require('@tailwindcss/aspect-ratio'),
    // require('@tailwindcss/line-clamp'), // V3.3+ ile varsayılan olarak dahil olduğu için kaldırıldı
  ],
  // Tailwind sınıflarınızın çakışmaması için özel bir ön ek ekleyin
  prefix: "tw-", // 'tw-' ön eki Bootstrap ile çakışmayı önlemeye yardımcı olur.
  // Önemli: Eğer Tailwind'in dark mode'unu kullanıyorsanız buradan ayarlayın
  darkMode: "class", // veya 'media'
};
