/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
interface IUserLoginRequest {
   email: string;
   password: string;
}
interface IUserLoginResponse {
    name: string;
    token: string;
    expires: string;
    claims?: string [];
}
interface ILoginUsersService {
    Login(email: string, password: string): angular.IPromise<IUserLoginResponse>;
}

class LoginUsersService implements ILoginUsersService {

    static $inject: any = ['httpq'];
    /*@ngInject*/
    constructor(private httpq: IHttpQ) {
    }
    Login(email: string, password: string): angular.IPromise<IUserLoginResponse> {
        var request: IUserLoginRequest = {
            email: email,
            password: password
        };
        var response: angular.IPromise<IUserLoginResponse> = this.httpq.Post<IUserLoginRequest, IUserLoginResponse>('/login', request);
        return response;
    }
}
appUsers.factory('loginUsersService', ['httpq', (httpq: IHttpQ) => new LoginUsersService(httpq)]);
