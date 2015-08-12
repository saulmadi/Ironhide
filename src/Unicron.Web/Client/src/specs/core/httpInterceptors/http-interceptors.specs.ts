/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../../../app/core/current-user.factory.ts"/>
/// <reference path="../../../app/core/httpInterceptors/auth.service.ts"/>
/// <reference path="../../../app/core/httpInterceptors/log-http.service.ts"/>
/// <reference path="../../../app/common/logger/logger.service.ts"/>
/* tslint:disable:typedef */
describe('HttpInterceptors', () => {

    var $httpProvider: angular.IHttpProvider;
    var $httpBackend: angular.IHttpBackendService;
    var $http: angular.IHttpService;

    var userMock: ICurrentUser = {
        email : 'email',
        name : 'name',
        token : 'tokenXXX',
        expires : JSON.stringify(new Date())
    };

    beforeEach(angular.mock.module('app.core', function(_$httpProvider_: angular.IHttpProvider) {
        $httpProvider = _$httpProvider_;
    }));
    beforeEach(inject(function(_$httpBackend_: angular.IHttpBackendService,
                            _$http_: angular.IHttpService) {


        $httpBackend = _$httpBackend_;
        $http = _$http_;

    }));
    afterEach(function() {

        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    describe('Authorization Interceptor', () => {
        var currentUser: ICurrentUserManager;
        var authorizationService: AuthorizationService;
        var stubCurrentUser: any;
        var spyCurrentUserRemove: any;
        var $state: angular.ui.IStateService;
        beforeEach(inject(function( _authorizationService_: AuthorizationService,
            _currentUser_: ICurrentUserManager, _$state_: angular.ui.IStateService) {
            currentUser = _currentUser_;
            authorizationService = _authorizationService_;
            stubCurrentUser = sinon.stub(currentUser, 'GetUser', () => userMock);
            spyCurrentUserRemove = sinon.spy(currentUser, 'RemoveUser');
            $state = _$state_;

        }));
        afterEach(function() {
            stubCurrentUser.restore();
            spyCurrentUserRemove.restore();

        });
        it('Should have the authorization service be defined', function() {
            chai.expect(authorizationService).to.be.not.undefined;
        });
        it('Should authorizationService listed has http interceptor', function() {
            chai.expect($httpProvider.interceptors).to.contain('authorizationService');
        });
        it('Should set the token in the headers', function() {

            $httpBackend.whenGET('/test', function(headers: any) {
                return headers.Authorization === 'Bearer ' + userMock.token;
            }).respond(' ');
            $http.get('/test');
            $httpBackend.flush();
        });
        it('Should redirect to login on 401 errors', function() {
            var errorData: any = {
                name: 'Error test',
                origin: 'Test'
            };
            $state.go = sinon.spy();
            $httpBackend.whenGET('/test401Error').respond(401, errorData);
            $http.get('/test401Error', errorData);
            $httpBackend.flush();
            chai.expect($state.go).to.have.been.calledWith('login');
        });
        it('Should remove user on 401 errors', function() {
            var errorData: any = {
                name: 'Error test',
                origin: 'Test'
            };
            $state.go = sinon.spy();
            $httpBackend.whenGET('/test401Error').respond(401, errorData);
            $http.get('/test401Error', errorData);
            $httpBackend.flush();
            chai.expect(spyCurrentUserRemove).to.have.been.called;
        });
    });
   describe('Logger Interceptor', () => {
       var logHttpService: LogHttpService;
       var $q: angular.IQService;
       var logger: ILogger;
       var spyLogger: any;

       beforeEach(inject(function(_logHttpService_: LogHttpService, _$q_: angular.IQService, _logger_: ILogger) {
           logHttpService = _logHttpService_;
           $q = _$q_;
           logger = _logger_;
           spyLogger = sinon.spy(logger, 'error');
       }));
       afterEach(function() {
          spyLogger.restore();

       });

       it('Should have the logger http service be defined', function() {
           chai.expect(logHttpService).to.be.not.undefined;
       });
        it('Should loggerHttpService listed has http interceptor', function() {
            chai.expect($httpProvider.interceptors).to.contain('logHttpService');
        });
       it('Should log the response error', function() {
            var errorData: any = {
                name: 'Error test',
                origin: 'Test'
            };
            $httpBackend.whenGET('/testLog').respond(402, errorData);
            $http.get('/testLog', errorData);
            $httpBackend.flush();
           chai.expect(spyLogger).to.have.been.calledWith('Error on Response', 'Error 402 in GET URL = /testLog');
       });

    });

});
