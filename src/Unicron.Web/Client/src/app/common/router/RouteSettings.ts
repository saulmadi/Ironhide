module RouteSettings {
    'use strict';
    export interface IAcklenAvenueRouteSettings {
        nav: number;
        content: string;
        notShowInMenu?: boolean;
        notShowSideBar?: boolean;
        isPublic?: boolean;
        claim?: string;
    }

   export  interface IAcklenAvenueRouteConfig extends angular.route.IRoute {
        url: string;
        templateUrl: string;
        controller: string;
        controllerAs: string;
        title?: string;
        settings?: IAcklenAvenueRouteSettings;
    }
    export interface IAcklenAvenueRoute {
        state: string;
        config: IAcklenAvenueRouteConfig;
    }
}
