/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
interface IUserLogoutService {
    Logout(): void;
}

class UserLogoutService implements IUserLogoutService {

    static $inject: any = ['currentUser', '$state'];
    /*@ngInject*/
    constructor(private currentUser: ICurrentUserManager, private $state: any) {
    }
    Logout(): void {
        this.currentUser.RemoveUser();
        this.$state.go('login');
    }
}
appUsers.factory('userLogoutService', ['currentUser', '$state',
    (currentUser: ICurrentUserManager, $state: any) => new UserLogoutService(currentUser, $state)]);
