//=================================================
angular.module('GPD').controller("PartnerCtrl", ['$scope', '$http', '$location', '$log', 'toastr', 'CommonServices', function ($scope, $http, $location, $log, toastr, CommonServices) {
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
        if (__UserEmail != "") {
            GetLogedinUserProfile();
        }
    });
}]);

//=================================================
angular.module('GPD').controller('GPDDashboardController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        var _chartObj;
        var _chartColor = ['#ff0000', '#ff6a00', '#ffd800', '#b6ff00', '#4cff00', '#5f798d', '#0094ff', '#0000ff'];
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.fromDate = "";
        $ctrl.data.toDate = "";

        var DestroyChartData = function () {
            if (_chartObj) { _chartObj = _chartObj.destroy(); }
        };

        var RenderChartData = function (d) {
            var tempPattern = [];
            var tempXs = {};
            var tempColumns = [];
            angular.forEach(d, function (v, k) {
                var i = "x" + (k + 1);
                //tempPattern.push(v.color);
                tempPattern.push(_chartColor[k]);
                tempXs[v.name] = i;
                v.dates.unshift(i);
                v.values.unshift(v.name)
                tempColumns.push(v.dates);
                tempColumns.push(v.values);
            });

            _chartObj = c3.generate({
                bindto: "#c3Chart",
                grid: {
                    x: { show: true },
                    y: { show: true }
                },
                color: { pattern: tempPattern },
                data: {
                    xs: tempXs,
                    columns: tempColumns,
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

        var GetChartData = function () {
            DestroyChartData();
            return CommonServices.GetProjectChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                RenderChartData(payload);
            });
        };

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
            GetChartData();
        });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) {
            GetChartData();
        });

        angular.element(document).ready(function () {
           
        });
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
