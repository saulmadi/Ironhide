/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../common/logger/logger.service.ts" />
/// <reference path="../../common/logger/logger.service.ts" />

interface IUserForgotPasswordScope extends angular.IScope {
    vm: UserForgotPassword;
}

interface IUserForgotPassword {
    email: string;
    success: boolean;
    ResetPassword():  void;
}

class UserForgotPassword implements IUserForgotPassword {

    email: string;
    success: boolean;
    static $inject: any = ['$scope', 'logger', 'forgotPasswordservice'];
    /*@ngInject*/
    constructor( private $scope: IUserForgotPasswordScope, private logger: ILogger,
            private userForgotPasswordService: IUserForgotPasswordService) {
        $scope.vm = this;
    }
    static controllerId(): string {
        return 'userForgotPasswordController';
    }
    ResetPassword(): void {
    this.userForgotPasswordService.ResetPassword(this.email)
            .then( (data: any) => {
                this.success = true;
            });
    }
}
// Update the app1 variable name to be that of your module variable
appUsers.controller(UserForgotPassword.controllerId(), UserForgotPassword);
