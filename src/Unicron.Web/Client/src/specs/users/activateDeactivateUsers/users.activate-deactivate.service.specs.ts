/// <reference path="../../../../typings/mocha/mocha.d.ts"/>
/// <reference path="../../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts"/>
/// <reference path="../../../../typings/sinon/sinon.d.ts"/>
/// <reference path="../../../../typings/chai/chai.d.ts"/>
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/// <reference path="../../../app/users/activateDeactivateUsers/users.activate-deactivate.service.ts"/>

describe('users.activate-deactivate.service', () => {
    var $httpBackend: angular.IHttpBackendService;
    var userActivateDeactivateService: IActivateDeactivateUsersService;
    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(_$httpBackend_: angular.IHttpBackendService,
      _usersActivateDeactivateService_: IActivateDeactivateUsersService) {
        $httpBackend = _$httpBackend_;
        userActivateDeactivateService = _usersActivateDeactivateService_;
    }));
    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });
    it('The service should be defined', () => {
        chai.expect(userActivateDeactivateService).to.not.be.undefined;
    });
    describe('EnableUser', () => {

        var enableUserRequest: IActivateDeactivateUserRequest = {
            id: 'id1',
            enable: true
        };
        it('Should enable user', () => {
            $httpBackend.whenPOST('/users/enable', enableUserRequest).respond(200, true);
            var promise = userActivateDeactivateService.enableUser(enableUserRequest.id);
            var result: any;
            promise.then(function(data) {
                result = data;
            });
            $httpBackend.flush();
            chai.expect(result).to.be.true;
        });
    });
    describe('DisableUser', () => {

        var disableUserRequest: IActivateDeactivateUserRequest = {
            id: 'id1',
            enable: false
        };
        it('Should disable user', () => {
            $httpBackend.whenPOST('/users/enable', disableUserRequest).respond(200, true);
            var promise = userActivateDeactivateService.disableUser(disableUserRequest.id);
            var result: any;
            promise.then(function(data) {
                result = data;
            });
            $httpBackend.flush();
            chai.expect(result).to.be.true;
        });
    });

});
