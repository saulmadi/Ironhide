/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/login/users.login.service.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/* tslint:disable:typedef */

describe('users.login.service', () => {
    var $httpBackend: angular.IHttpBackendService;
    var userResponse: IUserLoginResponse = {
        name: 'User Test',
        token: 'User Token',
        expires: 'Expire Time'
    };
    var loginService: ILoginUsersService;
    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(_$httpBackend_: angular.IHttpBackendService,
        _loginUsersService_: ILoginUsersService) {
      $httpBackend = _$httpBackend_;
        loginService = _loginUsersService_;
    }));
    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('Should return user data', () => {
        var userLoginRequest: IUserLoginRequest = {
            email: 'test@test.com',
            password: 'test'
        };
        $httpBackend.whenPOST('/login', userLoginRequest).respond(userResponse);
        var promise = loginService.Login(userLoginRequest.email, userLoginRequest.password);
        var result: any;
        promise.then(function(data) {
             result = data;
        });
        $httpBackend.flush();
        chai.expect(result.name).to.be.equal(userResponse.name);
    });
});
