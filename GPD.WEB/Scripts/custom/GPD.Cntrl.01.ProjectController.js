//=================================================



//=================================================
angular.module('Project').controller("PartnerCtrl", ['$scope', '$http', '$location', '$log', 'toastr', 'CommonServices', function ($scope, $http, $location, $log, toastr, CommonServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

    $ctrl.SelectPartner = function (d) {
        $ctrl.data.LogedinUserProfile.selectedPartner = d;
        CommonServices.ChangePartner(d);
    };

    var GetLogedinUserProfile = function () {
        CommonServices.GetLogedinUserProfile()
        .then(function (payload) {
            CommonServices.LogedinUserProfileLoaded();
        });
    };

    angular.element(document).ready(function () {
        if (__UserId != "") {
            GetLogedinUserProfile();
        }
    });
}]);

//=================================================
angular.module('Project').controller('ProjectController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', '$ngConfirm', 'toastr', 'CommonServices', 'ProjectServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, $ngConfirm, toastr, CommonServices, ProjectServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.projectListResponse = {};
        $ctrl.data.to = {};
        $ctrl.data.from = {};
        $ctrl.data.page = {};
        $ctrl.data.page.maxPage = 5;
        $ctrl.data.page.itemPerPage = __ItemPerPage;
        $ctrl.data.search = {};
        $ctrl.data.selectedProjects = [];
        //$scope.data.dateFormats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];

        var ResetPagination = function () { $ctrl.data.page.currentPage = 1; };
        var ResetDateRange = function (d) {
            if (d == "TO") {
                if ($ctrl.data.from.date > $ctrl.data.to.date) {
                    $ctrl.data.from.date = new Date($ctrl.data.to.date);
                }
            }
            else if (d == "FROM") {
                if ($ctrl.data.from.date > $ctrl.data.to.date) {
                    $ctrl.data.to.date = new Date($ctrl.data.from.date);
                }
            }
        };
        var ResetFilter = function () {
            ResetPagination();
            $ctrl.data.sort = [{ column: 'create-timestamp-formatted', descending: true }];
            $ctrl.data.globalSearchParam = "";
            $ctrl.data.tempGlobalSearchParam = "";
            $ctrl.data.projectIdentifier = "";
            $ctrl.data.search = { "user-email": "", name: "", client: "", status: "" };
            $ctrl.data.isAllSelected = false;

            $ctrl.data.to.date = new Date();
            $ctrl.data.to.maxDate = new Date();
            $ctrl.data.from.date = new Date();
            $ctrl.data.from.date.setMonth(new Date().getMonth() - 1);
            $ctrl.data.from.maxDate = new Date();
            $ctrl.data.to.popupOpened = false;
            $ctrl.data.from.popupOpened = false;
            $ctrl.data.to.popupOpened = false;
            $ctrl.data.from.popupOpened = false;
        };
        ResetFilter();

        $ctrl.toDatePopupOpen = function () {
            $ctrl.data.to.popupOpened = true;
        };
        $ctrl.fromDatePopupOpen = function () {
            $ctrl.data.from.popupOpened = true;
        };
        $ctrl.toDateSelected = function () {
            ResetPagination();
            ResetDateRange("TO");
            GetProjects();
        };
        $ctrl.fromDateSelected = function () {
            ResetPagination();
            ResetDateRange("FROM");
            GetProjects();
        };
        $ctrl.OnChangeSorting = function (column) {
            var t = { column: column, descending: true };
            if ($ctrl.data.sort.length > 0) {
                if ($ctrl.data.sort[0].column == column) {
                    t.descending = !$ctrl.data.sort[0].descending;
                } else {
                    t.descending = true;
                }
            }
            $ctrl.data.sort = [t];
        };
        $ctrl.ColumnSortClass = function (column) {
            var retVal = "fa fa-sort";
            if ($ctrl.data.sort && $ctrl.data.sort[0].column && column == $ctrl.data.sort[0].column) {
                if ($ctrl.data.sort[0].descending) { retVal = "fa fa-caret-down"; }
                else { retVal = "fa fa-caret-up"; }
            }
            return retVal;
        };
        $ctrl.ColumnSortOrder = function () {
            var retVal = [];
            angular.forEach($ctrl.data.sort, function (v, k) {
                retVal.push((v.descending ? "-" : "") + "'" + v.column + "'");
            });
            return retVal;
        };
        $ctrl.CheckEmpty = function (d) { return d && d != "" ? true : false; };
        $ctrl.ColExpClass = function (d) { return d.isExpanded == true ? "fa fa-caret-down" : "fa fa-caret-right"; };
        $ctrl.DeleteItemsColor = function (d) { return d['delete-status'] == "False" ? "color:#f00;" : "color:#CCC;"; };
        $ctrl.DeleteItemsTitle = function (d) { return d['delete-status'] == "False" ? "Delete" : "UnDelete"; };
        $ctrl.IsShowDetail = function (d) { return d.isExpanded == true && d.hasDetail == true; };
        $ctrl.GlobalSearchButtonStyle = function () {
            return $ctrl.data.tempGlobalSearchParam.length > 2 ? "input-group-addon btn btn-primary" : "input-group-addon btn btn-primary disabled";
        };
        $ctrl.OnPageChanged = function () { GetProjects(); };
        $ctrl.OnColExpDetail = function (d) { d.isExpanded = !(d.isExpanded); if (d.hasDetail == false) { GetProjectDetail(d); } };
        $ctrl.OnOpenItem = function (d) {
            var parentElem = angular.element('div[data-id="Project"]');
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'ProjectDetailContent.html',
                controller: 'ProjectDetailCtrl',
                controllerAs: '$ctrl',
                size: 'lg',
                appendTo: parentElem,
                resolve: {
                    project: function () {
                        GetProjectDetail(d);
                        return d;
                    }
                }
            });

            modalInstance.result.then(function (btnClicked) {
                //$log.info('Button Clicked: ' + btnClicked);
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        };
        $ctrl.OnEditItem = function (d) {
            $state.go('project.edit', { id: d.id, project: null });
        };
        $ctrl.OnGlobalSearch = function () {
            $ctrl.data.globalSearchParam = $ctrl.data.tempGlobalSearchParam;
            ResetPagination();
            GetProjects();
        };
        $ctrl.OnProjectByIdentifier = function (d) {
            $ctrl.data.page.currentPage = 1;
            $ctrl.data.projectIdentifier = "" + d + "";
            GetProjects();
        };
        $ctrl.OnExport = function () { alert("TODO:Not Implemented"); };
        $ctrl.OnDeleteProjects = function () {
            $ctrl.data.selectedProjects = [];
            $.each($ctrl.data.projectListResponse.projects, function (k, v) {
                if (v.isSelected) { $ctrl.data.selectedProjects.push(v.id); }
            });
            DeleteProjects(true);
        };
        $ctrl.OnDeleteProject = function (d) {
            $ctrl.data.selectedProjects = [];
            $ctrl.data.selectedProjects.push(d.id);
            DeleteProjects(d['delete-status'] === 'False');
        };
        $ctrl.OnInactivateProject = function (d) {
            $ctrl.data.selectedProjects = [];
            $ctrl.data.selectedProjects.push(d.id);
            InactivateProject();
        };
        $ctrl.OnResetFilter = function () {
            var isRefreshRequired = $ctrl.data.projectIdentifier != "" || $ctrl.data.globalSearchParam != "" ? true : false;
            ResetFilter();
            if (isRefreshRequired) {
                GetProjects();
            }
            else {
                $.each($ctrl.data.projectListResponse.projects, function (k, v) {
                    v.isSelected = $ctrl.data.isAllSelected;
                });
            }
        };
        $ctrl.GetPartnerImgUrl = function (pName) {
            // default value
            var defaultImg = __RootUrl + "Content/PartnerImg/DEFAULT.png";
            if ($ctrl.data.LogedinUserProfile.partnerDefImages != null) {
                angular.forEach($ctrl.data.LogedinUserProfile.partnerDefImages, function (item) {
                    if (item.name.toLowerCase() == pName.toLowerCase()) { defaultImg = item.image; }
                });
            }
            return defaultImg;
        };
        $ctrl.OnCheckAllChange = function () {
            $.each($ctrl.data.projectListResponse.projects, function (k, v) {
                v.isSelected = $ctrl.data.isAllSelected;
            });
        };
        var GetProjectDetail = function (d) {
            return ProjectServices.GetProjectDetail(d.id)
            .then(function (payload) {
                d.identifiers = payload.identifiers;
                d.items = payload.items;
                d.location = payload.location;
                d.session = payload.session;

                angular.forEach(d.items, function (v, k) {
                    v.isMaterialExpanded = false;
                    v.hasProductUrl = (v.product.url || v.product['image-url']) ? true : false;
                });
                d.hasDetail = true;
            });
        };
        var GetProjects = function () {
            return ProjectServices.GetProjects($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.globalSearchParam, $ctrl.data.from.date, $ctrl.data.to.date, $ctrl.data.projectIdentifier, $ctrl.data.page.currentPage, $ctrl.data.page.itemPerPage)
            .then(function (payload) {
                $ctrl.data.projectListResponse = payload;
            });
        };
        var DeleteProjects = function (deleteFlag) {
            var confirmMsg = !deleteFlag ? 'Are you sure you want to undelete this Project?' : 'Are you sure you want to delete this Project?';
            if (deleteFlag && $ctrl.data.selectedProjects.length > 1) {
                confirmMsg = '<strong>{{$ctrl.data.selectedProjects.length}}</strong> Projects will be deleted';
            }
            if ($ctrl.data.selectedProjects.length > 0) {
                $ngConfirm({
                    title: 'Confirm!',
                    content: confirmMsg,
                    scope: $scope,
                    buttons: {
                        cancel: {
                            text: 'Cancel',
                            btnClass: 'btn-blue',
                            action: function (scope, button) {
                                $ctrl.data.selectedProjects = [];
                            }
                        },
                        ok: {
                            text: deleteFlag ? 'Delete' : 'UnDelete',
                            btnClass: 'btn-warning',
                            action: function (scope, button) {
                                ProjectServices.ProjectListDelete($ctrl.data.selectedProjects, deleteFlag)
                                    .then(function (payload) {
                                        if (payload.status) {
                                            ResetPagination();
                                            GetProjects();
                                        }
                                        else {
                                            toastr.error(payload.message);
                                        }
                                        $ctrl.data.selectedProjects = [];
                                    });
                            }
                        }
                    }
                });
            }
        };
        var InactivateProject = function () {
            if ($ctrl.data.selectedProjects.length > 0) {
                $ngConfirm({
                    title: 'Confirm!',
                    content: 'Are you sure you want to inactivate this Project?',
                    scope: $scope,
                    buttons: {
                        cancel: {
                            text: 'Cancel',
                            btnClass: 'btn-blue',
                            action: function (scope, button) {
                                $ctrl.data.selectedProjects = [];
                            }
                        },
                        ok: {
                            text: 'Inactivate',
                            btnClass: 'btn-warning',
                            action: function (scope, button) {
                                ProjectServices.ProjectListActDact($ctrl.data.selectedProjects, false)
                                    .then(function (payload) {
                                        if (payload.status) {
                                            ResetPagination();
                                            GetProjects();
                                        }
                                        else {
                                            toastr.error(payload.message);
                                        }
                                        $ctrl.data.selectedProjects = [];
                                    });
                            }
                        }
                    }
                });
            }
        };

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
            ResetFilter();
            GetProjects();
        });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) {
            ResetFilter();
            GetProjects();
        });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            if (fromState.name == "project.edit") { GetProjects(); };
        });

        angular.element(document).ready(function () {
        });
    }]);

//=================================================
angular.module('Project').controller('ProjectDetailCtrl', ['$uibModalInstance', '$log', 'toastr', 'project', function ($uibModalInstance, $log, toastr, project) {
    var $ctrl = this;
    $ctrl.data = {};
    $ctrl.data.project = project;

    $ctrl.data.sort = [{ column: 'hasProductUrl', descending: true },
        { column: 'product.manufacturer', descending: false },
        { column: 'product.model', descending: false }];

    $ctrl.data.page = {};
    $ctrl.data.page.currentPage = 1;
    $ctrl.data.page.maxPage = 5;
    $ctrl.data.page.itemPerPage = 10;
    $ctrl.data.search = "";

    $ctrl.OnChangeSorting = function (column) {
        var t = { column: column, descending: true };
        if ($ctrl.data.sort.length > 0) {
            if ($ctrl.data.sort[0].column == column) {
                t.descending = !$ctrl.data.sort[0].descending;
            } else {
                t.descending = true;
            }
        }
        $ctrl.data.sort = [t];
    };
    $ctrl.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($ctrl.data.sort && $ctrl.data.sort[0].column && column == $ctrl.data.sort[0].column) {
            if ($ctrl.data.sort[0].descending) { retVal = "fa fa-caret-down"; }
            else { retVal = "fa fa-caret-up"; }
        }
        return retVal;
    };
    $ctrl.ColumnSortOrder = function () {
        var retVal = [];
        angular.forEach($ctrl.data.sort, function (v, k) {
            retVal.push((v.descending ? "-" : "+") + v.column);
        });
        return retVal;
    };
    $ctrl.OnColExpMaterial = function (d) { d.isMaterialExpanded = !(d.isMaterialExpanded); };
    $ctrl.IsShowMaterial = function (d) { return d.isMaterialExpanded == true };
    $ctrl.OnPageChanged = function () { };
    $ctrl.Ok = function () {
        //$uibModalInstance.close($ctrl.selected.item);
        $uibModalInstance.close("OK");
    };
    $ctrl.Cancel = function () {
        $uibModalInstance.dismiss('CANCEL');
    };
}]);

//=================================================
angular.module('Project').controller('ProjectEditController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices', 'ProjectServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices, ProjectServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        //$ctrl.data.project = $stateParams.project
        $ctrl.data.id = $stateParams.id

        var GetProjectDetail = function (d) {
            return ProjectServices.GetProjectDetail($ctrl.data.id)
            .then(function (payload) {
                $ctrl.data.project = payload;
            });
        };


        $ctrl.OnBackToProjectList = function () { $state.go('project.list'); };
        $ctrl.OnSaveProject = function () {
            return ProjectServices.UpdateProject($ctrl.data.id, $ctrl.data.project)
           .then(function (payload) {
               if (payload.status) {
                   $state.go('project.list');
               } else {
                   toastr.error(payload.message);
               }
           });
        };

        //$rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
        //    GetProjectDetail();
        //});
        //$rootScope.$on('EVENT-ChangePartner', function (event, data) {
        //    GetProjectDetail();
        //});
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            GetProjectDetail();
        });
    }]);

//=================================================
angular.module('ManageUser').controller('ManageUserController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', 'toastr', 'CommonServices', 'GpdManageServices', function ($scope, $rootScope, $http, $location, $uibModal, $log, toastr, CommonServices, GpdManageServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.users = [];
    $ctrl.data.globalSearchParam = "";
    $ctrl.data.sort = [{ column: 'firstName', descending: false }];
    $ctrl.data.page = {};
    $ctrl.data.page.currentPage = 1;
    $ctrl.data.page.maxPage = 5;
    $ctrl.data.page.itemPerPage = 10;
    $ctrl.data.search = {};
    $ctrl.data.search = { firstName: "", lastName: "", email: "", company: "" };

    $ctrl.OnChangeSorting = function (column) {
        var t = { column: column, descending: true };
        if ($ctrl.data.sort.length > 0) {
            if ($ctrl.data.sort[0].column == column) {
                t.descending = !$ctrl.data.sort[0].descending;
            } else {
                t.descending = true;
            }
        }
        $ctrl.data.sort = [t];
    };
    $ctrl.OnResetManageUser = function () {
        $ctrl.data.page.currentPage = 1;
        $ctrl.data.globalSearchParam = "";
        $ctrl.data.search.firstName = "";
        $ctrl.data.search.lastName = "";
        $ctrl.data.search.email = "";
        $ctrl.data.search.company = "";
        GetUsers();
    };
    $ctrl.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($ctrl.data.sort && $ctrl.data.sort[0].column && column == $ctrl.data.sort[0].column) {
            if ($ctrl.data.sort[0].descending) { retVal = "fa fa-caret-down"; }
            else { retVal = "fa fa-caret-up"; }
        }
        return retVal;
    };
    $ctrl.ColumnSortOrder = function () {
        var retVal = [];
        angular.forEach($ctrl.data.sort, function (v, k) {
            retVal.push((v.descending ? "-" : "") + v.column);
        });
        return retVal;
    };
    $ctrl.OnPageChanged = function () { };
    $ctrl.OnGlobalSearch = function () { GetUsers(); };
    $ctrl.OnColExpRole = function (d) {
        d.isRoleExpanded = !(d.isRoleExpanded);
        if (d.hasRole == false) { GetUserRoles(d); }
    };
    $ctrl.IsShowRole = function (d) { return d.isRoleExpanded == true && d.hasRole == true; };

    $ctrl.OnActDeactItem = function (d) {
        GpdManageServices.ActDactUser(d.userId, d.isActive)
        .then(function (payload) {
            if (payload == "SUCCESS") {
                toastr.success(d.isActive ? "Activated Successfuly" : "Deactivated Successfuly");
            }
        });
    };

    $ctrl.OnDeleteRole = function (user, userRole) {
        GpdManageServices.DeleteUserRole(userRole.userId, userRole.partnerId, userRole.groupId)
        .then(function (payload) {
            if (payload == "SUCCESS") {
                toastr.success("User-Role deleted");
                GetUserRoles(user);
            } else {
                toastr.error("Unable to delete User-Role");
            }
        });
    };

    $ctrl.OnAddRole = function (d) {
        var parentElem = angular.element('div[data-id="divManageUser"]');
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'addUserRole.html',
            controller: 'AddUserRoleCtrl',
            controllerAs: '$ctrl',
            size: 'md',
            appendTo: parentElem,
            resolve: {
                data: function () {
                    return { user: d, partners: $ctrl.data.partners, groups: $ctrl.data.groups };
                }
            }
        });

        modalInstance.result.then(function (d) {
            GetUserRoles(d);
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };

    var GetUserRoles = function (d) {
        GpdManageServices.GetUserRoles(d.userId)
        .then(function (payload) {
            d.hasRole = true;
            d.roles = payload;
        });
    };

    var GetUsers = function () {
        return GpdManageServices.GetUsers($ctrl.data.globalSearchParam)
        .then(function (payload) {
            $ctrl.data.users = payload;
        });
    };
    var GetPartners = function () {
        return GpdManageServices.GetPartners()
        .then(function (payload) {
            $ctrl.data.partners = payload;
        });
    };
    var GetGroups = function () {
        return GpdManageServices.GetGroups()
        .then(function (payload) {
            $ctrl.data.groups = payload;
        });
    };

    angular.element(document).ready(function () {
        GetUsers();
        GetPartners();
        GetGroups();
    });
}]);

//=================================================
angular.module('ManageUser').controller('AddUserRoleCtrl', ['$uibModalInstance', '$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', 'toastr', 'CommonServices', 'GpdManageServices', 'data',
    function ($uibModalInstance, $scope, $rootScope, $http, $location, $uibModal, $log, toastr, CommonServices, GpdManageServices, data) {
        var $ctrl = this;
        $ctrl.data = data;
        //$ctrl.data.selectedPartner = $ctrl.data.partners[0].partnerId;
        //$ctrl.data.selectedGroup = $ctrl.data.groups[0].groupId;
        $log.log($ctrl.data.groups);
        $ctrl.Ok = function () {
            var isValid = true;
            angular.forEach($ctrl.data.user.roles, function (v, k) {
                if (isValid == true && v.partnerId == $ctrl.data.selectedPartner && v.groupId == $ctrl.data.selectedGroup) { isValid = false; }
            });

            if (isValid) {
                GpdManageServices.AddUserRole($ctrl.data.user.userId, $ctrl.data.selectedPartner, $ctrl.data.selectedGroup)
                .then(function (payload) {
                    if (payload == "SUCCESS") {
                        toastr.success("User-Role added");
                        $uibModalInstance.close($ctrl.data.user);
                    } else {
                        toastr.error("Unable to add User-Role");
                    }
                });
            } else { toastr.error("This User-Role already exist."); }
        };
        $ctrl.Cancel = function () {
            $uibModalInstance.dismiss('CANCEL');
        };
    }]);

//=================================================
angular.module('ManagePartner').controller('ManagePartnerController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', 'toastr', 'CommonServices', 'GpdManageServices', function ($scope, $rootScope, $http, $location, $uibModal, $log, toastr, CommonServices, GpdManageServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.partners = [];
    $ctrl.data.sort = [{ column: 'name', descending: false }];
    $ctrl.data.page = {};
    $ctrl.data.page.currentPage = 1;
    $ctrl.data.page.maxPage = 5;
    $ctrl.data.page.itemPerPage = 10;
    $ctrl.data.search = {};
    $ctrl.data.search = { name: "", shortDescription: "", description: "" };

    $ctrl.data.onEditing = false;
    $ctrl.data.onAdding = false;
    $ctrl.data.tempPartner = {};

    $ctrl.OnChangeSorting = function (column) {
        var t = { column: column, descending: true };
        if ($ctrl.data.sort.length > 0) {
            if ($ctrl.data.sort[0].column == column) {
                t.descending = !$ctrl.data.sort[0].descending;
            } else {
                t.descending = true;
            }
        }
        $ctrl.data.sort = [t];
    };
    $ctrl.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($ctrl.data.sort && $ctrl.data.sort[0].column && column == $ctrl.data.sort[0].column) {
            if ($ctrl.data.sort[0].descending) { retVal = "fa fa-caret-down"; }
            else { retVal = "fa fa-caret-up"; }
        }
        return retVal;
    };
    $ctrl.ColumnSortOrder = function () {
        var retVal = [];
        angular.forEach($ctrl.data.sort, function (v, k) {
            retVal.push((v.descending ? "-" : "") + v.column);
        });
        return retVal;
    };
    $ctrl.OnPageChanged = function () { };

    $ctrl.OnEditItem = function (d) {
        $ctrl.data.onEditing = true;
        $ctrl.data.onAdding = false;
        $ctrl.data.tempPartner.partnerId = d.partnerId;
        $ctrl.data.tempPartner.name = d.name;
        $ctrl.data.tempPartner.url = d.url;
        $ctrl.data.tempPartner.shortDescription = d.shortDescription;
        $ctrl.data.tempPartner.description = d.description;
        $ctrl.data.tempPartner.isActive = d.isActive;
    };

    $ctrl.OnAddItem = function () {
        $ctrl.data.onEditing = false;
        $ctrl.data.onAdding = true;
        $ctrl.data.tempPartner = {
            partnerId: "",
            name: "",
            url: "",
            shortDescription: "",
            description: "",
            isActive: true
        };
    };

    $ctrl.OnCancelEditItem = function () {
        $ctrl.data.onEditing = false;
        $ctrl.data.tempPartner = {};
    };

    $ctrl.OnSaveEditItem = function () {
        if (IsValidPartner($ctrl.data.tempPartner)) {
            GpdManageServices.AddPartner($ctrl.data.tempPartner)
            .then(function (payload) {
                if (payload == "SUCCESS") {
                    GetPartners();
                    $ctrl.data.onEditing = false;
                    $ctrl.data.tempPartner = {};
                }
            });
        }
    };

    $ctrl.OnCancelAddItem = function () {
        $ctrl.data.onAdding = false;
        $ctrl.data.tempPartner = {};
    };

    $ctrl.OnSaveAddItem = function () {
        if (IsValidPartner($ctrl.data.tempPartner)) {
            GpdManageServices.AddPartner($ctrl.data.tempPartner)
            .then(function (payload) {
                if (payload == "SUCCESS") {
                    GetPartners();
                    $ctrl.data.onAdding = false;
                    $ctrl.data.tempPartner = {};
                }
            });
        } else {
            toastr.error("Mandatory information missing");
        }
    };

    $ctrl.OnActDeactItem = function (d) {
        GpdManageServices.ActDactPartner(d.partnerId, d.isActive)
        .then(function (payload) {
            if (payload == "SUCCESS") {
                toastr.success(d.isActive ? "Activated Successfuly" : "Deactivated Successfuly");
            }
        });
    };


    var IsValidPartner = function (d) {
        if ($ctrl.data.tempPartner
            && $ctrl.data.tempPartner.name
            && $ctrl.data.tempPartner.name != ''
            && $ctrl.data.tempPartner.url
            && $ctrl.data.tempPartner.url != ''
            && $ctrl.data.tempPartner.description
            && $ctrl.data.tempPartner.description != '') {
            return true;
        }
        else {
            return false;
        }
    };

    var GetPartners = function () {
        return GpdManageServices.GetPartners()
        .then(function (payload) {
            $ctrl.data.partners = payload;
        });
    };

    angular.element(document).ready(function () {
        GetPartners();
    });
}]);

//=================================================
angular.module('RegisterUser').controller('RegisterUserCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', 'filterFilter', 'toastr', 'CommonServices', 'GpdManageServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, filterFilter, toastr, CommonServices, GpdManageServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.user = {};
        $ctrl.data.profileImage = {};
        $ctrl.data.countries = [];
        $ctrl.data.filteredState = [];
        $ctrl.data.ACCompanies = [];
        $ctrl.data.isACVisible = false;

        var ResetProfileImage = function () {
            $ctrl.data.profileImage = {
                isPresent: false,
                url: '',
                file: null
            };
            $("#fileProfileImage").get(0).value = null;
        };
        var ResetData = function () {
            $ctrl.data.user = {
                firstName: "",
                lastName: "",
                email: "",
                jobTitle: "",
                phone: "",
                company: {
                    name: "",
                    website: "",
                    country: "",
                    address: "",
                    address2: "",
                    city: "",
                    state: "",
                    postalCode: "",
                    defaultIndustry: ""
                },
                password: "",
                confirmPassword: "",
                acceptTC: false,
                enewslettersCommunication: false,
                emailCommunication: false,
                isProfileImgAvailable: false
            };
            ResetProfileImage();
        };
        var GetCountries = function () {
            GpdManageServices.GetCountries().then(function (payload) {
                //$log.log(payload);
                $ctrl.data.countries = payload;
                $ctrl.CountryChange();
            });
        };
        var ValidateUserDetail = function () {
            var isValid = true;
            var regexEmptyString = /^\s*$/;
            var reValidEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            var rePassword = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/;
            var errMessage = "";
            if (regexEmptyString.test($ctrl.data.user.firstName)) {
                errMessage = errMessage + "'First Name' is required.<br/>";
                isValid = false;
            }
            if (regexEmptyString.test($ctrl.data.user.lastName)) {
                errMessage = errMessage + "'Last Name' is required.<br/>";
                isValid = false;
            }
            if (!reValidEmail.test($ctrl.data.user.email)) {
                errMessage = errMessage + "'Email' is not valid.<br/>";
                isValid = false;
            }
            if (!rePassword.test($ctrl.data.user.password)) {
                errMessage = errMessage + "'Password' does not meet require criteria.<br/>";
                isValid = false;
            }
            if ($ctrl.data.user.password != $ctrl.data.user.confirmPassword) {
                errMessage = errMessage + "'Re-enter Password' does not match with 'Password'.<br/>";
                isValid = false;
            }
            if ($ctrl.data.profileImage.isPresent) {
                if ($ctrl.data.profileImage.file[0].size > 200000) {
                    errMessage = errMessage + "Profile image size should be less than 200 KB.<br/>";
                    isValid = false;
                }
            }
            if (!isValid) { toastr.error(errMessage, { allowHtml: true }); }
            return isValid;
        };

        $ctrl.isACVisible = function () {
            return $ctrl.data.isACVisible;
        };
        $ctrl.GetCompanies = function (term) {
            //$log.log(term);
            var regex = /^\w(\w|\s|.){2,}/;
            if (regex.test(term)) {
                GpdManageServices.GetCompanies(term).then(function (payload) {
                    //$log.log(payload);
                    $ctrl.data.ACCompanies = payload;
                    $ctrl.data.isACVisible = payload.length > 0 ? true : false;
                });
            } else {
                $ctrl.data.ACCompanies = [];
                $ctrl.data.isACVisible = false;
            }
        };
        $ctrl.SelectACCompany = function (d) {
            //$log.log(d);
            GpdManageServices.GetCompanyDetails(d.id).then(function (payload) {
                //$log.log(payload);
                $ctrl.data.user.company = payload;
                $ctrl.CountryChange();
                $ctrl.data.isACVisible = false;
            });
        };
        $ctrl.CountryChange = function () {
            $ctrl.data.filteredState = [];
            if ($ctrl.data.countries.length > 0 && $ctrl.data.user.company.country != '') {
                var filteredCountries = filterFilter($ctrl.data.countries, { Name: $ctrl.data.user.company.country }, true);
                if (filteredCountries.length > 0) {
                    var filteredCountry = filteredCountries[0];
                    if (filteredCountry.States) {
                        $ctrl.data.filteredState = filteredCountry.States;
                    }
                }
            }
        };
        $ctrl.HasFilteredStates = function () {
            return $ctrl.data.filteredState.length > 0 ? 1 : 0;
        };
        $ctrl.SelectProfileImg = function () {
            $("#fileProfileImage").click();
        };
        $ctrl.OnImageSelected = function (d) {
            $scope.$apply(function () {
                $ctrl.data.profileImage.isPresent = d.isPresent;
                $ctrl.data.profileImage.url = d.url;
                $ctrl.data.profileImage.file = d.file;
            });
        }
        $ctrl.AnalyzePasswordStrength = function (type) {
            var calss = "fa-li fa fa-minus-circle text-danger";
            if (type == "length") {
                if (/(?=.{8,})/.test($ctrl.data.user.password)) { calss = "fa-li fa fa-check text-success"; }
            } else if (type == "lowercase") {
                if (/(?=.*[a-z])/.test($ctrl.data.user.password)) { calss = "fa-li fa fa-check text-success"; }
            } else if (type == "uppercase") {
                if (/(?=.*[A-Z])/.test($ctrl.data.user.password)) { calss = "fa-li fa fa-check text-success"; }
            } else if (type == "number") {
                if (/(?=.*[0-9])/.test($ctrl.data.user.password)) { calss = "fa-li fa fa-check text-success"; }
            } else if (type == "special") {
                if (/(?=.*[!@#\$%\^&\*])/.test($ctrl.data.user.password)) { calss = "fa-li fa fa-check text-success"; }
            }
            return calss;
        };
        $ctrl.OnReset = function () { ResetData(); $ctrl.CountryChange(); };
        $ctrl.OnProfileImg = function () { ResetProfileImage(); };
        $ctrl.OnSave = function () {
            if (ValidateUserDetail()) {
                GpdManageServices.RegisterUser($ctrl.data.user)
                .then(function (payload) {
                    if (payload.status) {
                        if ($ctrl.data.profileImage.isPresent == true) {
                            var data = new FormData();
                            if ($ctrl.data.profileImage.file.length > 0) {
                                data.append("file", $ctrl.data.profileImage.file[0]);
                            }
                            GpdManageServices.UploadProfileImage(payload.userId, data)
                            .then(function (payload) {
                                if (payload == "SUCCESS") {
                                    window.location.href = __RootUrl + 'Account/Login';
                                    return;
                                } else {
                                    toastr.error("ERROR : In File upload");
                                }
                            });
                        } else {
                            window.location.href = __RootUrl + 'Account/Login';
                        }                        
                    } else {
                        toastr.error("ERROR : " + payload.message);
                    }
                });
            }
        };
        angular.element(document).ready(function () {
            ResetData();
            GetCountries();
        });
    }])
    .directive('keyboardPoster', ['$parse', '$timeout', function ($parse, $timeout) {
        return {
            scope: {
                postFn: '&'
            },
            link: function (scope, elem, attr) {
                var DELAY_TIME_BEFORE_POSTING = 500;
                var currentTimeout = null;
                elem.bind("input", function (event) {
                    if (currentTimeout) {
                        $timeout.cancel(currentTimeout);
                    }
                    currentTimeout = $timeout(function () {
                        var t = elem.val();
                        scope.postFn({ term: t });
                    }, DELAY_TIME_BEFORE_POSTING);
                });
            }
        };
    }])
    .directive('filelistBind', function () {
        return {
            scope: { imageSelected: '&' },
            link: function (scope, elm, attrs) {
                elm.bind('change', function (evt) {
                    scope.$apply(function () {
                        scope[attrs.name] = evt.target.files;
                        //scope.imgAvailable = true;
                        //scope.objData.user.isProfileImgAvailable = true;
                        //renderImage(evt.target.files[0]);
                        // generate a new FileReader object
                        var reader = new FileReader();
                        // inject an image with the src url
                        reader.onload = function (event) {
                            scope.imageSelected({ d: { isPresent: true, url: event.target.result, file: evt.target.files } });
                            //scope.$apply(function () {
                            //    scope.the_url = event.target.result                               
                            //});                            
                        };
                        // when the file is read it triggers the onload event above.
                        reader.readAsDataURL(evt.target.files[0]);
                    });
                });
            }
        };
    });