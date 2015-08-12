/// <reference path="../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/angularjs/angular-route.d.ts" />


interface IAppUsers extends angular.IModule { }

// Create the module and define its dependencies.
var appUsers: IAppUsers = angular.module('app.users', [
    // Angular modules
    'ngRoute',           // routing
    // Custom modules
    'app.core'
    // 3rd Party Modules
]);

// Execute bootstrapping code and any dependencies.
appUsers.run([ '$rootScope', '$route', (
$rootScope: angular.IRootScopeService, $route: angular.route.IRoute) => {
}]);
