/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../users.module.ts" />
/// <reference path="../../common/logger/logger.service.ts" />

interface IUsersScope extends angular.IScope {
    vm: Users;
}
interface IUsers {
    firstNumber: number;
    secondNumber: number;
    result: number;
    sum(): void;
}
class Users implements IUsers {
    firstNumber: number;
    secondNumber: number;
    result: number;
    logger: ILogger;
    static $inject: any = ['$scope', 'logger'];
    constructor($scope: IUsersScope, logger: ILogger) {
        $scope.vm = this;
        this.logger = logger;
    }
    static controllerId(): string {
        return 'users.controller';
    }
    sum(): void {
        this.result = this.firstNumber + this.secondNumber;
        this.logger.info('Exito', 'Suma de Dos Numeros', this.result);
    }
}

// Update the app1 variable name to be that of your module variable
appUsers.controller(Users.controllerId(), Users);
