
/// <reference path="../../../typings/angularjs/angular.d.ts" />


interface IAppCore extends angular.IModule { }

// Create the module and define its dependencies.
var appCore: IAppCore = angular.module('app.core', [
    // Angular modules
    'ngAnimate',
    'ngSanitize',
    // Custom modules
    'common.logger',
    'common.exception',
    'common.router',
    // 3rd Party Modules
    'ui.router',
    'ngplus'
]);

// Execute bootstrapping code and any dependencies.
appCore.run([ () => {
}]);
