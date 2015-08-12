///<reference path='typings/node/node.d.ts'/>
'use strict';
var gulp = require('gulp');
var del = require('del');
var tsc = require('gulp-typescript');
var config = require('./gulp.config')();
var args = require('yargs').argv;
var plugins = require('gulp-load-plugins')({ lazy: true });
var browserSync = require('browser-sync');
var runSequence = require('run-sequence');
var path = require('path');
var _ = require('lodash');
var port = config.defaultPort;

gulp.task('help', plugins.taskListing);
gulp.task('default', ['help']);

gulp.task('build-dev', ['clean-code', 'vet', 'copyingTs', 'copyingHtmls'],
    function(done: () => any) {
        console.log('Compiling Typescript files for Dev');
        return compile_ts_with_maps(config.buildTs, config.build);
    }
    );
gulp.task('reloading_ts', function(callback: () => any) {
    var stream = runSequence('vet', 'copyingTs', callback);
    return stream;
});

gulp.task('rebuild-dev', ['clean-code', 'vet', 'copyingTs', 'copyingHtmls'],
    function(done: () => any) {
        console.log('Recompiling Typescript files for Dev');
        return compile_ts_with_maps(config.buildTs, config.build);
    }
    );
gulp.task('recompile-ts', ['vet', 'copyingTs'],
    function(done: () => any) {
        console.log('Recompiling Typescript files for Dev');
        return compile_ts_with_maps(config.buildTs, config.build);
    }
    );

gulp.task('build-release', ['vet', 'clean-code'], function() {
    console.log('Compiling Typescript files for Release');
    return gulp
        .src(config.sourceTs)
        .pipe(tsc(config.tsc))
        .pipe(gulp.dest(config.build));
}
    );

gulp.task('fonts', ['clean-fonts'], function() {
    console.log('Copying fonts');

    return gulp
        .src(config.fonts)
        .pipe(gulp.dest(config.build + 'fonts'));
});

gulp.task('images', ['clean-images'], function() {
    console.log('Copying and compressing the images');

    return gulp
        .src(config.images)
        .pipe(plugins.imagemin({ optimizationLevel: 4 }))
        .pipe(gulp.dest(config.build + 'images'));
});

gulp.task('serve-dev', function(callback: () => any) {
    var stream = runSequence('build-dev', 'inject', 'test-for-build', callback);
    return stream;
}
    );
gulp.task('reload-serve-dev', function(callback: () => any) {
    var stream = runSequence('rebuild-dev', 'inject', 'test-for-build', callback);
    return stream;
}
    );
gulp.task('reload-ts', function(callback: () => any) {
    console.log('Reloading TS files');
    var stream = runSequence('recompile-ts', 'inject', 'test-for-build', callback);
    return stream;
}
    );
gulp.task('serve-release', function(callback: () => any) {
    var stream = runSequence('build-release', 'inject', 'test-for-build', callback);
    return stream;
}
    );
gulp.task('compile-specs', function() {
    return compile_ts_with_maps(config.sourceSpecs, config.buildSpecs, true);
});
gulp.task('test', ['build-dev', 'templatecache', 'compile-specs'], function(done: () => any) {
    startTests(true /* singleRun */, done);
});
gulp.task('specs-html', ['build-specs-html'], function(done: () => any) {
    serve(true /* isDev */, true /* specRunner */);
    done();
});
function injectSpecs(): any {
    var wiredep = require('wiredep').stream;
    var options = config.getWiredepDefaultOptions();
    var specs = config.specs;
    options.devDependencies = true;

    if (args.startServers) {
        specs = [].concat(specs, config.serverIntegrationSpecs);
    }

    return gulp
        .src(config.specRunner)
        .pipe(wiredep(options))
        .pipe(plugins.inject(gulp.src(config.testlibraries),
            { name: 'inject:testlibraries', read: false }))
        .pipe(plugins.inject(gulp.src(config.compiledJs)))
        .pipe(plugins.inject(gulp.src(config.specHelpers),
            { name: 'inject:spechelpers', read: false }))
        .pipe(plugins.inject(gulp.src(config.compiledSpecs),
            { name: 'inject:specs', read: false }))
        .pipe(plugins.inject(gulp.src(config.temp + config.templateCache.file),
            { name: 'inject:templates', read: false }))
        .pipe(gulp.dest(config.buildSpecs));
}
gulp.task('reload-specs', ['compile-specs'], function(){
  console.log('Reloading Specs');
  return injectSpecs();
});
gulp.task('build-specs-html', ['serve-dev', 'fonts', 'images'], function() {
    console.log('building the spec runner');
    return injectSpecs();
});

gulp.task('autoTest', ['build-dev', 'templatecache', 'compile-specs'], function(done: () => any) {
    gulp.watch([config.sourceTs], ['build-dev'])
        .on('change', changeEvent);
    gulp.watch([config.sourceSpecs], ['compile-specs'])
        .on('change', changeEvent);
    startTests(false, done);
});
gulp.task('test-for-build', ['compile-specs'], function(done: () => any) {
    startTests(true /* singleRun */, done);
});

gulp.task('dev', ['serve-dev', 'fonts', 'images'], function() {
    serve(true);
    notifyBuilding('gulp build', 'Deployed as Dev', 'Running `gulp dev`');
}
    );
gulp.task('release', ['optimize', 'fonts', 'images'], function() {
    serve(false);
    notifyBuilding('gulp build', 'Deployed as Release', 'Running `gulp release`');
}
    );
gulp.task('release-ci', ['optimize', 'fonts', 'images']
    );

gulp.task('copyingTs', function() {
    console.log('Copying typescript files');
    var cache = config.cache.tsCopy;
    return gulp
        .src(config.sourceTs)
        .pipe(plugins.cached(cache))
        .pipe(plugins.debug({ title: 'Copying TS: ' }))
        .pipe(plugins.remember(cache))
        .pipe(gulp.dest(config.build));
}

    );
gulp.task('copyingHtmls', ['lintHtmls'], function() {
    console.log('Copying Html files');
    var cache: string = config.cache.htmlCopy;
    return gulp
        .src(config.sourceHtmls)
        .pipe(plugins.cached(cache))
        .pipe(plugins.debug({ title: 'Copying HTML: ' }))
        .pipe(plugins.remember(cache))
        .pipe(gulp.dest(config.build));
}

    );

gulp.task('lintHtmls', function() {
    console.log('Linting HTML files');
    var cache = config.cache.htmlLinted;
    return gulp
        .src(config.sourceHtmls)
        .pipe(plugins.cached(cache))
        .pipe(plugins.htmlhint())
        .pipe(plugins.remember(cache))
        .pipe(plugins.htmlhint.failReporter());
});

gulp.task('bootlintHTML', function() {
    console.log('Bootrsap HTML');
    return gulp
        .src(config.sourceHtmls)
        .pipe(plugins.bootlint(
            {
                disabledIds: config.bootlint.disableRules
            }
            ));
});

gulp.task('templatecache', function() {
    console.log('Creating AngularJS $templateCache');

    return gulp
        .src(config.sourceHtmls)
        .pipe(plugins.minifyHtml({ empty: true }))
        .pipe(plugins.angularTemplatecache(
            config.templateCache.file,
            config.templateCache.options
            ))
        .pipe(gulp.dest(config.temp));
});

gulp.task('vet', function() {
    console.log('Analyzing sources with TSLint, JSHint and JSCS');
    var allTs: string[] = config.sourceTs;
    var cache: string = config.cache.tsLinted;
    var stream = gulp
        .src(allTs)
        .pipe(plugins.cached(cache))
        .pipe(plugins.debug({ title: 'Linting: ' }))
    // .pipe(plugins.if(args.verbose, plugins.print()))
        .pipe(plugins.tslint())
        .pipe(plugins.remember(cache))
        .pipe(plugins.tslint.report('verbose'));
    return stream;

}
    );
gulp.task('styles', ['clean-styles'], function() {
    console.log('Compiling Less --> CSS');

    return gulp
        .src(config.less)
        .pipe(plugins.plumber())
        .pipe(plugins.less())
        .pipe(plugins.autoprefixer({ browsers: ['last 2 version', '> 5%'] }))
        .pipe(gulp.dest(config.temp));
});

gulp.task('less-watcher', function() {
    gulp.watch([config.less], ['styles']);
});

gulp.task('wiredep', function() {
    console.log('Wire up the bower css js and our app js into the html');
    var options = config.getWiredepDefaultOptions();
    var wiredep = require('wiredep').stream;

    return gulp
        .src(config.index)
        .pipe(wiredep(options))
        .pipe(plugins.inject(gulp.src(config.compiledJs)))
        .pipe(gulp.dest(config.build));
});

gulp.task('inject', ['wiredep', 'styles', 'templatecache'], function() {
    console.log('Wire up the app css into the html, and call wiredep ');
    var stream = gulp
        .src(config.buildIndex)
        .pipe(plugins.inject(gulp.src(config.css)))
        .pipe(gulp.dest(config.build));

    return stream;
});

gulp.task('clean-code', function(done: () => any) {
    console.log('***Begining to Clean Code***');
    var files = [].concat(
        config.buildTs, config.buildJs, config.buildMaps, config.buildHtmls, config.build + '/js');
    clean(files, done);
    console.log('***Finishing to Clean Code***');
}

    );

gulp.task('optimize', ['serve-release'], function() {
    console.log('Optimizing the javascript, css, html');

    var assets = plugins.useref.assets({ searchPath: '{' + config.root + ',' + config.bower.directory + '}' });
    var templateCache = config.temp + config.templateCache.file;
    var cssFilter = plugins.filter('**/*.css');
    var jsLibFilter = plugins.filter('**/' + config.optimized.lib);
    var jsAppFilter = plugins.filter('**/' + config.optimized.app);


    return gulp
        .src(config.buildIndex)
        .pipe(plugins.plumber())
        .pipe(plugins.inject(gulp.src(templateCache, { read: false }), {
            starttag: '<!-- inject:templates:js -->'
        }))
        .pipe(assets)
        .pipe(cssFilter)
        .pipe(plugins.csso())
        .pipe(cssFilter.restore())
        .pipe(jsLibFilter)
        .pipe(plugins.uglify())
        .pipe(jsLibFilter.restore())
        .pipe(jsAppFilter)
        .pipe(plugins.ngAnnotate())
        .pipe(plugins.uglify())
        .pipe(jsAppFilter.restore())
        .pipe(plugins.rev())
        .pipe(assets.restore())
        .pipe(plugins.useref())
        .pipe(plugins.revReplace())
        .pipe(gulp.dest(config.build))
        .pipe(plugins.rev.manifest())
        .pipe(gulp.dest(config.build))
        .on('end', function() {
            del(config.build + '/app');
        });
});
/**
 * Bump the version
 * --type=pre will bump the prerelease version *.*.*-x
 * --type=patch or no flag will bump the patch version *.*.x
 * --type=minor will bump the minor version *.x.*
 * --type=major will bump the major version x.*.*
 * --version=1.2.3 will bump to a specific version and ignore other flags
 */
gulp.task('bump', function() {
    var msg = 'Bumping versions';
    var type = args.type;
    var version = args.version;
    var options: {
        version: string; type: string
    } = { version: null, type: null };
    if (version) {
        options.version = version;
        msg += ' to ' + version;
    } else {
        options.type = type;
        msg += ' for a ' + type;
    }
    console.log(msg);

    return gulp
        .src(config.packages)
        .pipe(plugins.print())
        .pipe(plugins.bump(options))
        .pipe(gulp.dest(config.root));
});

gulp.task('clean-styles', function(done: () => any) {
    console.log('***Begining to Clean Styles***');
    var css = [].concat(config.temp + '**/*.css', config.build + '/styles');
    clean(css, done);
    console.log('***Finishing to Clean Styles***');
});
gulp.task('clean-fonts', function(done: () => any) {
    var fonts = [].concat(config.build + 'fonts/**/*.*');
    clean(fonts, done);
});
gulp.task('clean-images', function(done: () => any) {
    var images = [].concat(config.build + 'images/**/*.*');
    clean(images, done);
});
gulp.task('clean-specs', function(done: () => any) {
    var specs = [].concat(config.compiledSpecs);
    clean(specs, done);

});

function serve(isDev: boolean, isSpecRunner?: boolean): void {
    'use strict';
    startBrowserSync(isDev, isSpecRunner);
}

function clean(path: string[], done: () => any): void {
    'use strict';
    console.log('Cleaning: ' + plugins.util.colors.blue(path));
    del(path, done);
}

function changeEvent(event: any) {
    'use strict';
    var srcPattern = new RegExp('/.*(?=/' + config.source + ')/');
    console.log('File ' + event.path.replace(srcPattern, '') + ' ' + event.type);
    if (event.type === 'deleted') {
        var caches = _.values(config.cache);
        _.forEach(caches, function(cacheName) {
            var deletedFile: string = path.basename(event.path);

            var cacheKeys = _.keys(plugins.cached.caches[cacheName]);
            var pathOfFileToDelete: string = _.find(cacheKeys, function(key) {
                return _.endsWith(key, deletedFile);
            });
            if (pathOfFileToDelete) {
                if (_.endsWith(cacheName, 'Compiled')) {
                    var tsToDelete = pathOfFileToDelete;
                    var jsToDelete = pathOfFileToDelete.replace('ts', 'js');
                    pathOfFileToDelete = pathOfFileToDelete.replace('ts', 'js');
                    var mapToDelete = pathOfFileToDelete + '.map';
                    plugins.remember.forget(cacheName, pathOfFileToDelete + '.map');
                    del([tsToDelete, jsToDelete, mapToDelete], function(err, paths) {

                        console.log('Deleted files :\n', plugins.util.colors.red(paths.join('\n')));
                    });

                }
                delete plugins.cached.caches[cacheName][pathOfFileToDelete];
                plugins.remember.forget(cacheName, pathOfFileToDelete);
            }


        });
    }

}


function startBrowserSync(isDev: boolean, isSpecRunner: boolean): void {
    'use strict';
    var tunnel = null;
    var online = false;
    if (args.nosync || browserSync.active) {
        return;
    };
    if (args.online) {
        tunnel = config.subdomain;
        online = true;
    }
    console.log('Starting browser-sync on port ' + port);
    if (isDev) {
        gulp.watch([config.less, config.sourceHtmls], ['reload-serve-dev', browserSync.reload])
            .on('change', changeEvent);
        gulp.watch([config.sourceTs], ['reload-ts', browserSync.reload])
            .on('change', changeEvent);

        gulp.watch([config.sourceSpecs], ['reload-specs', browserSync.reload])
            .on('change', changeEvent);
    } else {
        gulp.watch([config.less, config.sourceTs, config.sourceHtmls], ['optimize', browserSync.reload])
            .on('change', changeEvent);
        gulp.watch([config.sourceSpecs], ['compile-specs', browserSync.reload])
            .on('change', changeEvent);
    }
    var options = {
        proxy: 'localhost:' + port,
        port: 3000,
        files: isDev ? [
            config.build + '**/*.*',
            config.temp + '**/*.css'
        ] : [],
        ghostMode: true,
        injectChanges: true,
        logFileChanges: false,
        logLevel: 'debug',
        logPrefix: 'Acklen Avenue',
        notify: true,
        reloadDelay: 5000,
        startPath: null,
        tunnel: tunnel,
        online: online
    };
    if (isSpecRunner) {
        options.startPath = config.specRunnerFile;
    }
    browserSync(options);

}
function startTests(singleRun: boolean, done: any) {
    'use strict';
    var karma = require('karma').server;
    var excludeFiles = [];
    var serverSpecs = config.serverIntegrationSpecs;

    excludeFiles = serverSpecs;

    karma.start({
        configFile: __dirname + '/karma.conf.js',
        exclude: excludeFiles,
        singleRun: singleRun
    }, karmaCompleted);

    function karmaCompleted(karmaResult: any) {
        console.log('Karma completed!');
        if (karmaResult === 1) {
            done('karma: tests failed with code ' + karmaResult);
        } else {
            done();
        }
    }
}
function compile_ts_with_maps(source: string, dest: string, isSpecs?: boolean): any {
    'use strict';
    var path, configMaps: any;
    var cache: string;
    if (isSpecs) {
        path = config.sourceMapsForSpecs.pathToWrite;
        configMaps = config.sourceMapsForSpecs.configMaps;
        cache = config.cache.tsSpecCompiled;
    } else {
        path = config.sourceMaps.pathToWrite;
        configMaps = config.sourceMaps.configMaps;
        cache = config.cache.tsCompiled;
    }
    var tsResults = gulp
        .src(source)
        .pipe(plugins.cached(cache))
        .pipe(plugins.debug({ title: 'Compiled:' }))
        .pipe(plugins.sourcemaps.init())
        .pipe(tsc(config.tsc));
    var stream = tsResults
        .pipe(plugins.sourcemaps.write(
            path,
            configMaps
            ))
        .pipe(plugins.remember(cache))
        .pipe(
            gulp.dest(dest)
            );
    return stream;
}
function notify(options: any) {
    'use strict';
    var notifier = require('node-notifier');
    var notifyOptions = {
        sound: 'Bottle',
        contentImage: path.join(__dirname, 'gulp.png'),
        icon: path.join(__dirname, 'gulp.png')
    };
    _.assign(notifyOptions, options);
    notifier.notify(notifyOptions);
}

function notifyBuilding(title: string, subtittle: string, message: string) {
    'use strict';
    console.log('Building everything');

    var msg = {
        title: title,
        subtitle: subtittle,
        message: message
    };

    console.log(msg);
    notify(msg);
}
