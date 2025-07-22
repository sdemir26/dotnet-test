module.exports = {
  root: true,
  env: {
    browser: true,
    es2021: true,
    node: true,
  },
  extends: [
    'eslint:recommended',
  ],
  parserOptions: {
    ecmaVersion: 12,
    sourceType: 'module',
  },
  rules: {
    // İhtiyaçlarınıza göre kuralları özelleştirebilirsiniz
    'no-console': 'warn', // console.log kullanımına uyarı ver
    'no-unused-vars': 'warn', // Kullanılmayan değişkenlere uyarı ver
  },
};