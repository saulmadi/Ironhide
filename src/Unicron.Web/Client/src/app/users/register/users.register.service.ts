/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
interface IUserRegisterRequest {
    email: string;
    name: string;
    password: string;
    phoneNumber?: string;

}
interface IUserRegisterService {
    Register(request: IUserRegisterRequest): angular.IPromise<boolean>;
}

class UserRegisterService implements IUserRegisterService {

    static $inject: any = ['httpq'];
    /*@ngInject*/
    constructor(private httpQ: IHttpQ) {
    }
    Register(request: IUserRegisterRequest): angular.IPromise<boolean> {
        return this.httpQ.Post<IUserRegisterRequest, boolean>('/register', request);
    }
}

appUsers.factory('registerUsersService', ['httpq', (httpq: IHttpQ) => new UserRegisterService(httpq)]);
