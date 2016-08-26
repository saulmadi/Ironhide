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

var sonarQubeCore = require('./sonarQubeAppVeyorUtil')(shell, {
        sonarRunnerDownloadPath: config.sonarRunnerDownloadPath,
        sonarMSRunnerFolderName: config.sonarRunnerFolderName,
        sonarRunner: config.sonarRunner,
        sonarServerURL: 'http://ec2-54-218-88-140.us-west-2.compute.amazonaws.com:9000/',
        msCoverageReportPath: 'coverage.xml',
        projectName: 'Ironhide Core',
        projectKey: 'Ironhide-core',
        projectVersion: '1.0',
        projectRepo: 'AcklenAvenue/Ironhide',
        projectSolutionPath:'Ironhide.Core\\Ironhide.Core.sln'
    });

var sonarQubeLogin = require('./sonarQubeAppVeyorUtil')(shell, {
        sonarRunnerDownloadPath: config.sonarRunnerDownloadPath,
        sonarMSRunnerFolderName: config.sonarRunnerFolderName,
        sonarRunner: config.sonarRunner,
        sonarServerURL: 'http://ec2-54-218-88-140.us-west-2.compute.amazonaws.com:9000/',
        msCoverageReportPath: 'coverage.xml',
        projectName: 'Ironhide Login',
        projectKey: 'Ironhide-login',
        projectVersion: '1.0',
        projectRepo: 'AcklenAvenue/Ironhide',
        projectSolutionPath:'Ironhide.Login\\Ironhide.Login.sln'
    });

var sonarQubeUsers = require('./sonarQubeAppVeyorUtil')(shell, {
        sonarRunnerDownloadPath: config.sonarRunnerDownloadPath,
        sonarMSRunnerFolderName: config.sonarRunnerFolderName,
        sonarRunner: config.sonarRunner,
        sonarServerURL: 'http://ec2-54-218-88-140.us-west-2.compute.amazonaws.com:9000/',
        msCoverageReportPath: 'coverage.xml',
        projectName: 'Ironhide Users',
        projectKey: 'Ironhide-users',
        projectVersion: '1.0',
        projectRepo: 'AcklenAvenue/Ironhide',
        projectSolutionPath:'Ironhide.Users\\Ironhide.Users.sln'
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

gulp.task('reftroll', shell.task([
  'node_modules\\reftroll\\RefTroll.exe Ironhide.Core',
  'node_modules\\reftroll\\RefTroll.exe Ironhide.Login',
  'node_modules\\reftroll\\RefTroll.exe Ironhide.Users'
]))

gulp.task('build-with-coverage-report', function(callback) {
  runSequence(
  	'build', 'coverage-report', callback);
});

gulp.task('restore-nuget-packages', function (taskDone) {
	return gulp.src('/**/*.sln')
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
		}))
		.pipe(shell(['nuget.exe restore -verbosity detailed'], { cwd: '<%= file.folder %>'}));
});

gulp.task('init', ['copy-logging-config', 'restore-nuget-packages']);

gulp.task('copy-logging-config', ['copy-logging-config-core', 'copy-logging-config-users', 'copy-logging-config-login']);

gulp.task('copy-logging-config-core', function(){
	fs.stat('Ironhide.Core/logging.config', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('logging.config.default')
	    		.pipe(rename("logging.config"))
	    		.pipe(gulp.dest('src'));
	    }else{
			return;
	    }
	});
});

gulp.task('copy-logging-config-users', function(){
	fs.stat('Ironhide.Users/logging.config', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('logging.config.default')
	    		.pipe(rename("logging.config"))
	    		.pipe(gulp.dest('src'));
	    }else{
			return;
	    }
	});
});

gulp.task('copy-logging-config-login', function(){
	fs.stat('Ironhide.Login/logging.config', function(doesNotExist, stat) {
		if(doesNotExist) {
			return gulp.src('logging.config.default')
	    		.pipe(rename("logging.config"))
	    		.pipe(gulp.dest('src'));
	    }else{
			return;
	    }
	});
});

gulp.task('compile-apps', ['clean-build', 'copy-logging-config'], function () {

	return gulp.src(['**/.deployable'])
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

	return gulp.src('**/*.Specs.csproj')
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
	return gulp.src('/**/*.nuspec')
		.pipe(tap(function(file){
			file.folder = file.path.substring(0,file.path.lastIndexOf("\\")+1);
		}))
		.pipe(shell([config.util.msbuild], { cwd: '<%= file.folder %>'}))
		.pipe(shell(['nuget pack -Properties "Configuration=Debug;Platform=AnyCPU"'],
			{ cwd: '<%= file.folder %>'}));
});

gulp.task('copy-nuget-packages', function(){
	return gulp.src('/**/*.nupkg')
		.pipe(gulp.dest('deploy'));
});

gulp.task('deploy-nuget', function(){
	return gulp.src('/**/*.nupkg')
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


gulp.task('download-sonar-scanner', sonarQubeCore.downloadMSBuildScanner());

gulp.task('run-sonar-analysis-core', sonarQubeCore.runMsBuildScanner());

gulp.task('run-sonar-analysis-login', sonarQubeLogin.runMsBuildScanner());

gulp.task('run-sonar-analysis-users', sonarQubeUsers.runMsBuildScanner());