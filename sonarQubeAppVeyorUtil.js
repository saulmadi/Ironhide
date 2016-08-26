module.exports = function(shell,
                          options) {

    if (!options.sonarRunnerDownloadPath || options.sonarRunnerDownloadPath === '') {
        throw 'Sonar runner download path is required.'
    }

    if (!options.sonarRunner || options.sonarRunner === '') {
        throw 'Sonar runner executable name is required.'
    }

    if (!options.projectRepo || options.projectRepo === '') {
        throw 'Project repository is required.'
    }

    var defaultOptions = {
        sonarRunnerDownloadPath: options.sonarRunnerDownloadPath,
        sonarMSRunnerFolderName: options.sonarMSRunnerFolderName || '',
        sonarRunner: options.sonarRunner,
        sonarServerURL: options.sonarServerURL || 'http://ec2-54-218-88-140.us-west-2.compute.amazonaws.com:9000/',
        msCoverageReportPath: options.msCoverageReportPath || '',
        projectName: options.projectName,
        projectKey: options.projectKey,
        projectVersion: options.projectVersion,
        projectRepo: options.projectRepo,
        projectSolutionPath: options.projectSolutionPath || ''
    }

    var runnerZipFileName = defaultOptions.sonarRunnerDownloadPath.substring(defaultOptions.sonarRunnerDownloadPath.lastIndexOf("/") + 1);
    var runnerFileNameNoExt = runnerZipFileName.substring(runnerZipFileName.lastIndexOf('.') + 1);

	return {
		downloadMSBuildScanner: function() {
            if (defaultOptions.sonarMSRunnerFolderName === '') {
                throw 'Folder name for MSBuild runner is required.'
            }

            return shell.task([
                'if not exist %APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.sonarMSRunnerFolderName +
                    ' appveyor DownloadFile ' + defaultOptions.sonarRunnerDownloadPath,
                'if not exist %APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.sonarMSRunnerFolderName + ' 7z x ' + runnerZipFileName + ' -y -o' + defaultOptions.sonarMSRunnerFolderName,
            ]);
        },
        runMsBuildScanner: function() {
            if (defaultOptions.sonarMSRunnerFolderName === '') {
                throw 'Folder name for MSBuild runner is required.'
            }
            if (defaultOptions.msCoverageReportPath === '') {
                throw 'Path to coverage report for MSBuild runner is required.'
            }
            if (defaultOptions.projectSolutionPath === '') {
                throw 'Path to solution file for MSBuild runner is required.'
            }

            if (!options.projectName || options.projectName === '') {
                throw 'Project name is required.'
            }

            if (!options.projectKey || options.projectKey === '') {
                throw 'Project key is required.'
            }

            if (!options.projectVersion || options.projectVersion === '') {
                throw 'Project version is required.'
            }

            return shell.task([
                (!process.env.APPVEYOR_PULL_REQUEST_NUMBER) ?
                '%APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.sonarMSRunnerFolderName + '\\' + defaultOptions.sonarRunner + ' begin' +
                ' /d:sonar.cs.opencover.reportsPaths='+ defaultOptions.msCoverageReportPath + ' /d:sonar.host.url=' + defaultOptions.sonarServerURL +
                ' /k:' + defaultOptions.projectKey + ' /n:' + defaultOptions.projectName + ' /v:' + defaultOptions.projectVersion +
                ' /d:sonar.analysis.mode=preview /d:sonar.github.pullRequest=' + process.env.APPVEYOR_PULL_REQUEST_NUMBER +
                ' /d:sonar.github.repository=' + defaultOptions.projectRepo + ' /d:sonar.github.oauth=' +process.env.GITHUB_SONAR_TOKEN
                : '%APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.sonarMSRunnerFolderName + '\\' + defaultOptions.sonarRunner + ' begin' +
                ' /d:sonar.cs.opencover.reportsPaths='+ defaultOptions.msCoverageReportPath + ' /d:sonar.host.url=' + defaultOptions.sonarServerURL +
                ' /k:' + defaultOptions.projectKey + ' /n:' + defaultOptions.projectName + ' /v:' + defaultOptions.projectVersion,
                '"C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" %APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.projectSolutionPath,
                '%APPVEYOR_BUILD_FOLDER%\\' + defaultOptions.sonarMSRunnerFolderName + '\\' + defaultOptions.sonarRunner + ' end'
            ]);
        },
        downloadSonarScanner: function() {
            return shell.task([
                'if not exist %APPVEYOR_BUILD_FOLDER%\\' + runnerFileNameNoExt +
                    ' appveyor DownloadFile ' + defaultOptions.sonarRunnerDownloadPath,
                'if not exist %APPVEYOR_BUILD_FOLDER%\\' + runnerFileNameNoExt + ' 7z x ' + runnerZipFileName +' -y',
            ]);
        },
        runSonarScanner: function() {
            return shell.task([
                (process.env.APPVEYOR_PULL_REQUEST_NUMBER) ? '%APPVEYOR_BUILD_FOLDER%\\' + runnerFileNameNoExt + '\\bin\\' + defaultOptions.sonarRunner +
                ' -Dsonar.analysis.mode=preview -Dsonar.github.pullRequest=%APPVEYOR_PULL_REQUEST_NUMBER% ' +
                ' -Dsonar.github.repository=' + defaultOptions.projectRepo + ' -Dsonar.github.oauth=%GITHUB_SONAR_TOKEN%' :
                    '%APPVEYOR_BUILD_FOLDER%\\' + runnerFileNameNoExt + '\\bin\\' + defaultOptions.sonarRunner
            ]);
        }
	}
};

