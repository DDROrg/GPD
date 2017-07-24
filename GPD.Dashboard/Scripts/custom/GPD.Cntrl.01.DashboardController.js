

//=================================================
angular.module('GPD').controller('GPDDashboardController', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        var _chartObj;
        var _chartColor = ['#ff0000', '#ff6a00', '#ffd800', '#b6ff00', '#4cff00', '#5f798d', '#0094ff', '#0000ff'];
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.fromDate = "";
        $ctrl.data.toDate = "";
        $ctrl.data.test = "GPD-Dashboard-Controller";
        var DestroyChartData = function () {
            if (_chartObj) { _chartObj = _chartObj.destroy(); }
        }

        var RenderChartData = function (d) {
            var tempPattern = [];
            var tempXs = {};
            var tempColumns = [];
            angular.forEach(d, function (v, k) {
                var i = "x" + (k + 1);
                //tempPattern.push(v.color);
                tempPattern.push(_chartColor[k]);
                tempXs[v.Name] = i;
                v.dates.unshift(i);
                v.values.unshift(v.Name)
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
        }

        var GetChartData = function () {
            DestroyChartData();
            //return CommonServices.GetChartData($ctrl.data.fromDate, $ctrl.data.toDate)
            //.then(function (payload) {
            //    //RenderChartData(payload, $scope.data.selectedChart.cName, $scope.data.selectedChart.color);
            //    RenderChartData(payload, "Test", "#FF0000");
            //});

            var dd = [];
            dd.push({
                Name: "Revit",
                color: "#FF0000",
                dates: ["2017-05-01", "2017-05-02", "2017-05-03", "2017-05-04", "2017-05-05", "2017-05-06"],
                values: [10, 12, 9, 17, 20, 14]
            });
            dd.push({
                Name: "AutoCAD",
                color: "#FFFF00",
                dates: ["2017-05-01", "2017-05-02", "2017-05-03", "2017-05-04", "2017-05-05", "2017-05-06"],
                values: [12, 12, 19, 21, 24, 4]
            });

            RenderChartData(dd);
        }

        angular.element(document).ready(function () {
            GetChartData();
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
