/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../common/logger/logger.service.ts" />
/// <reference path="../users.module.ts" />
interface IHomeScope extends angular.IScope {
    vm: Home;
}

interface IHome {
}

class Home implements IHome {
    logger: ILogger;
    static $inject: any = ['$scope', 'logger'];
    /*@ngInject*/
    constructor( $scope: IHomeScope, logger: ILogger) {
        $scope.vm = this;
        this.logger = logger;
    }
    static controllerId(): string {
        return 'users.home.controller';
    }
}

// Update the app1 variable name to be that of your module variable
appUsers.controller(Home.controllerId(), Home);
