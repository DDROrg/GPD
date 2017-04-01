//=================================================
angular.module('Project').controller('ProjectController', function ($scope, $http, $location, CommonServices, ProjectServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.projects = [];
    $scope.data.TestData = "Debabrata";

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