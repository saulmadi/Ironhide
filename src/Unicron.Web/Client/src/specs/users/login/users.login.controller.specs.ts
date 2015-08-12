/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/login/users.login.service.ts"/>
/// <reference path="../../../app/users/login/users.login.controller.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/// <reference path="../../../app/core/current-user.factory.ts"/>
/* tslint:disable:typedef */

describe('users.login.controller', () => {
    var userLoginController: IUsersLoginContoller;
    var scope: IUsersLoginScope;
    var userLoggedData: IUserLoginResponse = {
        name: 'User Test',
        expires: JSON.stringify(new Date()),
        token: 'token',
        claims: ['test']
    };
    var email = 'test@test.com';
    var password = 'password';
    var errorMessage = 'Invalid email address or password. Please try again.';
    var userLoggedPromise: angular.IPromise<IUserLoginResponse>;
    var failedUserPromise: angular.IPromise<any>;
    var loginUsersService: ILoginUsersService;
    var currentUser: ICurrentUserManager;
    var stubLoginUserService: any;
    var spyCurrentUserManagerLocal: any;
    var spyCurrentUserManagerSession: any;
    var spyLogService: any;
    var $rootScope: angular.IRootScopeService;
    var logger: ILogger;

    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function($controller: angular.IControllerService, _$rootScope_: angular.IRootScopeService,
        _$q_: angular.IQService, _loginUsersService_: ILoginUsersService,
        _currentUser_: ICurrentUserManager, _logger_: ILogger) {
        $rootScope = _$rootScope_;
        scope = <IUsersLoginScope>$rootScope.$new();
        userLoginController = $controller('users.login.controller', {$scope: scope});
        userLoggedPromise = getUserPromise(_$q_);
        failedUserPromise = getUserFailedPromise(_$q_);
        loginUsersService = _loginUsersService_;
        currentUser = _currentUser_;
        logger = _logger_;
    }));
    beforeEach(function() {
        stubLoginUserService = sinon.stub(loginUsersService, 'Login');
        spyCurrentUserManagerLocal = sinon.spy(currentUser, 'SetUserLocal');
        spyCurrentUserManagerSession = sinon.spy(currentUser, 'SetUserOnSession');
        spyLogService = sinon.spy(logger, 'error');
    });
    afterEach(function() {
        stubLoginUserService.restore();
        spyCurrentUserManagerLocal.restore();
        spyCurrentUserManagerSession.restore();
        spyLogService.restore();
    });

    it('Should be register as users.login.controller ', function() {
        chai.expect(userLoginController).to.be.not.undefined;
    });
    it('Should call the login service', function() {
        stubLoginUserService.returns(userLoggedPromise);
        userLoginController.email = email;
        userLoginController.password = password;
        userLoginController.login();
        $rootScope.$apply();
        chai.expect(stubLoginUserService).to.have.been.calledWith(email, password);
    });
    it('Should save the user logged on local storage because user select remember ', function() {
        stubLoginUserService.returns(userLoggedPromise);
        userLoginController.email = email;
        userLoginController.password = password;
        userLoginController.rememberMe = true;
        userLoginController.login();
        $rootScope.$apply();
        chai.expect(spyCurrentUserManagerLocal).to.have.been.calledWith
                    (email, userLoggedData.name, userLoggedData.token);
    });
    it('Should save the user logged on session storage because user does not select remember ', function() {
        stubLoginUserService.returns(userLoggedPromise);
        userLoginController.email = email;
        userLoginController.password = password;
        userLoginController.rememberMe = false;
        userLoginController.login();
        $rootScope.$apply();
        chai.expect(spyCurrentUserManagerSession).to.have.been.calledWith
                        (email, userLoggedData.name, userLoggedData.token);
    });
    it('Should call the logger service on error request', function() {
        stubLoginUserService.returns(failedUserPromise);
        userLoginController.email = email;
        userLoginController.password = password;
        userLoginController.login();
        $rootScope.$apply();
        chai.expect(spyLogService).to.have.been.calledWith('Error', errorMessage);
    });
    it('Should redirect the user to home if is logged succesfully', inject(function($state: any) {
        $state.go = sinon.spy();
        stubLoginUserService.returns(userLoggedPromise);
        userLoginController.login();
        $rootScope.$apply();
        chai.expect($state.go).to.have.been.calledWith('home');
    }));

    function getUserPromise($q: angular.IQService): angular.IPromise<IUserLoginResponse> {
        var deferred = $q.defer<IUserLoginResponse>();
        deferred.resolve(userLoggedData);
        return deferred.promise;
    }

    function getUserFailedPromise($q: angular.IQService): angular.IPromise<any> {
        var deferred = $q.defer();
        deferred.reject('Invalid email address or password. Please try again.');
        return deferred.promise;
    }
});
