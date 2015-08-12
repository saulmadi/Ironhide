/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/toastr/toastr.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../app/common/logger/logger.service.ts"/>
/// <reference path="../../../app/common/logger/logger.module.ts"/>

describe('Logger', () => {
    var logger: ILogger;
    var toastrService: Toastr;
    beforeEach(angular.mock.module(function($provide: any) {

        $provide.value('toastr', toastr);
    }));
    beforeEach(angular.mock.module('common.logger'));

    beforeEach(inject(function(_logger_: ILogger, _toastr_: Toastr) {
        logger = _logger_;
        toastrService = _toastr_;
    }));
    it('should be defined', function() {
       chai.expect(logger).to.be.not.undefined;
    });
    describe('Error', () => {
        var spyToastrError: any;
        beforeEach(function() {
            spyToastrError = sinon.spy(toastrService, 'error');
        });
        afterEach(function() {
            spyToastrError.restore();
        });
        it('should call tostr if notShowOnScreen is false', function() {
            logger.error('Test', 'Test', null);
            chai.expect(spyToastrError).to.have.been.called;
        });
        it('should not call toastr if notShownScreen is true', function() {
            logger.error('Test', 'Test', null, true);
            chai.expect(spyToastrError).to.not.have.been.called;
        });
    });
    describe('Info', () => {
        var spyToastrInfo: any;
        beforeEach(function() {
            spyToastrInfo = sinon.spy(toastrService, 'info');
        });
        afterEach(function() {
            spyToastrInfo.restore();
        });
        it('should call tostr if notShowOnScreen is false', function() {
            logger.info('Test', 'Test', null);
            chai.expect(spyToastrInfo).to.have.been.called;
        });
    });
    describe('Success', () => {
        var spyToastrSuccess: any;
        beforeEach(function() {
            spyToastrSuccess = sinon.spy(toastrService, 'success');
        });
        afterEach(function() {
            spyToastrSuccess.restore();
        });
        it('should call tostr if notShowOnScreen is false', function() {
            logger.success('Test', 'Test', null);
            chai.expect(spyToastrSuccess).to.have.been.called;
        });
    });
    describe('Success', () => {
        var spyToastrWarning: any;
        beforeEach(function() {
            spyToastrWarning = sinon.spy(toastrService, 'warning');
        });
        afterEach(function() {
            spyToastrWarning.restore();
        });
        it('should call tostr if notShowOnScreen is false', function() {
            logger.warning('Test', 'Test', null);
            chai.expect(spyToastrWarning).to.have.been.called;
        });
    });


});
