//AccountSelectController =================================
angular.module('AccountSelect').controller('AccountSelectController', function ($scope, $location) {

    $scope.OnAccountChange = function () {
        $scope.$emit("AccountSelect_OnAccountChange", "");
    };

    $scope.OnCompanyChange = function () {
        $scope.$emit("AccountSelect_OnCompanyChange", "");
    };

    angular.element(document).ready(function () { });
});