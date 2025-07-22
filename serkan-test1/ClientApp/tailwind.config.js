// tailwind.config.js
const path = require("path");

/** @type {import('tailwindcss').Config} */
module.exports = {
  // Tailwind'in hangi dosyalarda sınıf isimlerini arayacağını belirtin
  content: [
    path.resolve(__dirname, "../wwwroot/**/*.html"), // wwwroot klasöründeki tüm HTML dosyaları (index.html dahil)
    path.resolve(__dirname, "./src/**/*.{html,js,ts,jsx,tsx}"), // src klasöründeki tüm HTML, JS vb. dosyalar
    path.resolve(__dirname, "../Views/**/*.{html,cshtml,razor}"),
    path.resolve(__dirname, "../**/*.{html,cshtml,razor}"),
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
  plugins: [], // V4'te plugin'ler buraya eklenmez, PostCSS'e entegre edilir
  // Tailwind sınıflarınızın çakışmaması için özel bir ön ek ekleyin
 // prefix: "tw", // 'tw-' ön eki Bootstrap ile çakışmayı önlemeye yardımcı olur.
  // Önemli: Eğer Tailwind'in dark mode'unu kullanıyorsanız buradan ayarlayın
  darkMode: "class", // veya 'media'
};
