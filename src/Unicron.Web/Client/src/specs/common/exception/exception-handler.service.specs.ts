/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../app/common/exception/exception-handler.service.ts"/>

/* tslint:disable:typedef */
var expect: any = chai.expect;

describe('common.exception', () => {
    var logger: ILogger;
    var toastr: any;
    var mocks = {
        errorMessage: 'fake error',
        prefix: '[TEST]'
    };
    var exceptionHandler: any;
    beforeEach( angular.mock.module(function($provide: any) {
        logger = <Logger><any>sinon.createStubInstance(Logger);
        toastr = sinon.stub();
       $provide.value('logger', logger);
       $provide.value('toastr', toastr);
        })
    );

     beforeEach(function() {
         angular.mock.module('common.exception');
     });

     beforeEach(inject(function(_exceptionHandler_) {
          exceptionHandler = _exceptionHandler_.getConfig();

         })
    );

    describe('$exceptionHandler', () => {
        it('should be defined', inject(function($exceptionHandler) {
            expect($exceptionHandler).to.be.defined;

        }));
        it('should have configuration', inject(function($exceptionHandler) {
            expect($exceptionHandler.config).to.be.defined;
        }));

        describe('with appErrorPrefix', () => {
            var $rootScope: any;
            beforeEach(function () {
                exceptionHandler.configure(mocks.prefix);
            });
            beforeEach(inject(function(_$rootScope_) {
                $rootScope = _$rootScope_;
            }));
            it('shoudl have exceptionHandlerProvider defined', inject(function() {
                expect(exceptionHandler).to.be.defined;
            }));
            it('should have appErrorPrefix set properly', inject(function() {
                expect(exceptionHandler.appErrorPrefix).to.equal(mocks.prefix);
            }));
            it('should throw an error when forced', inject(function() {
                expect(functionThatWillThrow).to.throw();
            }));
            it('manual error is handled by decorator', function() {
                var exception: any;
                exceptionHandler.configure(mocks.prefix);
                try {
                   $rootScope.$apply(functionThatWillThrow);
                } catch (ex) {
                    exception = ex;
                   expect(ex.message).to.equal(mocks.prefix + mocks.errorMessage);
                }
            });


        });
    });

     function functionThatWillThrow() : void {
         'use strict';
        throw new Error(mocks.errorMessage);
    }


});
