angular.module('NewsLetterActivity')
.controller('NewsLetterActivityController', function ($scope, $http, $location, $q, CommonServices, NewsLetterActivityServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.websiteActivityLoading = false;

    $scope.data.reports = [];

    $scope.data.sort = {
        column: 'name',
        descending: false
    };  

    var GetAccount = function () {
        return CommonServices.GetAccount($scope.data.searchedAccountNo, $scope.data.selectedAccount)
        .then(function (payload) {
            $scope.data.accounts = payload.accounts;
            $scope.data.selectedAccount = payload.selectedAccount;
            $scope.data.allCompanies = payload.allCompanies;
            GetCompanies();
        });
    };

    var GetCompanies = function () {
        if ($scope.data.selectedAccount == "All") { $scope.data.selectedCompany = "All"; }
        var tempData = CommonServices.GetCompanies($scope.data.allCompanies, $scope.data.selectedAccount, $scope.data.selectedCompany);
        $scope.data.companies = tempData.companies;
        $scope.data.selectedCompany = tempData.selectedCompany;
    };

    var GetNewsLetterActivityData = function () {
        $scope.data.websiteActivityLoading = true;
        return NewsLetterActivityServices.GetNewsLetterActivityData(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.reports = payload.reports;
            $scope.data.websiteActivityLoading = false;
        });
    };

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
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

    $scope.IsLoading = function () {
        return $scope.data.websiteActivityLoading
                || $scope.data.downloadingReport;
    };

    $scope.SetBreadCrumbValue = function () {
        var url = __RootUrl + "/LeadsDashboard.aspx";
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate
        }
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }

        return url + "#?" + $.param(p);
    };

    $scope.$on("AccountSelect_OnAccountChange", function (evt, data) {
        GetCompanies();
        GetNewsLetterActivityData();
    });

    $scope.$on("AccountSelect_OnCompanyChange", function (evt, data) {
        GetNewsLetterActivityData();
    });

    $scope.$on("AccountSearch_ClearAccount", function (evt, data) {
        GetAccount().then(function () { GetNewsLetterActivityData(); });
    });

    $scope.$on("AccountSearch_SearchAccount", function (evt, data) {
        GetAccount().then(function () { GetNewsLetterActivityData(); });
    });

    $scope.$on("DateSelect_DateChanged", function (evt, data) {
        GetNewsLetterActivityData();
    });

    $scope.$watch(
    function () {
        var retval = 700;
        try { retval = $("div.body").height(); } catch (e) { }
        return retval > 700 ? retval : 700;
    },
    function (newValue, oldValue) {
        if (newValue != oldValue) {
            try {
                window.parent.resizeIframe(newValue);
            } catch (e) { }
        }
    });

    angular.element(document).ready(function () {
        CommonServices.SetDefaultDate($scope);
        GetAccount().then(function () { GetNewsLetterActivityData(); });
    });

});

