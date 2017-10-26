//=================================================
angular.module('GPD').controller("GPDLeftMenuCtrl", ['$scope', '$http', '$location', '$log', 'toastr', 'CommonServices', function ($scope, $http, $location, $log, toastr, CommonServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

    $ctrl.MenuClass = function (d) {
        return $ctrl.data.LogedinUserProfile.selectedMenu == d ? "menu-item-top selected" : "menu-item-top";
    };

    angular.element(document).ready(function () { });
}]);

//=================================================
angular.module('GPD').controller("GPDPartnerCtrl", ['$scope', '$http', '$location', '$log', 'toastr', 'CommonServices', function ($scope, $http, $location, $log, toastr, CommonServices) {
    var $ctrl = this;
    CommonServices.SetDefaultData($ctrl, $location);
    $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

    //$ctrl.data.opts = {
    //    dateFormat: 'dd/mm/yy',
    //    changeMonth: true,
    //    changeYear: true
    //};
    //$ctrl.data.valor= "10/09/2013";

    $ctrl.SelectPartner = function (d, $event) {
        $ctrl.data.LogedinUserProfile.selectedPartner = d;
        CommonServices.ChangePartner(d);
        angular.element($event.currentTarget).parents("ul[data-id='menu']").hide();
    };

    var GetLogedinUserProfile = function () {
        CommonServices.GetLogedinUserProfile()
        .then(function (payload) {
            CommonServices.LogedinUserProfileLoaded();
        });
    };

    angular.element(document).ready(function () {
        if (__UserId != "") {
            setTimeout(function () {
                GetLogedinUserProfile();
            }, 2500);
        }
    });
}]);


//=================================================
angular.module('GPD').controller('GPDDashboardCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', '$filter', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, $filter, toastr, CommonServices) {
        var $ctrl = this;
        var _projectChartObj, _pctAppChart, _pctTopAppChart, _topCustomers, _topProductChart, _pctProjectManufacture, _pctProjectProductTAG;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.to = {};
        $ctrl.data.from = {};
        
        var ResetFilter = function () {
            $ctrl.data.projectListResponse = {};
            $ctrl.data.UniqueUserCount = 0;
            $ctrl.data.ProjectCount = 0;
            $ctrl.data.BPMCount = 0;
            $ctrl.data.PartnerCount = 0;

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

        /*=====================================
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

        $ctrl.toDatePopupOpen = function () {
            $ctrl.data.to.popupOpened = true;
        };
        $ctrl.fromDatePopupOpen = function () {
            $ctrl.data.from.popupOpened = true;
        };
        $ctrl.toDateSelected = function () {
            ResetDateRange("TO");
            GetAllChartData();
        };
        $ctrl.fromDateSelected = function () {
            ResetDateRange("FROM");
            GetAllChartData();
        };
        ====================================*/

        var RenderProjectChartDataD3 = function (placeholder, d) {
            var tempData = CommonServices.TransformChartDataD3(d);

            _projectChartObj = c3.generate({
                bindto: placeholder,
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
                point: { r: 3 },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: { format: "%d-%b" } // %b - month name, %d - date, %m - month, %y - year
                    }
                },
                legend: { show: true }
            });
        };

        var RenderPieChartDataD3 = function (placeholder, d) {
            var pieChart = c3.generate({
                bindto: placeholder,
                data: {
                    columns: d,
                    type: 'pie'
                },
                legend: { show: true, position: 'right' },
                pie: {
                    label: { show: false }
                }
            });
            return pieChart;
        };

        var RenderDonutChartDataD3 = function (placeholder, d) {
            var donutChart = c3.generate({
                bindto: placeholder,
                data: {
                    columns: d,
                    type: 'donut'
                },
                legend: { show: false, position: 'right' },
                donut: {
                    width: 10,
                    label: { show: false }
                }
            });
            return donutChart;
        };

        var RenderGaugeChartDataD3 = function (placeholder, color, d) {
            var donutChart = c3.generate({
                bindto: placeholder,
                color: { pattern: color },
                data: {
                    columns: d,
                    type: 'gauge'
                },
                legend: { show: false, position: 'right' },
                gauge: {
                    width: 10,
                    label: { show: false }
                }
            });
            return donutChart;
        };

        var GetProjects = function () {
            return CommonServices.GetProjects($ctrl.data.LogedinUserProfile.selectedPartner, "", "", "", "", 1, 10)
            .then(function (payload) {
                $ctrl.data.projectListResponse = payload;
            });
        };

        var GetProjectChartData = function () {
            if (_projectChartObj) { _projectChartObj = _projectChartObj.destroy(); }
            return CommonServices.GetProjectChartData($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                RenderProjectChartDataD3("#ProjectChartD3", payload);
            });
        };

        var GetAppChartData = function () {
            if (_pctAppChart) { _pctAppChart = _pctAppChart.destroy(); }
            if (_pctTopAppChart) { _pctTopAppChart = _pctTopAppChart.destroy(); }
            return CommonServices.GetAppChartData($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                var td = [];
                angular.forEach(payload.lines, function (v1, k1) {
                    var sum = v1.values.reduce(function (t, v) { return t + v; }, 0);
                    td.push([v1.name, sum]);
                });
                _pctAppChart = RenderDonutChartDataD3("#pctAppProject", td);
                var len = td.length > 5 ? 5 : td.length;
                _pctTopAppChart = RenderPieChartDataD3("#topApplication", td.slice(0, len));
            });
        };

        var GetUniqueUserCount = function () {
            return CommonServices.GetUniqueUserCount($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                $ctrl.data.UniqueUserCount = payload;
            });
        };

        var GetProjectCount = function () {
            return CommonServices.GetProjectCount($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                $ctrl.data.ProjectCount = payload;
            });
        };

        var GetBPMCount = function () {
            return CommonServices.GetBPMCount($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                $ctrl.data.BPMCount = payload;
            });
        };

        var GetPctProjectWithManufacture = function () {
            if (_pctProjectManufacture) { _pctProjectManufacture = _pctProjectManufacture.destroy(); }
            return CommonServices.GetPctProjectWithManufacturer($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                _pctProjectManufacture = RenderGaugeChartDataD3("#pctProjectManufacture", ["#63B2F7"], [["Manufacture", payload]])
            });
        };

        var GetPctProjectWithProductTAG = function () {
            if (_pctProjectProductTAG) { _pctProjectProductTAG = _pctProjectProductTAG.destroy(); }
            return CommonServices.GetPctProjectWithProductTAG($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                _pctProjectProductTAG = RenderGaugeChartDataD3("#pctProjectProductTAG", ["#65B465"], [["ProductTAG", payload]])
            });
        };

        var GetPartnerCount = function () {
            return CommonServices.GetPartnerCount()
            .then(function (payload) {
                $ctrl.data.PartnerCount = payload;
            });
        };

        var GetProjectYOYChartData = function () {
            var d = [
                        ["Project This Year (" + $filter('number')(2300) + ")", 2300],
                        ["Project Last Year (" + $filter('number')(6000) + ")", 6000]
            ];
            RenderPieChartDataD3("#projectYOYChart", d);
        };

        var GetTopProductChartData = function () {
            if (_topProductChart) { _topProductChart = _topProductChart.destroy(); }
            return CommonServices.GetTopProductChartData($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                var td = [];
                angular.forEach(payload.lines, function (v1, k1) {
                    var sum = v1.values.reduce(function (t, v) { return t + v; }, 0);
                    td.push([v1.name + "(" + $filter('number')(sum) + ")", sum]);
                });
                var len = td.length > 5 ? 5 : td.length;
                _topProductChart = RenderPieChartDataD3("#topProducts", td.slice(0, len));
            });
        };

        var GetTopCustomerChartData = function () {
            if (_topCustomers) { _topCustomers = _topCustomers.destroy(); }
            return CommonServices.GetTopCustomerChartData($ctrl.data.LogedinUserProfile.selectedPartner)
            .then(function (payload) {
                var td = [];
                angular.forEach(payload.lines, function (v1, k1) {
                    var sum = v1.values.reduce(function (t, v) { return t + v; }, 0);
                    td.push([v1.name + "(" + $filter('number')(sum) + ")", sum]);
                });
                var len = td.length > 5 ? 5 : td.length;
                _topCustomers = RenderPieChartDataD3("#topCustomers", td.slice(0, len));
            });
        };

        var GetAllChartData = function () {
            GetUniqueUserCount();
            GetProjectCount();
            GetBPMCount();
            GetPartnerCount();
            GetPctProjectWithManufacture();
            GetPctProjectWithProductTAG();
            GetProjectChartData();
            GetAppChartData();
            GetTopProductChartData();
            GetTopCustomerChartData();
            GetProjects();
            GetProjectYOYChartData();
        };

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) { ResetFilter(); GetAllChartData(); });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) { ResetFilter(); GetAllChartData(); });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Dashboard";
            if (fromState.name != '') { GetAllChartData(); }
        });

        angular.element(document).ready(function () {            
        });
    }]);

//=================================================
angular.module('GPD').controller('GPDProjectCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) { });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) { });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Project";
            if (fromState.name != '') { }
        });
        angular.element(document).ready(function () { });
    }]);

//=================================================
angular.module('GPD').controller('GPDReportCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) { });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) { });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Report";
            if (fromState.name != '') { }
        });
        angular.element(document).ready(function () { });
    }]);

//=================================================
angular.module('GPD').controller('GPDManageCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) { });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) { });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Manage";
            if (fromState.name != '') { }
        });
        angular.element(document).ready(function () { });
    }]);

//=================================================
angular.module('GPD').controller('GPDMapCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) { });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) { });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Map";
            if (fromState.name != '') { }
        });
        angular.element(document).ready(function () { });
    }]);

