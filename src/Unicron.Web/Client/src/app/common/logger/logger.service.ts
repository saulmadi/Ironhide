/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/toastr/toastr.d.ts" />
/// <reference path="logger.module.ts" />
'use strict';

interface ILogger {
    error(title: string, message: string, data: any, notShowOnScreen?: boolean): void;
    info(title: string, message: string, data: any): void;
    success(title: string, message: string, data: any): void;
    warning(title: string, message: string, data: any): void;
}

class Logger implements ILogger {
    log: angular.ILogService;
    toastr: Toastr;
    static $inject: any = ['$log', 'toastr'];
    /*@ngInject*/
    constructor (log: angular.ILogService, toastr: Toastr) {
        this.log = log;
        this.toastr = toastr;
    }
    error(title: string, message: string, data: any, notShowOnScreen?: boolean): void {
        if (!notShowOnScreen) {
            this.toastr.error(message, title);
        }
         this.log.error('Error: ' + message, data);
     }
    info(title: string, message: string, data: any): void {
        this.toastr.info(message, title);
        this.log.info('Info: ' + message, data);
    }
    success(title: string, message: string, data: any): void {
        this.toastr.success(message, title);
        this.log.info('Success: ' + message, data);
    }
    warning(title: string, message: string, data: any): void {
         this.toastr.warning(message, title);
        this.log.info('Warning: ' + message, data);
    }
}
commonLogger.factory('logger', ['$log', 'toastr', ($log: angular.ILogService, toastr: Toastr) => new Logger($log, toastr)]
);
