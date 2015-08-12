/// <reference path="../../../../typings/mocha/mocha.d.ts"/>
/// <reference path="../../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts"/>
/// <reference path="../../../../typings/sinon/sinon.d.ts"/>
/// <reference path="../../../../typings/chai/chai.d.ts"/>
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts"/>
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>
/// <reference path="../../../app/users/users/users.service.ts"/>

/* tslint:disable:typedef */
describe('users.service', () => {
    var $httpBackend: angular.IHttpBackendService;
    var usersService: IUsersService;
    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(_$httpBackend_: angular.IHttpBackendService,
        _usersService_: IUsersService) {
        $httpBackend = _$httpBackend_;
        usersService = _usersService_;
    }));
    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('The service should be defined', () => {
        chai.expect(usersService).to.not.be.undefined;
    });
    describe('GetUser', () => {
        var userResponse: IUserResponse = {
            id: 'id',
            name: 'user name',
            email: 'user email',
            isActive: true
        }
        it('Should return the expected user by his Id', () => {
            var id = 'id';
            $httpBackend.whenGET('/user/' + id).respond(userResponse);
            var promise = usersService.getUser(id);
            var result: any;
            promise.then(function(data) {
                result = data;
            });
            $httpBackend.flush();
            chai.expect(result).to.be.deep.equal(userResponse);
        });
    });
    describe('GetPagedUsers', () => {
        var usersPagedResponse: IUserResponse[] = [
            {
                id: 'id1', name: 'user name 1', email: 'user email', isActive: true
            },
            {
                id: 'id2', name: 'user name 2', email: 'user email 2', isActive: true
            }
        ];
        var usersPagedRequest: IUserPagedRequest = {
            pageNumber: 1, pageSize: 1, field: 'id'
        };

        it('Should return paged users', () => {
            $httpBackend.whenGET('/users?' + 'PageNumber='
                + usersPagedRequest.pageNumber + '&PageSize='
                + usersPagedRequest.pageSize + '&Field='
                + usersPagedRequest.field).respond(usersPagedResponse);
            var promise = usersService.getPagedUsers(usersPagedRequest);
            var result: any;
            promise.then(function(data){
              result = data;
            });
            $httpBackend.flush();
            chai.expect(result).to.be.deep.equal(usersPagedResponse);
        });
    });

});
