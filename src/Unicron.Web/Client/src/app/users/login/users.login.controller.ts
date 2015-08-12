/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../common/logger/logger.service.ts" />
/// <reference path="../users.module.ts" />
interface IUsersLoginScope extends angular.IScope {
    vm: IUsersLoginContoller;
}

interface IUsersLoginContoller {
    email: string;
    password: string;
    rememberMe: boolean;
    login(): void;
}

class UsersLogin implements IUsersLoginContoller {

    email: string;
    password: string;
    rememberMe: boolean;
    static $inject: any = ['$scope', 'logger', 'loginUsersService', 'currentUser', '$state'];
    /*@ngInject*/
    constructor($scope: IUsersLoginScope, private logger: ILogger, private loginUserService: ILoginUsersService,
                private currentUser: ICurrentUserManager, private $state: any) {
        $scope.vm = this;
    }
    static controllerId(): string {
        return 'users.login.controller';
    }
    login(): void {
        this.loginUserService.Login(this.email, this.password).then((data: IUserLoginResponse) =>  {
            this.SaveUser(data);
            this.$state.go('home');
        }).catch((error: any) => {
            this.logger.error('Error', error, null);
        });
    }
    private SaveUser(data: IUserLoginResponse): void {
        var expireDate: Date = new Date(Date.parse(data.expires));
        if (this.rememberMe) {
            this.currentUser.SetUserLocal(this.email, data.name, data.token, expireDate, data.claims);
        } else {
            this.currentUser.SetUserOnSession(this.email, data.name, data.token, expireDate, data.claims);
        }
    }
}
// Update the app1 variable name to be that of your module variable
appUsers.controller(UsersLogin.controllerId(), UsersLogin);
