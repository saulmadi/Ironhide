/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/// <reference path="../../../app/users/activateDeactivateUsers/users.activate-deactivate.service.ts"/>
/// <reference path="../../../app/users/activateDeactivateUsers/users.activate-deactivate.controller.ts"/>
/// <reference path="../../../app/users/users/users.service.ts"/>


/* tslint:disable:typedef */
describe('users.activate-deactivate.controller', () => {
    var usersActivateDeactivateUsersController: IUsersActivateDeactivateContoller;
    var $scope: IUsersActivateDeactivateScope;
    var $rootScope: angular.IRootScopeService;
    var $q: angular.IQService;
    var usersPromise: angular.IPromise<IUserResponse[]>;
    var userServiceMock: IUsersService;
    var userStubService: SinonStub;
    var usersActivateDeactivateService: IActivateDeactivateUsersService;
    var users: IUserResponse[] = [
        {
            id: 'id1', name: 'user1', email: 'email1', isActive: true
        },
        {
            id: 'id2', name: 'user2', email: 'email2', isActive: true
        }
    ];
    beforeEach(function() {
        userServiceMock = {
            getUser: function(id: string): angular.IPromise<IUserResponse> {
                return undefined;
            },
            getPagedUsers: function(payload: IUserPagedRequest): angular.IPromise<IUserResponse[]> {
                return undefined;
            }
        }
    });
    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function($controller: angular.IControllerService,
        _$rootScope_: angular.IRootScopeService,
        _$q_: angular.IQService,
        _usersActivateDeactivateService_: IActivateDeactivateUsersService) {
        $q = _$q_;
        usersPromise = getUsersPromise($q);
        userStubService = sinon.stub(userServiceMock, 'getPagedUsers');
        usersActivateDeactivateService = _usersActivateDeactivateService_;
        userStubService.returns(usersPromise);
        $rootScope = _$rootScope_;
        $scope = <IUsersActivateDeactivateScope>$rootScope.$new();
        usersActivateDeactivateUsersController = $controller('users.activate-deactivate.controller'
            , { $scope: $scope, usersService: userServiceMock });


    }));
    it('Should be register as users.activate-deactivate.controller', function() {
        chai.expect(usersActivateDeactivateUsersController).to.be.not.undefined;
    });
    describe('when is created', () => {
        it('Should set ordered by name by default ', () => {
            chai.expect(usersActivateDeactivateUsersController.orderedByName).to.be.true;
            chai.expect(usersActivateDeactivateUsersController.orderedByEmail).to.be.false;
        });
        it('Should set initial page size to 20', () => {
            chai.expect(usersActivateDeactivateUsersController.pageSize).to.be.eq(20);
        })
        it('Should set initial page to 1', () => {
            chai.expect(usersActivateDeactivateUsersController.pageNumber).to.be.eq(1);
        });
        it('Should retrieve the users from the service', () => {
            $rootScope.$apply();
            var usersRetrived = usersActivateDeactivateUsersController.users;
            chai.expect(usersRetrived).to.not.be.undefined;
            chai.expect(usersRetrived).to.be.deep.equal(users);
        });
    });
    describe('GetUsersSortByEmail', () => {
        it('Should get users ordered by email', () => {
            var expectedRequest: IUserPagedRequest = {
                pageNumber: usersActivateDeactivateUsersController.pageNumber,
                pageSize: usersActivateDeactivateUsersController.pageSize,
                field: 'Email'
            };
            usersActivateDeactivateUsersController.getUsersSortByEmail();
            $rootScope.$apply();
            chai.expect(userStubService).to.have.been.calledWith(expectedRequest);
            chai.expect(usersActivateDeactivateUsersController.orderedByEmail).to.be.true;
            chai.expect(usersActivateDeactivateUsersController.orderedByName).to.be.false;
        });
    });
    describe('GetUsersSortByName', () => {
        it('Should get users ordered by name', () => {
            var expectedRequest: IUserPagedRequest = {
                pageNumber: usersActivateDeactivateUsersController.pageNumber,
                pageSize: usersActivateDeactivateUsersController.pageSize,
                field: 'Name'
            };
            usersActivateDeactivateUsersController.getUsersSortByName();
            chai.expect(userStubService).to.have.been.calledWith(expectedRequest);
            chai.expect(usersActivateDeactivateUsersController.orderedByName).to.be.true;
            chai.expect(usersActivateDeactivateUsersController.orderedByEmail).to.be.false;
        });
    });
    describe('Back', () => {

        it('Should get the previus page of users', () => {
            usersActivateDeactivateUsersController.pageNumber = 3;
            var expectedRequest: IUserPagedRequest = {
                pageNumber: 2,
                pageSize: usersActivateDeactivateUsersController.pageSize,
                field: 'Name'
            };
            usersActivateDeactivateUsersController.back();
            chai.expect(userStubService).to.have.been.calledWith(expectedRequest);
        });
    });
    describe('Next', () => {

        it('Should get the next page of users', () => {
            usersActivateDeactivateUsersController.pageNumber = 4;
            var expectedRequest: IUserPagedRequest = {
                pageNumber: 5,
                pageSize: usersActivateDeactivateUsersController.pageSize,
                field: 'Name'
            };
            usersActivateDeactivateUsersController.next();
            chai.expect(userStubService).to.have.been.calledWith(expectedRequest);
        });
    });
    describe('EnableUser', () => {
        var activateUsersSpy: SinonSpy;
        var disableUsersSpy: SinonSpy;

        beforeEach(function() {
            activateUsersSpy = sinon.spy(usersActivateDeactivateService, 'enableUser');
            disableUsersSpy = sinon.spy(usersActivateDeactivateService, 'disableUser');
        });
        it('Should enable user', () => {
            var enable = true;
            var id = 'id';
            usersActivateDeactivateUsersController.enableUser(id, enable);
            chai.expect(activateUsersSpy).to.have.been.calledWith(id);
            chai.expect(disableUsersSpy).to.not.have.been.called;
        });
        it('Should disable user', () => {
            var enable = false;
            var id = 'id';
            usersActivateDeactivateUsersController.enableUser(id, enable);
            chai.expect(disableUsersSpy).to.have.been.calledWith(id);
            chai.expect(activateUsersSpy).to.not.have.been.called;
        });
        it('Should reload the users', () => {
          var enable = true;
          var id = 'id';
          usersActivateDeactivateUsersController.enableUser(id, enable);
          chai.expect(userStubService).to.have.been.called;
        });

    })
    function getUsersPromise($q: angular.IQService): angular.IPromise<IUserResponse[]> {
        var deferred = $q.defer<IUserResponse[]>();
        deferred.resolve(users);
        return deferred.promise;
    }
});
