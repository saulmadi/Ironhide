/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../common/logger/logger.service.ts" />
/// <reference path="../users.module.ts" />>


interface IUserRegisterScope extends angular.IScope {
    vm: UserRegister;
}

interface IUserRegister {
    Success: boolean;
    Name: string;
    Email: string;
    PhoneNumber: string;
    Password: string;
    ConfirmPassword: string;
    Register(): void;
}

class UserRegister implements IUserRegister {

    Success: boolean;
    Name: string;
    Email: string;
    PhoneNumber: string;
    Password: string;
    ConfirmPassword: string;
    static $inject: any = ['$scope', 'logger', 'registerUsersService'];
    /*@ngInject*/
    constructor(private $scope: IUserRegisterScope, private logger: ILogger,
            private registerUsersService: IUserRegisterService) {
        $scope.vm = this;
    }
    static controllerId(): string {
        return 'userRegisterController';
    }

    Register(): void {
        var request: IUserRegisterRequest = {
            name: this.Name,
            email: this.Email,
            phoneNumber: this.PhoneNumber,
            password: this.Password
        };
        this.registerUsersService.Register(request)
            .then((data: any) => {
                this.Success = true;
            });
    }

}

// Update the app1 variable name to be that of your module variable
appUsers.controller(UserRegister.controllerId(), UserRegister);
