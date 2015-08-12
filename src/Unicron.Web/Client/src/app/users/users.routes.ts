/// <reference path="../../../typings/angularjs/angular-route.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
'use strict';

interface IProvideRoutes {
    getRoutes(): RouteSettings.IAcklenAvenueRoute[];
}

class UserRoutes implements IProvideRoutes {
    getRoutes(): RouteSettings.IAcklenAvenueRoute[] {
            var userRoutes: RouteSettings.IAcklenAvenueRoute [] = [
                {
                    state: 'home',
                    config: {
                        url: '/home',
                        templateUrl: 'app/users/home/users.home.html',
                        controller: 'users.home.controller',
                        controllerAs: 'vm',
                        title: 'Home',
                        settings: {
                            nav: 1,
                            content: '<i class="fa fa-home"></i> Home',
                            claim: 'Home'
                        }
                    }
                },
                {
                    state: 'users',
                    config: {
                        url: '/users',
                        templateUrl: 'app/users/activateDeactivateUsers/users.activate-deactivate.html',
                        controller: 'users.activate-deactivate.controller',
                        controllerAs: 'vm',
                        title: 'Users Managment',
                        settings: {
                            nav: 2,
                            content: '<i class="fa fa-group"></i> Users',
                            claim: 'ActivateDeactivateUsers'
                        }
                    }
                },
                {
                    state: 'login',
                    config: {
                        url: '/login',
                        templateUrl: 'app/users/login/users.login.html',
                        controller: 'users.login.controller',
                        controllerAs: 'vm',
                        title: 'Login',
                        settings: {
                            nav: 0,
                            content: '<i></i> Login',
                            notShowInMenu: true,
                            notShowSideBar: true,
                            isPublic: true
                        }
                    }
                },

            ];


            return userRoutes;
        }
}
var appUsers: IAppUsers = angular.module('app.users');
appUsers.constant('userRoutes', new UserRoutes() );
appUsers.run(['userRoutes', 'routeHelper', (userRoutes: UserRoutes, routeHelper: any) => {
     routeHelper.configureStates(userRoutes.getRoutes(), '/');
} ]);
