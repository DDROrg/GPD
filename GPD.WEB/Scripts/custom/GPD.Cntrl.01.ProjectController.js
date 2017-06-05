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
    $ctrl.data.page.itemPerPage = 2;
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
    $ctrl.data.search = {};
    //$ctrl.data.search = { name: "", number: "", "organization-name": "", author: "", client: "", status: "" };
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