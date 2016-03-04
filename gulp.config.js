module.exports = function() {
	
	return {
		util: {
			sevenZip: "C:/Program Files/7-Zip/7z.exe",
			msbuild: "C:/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe",
			mspec: process.cwd() + "/lib/Machine.Specifications.Runner.Console.0.9.2/tools/mspec-clr4.exe",
			openCover: process.cwd() + "/lib/OpenCover.4.6.519/tools/OpenCover.Console.exe"
		},
		appVersion: process.env["APPVEYOR_BUILD_VERSION"] || "local",
		buildPath: process.cwd() + '/build',
		specsPath: process.cwd() + '/specs',
		deployPath: process.cwd() + '/deploy',
		reportsPath: process.cwd() + '/reports',		
	}
};