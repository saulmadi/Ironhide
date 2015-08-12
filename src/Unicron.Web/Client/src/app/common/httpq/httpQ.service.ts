/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../../../app/core/core.module.ts" />
interface IHttpQ {
    Get<T>(resource: string): angular.IPromise<T>;
    Post<T, K>(resource: string, payload: T): angular.IPromise<K>;
    Put<T, K>(resource: string, payload: T): angular.IPromise<K>;
}
class HttpQ implements IHttpQ {
    static $inject: any = ['$http', '$q'];
    /*@ngInject*/
    constructor(private $http: angular.IHttpService, private $q: angular.IQService) {
    }
    getLocalStorageUrl(resource: string): string {
        return (localStorage.getItem('apiUrl') || '') + resource;
    }
    Get<T>(resource: string): angular.IPromise<T> {
        var defer: angular.IDeferred<T> = this.$q.defer();
        this.$http.get<T>(this.getLocalStorageUrl(resource))
            .success(function(data: T): void {
                defer.resolve(data);
            })
            .error(function(error: T): void {
                defer.reject(error);
            });
        return defer.promise;
    }
    Post<T, K>(resource: string, payload: T): angular.IPromise<K> {
        var defer: angular.IDeferred<K> = this.$q.defer();
        this.$http.post<T>(this.getLocalStorageUrl(resource), payload)
            .success(function(data: any): void {
                defer.resolve(data);
            })
            .error(function(error: any): void {
                defer.reject(error);
            });
        return defer.promise;
    }
    Put<T, K>(resource: string, payload: T): angular.IPromise<K> {
        var defer: angular.IDeferred<K> = this.$q.defer();
        this.$http.put<T>(this.getLocalStorageUrl(resource), payload)
            .success(function(data: any): void {
                defer.resolve(data);
            })
            .error(function(error: any): void {
                defer.reject(error);
            });
        return defer.promise;
    }
}
appCore.factory('httpq', ['$http', '$q', ($http: angular.IHttpService, $q: angular.IQService) =>
    new HttpQ($http, $q)]);
