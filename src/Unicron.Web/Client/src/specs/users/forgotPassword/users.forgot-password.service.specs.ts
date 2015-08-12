/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/forgotPassword/users.forgot-password.service.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/* tslint:disable:typedef */
describe('users.forgotPassword.service', () => {

    var forgotPasswordService: IUserForgotPasswordService;
    var $httpBackend: angular.IHttpBackendService;
    var emailSent: IUserForgotPasswordRequest = {
        email: 'test@test.com'
    };

    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(_forgotPasswordservice_: IUserForgotPasswordService,
            _$httpBackend_: angular.IHttpBackendService) {
        forgotPasswordService = _forgotPasswordservice_;
        $httpBackend = _$httpBackend_;
    }));
   ;
    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });
    it('Should be definied', function() {
        chai.expect(forgotPasswordService).to.not.be.undefined;
    });
    it('Should return true if backend respond without error', function() {
        $httpBackend.whenPOST('/password/requestReset/', emailSent).respond(true);

        var promise = forgotPasswordService.ResetPassword('test@test.com');
        var result: any;
        promise.then(function(data) {
            result = data;
        });
        $httpBackend.flush();
        chai.expect(result).to.be.true;

    });
    it('Should return false if backend respond with error', function() {
        $httpBackend.whenPOST('/password/requestReset/', emailSent).respond(405, false);

        var promise = forgotPasswordService.ResetPassword('test@test.com');
        var result: any;
        promise.catch(function(data) {
            result = data;
        });
        $httpBackend.flush();
        chai.expect(result).to.be.false;

    });


});
