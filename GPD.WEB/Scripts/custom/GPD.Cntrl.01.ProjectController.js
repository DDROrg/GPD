//=================================================
angular.module('Project').controller("PartnerCtrl", function ($scope, $http, $location, $log, CommonServices) {
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
            //$ctrl.data.LogedinUserProfile = payload;
            CommonServices.LogedinUserProfileLoaded();
        });
    };

    angular.element(document).ready(function () {
        if (__UserEmail != "") {
            GetLogedinUserProfile();
        }
    });
});

//=================================================
angular.module('Project').controller('ProjectController', function ($scope, $rootScope, $http, $location, $uibModal, $log, CommonServices, ProjectServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
    $ctrl.data.projects = [];
    $ctrl.data.sort = [{ column: 'name', descending: false }];
    $ctrl.data.page = {};
    $ctrl.data.page.currentPage = 1;
    $ctrl.data.page.maxPage = 5;
    $ctrl.data.page.itemPerPage = __ItemPerPage;
    $ctrl.data.globalSearchParam = "";
    $ctrl.data.search = {};
    $ctrl.data.search = { name: "", number: "", "organization-name": "", author: "", client: "", status: "" };

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
    $ctrl.CheckEmpty = function (d) { return d && d != "" ? true : false; };
    $ctrl.ColExpClass = function (d) { return d.isExpanded == true ? "fa fa-caret-down" : "fa fa-caret-right"; };
    $ctrl.IsShowDetail = function (d) { return d.isExpanded == true && d.hasDetail == true; };
    $ctrl.OnColExpDetail = function (d) { d.isExpanded = !(d.isExpanded); if (d.hasDetail == false) { GetProjectDetail(d); } };
    $ctrl.OnOpenItem = function (d) {
        var parentElem = angular.element('div[data-id="Project"]');
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
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
            $log.info('Button Clicked: ' + btnClicked);
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
    $ctrl.OnGlobalSearch = function () { GetProjects(); };
    $ctrl.pageChanged = function () { GetProjects(); };

    var GetProjectDetail = function (d) {
        return ProjectServices.GetProjectDetail($ctrl.data.LogedinUserProfile.selectedPartner, d.id)
        .then(function (payload) {
            d.identifiers = payload.identifiers;
            d.items = payload.items;
            d.location = payload.location;
            d.session = payload.session;

            angular.forEach(d.items, function (v, k) {
                v.isMaterialExpanded = false;
            });
            d.hasDetail = true;
        });
    };
    var GetProjects = function () {
        return ProjectServices.GetProjects($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.globalSearchParam, $ctrl.data.page.currentPage, $ctrl.data.page.itemPerPage)
        .then(function (payload) {
            $ctrl.data.projectListResponse = payload;
        });
    };

    $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
        GetProjects();
    });

    $rootScope.$on('EVENT-ChangePartner', function (event, data) {
        GetProjects();
    });

    angular.element(document).ready(function () {
    });
});

//=================================================
angular.module('Project').controller('ModalInstanceCtrl', function ($uibModalInstance, $log, project) {
    var $ctrl = this;
    $ctrl.data = {};
    $ctrl.data.project = project;

    $ctrl.data.sort = [{ column: 'product.name', descending: false }];
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
            retVal.push((v.descending ? "-" : "") + v.column);
        });
        return retVal;
    };
    $ctrl.OnColExpMaterial = function (d) { d.isMaterialExpanded = !(d.isMaterialExpanded); };
    $ctrl.IsShowMaterial = function (d) { return d.isMaterialExpanded == true };
    $ctrl.pageChanged = function () {
        $log.log("Index = " + $ctrl.data.page.currentPage);
    };
    $ctrl.Ok = function () {
        //$uibModalInstance.close($ctrl.selected.item);
        $uibModalInstance.close("OK");
    };
    $ctrl.Cancel = function () {
        $uibModalInstance.dismiss('CANCEL');
    };
});

//=================================================
angular.module('ManageUser').controller('ManageUserController', function ($scope, $rootScope, $http, $location, $uibModal, $log, CommonServices, GpdManageServices) {
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
    $ctrl.OnGlobalSearch = function () { GetUsers(); };
    $ctrl.OnEditItem = function (d) {
        $log.log("TODO:EDIT");
        $log.log(d);
    };

    var GetUsers = function () {
        return GpdManageServices.GetUsers($ctrl.data.globalSearchParam)
        .then(function (payload) {
            $ctrl.data.users = payload;
            $log.log(payload);
        });
    };

    angular.element(document).ready(function () {
    });
});

//=================================================
angular.module('ManagePartner').controller('ManagePartnerController', function ($scope, $rootScope, $http, $location, $uibModal, $log, CommonServices, GpdManageServices) {
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
            name: "sweets",
            url: "http://sweets.cnstruction.com",
            shortDescription: "N/A",
            description: "N/A",
            isActive: true
        };
    };

    $ctrl.OnCancelEditItem = function () {
        $ctrl.data.onEditing = false;
        $ctrl.data.tempPartner = {};
    };

    $ctrl.OnSaveEditItem = function () {
        if (IsValidPartner($ctrl.data.tempPartner)) {
            GpdManageServices.SavePartner($ctrl.data.tempPartner)
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
            GpdManageServices.SavePartner($ctrl.data.tempPartner)
            .then(function (payload) {
                if (payload == "SUCCESS") {
                    GetPartners();
                    $ctrl.data.onAdding = false;
                    $ctrl.data.tempPartner = {};
                }
            });
        }
    };

    $ctrl.OnActDeactItem = function (d) {
        GpdManageServices.ActDactPartner(d.partnerId, d.isActive)
        .then(function (payload) {
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
});