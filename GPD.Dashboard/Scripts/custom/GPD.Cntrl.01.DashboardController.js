//=================================================
angular.module('GPD').controller("FilterCtrl", ['$scope', '$http', '$location', '$log', 'toastr', 'CommonServices', function ($scope, $http, $location, $log, toastr, CommonServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

    //$ctrl.data.opts = {
    //    dateFormat: 'dd/mm/yy',
    //    changeMonth: true,
    //    changeYear: true
    //};
    //$ctrl.data.valor= "10/09/2013";

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
        if (__UserEmail != "") { GetLogedinUserProfile(); }
    });
}]);

//=================================================
angular.module('GPD').controller('GPDDashboardController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        var _projectChartObj, _categoriesChartObj;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.fromDate = "";
        $ctrl.data.toDate = "";
        $ctrl.data.UniqueUserCount = 0;
        $ctrl.data.ProjectCount = 0;

        var RenderProjectChartData = function (d) {
            var tempData = CommonServices.TransformChartData(d);
            
            _projectChartObj = c3.generate({
                bindto: "#c3ProjectChart",
                grid: {
                    x: { show: true },
                    y: { show: true }
                },
                color: { pattern: tempData.tempPattern },
                data: {
                    xs: tempData.tempXs,
                    columns: tempData.tempColumns,
                    type: 'spline'
                },
                point: { r: 2 },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: { format: "%d-%b" } // %b - month name, %d - date, %m - month, %y - year
                    }
                },
                legend: { show: true }
            });
        };

        var RenderCategoriesChartData = function (d) {
            var tempData = CommonServices.TransformChartData(d);

            _categoriesChartObj = c3.generate({
                bindto: "#c3CategoriesChart",
                interaction: {
                    enabled: false
                },
                grid: {
                    x: { show: true },
                    y: { show: true }
                },
                color: { pattern: tempData.tempPattern },
                data: {
                    xs: tempData.tempXs,
                    columns: tempData.tempColumns,
                    type: 'bar'
                },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: { format: "%y" } // %b - month name, %d - date, %m - month, %y - year
                    }
                },
                bar: {
                    width: {
                        ratio: 0.5 
                    }
                },
                legend: { show: true }
            });
        };

        var GetProjectChartData = function () {
            if (_projectChartObj) { _projectChartObj = _projectChartObj.destroy(); }
            return CommonServices.GetProjectChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                RenderProjectChartData(payload);
            });
        };

        var GetCategoriesChartData = function () {
            if (_categoriesChartObj) { _categoriesChartObj = _categoriesChartObj.destroy(); }
            return CommonServices.GetCategoriesChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                RenderCategoriesChartData(payload);
            });
        };

        var GetUniqueUserCount = function () {
            return CommonServices.GetUniqueUserCount($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                $ctrl.data.UniqueUserCount = payload;
            });
        };

        var GetProjectCount = function () {
            return CommonServices.GetProjectCount($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                $ctrl.data.ProjectCount = payload;
            });
        };


        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
            GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount();
        });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) {
            GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount();
        });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            if (fromState.name != '') { GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount(); }
        });
        angular.element(document).ready(function () { });
    }]);

//=================================================
angular.module('GPD').controller('GPDTopCategoriesController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.test = "GPD-TopCategories-Controller";

        angular.element(document).ready(function () {
        });
    }]);
