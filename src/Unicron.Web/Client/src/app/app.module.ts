/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="../../typings/angularjs/angular-route.d.ts" />
/// <reference path="../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="./core/current-user.factory.ts"/>
'use strict';

interface Iapp extends angular.IModule { }

// Create the module and define its dependencies.
var app: Iapp = angular.module('app', [
// Angular modules
    'ngAnimate',        // animations
    'ngRoute',          // routing
    'ngResource',       // $resource for REST queries
    'ngSanitize',       // sanitizes html bindings (ex: sidebar.js)
    'ngMessages',
// Custom modules
    'app.core',
    'app.widgets',
    'app.users',
    'app.layout',
// Third Party Modules
    'mgcrea.ngStrap'   // instead of angular.ui
]);

// Handle routing errors and success events
app.run(['$state', 'currentUser', ($state: any, currentUser: ICurrentUserManager) => {
    // Include $route to kick start the router.
    var user: ICurrentUser = currentUser.GetUser();
    if (user) {
        $state.go('home');
    } else {
        $state.go('login');
    }
}]);
