//=================================================
angular.module('Project').controller('ProjectController', function ($scope, $http, $location, CommonServices, ProjectServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.projects = [];
    $scope.data.sort = [{ column: 'name', descending: false }];
    $scope.data.page = {};
    $scope.data.page.currentPage = 1;
    $scope.data.page.maxPage = 5;
    $scope.data.page.itemPerPage = 2;
    $scope.data.search = {};
    $scope.data.search.name = "";

    $scope.OnChangeSorting = function (column) {
        if ($scope.data.sort.column == column) {
            $scope.data.sort.descending = !$scope.data.sort.descending;
        } else {
            $scope.data.sort.column = column;
            $scope.data.sort.descending = true;
        }
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