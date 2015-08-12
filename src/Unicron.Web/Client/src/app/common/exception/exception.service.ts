/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="exception.module.ts" />
/// <reference path="../logger/logger.service.ts" />
'use strict';

interface IException {
   catcher(message: string): (reason: string) => void;
}

class Exception implements IException {

    logger: ILogger;
    static $inject: any = ['logger'];
    /*@ngInject*/
    constructor (logger: ILogger) {
       this.logger = logger;
    }
    catcher(message: string ): (reason: string) => void {
        return function(reason: string): void {
          this.logger.error(reason, message);
        };
    }
}
commonException.factory('exception', ['logger', (logger: ILogger) => new Exception(logger)]
);
