/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/register/users.register.service.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/* tslint:disable:typedef */
describe('user.register.service', () => {
    var userRegisterService: IUserRegisterService;
    var $httpBackend: angular.IHttpBackendService;

    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(
    _registerUsersService_: IUserRegisterService,
    _$httpBackend_: angular.IHttpBackendService) {
        $httpBackend = _$httpBackend_;
        userRegisterService = _registerUsersService_;
    }));

    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });
    it('Should call the api method with the right information of users', function() {
        var userRegisterRequest: IUserRegisterRequest = {
            name: 'user test',
            email: 'test@test',
            password: 'password',
            phoneNumber: '000'
        };
        $httpBackend.whenPOST('/register', userRegisterRequest).respond(200, true);
        var promise = userRegisterService.Register(userRegisterRequest);

        var result: any;
        promise.then((data: boolean): any => {
            result = data;
        });
        $httpBackend.flush();
        chai.expect(result).to.be.true;

    });
});
