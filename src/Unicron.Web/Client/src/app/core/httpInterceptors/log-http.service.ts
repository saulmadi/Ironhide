/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core.module.ts" />
class LogHttpService {
    static $inject: any = ['$q', 'logger'];
    /*@ngInject*/
    constructor(private $q: angular.IQService, private logger: ILogger) {
    }
    responseError: any = (rejection: any): any =>  {
        this.logError(rejection);
        return this.$q.reject(rejection);
    };
    requestError: any = (rejection: any): any => {
        this.logError(rejection);
        return this.$q.reject(rejection);
    };
    private logError(rejection: any): void {
         var errorData: any = {
            method: rejection.config.method,
            url: rejection.config.url,
            data: rejection.data,
            headers: rejection.config.headers,
            status: rejection.status,
            statusText: rejection.statusText
        };
        // Not show on screen
       this.logger.error('Error on Response', 'Error '
        + errorData.status + ' in ' + errorData.method + ' URL = ' + errorData.url , errorData, true);
    }
}
appCore.factory('logHttpService', ['$q', 'logger', ($q: angular.IQService, logger: ILogger) =>
  new LogHttpService($q, logger)]);

appCore.config(['$httpProvider', function($httpProvider: angular.IHttpProvider): void {
    $httpProvider.interceptors.push('logHttpService');
}]);
