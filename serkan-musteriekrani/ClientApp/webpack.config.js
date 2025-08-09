// webpack.config.js
const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const ESLintWebpackPlugin = require("eslint-webpack-plugin");
const { BundleAnalyzerPlugin } = require("webpack-bundle-analyzer");
const { PurgeCSSPlugin } = require("purgecss-webpack-plugin");
const glob = require("glob"); // glob'u dahil ediyoruz
const Dotenv = require("dotenv-webpack");

// Yeni proje yapısına göre dizin yolları
const PROJECT_ROOT = __dirname; // ClientApp klasörü
const SRC_DIR = path.resolve(PROJECT_ROOT, "src");
const WWWROOT_DIR = path.resolve(PROJECT_ROOT, "../wwwroot"); // wwwroot klasörü ClientApp'in bir üst seviyesinde
const NODE_MODULES_DIR = path.resolve(PROJECT_ROOT, "node_modules");

const isDev = process.env.NODE_ENV === "development";
const useAnalyzer = process.env.USE_ANALYZE === "true";

let dotEnvFileName = ".env.development";
if (process.env.BUILD_GOAL) {
  dotEnvFileName = `.env.${process.env.BUILD_GOAL}`;
}

const config = {
  mode: isDev ? "development" : "production",
  entry: {
    app: path.resolve(SRC_DIR, "js", "index.js"), // js klasörü altına taşındığı için yol güncellendi
  },
  output: {
    path: WWWROOT_DIR, // Çıktı klasörü artık wwwroot
    publicPath: "/", // .NET Core MVC projesi için genellikle bu / olmalıdır
    filename: "static/js/[name].js", // wwwroot/static/js altına
    chunkFilename: "static/js/[name].js",
    //clean: true, // Her build öncesi dist klasörünü temizler (şimdi wwwroot'u temizleyecek)
  },
  resolve: {
    extensions: [".js", ".json", ".css", ".scss", "..."],
    alias: {
      "~bootstrap": path.resolve(NODE_MODULES_DIR, "bootstrap"),
    },
    symlinks: false,
  },
  stats: isDev ? "errors-warnings" : "normal",
  devtool: isDev ? "eval-cheap-module-source-map" : false,

  devServer: isDev
      ? {
        allowedHosts: "all",
        historyApiFallback: true,
        client: {
          logging: "error",
          progress: true,
          overlay: {
            errors: true,
            warnings: false,
          },
        },
        static: {
          // Development server, public klasöründeki statik dosyaları servis eder
          directory: WWWROOT_DIR,
        },
        compress: false, // Burayı false yaptık, önceki hatayı gidermek için
        open: false,
        hot: true,
        port: 8080,
      }
      : undefined,

  plugins: [
    // wwwroot klasörünü temizler
    new CleanWebpackPlugin({
      cleanOnceBeforeBuildPatterns: [WWWROOT_DIR + "/static/**/*"],
      dangerouslyAllowCleanPatternsOutsideProject: true,
      dry: false,
    }),
    new Dotenv({
      path: path.resolve(PROJECT_ROOT, dotEnvFileName),
    }),
    new ESLintWebpackPlugin({
      context: SRC_DIR,
      exclude: NODE_MODULES_DIR,
      cache: true,
      cacheLocation: path.resolve(NODE_MODULES_DIR, ".cache/.eslintcache"),
      extensions: ["js"],
    }),
    new MiniCssExtractPlugin({
      filename: "static/css/[name].css",
      chunkFilename: "static/css/[id].css",
      ignoreOrder: true,
    }),
    new CopyWebpackPlugin({
      patterns: [
        // src/fonts klasörünü wwwroot/static/fonts'a kopyalar
        {
          from: path.resolve(SRC_DIR, "fonts"),
          to: path.resolve(WWWROOT_DIR, "static", "fonts"),
          noErrorOnMissing: true,
        },
        // src/images klasörünü wwwroot/static/images'a kopyalar
        {
          from: path.resolve(SRC_DIR, "images"),
          to: path.resolve(WWWROOT_DIR, "static", "images"),
          noErrorOnMissing: true,
        },
        // src/libs klasörünü wwwroot/static/libs'e kopyalar
        {
          from: path.resolve(SRC_DIR, "libs"),
          to: path.resolve(WWWROOT_DIR, "static", "libs"),
          noErrorOnMissing: true,
        },
      ],
    }),
    // PurgeCSSPlugin'i sadece üretim modunda etkinleştiriyoruz
    ...(!isDev
        ? [
          new PurgeCSSPlugin({
            // PurgeCSS'in tarayacağı dosyalar
            paths: glob.sync(
                [
                  path.resolve(SRC_DIR, "**/*"),
                  path.resolve(WWWROOT_DIR, "**/*.html"),
                  path.resolve(PROJECT_ROOT, "../Views/**/*.{cshtml,razor}"),
                  // node_modules'ü hariç tut
                  ...glob.sync(path.join(PROJECT_ROOT, '..', '**', '*.{html,cshtml,razor}'), {
                    ignore: [
                      path.join(PROJECT_ROOT, '..', 'node_modules', '**', '*')
                    ]
                  }),
                ],
                { nodir: true }
            ),
            // Content ayarları - hangi dosyalarda sınıf arayacağını belirtir
            content: [
              path.resolve(SRC_DIR, "**/*.{html,js,ts,jsx,tsx}"),
              path.resolve(WWWROOT_DIR, "**/*.html"),
              path.resolve(PROJECT_ROOT, "../Views/**/*.{cshtml,razor}"),
              // node_modules'ü hariç tut
              ...glob.sync(path.join(PROJECT_ROOT, '..', '**', '*.{html,cshtml,razor}'), {
                ignore: [
                  path.join(PROJECT_ROOT, '..', 'node_modules', '**', '*')
                ]
              }),
            ],
            // Güvenli listeye eklenen sınıflar (silinmemesi gerekenler)
            safelist: {
              standard: [
                // HTML elementleri ve temel sınıflar
                "html", "body", "div", "span", "p", "a", "img", "h1", "h2", "h3", "h4", "h5", "h6",

                // Tailwind ön eki ile başlayan tüm sınıflar
                /^tw-/,

                // Bootstrap bileşen sınıfları - daha geniş pattern'ler
                /^(btn|button)/, /^card/, /^container/, /^row/, /^col/, /^navbar/, /^nav/, /^dropdown/,
                /^form/, /^input/, /^modal/, /^alert/, /^badge/, /^table/, /^list/, /^pagination/,
                /^spinner/, /^carousel/, /^toast/, /^tooltip/, /^popover/, /^collapse/, /^accordion/,
                /^offcanvas/, /^progress/, /^placeholder/,

                // Bootstrap renk sınıfları
                /^text-(primary|secondary|success|danger|warning|info|light|dark|body|muted|white|black)/,
                /^bg-(primary|secondary|success|danger|warning|info|light|dark|body|white|transparent)/,
                /^border-(primary|secondary|success|danger|warning|info|light|dark|white)/,

                // Bootstrap spacing sınıfları
                /^p[xyteblrs]?-[0-5]$/, /^m[xyteblrs]?-[0-5]$/, /^p[xyteblrs]?-auto$/, /^m[xyteblrs]?-auto$/,

                // Bootstrap grid ve flexbox
                /^d-(none|block|inline|inline-block|flex|grid)/, /^flex/, /^justify-content/, /^align/,
                /^order/, /^gap/, /^g-/, /^gx-/, /^gy-/,

                // Bootstrap boyut sınıfları
                /^w-(25|50|75|100|auto)$/, /^h-(25|50|75|100|auto)$/, /^mw-/, /^mh-/, /^vw-/, /^vh-/,

                // Bootstrap pozisyon sınıfları
                /^position/, /^(top|bottom|left|right|start|end)-/, /^z-index/,

                // Bootstrap metin sınıfları
                /^text-(start|end|center|justify)/, /^text-(lowercase|uppercase|capitalize)/,
                /^fw-(light|normal|bold|bolder|lighter)/, /^fs-/, /^lh-/,

                // Bootstrap border ve shadow
                /^border/, /^rounded/, /^shadow/,

                // Bootstrap overflow ve display
                /^overflow/, /^float/, /^user-select/, /^pe-/, /^ps-/,

                // Bootstrap utilities
                /^visually-hidden/, /^stretched-link/, /^text-decoration/, /^text-wrap/, /^text-break/,
                /^font-/, /^opacity-/, /^ratio/, /^vstack/, /^hstack/,

                // Form sınıfları
                /^form-(control|select|check|range|floating|label|text)/, /^input-group/,
                /^is-(valid|invalid)/, /^valid/, /^invalid/,

                // Interactive state'ler
                /^focus-ring/, /^btn-close/, /^dropdown-/, /^modal-/, /^offcanvas-/, /^nav-/, /^tab-/,
                /^accordion-/, /^carousel-/, /^popover-/, /^tooltip-/,

                // Data attributes
                /^data-bs-/,

                // Responsive sınıfları
                /^col-(\w+)-(\d+)$/, /^d-(\w+)-(none|block|inline|flex|grid)$/, /^text-(\w+)-(start|end|center)$/,
              ],
              deep: [
                // Dinamik olarak eklenen sınıflar
                /show$/, /active$/, /disabled$/, /selected$/, /checked$/, /focus$/, /hover$/,
                /collapsing$/, /fade$/, /modal-open/, /carousel-item-(start|end|next|prev)/,
                /no-transition/, /tooltip-/, /popover-/, /accordion-/, /dropdown-/, /navbar-/,
                /nav-/, /btn-/, /form-/, /input-/, /table-/, /list-/, /card-/, /alert-/, /badge-/,
                /pagination-/, /breadcrumb-/, /spinner-/, /progress-/, /placeholder-/,

                // RTL/LTR sınıfları
                /rtl/, /ltr/,
              ],
              // Greedy matching - daha esnek eşleştirme
              greedy: [
                /^btn-/, /^card-/, /^nav-/, /^dropdown-/, /^form-/, /^input-/, /^modal-/, /^alert-/,
                /^badge-/, /^table-/, /^list-/, /^pagination-/, /^carousel-/, /^accordion-/,
                /^offcanvas-/, /^progress-/, /^spinner-/, /^toast-/, /^tooltip-/, /^popover-/,
                /^breadcrumb-/, /^placeholder-/,
                /^tw-/, // Tailwind ön ekli sınıfları koru
              ],
            },
            // PurgeCSS'e hangi sınıfların korunacağını daha iyi anlayabilmesi için
            fontFace: true,
            keyframes: true,
            variables: true,
            // Daha az agresif temizlik için
            rejected: false,
          }),
          ...(useAnalyzer ? [new BundleAnalyzerPlugin()] : []),
        ]
        : []),
  ],
  module: {
    strictExportPresence: true,
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "babel-loader",
            options: {
              presets: ["@babel/preset-env"],
            },
          },
        ],
      },
      {
        test: /\.css$/,
        use: [
          isDev ? "style-loader" : MiniCssExtractPlugin.loader,
          "css-loader",
          {
            loader: "postcss-loader",
            options: {
              postcssOptions: {
                config: path.resolve(PROJECT_ROOT, "postcss.config.js"),
              },
            },
          },
        ],
      },
      {
        test: /\.s[ac]ss$/i,
        use: [
          isDev ? "style-loader" : MiniCssExtractPlugin.loader,
          "css-loader",
          {
            loader: "postcss-loader",
            options: {
              postcssOptions: {
                config: path.resolve(PROJECT_ROOT, "postcss.config.js"),
              },
            },
          },
          {
            loader: "sass-loader",
            options: {
              sassOptions: {
                quietDeps: true,
                silenceDeprecations: ["import"],
                // Bootstrap için includePaths ekle
                includePaths: [
                  path.resolve(NODE_MODULES_DIR, "bootstrap/scss"),
                  path.resolve(NODE_MODULES_DIR),
                ],
              },
            },
          },
        ],
      },
      {
        test: /\.(png|jpe?g|gif|webp|eot|ttf|woff|woff2|mp4|mp3|mkv|pdf)$/i,
        type: "asset",
        parser: {
          dataUrlCondition: {
            maxSize: 25 * 1024,
          },
        },
        generator: {
          filename: "static/media/[name][ext][query]",
        },
      },
      {
        test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
        type: "asset/resource",
        generator: {
          filename: "static/media/[name][ext][query]",
        },
      },
    ],
  },
  optimization: {
    minimize: !isDev,
    minimizer: [
      new TerserPlugin({
        extractComments: isDev ? true : false,
      }),
      new CssMinimizerPlugin(),
    ],
  },
  performance: {
    hints: isDev ? false : "warning",
    maxEntrypointSize: 512000,
    maxAssetSize: 512000,
  },
};

module.exports = config;
