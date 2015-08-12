/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
interface IUserForgotPasswordRequest {
    email: string;
}
interface IUserForgotPasswordService {
    ResetPassword(email: string): angular.IPromise<boolean>;
};
class UserForgotPasswordService implements IUserForgotPasswordService {

    static $inject: any = ['httpq'];

    constructor (private httpq: IHttpQ) {
    }
    ResetPassword(email: string): angular.IPromise<boolean> {
        var request: IUserForgotPasswordRequest = {
            email: email
        };
            return this.httpq.Post<IUserForgotPasswordRequest, boolean>('/password/requestReset/', request);
    }
}

appUsers.service('forgotPasswordservice', UserForgotPasswordService);
