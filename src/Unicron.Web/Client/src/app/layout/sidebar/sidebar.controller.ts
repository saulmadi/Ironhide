/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../layout.module.ts" />
/// <reference path="../../common/logger/logger.service.ts" />
/// <reference path="../../common/router/RouteSettings.ts" />
/// <reference path="../../core/config.ts" />

interface ISidebarScope extends angular.IScope {
    vm: Sidebar;
}

interface ISidebar {
    navRoutes: any;
    activate(): void;
    isCurrent(route: any): any;
    isUserLogged(): boolean;
    showSideBar(): boolean;
}

class Sidebar implements ISidebar {
    navRoutes: any;
    states: any;
    static $inject: any = ['$scope', '$state', 'routeHelper', 'currentUser', '_'];
    constructor(private $scope: ISidebarScope, private $state: angular.ui.IStateService,
            private routeHelper: any, private currentUser: ICurrentUserManager, private _: _.LoDashStatic) {
        $scope.vm = this;
        this.states = routeHelper.getStates();
    }
     getNavRoutes(): void {
        var routes: RouteSettings.IAcklenAvenueRoute[] = this.states.filter(function(r: any): any {
            return r.settings && r.settings.nav && !r.settings.notShowInMenu;
        }).sort(function(r1: any, r2: any): any {
            return r1.settings.nav - r2.settings.nav;
        });
         var user: ICurrentUser = this.currentUser.GetUser();
         if (user) {
            var userRoutes: RouteSettings.IAcklenAvenueRoute [] = _.filter(routes,
            function(route: any): boolean {
                var routeClaim: string = route.settings.claim;
                    return _.contains(user.claims, routeClaim);
            });
            this.navRoutes = userRoutes;
         }
    }
    activate(): void {
       this.getNavRoutes();
    }
    isUserLogged(): boolean {
        var user: ICurrentUser = this.currentUser.GetUser();
        if (user) {
            this.activate();
            return true;
        }
        return false;
    }
    showSideBar(): boolean {
        if (this.$state.current.url !== '^') {
            var current: RouteSettings.IAcklenAvenueRouteConfig = <RouteSettings.IAcklenAvenueRouteConfig>this.$state.current;
            var settings: RouteSettings.IAcklenAvenueRouteSettings = current.settings;
            if (settings) {
                if (settings.notShowSideBar) {
                        return false;
                }
            }
            return true;

        }
        return true;

    }

    isCurrent(route: RouteSettings.IAcklenAvenueRouteConfig): string {
        var current: RouteSettings.IAcklenAvenueRouteConfig = <RouteSettings.IAcklenAvenueRouteConfig>this.$state.current;
        if (!route.title
            || !this.$state.current
            || !current.title) {
                return '';
        }
        var menuName: string = route.title;
        return current.title.substr(0, menuName.length) === menuName ? 'current' : '';
    }
}
// Update the app1 variable name to be that of your module variable
appLayout.controller('Sidebar', Sidebar);
