/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../app/common/httpq/httpQ.service.ts"/>

/* tslint:disable:typedef */
var expect: any = chai.expect;
interface IDataMock {
    name: string;
}
describe('httpq', () => {
    var $httpBackend: angular.IHttpBackendService;
    var $q: angular.IQService;
    var httpq: HttpQ;
    var dataMock: IDataMock = {
        name: 'mocked'
    };
    beforeEach(angular.mock.module('app.core'));
    beforeEach(inject(function(_$httpBackend_: angular.IHttpBackendService
                                , _$q_: angular.IQService
                                , _httpq_: HttpQ
                               ) {
        $httpBackend = _$httpBackend_;
        $q = _$q_;

        httpq = _httpq_;
    }));
    afterEach(function() {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });
    describe('GET', () => {
        it('Promise should be defined', function() {
            $httpBackend.whenGET('/test').respond(() => dataMock);
            var promise = httpq.Get<IDataMock>('/test');
            $httpBackend.flush();
            expect(promise).to.be.defined;
        });
        it('Get should return correct data on success', function() {
            $httpBackend.whenGET('/test').respond(dataMock);
            var response: IDataMock;
            var promise = httpq.Get<IDataMock>('/test');
            promise.then(function(data: IDataMock) {
                response = data;
            });
            $httpBackend.flush();
            expect(response.name).to.be.equal(dataMock.name);
        });
        it('Get should reject the promise and respond with error', function() {
            $httpBackend.whenGET('/test').respond(405, 'error');
            var promise = httpq.Get<IDataMock>('/test');
            var result: any;
            promise.then(function(data: IDataMock) {
                result = data;
            }, function(error) {
                result = error;
            });
            $httpBackend.flush();
            expect(result).to.be.equal('error');
        });
   });
   describe('POST', () => {
       it('Post should send the data and the server respond correctly', function() {
           $httpBackend.whenPOST('/test', dataMock).respond(201, 'success');
           var promise = httpq.Post<IDataMock, IDataMock>('/test', dataMock);
           var result: any;
           promise.then(function(data: any) {
               result = data;
            });
           $httpBackend.flush();
           expect(result).to.be.equal('success');
        });
       it('Post should reject the promise and responde with error', function() {
           $httpBackend.whenPOST('/test', dataMock).respond(405, 'error');
           var promise = httpq.Post<IDataMock, IDataMock>('/test', dataMock);
           var result: any;
           promise.then(function(data: any) {
               result = data;
               }, function(error) {
                  result = error;
           });
           $httpBackend.flush();
            expect(result).to.be.equal('error');
        });
    });
    describe('PUT', () => {
       it('Put should send the data and the server responde correctly', function() {
          $httpBackend.whenPUT('/test', dataMock).respond(201, 'success');
           var promise = httpq.Put<IDataMock, IDataMock>('/test', dataMock);
           var result: any;
           promise.then(function(data: any) {
               result = data;
            });
           $httpBackend.flush();
           expect(result).to.be.equal('success');
       });
       it('Put should reject the promise and responde with error', function() {
           $httpBackend.whenPUT('/test', dataMock).respond(405, 'error');
           var promise = httpq.Put<IDataMock, IDataMock>('/test', dataMock);
           var result: any;
           promise.then(function(data: any) {
               result = data;
               }, function(error) {
                  result = error;
           });
           $httpBackend.flush();
           expect(result).to.be.equal('error');
        });
    });


});
