/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
interface IActivateDeactivateUserRequest {
    id: string;
    enable: boolean;
}
interface IActivateDeactivateUsersService {
    enableUser(id: string): angular.IPromise<boolean>;
    disableUser(id: string): angular.IPromise<boolean>;
}

class ActivateDeactivateUsersService implements IActivateDeactivateUsersService {
    static $inject: any = ['httpq'];
    /*@ngInject*/
    constructor(private httpq: IHttpQ) {
    }
    enableUser(id: string): angular.IPromise<boolean> {
        var payload: IActivateDeactivateUserRequest = {
            id: id,
            enable: true
        };
        return this.httpq.Post('/users/enable', payload);
    }
    disableUser(id: string): angular.IPromise<boolean> {
      var payload: IActivateDeactivateUserRequest = {
          id: id,
          enable: false
      };
      return this.httpq.Post('/users/enable', payload);
    }
}

appUsers.factory('usersActivateDeactivateService', ['httpq', (httpq: IHttpQ) => new ActivateDeactivateUsersService(httpq)]);
