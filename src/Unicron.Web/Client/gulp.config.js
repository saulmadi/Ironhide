module.exports = function() {
  var ts = '**/*.ts';
  var js = '**/*.js';
  var specs = '**/*.specs.ts';
  var htmls = '**/*.html';
  var root = './';
  var src = root + 'src/';
  var srcWithOutRoot = 'src/'
  var build = root + 'build/';
  var temp = root + '.tmp/';
  var index = src + 'index.html';
  var wiredep = require('wiredep');
  var report = './report/';
  var bowerFiles = wiredep({
    devDependencies: true
  })['js'];
  var buildSpecs = src + 'specs/';
  var buildJs = build + js;
  var specRunnerFile = '/specs/specs.html';
  var specRunner = src + specRunnerFile;
  var config = {
    /**
     * Files paths
     */
    sourceTs: [
      srcWithOutRoot + ts,
      '!' + srcWithOutRoot + specs
    ],
    sourceHtmls: [
      src + htmls,
      '!' + index,
      '!' + specRunner
    ],
    sourceSpecs: [
      srcWithOutRoot + 'specs/' + specs
    ],
    build: './build/',
    buildTs: build + ts,
    buildJs: buildJs,
    buildHtmls: build + htmls,
    buildSpecs: buildSpecs,
    src: src,
    root: root,
    fonts: './bower_components/font-awesome/fonts/**/*.*',
    images: src + 'images/**/*.*',
    compiledJs: [
      build + '**/*.module.js',
      build + js
    ],
    css: temp + 'styles.css',
    buildMaps: build + js + '.map',
    configTs: [
      './*.ts'
    ],
    temp: temp,
    less: src + 'styles/styles.less',
    index: index,
    buildIndex: build + 'index.html',
    /**
    Caches
    */
    cache: {
      tsLinted: 'tsLinted',
      tsCompiled: 'tsCompiled',
      tsSpecCompiled: 'tsSpecCompiled',
      tsCopy: 'tsCopy',
      htmlLinted: 'htmlLinted',
      htmlCopy: 'htmlCopy'
    },
    /**
    + TSC options
    */
    tsc: {
      module: 'commonjs',
      declarationFiles: true,
      emitError: false
    },
    /**
     * SourceMaps Options
     **/
    sourceMaps: {
      pathToWrite: '../build',
      configMaps: {
        includeContent: false,
        sourceRoot: '/build/app'
      }
    },
    sourceMapsForSpecs: {
      pathToWrite: '/',
      configMaps: {
        includeContent: false,
        sourceRoot: '/src/'
      }
    },
    /**
     * Bower and NPM locations
     */
    bower: {
      json: require('./bower.json'),
      directory: './bower_components/',
      ignorePath: '../..'
    },
    packages: [
      './package.json',
      './bower.json'
    ],
    /**
     * optimized files
     */
    optimized: {
      app: 'app.js',
      lib: 'lib.js'
    },
    /**
     * template cache
     */
    templateCache: {
      file: 'templates.js',
      options: {
        module: 'app.core',
        standAlone: false

      }
    },
    /**
     * Karma and testing settings
     */
    specHelpers: [src + 'test-helpers/*.*'],
    serverIntegrationSpecs: [src + 'specsIntegration/**/*.spec.*'],
    compiledSpecs: buildSpecs + '**/*.js',
    /**
     * specs.html, our HTML spec runner
     */
    specRunner: specRunner,
    specRunnerFile: specRunnerFile,
    /*
     * bootlint settings
     */
    bootlint: {
      disableRules: ['W001', 'W005']
    },
    testlibraries: [
      'node_modules/mocha/mocha.js',
      'node_modules/chai/chai.js',
      'node_modules/mocha-clean/index.js',
      'node_modules/sinon-chai/lib/sinon-chai.js'
    ],
    /**
     * Nancy Settings
     */
    defaultPort: 5976,
    subdomain: 'unicron'
  };
  config.getWiredepDefaultOptions = function() {
    var options = {
      bowerJson: config.bower.json,
      directory: config.bower.directory,
      ignorePath: config.bower.ignorePath
    };
    return options;
  };
  config.karma = getKarmaOptions();

  return config;

  function getKarmaOptions() {
    var options = {
      files: [].concat(
        bowerFiles,
        config.specHelpers,
        build + '**/*.module.js',
        build + '**/*.js',
        buildSpecs + '**/*.js',
        buildSpecs + '**/**/*.js',
        temp + config.templateCache.file,
        config.serverIntegrationSpecs

      ),
      exclude: [],
      coverage: {
        dir: report + 'coverage',
        reporters: [{
          type: 'html',
          subdir: 'report-html'
        }, {
          type: 'lcov',
          subdir: 'report-lcov'
        }, {
          type: 'text-summary'
        }]
      },
      preprocessors: {}
    };
    options.preprocessors[buildSpecs + '**/!(*.spec)+(.js)'] = ['coverage'];
    options.preprocessors[buildSpecs + '**/**/!(*.spec)+(.js)'] = ['coverage'];
    options.preprocessors[buildJs] = ['coverage'];
    return options;
  }
};
