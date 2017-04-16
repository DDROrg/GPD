//=================================================
angular.module('Project').controller('ProjectController', function ($scope, $http, $location, $uibModal, CommonServices, ProjectServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.projects = [];
    $scope.data.sort = [{ column: 'name', descending: false }];
    $scope.data.page = {};
    $scope.data.page.currentPage = 1;
    $scope.data.page.maxPage = 5;
    $scope.data.page.itemPerPage = 10;
    $scope.data.search = {};
    $scope.data.search = { name: "", number: "", "organization-name": "", author: "", client: "", status: "" };

    $scope.OnChangeSorting = function (column) {
        var t = { column: column, descending: true };
        if ($scope.data.sort.length > 0) {
            if ($scope.data.sort[0].column == column) {
                t.descending = !$scope.data.sort[0].descending;
            } else {
                t.descending = true;
            }
        }
        $scope.data.sort = [t];
    };

    $scope.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($scope.data.sort && $scope.data.sort.column && column == $scope.data.sort.column) {
            if ($scope.data.sort.descending) { retVal = "fa fa-sort-down"; }
            else { retVal = "fa fa-sort-up"; }
        }
        return retVal;
    };

    $scope.ColExpClass = function (d) { return d.isExpanded == true ? "fa fa-caret-down" : "fa fa-caret-right"; };

    $scope.IsShowDetail = function (d) { return d.isExpanded == true && d.hasDetail == true; };

    $scope.OnOpenItem = function (d) {
        var parentElem = angular.element('div[data-id="Project"]');
        var modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            controller: 'ProjectController',
            size: 'lg',
            appendTo: parentElem,
            resolve: {
                //items: function () {
                //    return $ctrl.items;
                //}
            }
        });
    };

    $scope.OnModalOk = function () {
        alert("TODO: OnModalOk");
    };

    $scope.OnModalCancel = function (d) {
        alert("TODO: OnModalCancel");
    };

    $scope.OnColExpDetail = function (d) {
        d.isExpanded = !(d.isExpanded);
        if (d.hasDetail == false) { GetProjectDetail(d); }
    };

    $scope.ColumnSortOrder = function () {
        var retVal = [];
        angular.forEach($scope.data.sort, function (v, k) {
            retVal.push((v.descending ? "-" : "") + v.column);
        });
        return retVal;
    };

    var GetProjectDetail = function (d) {
        return ProjectServices.GetProjectDetail(d.id)
        .then(function (payload) {
            d.identifiers = payload.identifiers;
            d.items = payload.items;
            d.location = payload.location;
            d.session = payload.session;
            d.hasDetail = true;
        });
    };

    var GetProjects = function () {
        return ProjectServices.GetProjects()
        .then(function (payload) {
            $scope.data.projects = payload;
        });
    };

    angular.element(document).ready(function () {
        GetProjects();
    });
});