/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/forgotPassword/users.forgot-password.service.ts"/>
/// <reference path="../../../app/users/forgotPassword/users.forgot-password.controller.ts"/>


/* tslint:disable:typedef */

describe('users.forgotPassword.controller', () => {
    var $rootScope: angular.IRootScopeService;
    var scope: IUserForgotPasswordScope;
    var userForgotController: IUserForgotPassword;
    var successResponse: angular.IPromise<boolean>;
    var stubForgotPasswordService: any;
    var spyModalProvider: any;

    beforeEach(angular.mock.module('app.users'));

    beforeEach(angular.mock.module(function($provide: any) {
        spyModalProvider = sinon.spy();
        $provide.value('$modal', spyModalProvider);
    }));


    beforeEach(inject(function($controller: angular.IControllerService, _$rootScope_: angular.IRootScopeService,
                    _$q_: angular.IQService, _forgotPasswordservice_: IUserForgotPasswordService, _logger_: ILogger) {
        $rootScope = _$rootScope_;
        scope = <IUserForgotPasswordScope>$rootScope.$new();
        userForgotController = $controller('userForgotPasswordController', {$scope: scope});
        successResponse = getSuccessPromise(_$q_);
        stubForgotPasswordService = sinon.stub(_forgotPasswordservice_, 'ResetPassword');

    }));

    afterEach(function() {
        stubForgotPasswordService.restore();
    });
    it('Should be defined', function() {
       chai.expect(userForgotController).to.not.be.undefined;
    });
    it('Should set to true success if the server responded', function() {
        stubForgotPasswordService.returns(successResponse);
        userForgotController.email = 'test@test.com';
        userForgotController.ResetPassword();
        $rootScope.$apply();
        chai.expect(userForgotController.success).to.be.true;

    });
    it('Should call the service with the right email', function() {
        stubForgotPasswordService.returns(successResponse);
        userForgotController.email = 'test@test.com';
        userForgotController.ResetPassword();
        $rootScope.$apply();
        chai.expect(stubForgotPasswordService).to.have.been.calledWith('test@test.com');
    });

    function getSuccessPromise($q: angular.IQService): angular.IPromise<boolean> {
        var deferred = $q.defer<boolean>();
        deferred.resolve(true);
        return deferred.promise;
    }
});
