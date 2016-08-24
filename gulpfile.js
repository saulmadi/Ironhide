var gulp = require('gulp');
var config = require('./gulp.config')();
var shell = require('gulp-shell');
var executeChildProcess = require('child_process').exec;
var _ = require('lodash');
var clean = require('gulp-clean');
var runSequence = require('run-sequence');
var tap = require('gulp-tap');
var debug = require('gulp-debug');
var zip = require('gulp-zip');
var fs = require('fs');
var path = require('path');
var merge = require('merge-stream');
var rename = require('gulp-rename');
var glob = require('glob');

var sonarQubeUtil = require('./sonarQubeAppVeyorUtil')(shell, {
        sonarRunnerDownloadPath: config.sonarRunnerDownloadPath,
        sonarMSRunnerFolderName: config.sonarRunnerFolderName,
        sonarRunner: config.sonarRunner,
        sonarServerURL: 'http://ec2-54-218-88-140.us-west-2.compute.amazonaws.com:9000/',
        msCoverageReportPath: 'coverage.xml',
        projectName: 'Ironhide',
        projectKey: 'Ironhide',
        projectVersion: '1.0',
        projectRepo: 'AcklenAvenue/Ironhide',
        projectSolutionPath:'src\\Ironhide.sln'
    });

gulp.task('copy-local-config',function(callback){
	fs.stat('localconfig.json', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('localconfig.json.default', { allowEmpty: true })
	    		.pipe(rename("localconfig.json"))
	    		.pipe(gulp.dest('./'));
	    }else{
			return;
	    }
	});
});

gulp.task('set-local-env', function(callback){
	return gulp.src('setLocalEnv.ps1')		
		.pipe(shell(['powershell.exe -File setLocalEnv.ps1'], { cwd: '<%= file.folder %>'}));
});

gulp.task('default', function(callback){
	runSequence('build', 'specs', 'package', 'deploy', callback);
});

gulp.task('build', ['reftroll'], function(callback) {
  runSequence(
  	'restore-nuget-packages', 'compile-apps', callback);
});

gulp.task('reftroll', function(){
	return gulp.src('').pipe(shell('node_modules\\reftroll\\RefTroll.exe src'));
});

gulp.task('build-with-coverage-report', function(callback) {
  runSequence(
  	'build', 'coverage-report', callback);
});

gulp.task('restore-nuget-packages', function (taskDone) {
	return gulp.src('src/**/*.sln')
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
		}))
		.pipe(shell(['nuget.exe restore -verbosity detailed'], { cwd: '<%= file.folder %>'}));
});

gulp.task('init', ['copy-connection-string', 'copy-logging-config', 'restore-nuget-packages']);

gulp.task('copy-connection-string', function(){
	fs.stat('src/connectionStrings.config', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('connectionStrings.config.default')
	    		.pipe(rename("connectionStrings.config"))
	    		.pipe(gulp.dest('src'));
	    }else{
			return;
	    }
	});
});

gulp.task('copy-logging-config', function(){
	fs.stat('src/logging.config', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('logging.config.default')
	    		.pipe(rename("logging.config"))
	    		.pipe(gulp.dest('src'));
	    }else{
			return;
	    }
	});
});

gulp.task('compile-apps', ['clean-build', 'copy-connection-string', 'copy-logging-config'], function () {

	return gulp.src(['src/**/.deployable'])
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
			var pathParts = file.folder.split("\\");
			file.folderName = pathParts[pathParts.length-2];
			file.csprojPath = file.folder + '\\' + file.folderName+ '.csproj'
		}))
		.pipe(shell([
			config.util.msbuild 
			+ ' /p:Configuration=Release'
			+ ' /p:OutDir=\"' + config.buildPath + '\\<%= file.folderName %>\"'
			+ ' \"<%= file.csprojPath %>\"'
			+ ' /p:WebProjectOutputDir=\"' + config.buildPath + '\\<%= file.folderName %>-publish\"' 
			]));
});

gulp.task('compile-specs', ['restore-nuget-packages', 'clean-spec'], function () {

	return gulp.src('src/**/*.Specs.csproj')
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
			var pathParts = file.folder.split("\\");
			file.folderName = pathParts[pathParts.length-2];
		}))
		.pipe(shell([
			config.util.msbuild 
			+ ' /p:Configuration=Release'
			+ ' /p:OutDir=\"' + config.specsPath + '\\<%= file.folderName %>\"'
			+' \"<%= file.path %>\"'
			]));
});

gulp.task('create-nuget-packages', function(){
	return gulp.src('src/**/*.nuspec')
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
		}))
		.pipe(shell([config.util.msbuild], { cwd: '<%= file.folder %>'}))
		.pipe(shell(['nuget pack -Properties "Configuration=Debug;Platform=AnyCPU"'],
			{ cwd: '<%= file.folder %>'}));
});

gulp.task('copy-nuget-packages', function(){
	return gulp.src('src/**/*.nupkg')
		.pipe(gulp.dest('deploy'));
});

gulp.task('deploy-nuget', function(){
	return gulp.src('src//**/*.nupkg')
		.pipe(shell(['nuget push <%= file.path %> -ApiKey 1w9s9pgveu1ruy5onxo9ko02 -Source https://ci.appveyor.com/nuget/acklenavenue/api/v2/package']));
});

gulp.task('package', function(callback){
	runSequence('clean-deploy', 'zip-apps', 'create-nuget-packages', 'copy-nuget-packages', callback);
});

gulp.task('clean-deploy', function(){
	return gulp.src('deploy').pipe(clean());
});

function getFolders(dir) {
    return fs.readdirSync(dir)
      .filter(function(file) {
        return fs.statSync(path.join(dir, file)).isDirectory();
      });
}

gulp.task('zip-apps', function(){
	var folders = getFolders(config.buildPath);

	var tasks = folders.map(function(folder) {
      return gulp.src([config.buildPath+'/'+folder+'/**/'])
        .pipe(zip(folder+'-'+config.appVersion+'.zip'))
        .pipe(gulp.dest('deploy'));
   	});

   	return tasks;
});

gulp.task('deploy', function(){

});

var specs = './specs/**/*.Specs.dll';

gulp.task('specs', ['compile-specs'], function(){

	gulp.src(specs)
	    .pipe(tap(function(file){
	    	file.path = file.path.replace('/','\\');
    	}))
	    .pipe(shell(config.util.mspec + ' --html ' + config.reportsPath + ' "<%= file.path %>"'));

});

gulp.task('coverage', ['compile-specs'], function(done){

	var coverageFilters = '+[Ironhide*]*';

	glob(specs, {}, function (er, files) {
		var dlls = _.uniq(files, function(f){
				var parts = f.split('/');
				var lastPart = parts[parts.length-1];
				return lastPart;
			}).map(function(f){
				return '' + f.replace('/','\\') + '';
			})
			.join(" ").trim();

  	  	var cmd = config.util.openCover + ' -register:user -returntargetcode -filter:"' + coverageFilters + '" -target:"' + config.util.mspec + '" -targetargs:"' + dlls + '" -output:"./coverage.xml"';

		executeChildProcess(cmd, function(err, stdout, stderr){
			if(err){
				console.log(stderr);
				done(err);
			}
			console.log(stdout);
			done();
		});
	});
});

gulp.task('coverage-report', ['coverage'], shell.task([config.util.reportGenerator + ' -reports:"coverage.xml" -targetdir:"coverage"']));

gulp.task('clean-build', function(){
	return gulp.src(config.buildPath).pipe(clean());
});

gulp.task('clean-spec', function(){
	return gulp.src(config.specsPath).pipe(clean());
});


gulp.task('download-sonar-scanner', sonarQubeUtil.downloadMSBuildScanner());

gulp.task('run-sonar-analysis', sonarQubeUtil.runMsBuildScanner());