module.exports = function() {
	
	return {
		util: {
			sevenZip: "C:/Program Files/7-Zip/7z.exe",
			msbuild: '"C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe"',
			mspec: process.cwd() + "/lib/Machine.Specifications.Runner.Console.0.9.2/tools/mspec-clr4.exe",
			openCover: process.cwd() + "/lib/OpenCover.4.6.519/tools/OpenCover.Console.exe",
			reportGenerator: process.cwd() + "/lib/ReportGenerator.2.4.4.0/tools/ReportGenerator.exe"
		},
		appVersion: process.env["APPVEYOR_BUILD_VERSION"] || "local",
		buildPath: process.cwd() + '/build',
		specsPath: process.cwd() + '/specs',
		deployPath: process.cwd() + '/deploy',
		reportsPath: process.cwd() + '/reports',
		sonarRunnerDownloadPath: 'https://github.com/SonarSource-VisualStudio/sonar-msbuild-runner/releases/download/2.1/MSBuild.SonarQube.Runner-2.1.zip',
		sonarRunnerFolderName: 'MSBuild.SonarQube.Runner-2.1',
		sonarRunner: 'MSBuild.SonarQube.Runner.exe'
	}
};