/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="exception.module.ts" />
/// <reference path="../logger/logger.service.ts" />
'use strict';

interface IConfigureExceptionHandlerProvider {
    appErrorPrefix: string;
    config(): void;
    configure(errorPrefix: string): void;
}

class ConfigureExceptionHandlerProvider implements IConfigureExceptionHandlerProvider {
    appErrorPrefix: string;
    constructor() {
        this.config();
    }
    config(): void {
         this.appErrorPrefix = undefined;
     }
    configure(errorPrefix: string): void {
        this.appErrorPrefix = errorPrefix;
    }
}

class ExceptionHandlerProvider implements angular.IServiceProvider {
    config: IConfigureExceptionHandlerProvider;
    constructor(config: IConfigureExceptionHandlerProvider) {
        this.config = config;
    }
    $get(): any {
            return {
                getConfig: (): any => {
                return this.config;
                }
            };
    }
}
commonException.provider('exceptionHandler', [ () => new ExceptionHandlerProvider(new ConfigureExceptionHandlerProvider())
]).config(configure);

configure.$inject = ['$provide'];
function configure($provide: any): void {
        'use strict';
        $provide.decorator('$exceptionHandler', extendExceptionHandler);
    }
extendExceptionHandler.$inject = ['$delegate', 'exceptionHandler', 'logger'];
/* @ngInject */
    function extendExceptionHandler($delegate: any, exceptionHandler: any, logger: ILogger): any {
        'use strict';
        return function(exception: any, cause: any): void {
            var appErrorPrefix: string = exceptionHandler.getConfig().appErrorPrefix || '';
            var errorData: any = {exception: exception, cause: cause};
            exception.message = appErrorPrefix + exception.message;
            $delegate(exception, cause);
            logger.error('', exception.message, errorData);
        };
    }
