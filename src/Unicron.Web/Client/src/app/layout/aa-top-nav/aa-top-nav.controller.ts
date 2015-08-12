/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../layout.module.ts" />

interface IAATopNavScope extends angular.IScope {
    vm: AATopNavController;
}
interface IAATopNav {
    Logout(): void;
}
class AATopNavController implements IAATopNav {

    static title: string;
    static $inject: any = ['$scope', 'userLogoutService'];
    /*@ngInject*/
    constructor(private $scope: IAATopNavScope, private userLogoutService: IUserLogoutService ) {
        this.$scope.vm = this;
    }
    Logout(): void {
        this.userLogoutService.Logout();
    }
}
appLayout.controller('aa-TopNavController', AATopNavController);
