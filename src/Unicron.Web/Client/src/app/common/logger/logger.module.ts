/// <reference path="../../../../typings/angularjs/angular.d.ts" />
interface ICommonLogger extends angular.IModule { }

// Create the module and define its dependencies.
var commonLogger: ICommonLogger = angular.module('common.logger', [
    // Angular modules
    // Custom modules

    // 3rd Party Modules
]);

// Execute bootstrapping code and any dependencies.
commonLogger.run([( ) => {
}]);
