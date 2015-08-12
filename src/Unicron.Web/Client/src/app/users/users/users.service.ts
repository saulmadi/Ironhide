/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />

interface IUserPagedRequest {
    pageNumber: number;
    pageSize: number;
    field: string;
}
interface IUserResponse {
    id: string;
    name: string;
    email: string;
    isActive: boolean;
}

interface IUsersService {
    getPagedUsers(payload: IUserPagedRequest): angular.IPromise<IUserResponse[]>;
    getUser(id: string): angular.IPromise<IUserResponse>;
}

class UsersService implements IUsersService {

    static $inject: any = ['httpq'];
    /*@ngInject*/
    constructor(private httpq: IHttpQ) {
    }
    getPagedUsers(payload: IUserPagedRequest): angular.IPromise<IUserResponse[]> {
        return this.httpq.Get<IUserResponse[]>('/users?' + 'PageNumber='
            + payload.pageNumber + '&PageSize='
            + payload.pageSize + '&Field='
            + payload.field);
    }
    getUser(id: string): angular.IPromise<IUserResponse> {

        return this.httpq.Get<IUserResponse>('/user/' + id);
    }
}

appUsers.factory('usersService', ['httpq', (httpq: IHttpQ) => new UsersService(httpq)]);
