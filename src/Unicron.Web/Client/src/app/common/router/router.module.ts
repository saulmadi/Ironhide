/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
interface ICommonRouter extends angular.IModule { }

// Create the module and define its dependencies.
var commonRouter: ICommonRouter = angular.module('common.router', [
    // Angular modules
    'ui.router',
    // Custom modules
    'common.logger'
    // 3rd Party Modules
]);

// Execute bootstrapping code and any dependencies.
commonRouter.run([( ) => {
}]);
