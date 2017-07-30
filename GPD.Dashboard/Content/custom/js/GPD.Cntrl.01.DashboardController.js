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
        if (__UserEmail != "") { GetLogedinUserProfile(); }
    });
}]);


//=================================================
angular.module('GPD').controller('GPDDashboardCtrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$log', '$state', '$stateParams', 'toastr', 'CommonServices',
    function ($scope, $rootScope, $http, $location, $uibModal, $log, $state, $stateParams, toastr, CommonServices) {
        var $ctrl = this;
        var _projectChartObj, _categoriesChartObj;
        CommonServices.SetDefaultData($ctrl, $location);
        $ctrl.data.LogedinUserProfile = CommonServices.LogedinUserProfile;
        $ctrl.data.fromDate = "";
        $ctrl.data.toDate = "";
        $ctrl.data.UniqueUserCount = 0;
        $ctrl.data.ProjectCount = 0;
        $ctrl.data.BPMCount = 0;
        $ctrl.data.PartnerCount = 0;
        $ctrl.data.TopCategories = [];

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

        var GetProjectChartData = function () {
            if (_projectChartObj) { _projectChartObj = _projectChartObj.destroy(); }
            return CommonServices.GetProjectChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                circloidLineChartFlot("#graphProjectChart", payload);
            });
        };

        var GetCategoriesChartData = function () {
            if (_categoriesChartObj) { _categoriesChartObj = _categoriesChartObj.destroy(); }
            return CommonServices.GetCategoriesChartData($ctrl.data.LogedinUserProfile.selectedPartner, $ctrl.data.fromDate, $ctrl.data.toDate)
            .then(function (payload) {
                var td = [];
                angular.forEach(payload.lines, function (v1, k1) {
                    var tempLine = { name: v1.name , value : 0};
                    angular.forEach(v1.values, function (v2, k2) {
                        tempLine.value = tempLine.value +  v2;
                    });
                    td.push(tempLine);
                });

                $ctrl.data.TopCategories = td;
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


        $rootScope.$on('EVENT-LogedinUserProfileLoaded', function (event, data) {
            GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount(); GetPartnerCount();
        });
        $rootScope.$on('EVENT-ChangePartner', function (event, data) {
            GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount();
        });
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $ctrl.data.LogedinUserProfile.selectedMenu = "GPD.Dashboard";
            if (fromState.name != '') {
                GetProjectChartData(); GetCategoriesChartData(); GetUniqueUserCount(); GetProjectCount(); GetBPMCount(); GetPartnerCount();
            }
        });

        angular.element(document).ready(function () { circloidMapWorld("#world-map"); });
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

