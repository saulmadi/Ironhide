/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/register/users.register.service.ts"/>
/// <reference path="../../../app/users/register/users.register.controller.ts"/>

describe('user.register.controller', () => {

    var userRegisterController: IUserRegister;
    var $rootScope: angular.IRootScopeService;
    var scope: IUserRegisterScope;
    var successResponse: angular.IPromise<boolean>;
    var stubRegisterService: any;

    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function($controller: angular.IControllerService,
                            _$rootScope_: angular.IRootScopeService,
                            _$q_: angular.IQService,
                            _registerUsersService_: IUserRegisterService) {
        $rootScope = _$rootScope_;
        scope = <IUserRegisterScope>$rootScope.$new();
        userRegisterController = $controller('userRegisterController', {$scope: scope});
        successResponse = getSuccessPromise(_$q_);
        stubRegisterService = sinon.stub(_registerUsersService_, 'Register');

    }));
    afterEach(function() {
       stubRegisterService.restore();
    });
    it('Should be defined', () => {
        chai.expect(userRegisterController).to.not.be.undefined;
    });
    it('Should set to true success if the server responded', function() {
        stubRegisterService.returns(successResponse);
        userRegisterController.Email = 'test@test.com';
        userRegisterController.Name = 'test';
        userRegisterController.PhoneNumber = '1111';
        userRegisterController.Password = 'password';
        userRegisterController.Register();
        $rootScope.$apply();
        chai.expect(userRegisterController.Success).to.be.true;

    });
    it('Should call the service with the right data', function() {
        stubRegisterService.returns(successResponse);
        userRegisterController.Email = 'test@test.com';
        userRegisterController.Name = 'test';
        userRegisterController.PhoneNumber = '1111';
        userRegisterController.Password = 'password';
        userRegisterController.Register();
        $rootScope.$apply();

        var expectedRequest: IUserRegisterRequest = {
            email: userRegisterController.Email,
            name: userRegisterController.Name,
            password: userRegisterController.Password,
            phoneNumber: userRegisterController.PhoneNumber
        };
        chai.expect(stubRegisterService).to.have.been.calledWith(expectedRequest);

    });

    function getSuccessPromise($q: angular.IQService): angular.IPromise<boolean> {
        var deferred = $q.defer<boolean>();
        deferred.resolve(true);
        return deferred.promise;
    }
});
