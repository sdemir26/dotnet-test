// postcss.config.js
module.exports = {
  plugins: {
    // Tailwind CSS v3 için bu şekilde kullanılır
    'tailwindcss/nesting': {}, // Tailwind CSS v3 için nesting desteği
    'tailwindcss': {
      // Tailwind v3 için config dosyasını buradan belirtebilirsiniz
      config: './tailwind.config.js'
    },
    'autoprefixer': {},
    'postcss-flexbugs-fixes': {},
    'postcss-preset-env': {
      autoprefixer: {
        flexbox: 'no-2009',
      },
      stage: 3,
    },
    'postcss-normalize': {},
    // RTL dönüşümü için bu satırı webpack sürecinde değil,
    // ayrı bir işlemde yapacağız (generate-rtl.js)
    // 'postcss-rtlcss': {},  // Eğer PostCSS zincirinde RTL dönüşümü yapmak isterseniz etkinleştirin
  },
};