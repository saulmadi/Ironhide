/// <reference path="../../../../typings/angularjs/angular.d.ts" />
interface ICommonException extends angular.IModule { }

// Create the module and define its dependencies.
var commonException: ICommonException = angular.module('common.exception', [
    // Angular modules
    // Custom modules
    'common.logger'
    // 3rd Party Modules
]);

// Execute bootstrapping code and any dependencies.
commonException.run([ () => {
}]);
