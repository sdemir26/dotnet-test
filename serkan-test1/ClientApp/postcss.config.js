// postcss.config.js
module.exports = {
  plugins: {
    '@tailwindcss/postcss': {
      // Tailwind v4 için config dosyasını buradan belirtebilirsiniz
      config: './tailwind.config.js'
    }, // Tailwind CSS v4 için bu şekilde kullanılır ve çok önemlidir!
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