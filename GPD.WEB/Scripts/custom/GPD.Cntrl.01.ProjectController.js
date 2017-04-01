//=================================================
angular.module('Project').controller('ProjectController', function ($scope, $http, $location, CommonServices, ProjectServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.projects = [];
    $scope.data.sort = {
        column: 'name',
        descending: false
    };

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

    var GetAccount = function () {
        return ProjectServices.GetProjects()
        .then(function (payload) {
            $scope.data.projects = payload;            
        });
    };

    angular.element(document).ready(function () {
        GetAccount();
    });
});