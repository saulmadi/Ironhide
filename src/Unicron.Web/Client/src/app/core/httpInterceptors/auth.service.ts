/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../core.module.ts" />
class AuthorizationService {
    static $inject: any = ['$q', 'currentUser'];
    /*@ngInject*/
    constructor(private $q: angular.IQService, private currentUser: ICurrentUserManager,
            private $injector: angular.auto.IInjectorService) {
    }
    request: any = (config: angular.IRequestConfig): any => {
        var user: ICurrentUser = this.currentUser.GetUser();
        if (user) {
            config.headers.Authorization = 'Bearer ' + user.token;
        }
        return config || this.$q.when();
    };
    requestError: any = (rejection: any): any => {
        return this.$q.reject(rejection);
    };
    responseError: any = (rejection: any): any => {
        if (rejection.status === 401) {
            var $state: any = this.$injector.get('$state');
            $state.go('login');
            this.currentUser.RemoveUser();
        }
        return this.$q.reject(rejection);
    };
}
appCore.factory('authorizationService', ['$q', 'currentUser', '$injector',
    ($q: angular.IQService, currentUser: ICurrentUserManager, $injector: angular.auto.IInjectorService) =>
    new AuthorizationService($q, currentUser, $injector)]);

appCore.config(['$httpProvider', function($httpProvider: angular.IHttpProvider): void {
    $httpProvider.interceptors.push('authorizationService');
}]);
