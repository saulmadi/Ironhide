/// <reference path="../../../../typings/angularjs/angular-resource.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../users.module.ts" />
/// <reference path="./users.activate-deactivate.service.ts"/>
/// <reference path="../users/users.service.ts"/>

interface IUsersActivateDeactivateScope extends angular.IScope {
    vm: IUsersActivateDeactivateContoller;
}

interface IUsersActivateDeactivateContoller {
    users: IUserResponse[];
    orderedByName: boolean;
    orderedByEmail: boolean;
    pageNumber: number;
    pageSize: number;
    back(): void;
    next(): void;
    getUsersSortByName(): void;
    getUsersSortByEmail(): void;
    enableUser(id: string, enableUser: boolean): void;
}

class UsersActivateDeactivateController implements IUsersActivateDeactivateContoller {
    users: IUserResponse[] = [];
    orderedByName: boolean;
    orderedByEmail: boolean;
    pageNumber: number;
    pageSize: number;
    static $inject: any = ['$scope', 'usersService', 'usersActivateDeactivateService'];
    constructor(private $scope: IUsersActivateDeactivateScope, private usersService: IUsersService,
        private usersActivateDeactivateService: IActivateDeactivateUsersService) {
        $scope.vm = this;
        this.orderedByName = true;
        this.orderedByEmail = false;
        this.pageSize = 20;
        this.pageNumber = 1;
        this.getUsersSortByName();
    }
    back(): void {
        this.pageNumber -= 1;
        this.getUsersPage();
    }
    next(): void {
        this.pageNumber += 1;
        this.getUsersPage();
    }

    getUsersSortByName(): void {
        this.orderedByName = true;
        this.orderedByEmail = false;
        var request: IUserPagedRequest = {
            pageSize: this.pageSize, pageNumber: this.pageNumber, field: 'Name'
        };
        this.getUsers(request);

    }
    getUsersSortByEmail(): void {
        this.orderedByEmail = true;
        this.orderedByName = false;
        var request: IUserPagedRequest = {
            pageSize: this.pageSize, pageNumber: this.pageNumber, field: 'Email'
        };

        this.getUsers(request);
    }
    enableUser(id: string, enableUser: boolean): void {
        if (enableUser) {
            this.usersActivateDeactivateService.enableUser(id).then(() => {
                this.getUsersPage();
            });
        } else {
            this.usersActivateDeactivateService.disableUser(id).then(() => {
                this.getUsersPage();
            });
        }

    }
    private getUsersPage(): void {
        if (this.orderedByName) {
            this.getUsersSortByName();
        } else {
            this.getUsersSortByEmail();
        }
    }
    private getUsers(paginationRequest: IUserPagedRequest): void {

        this.usersService.getPagedUsers(paginationRequest)
            .then((data: IUserResponse[]): void => {
                this.users = data;
            });
    }

    static controllerId(): string {
        return 'users.activate-deactivate.controller';
    }
}
appUsers.controller(UsersActivateDeactivateController.controllerId(), UsersActivateDeactivateController);
