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
        var _projectChartObj, _topProductChart;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.fromDate = "";
        $ctrl.data.toDate = "";
        $ctrl.data.UniqueUserCount = 0;
        $ctrl.data.ProjectCount = 0;
        $ctrl.data.BPMCount = 0;
        $ctrl.data.PartnerCount = 0;

        var circloidLineChartFlot = function (placeholder, d) {
            var td = CommonServices.TransformChartData(d);
            var colors = CommonServices.GetColor();

            var options = {
                series: {
                    lines: {
                        show: true,
                        fill: true,
                        lineWidth: 1.5
                    },
                    points: {
                        show: true,
                        radius: 6
                    }
                },
                shadowSize: 0,
                grid: {
                    backgroundColor: '#FFFFFF',
                    borderColor: '#D6D6D9',
                    borderWidth: 1,
                    hoverable: true
                },
                legend: {
                    show: true,
                    position: "nw"
                },
                xaxis: {
                    ticks: td.xaxis
                },
                tooltip: true,
                tooltipOpts: {
                    content: "%s: <b>%y</b>",
                    shifts: {
                        x: -40,
                        y: 25
                    },
                    defaultTheme: false
                },
                colors: colors
            }

            $.plot(placeholder, td.lines, options);
        };

        var circloidDialChart = function (placeholder) {

            var colors = $(placeholder).data("graph-colors").split(',');
            var chartSize = $(placeholder).height();

            // Set the width of the Graph placeholder
            $(placeholder).width(chartSize);

            // Set inner text line-height
            $(placeholder).find(".percent").css({ "line-height": chartSize + "px" });

            $(placeholder).easyPieChart({
                barColor: function (percent) {
                    if (colors[1] === undefined) {
                        return colors;
                    } else {
                        if (percent < 25) {
                            return colors[1];
                        } else if ((percent >= 25) && (percent < 50)) {
                            return colors[2];
                        } else if ((percent >= 50) && (percent < 75)) {
                            return colors[3];
                        } else {
                            return colors[0];
                        }
                    }
                },
                size: chartSize,
                lineCap: "square",
                scaleColor: "#7A7A7A",
                trackColor: "#E8E8E8",
                onStep: function (from, to, percent) {
                    $(this.el).find('.percent').text(Math.round(percent));
                }
            });

            var chart = window.chart = $(placeholder).data('easyPieChart');
            $(placeholder).closest(".c-widget").find('.update-graph').on('click', function (e) {
                chart.update(Math.random() * (90 - 8) + 8);
                e.preventDefault();
            });
        };

        var circloidDonutChartFlot = function (placeholder, graphSize, legend) {

            var colors = $(placeholder).data("graph-colors").split(',');

            if (graphSize === undefined) {
                graphSize = 0.88;
            } else if (graphSize == "micro") {
                graphSize = 0.75;
            } else if (graphSize == "small") {
                graphSize = 0.85;
            } else if (graphSize == "medium") {
                graphSize = 0.87;
            } else if (graphSize == "normal" || graphSize == "large") {
                graphSize = 0.88;
            }

            if (legend === undefined) {
                legend = true
            }

            var data = [
                { data: 10900.0000, color: colors[0], label: "Servers" },
                { data: 10240.0000, color: colors[1], label: "Laptops/Desktops" },
                { data: 3900.0000, color: colors[2], label: "Software Licenses" },
                { data: 2050.0000, color: colors[3], label: "General Repairs" },
                { data: 1050.0000, color: colors[5], label: "Administrative Items" }
            ];
            var options = {
                series: {
                    pie: {
                        show: true,
                        radius: 1,
                        innerRadius: graphSize,
                        label: false
                    }
                },
                legend: {
                    show: legend
                },
                grid: {
                    hoverable: true
                },
                tooltip: true,
                tooltipOpts: {
                    content: function (label, xval, yval, flotItem) {
                        return label + ": <b>$" + yval.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + "</b>";
                    },
                    shifts: {
                        x: -60,
                        y: 25
                    },
                    defaultTheme: false
                }
            };

            // Plot the chart and set options
            var plotChart = $.plot(placeholder, data, options);

            if (isNaN(plotChart.getData()[0].percent)) {
                var canvas = plotChart.getCanvas();
                var ctx = canvas.getContext("2d");
                var x = canvas.width / 2;
                var y = canvas.height / 2;
                ctx.textAlign = 'center';
                ctx.fillText('No Data for this date range', x, y);
            }
        };

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

        /*============================================================
        var circloidMapWorld = function (placeholder) {
            var sample_data = { "af": "16.63", "al": "11.58", "dz": "158.97", "ao": "85.81", "ag": "1.1", "ar": "351.02", "am": "8.83", "au": "1219.72", "at": "366.26", "az": "52.17", "bs": "7.54", "bh": "21.73", "bd": "105.4", "bb": "3.96", "by": "52.89", "be": "461.33", "bz": "1.43", "bj": "6.49", "bt": "1.4", "bo": "19.18", "ba": "16.2", "bw": "12.5", "br": "2023.53", "bn": "11.96", "bg": "44.84", "bf": "8.67", "bi": "1.47", "kh": "11.36", "cm": "21.88", "ca": "1563.66", "cv": "1.57", "cf": "2.11", "td": "7.59", "cl": "199.18", "cn": "5745.13", "co": "283.11", "km": "0.56", "cd": "12.6", "cg": "11.88", "cr": "35.02", "ci": "22.38", "hr": "59.92", "cy": "22.75", "cz": "195.23", "dk": "304.56", "dj": "1.14", "dm": "0.38", "do": "50.87", "ec": "61.49", "eg": "216.83", "sv": "21.8", "gq": "14.55", "er": "2.25", "ee": "19.22", "et": "30.94", "fj": "3.15", "fi": "231.98", "fr": "2555.44", "ga": "12.56", "gm": "1.04", "ge": "11.23", "de": "3305.9", "gh": "18.06", "gr": "305.01", "gd": "0.65", "gt": "40.77", "gn": "4.34", "gw": "0.83", "gy": "2.2", "ht": "6.5", "hn": "15.34", "hk": "226.49", "hu": "132.28", "is": "12.77", "in": "1430.02", "id": "695.06", "ir": "337.9", "iq": "84.14", "ie": "204.14", "il": "201.25", "it": "2036.69", "jm": "13.74", "jp": "5390.9", "jo": "27.13", "kz": "129.76", "ke": "32.42", "ki": "0.15", "kr": "986.26", "undefined": "5.73", "kw": "117.32", "kg": "4.44", "la": "6.34", "lv": "23.39", "lb": "39.15", "ls": "1.8", "lr": "0.98", "ly": "77.91", "lt": "35.73", "lu": "52.43", "mk": "9.58", "mg": "8.33", "mw": "5.04", "my": "218.95", "mv": "1.43", "ml": "9.08", "mt": "7.8", "mr": "3.49", "mu": "9.43", "mx": "1004.04", "md": "5.36", "mn": "5.81", "me": "3.88", "ma": "91.7", "mz": "10.21", "mm": "35.65", "na": "11.45", "np": "15.11", "nl": "770.31", "nz": "138", "ni": "6.38", "ne": "5.6", "ng": "206.66", "no": "413.51", "om": "53.78", "pk": "174.79", "pa": "27.2", "pg": "8.81", "py": "17.17", "pe": "153.55", "ph": "189.06", "pl": "438.88", "pt": "223.7", "qa": "126.52", "ro": "158.39", "ru": "1476.91", "rw": "5.69", "ws": "0.55", "st": "0.19", "sa": "434.44", "sn": "12.66", "rs": "38.92", "sc": "0.92", "sl": "1.9", "sg": "217.38", "sk": "86.26", "si": "46.44", "sb": "0.67", "za": "354.41", "es": "1374.78", "lk": "48.24", "kn": "0.56", "lc": "1", "vc": "0.58", "sd": "65.93", "sr": "3.3", "sz": "3.17", "se": "444.59", "ch": "522.44", "sy": "59.63", "tw": "426.98", "tj": "5.58", "tz": "22.43", "th": "312.61", "tl": "0.62", "tg": "3.07", "to": "0.3", "tt": "21.2", "tn": "43.86", "tr": "729.05", "tm": 0, "ug": "17.12", "ua": "136.56", "ae": "239.65", "gb": "2258.57", "us": "14624.18", "uy": "40.71", "uz": "37.72", "vu": "0.72", "ve": "285.21", "vn": "101.99", "ye": "30.02", "zm": "15.69", "zw": "5.57" };
            var colors = $(placeholder).data("graph-colors").split(',');

            $(placeholder).vectorMap({
                map: 'world_en',
                backgroundColor: '#FFFFFF',
                color: '#ffffff',
                hoverOpacity: 0.7,
                selectedColor: '#666666',
                enableZoom: true,
                showTooltip: true,
                values: sample_data,
                scaleColors: colors,
                normalizeFunction: 'polynomial'
            });
        };
        ============================================================*/

        var GetProjectChartData = function () {
            if (_projectChartObj) { _projectChartObj = _projectChartObj.destroy(); }
            return CommonServices.GetProjectChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                circloidLineChartFlot("#graphProjectChart", payload);
                RenderProjectChartDataD3("#ProjectChartD3", payload);
            });
        };

        var GetCategoriesChartData = function () {
            //if (_categoriesChartObj) { _categoriesChartObj = _categoriesChartObj.destroy(); }
            //return CommonServices.GetCategoriesChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            //.then(function (payload) {
            //    var td = [];
            //    angular.forEach(payload.lines, function (v1, k1) {
            //        var tempLine = { name: v1.name, value: 0 };
            //        angular.forEach(v1.values, function (v2, k2) {
            //            tempLine.value = tempLine.value + v2;
            //        });
            //        td.push(tempLine);
            //    });
            //    debugger;
            //    $ctrl.data.TopCategories = td;
            //});
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

        var GetBPMCount = function () {
            return CommonServices.GetBPMCount($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                $ctrl.data.BPMCount = payload;
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
            return CommonServices.GetTopProductChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                var td = [];
                angular.forEach(payload.lines, function (v1, k1) {
                    var sum = v1.values.reduce(function (t, v) { return t + v; }, 0);
                    td.push([v1.name + "(" + $filter('number')(sum) + ")", sum]);
                });
                _topProductChart = RenderPieChartDataD3("#topProducts", td);
            });
        };
        var GetTopApplicationChartData = function () {
            var d = [
                        ["UL SPOT App for Autodesk Revit", 2380],
                        ["AEC Daily App for Autodesk Revit", 6000],
                        ["AEC Daily App for Autodesk AutoCAD", 4700],
                        ["UL SPOT App for Autodesk AutoCAD", 2800],
                        ["UL SPOT App for Sketchup", 3900]
            ];
            RenderPieChartDataD3("#topApplication", d);
        };
        var GetTopCustomerChartData = function () {
            var d = [
                        ["Delta (" + $filter('number')(2300) + ")", 2300],
                        ["Harman Miller (" + $filter('number')(10000) + ")", 6000],
                        ["Kawneer (" + $filter('number')(4700) + ")", 4700],
                        ["Kimball (" + $filter('number')(2800) + ")", 2800],
                        ["Peachtree Doors & Window (" + $filter('number')(3900) + ")", 3900]
            ];
            RenderPieChartDataD3("#topCustomers", d);
        };
        //topCustomers

        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
            GetProjectChartData(); GetTopProductChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount(); GetPartnerCount();
        });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) {
            GetProjectChartData(); GetTopProductChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount();
        });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Dashboard";
            if (fromState.name != '') {
                GetProjectChartData(); GetTopProductChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount(); GetPartnerCount();
            }
        });

        angular.element(document).ready(function () {
            circloidDialChart("#pctProjectManufacture");
            circloidDialChart("#pctProjectProductTAG");
            circloidDonutChartFlot("#pctAppProject", "small", false);
            GetProjectYOYChartData();
            GetTopApplicationChartData();
            GetTopCustomerChartData();
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

