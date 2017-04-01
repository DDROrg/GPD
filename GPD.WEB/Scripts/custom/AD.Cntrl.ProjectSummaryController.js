angular.module('ProjectSummary')
.controller('ProjectSummaryController', function ($scope, $http, $location, $q, CommonServices, ProjectSummaryServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.projectSummaryLoading = false;

    $scope.data.opportunityTypes = [
        { id: "allop", text: "All" },
        { id: "notsel", text: "Not Selected" },
        { id: "sel", text: "Selected" }
    ];
    var tempSel = $($scope.data.opportunityTypes).filter(function (index) { return this.id === $scope.data.selectedOpportunityTypes; });
    if (tempSel.length == 0) { $scope.data.selectedOpportunityTypes = "allop"; }

    $scope.data.reports = [];
    $scope.data.productCategories = [];
    $scope.data.totalresults = 0;

    $scope.data.sort = {
        column: 'asId',
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
        var tempData = CommonServices.GetCompanies($scope.data.allCompanies, $scope.data.selectedAccount, $scope.data.selectedCompany);
        $scope.data.companies = tempData.companies;
        $scope.data.selectedCompany = tempData.selectedCompany;
    };

    var GetProjectSummaryData = function (includeProdCtg, resetPage) {
        $scope.data.projectSummaryLoading = true;
        if ($scope.data.showAccountSearch) {
            $location.search("searchedAccNo", $scope.data.searchedAccountNo);
        }
        $location.search("selAccNo", $scope.data.selectedAccount);
        $location.search("selProNo", $scope.data.selectedCompany);
        $location.search("OppId", $scope.data.selectedOpportunityTypes);
        $location.search("stdt", $scope.data.fromDate);
        $location.search("enddt", $scope.data.toDate);

        var prodCtg = includeProdCtg == true ? "All" : GetSelectedProductCategory();
        if (resetPage == true) { $scope.data.Pagination.currentPageIndex = 1; }
        return ProjectSummaryServices.GetProjectSummaryData(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate, $scope.data.selectedOpportunityTypes, prodCtg, $scope.data.Pagination.selectedPageSize, $scope.data.Pagination.currentPageIndex, includeProdCtg)
        .then(function (payload) {
            $scope.data.Pagination.totalRecord = payload.totalCount;
            $scope.data.reports = payload.projectSummaries;
            $scope.data.dodgeProjectUrl = payload.dodgeProjectUrl;
            if (includeProdCtg == true) {
                $scope.data.productCategories = [{ value: "All", IsChecked: true }];
                $.each(payload.productCategories, function (k, v) {
                    $scope.data.productCategories.push({ value: v, IsChecked: true });
                });
            }
            $scope.$broadcast("BeforePaginationRendered", "");
            $scope.data.projectSummaryLoading = false;
        });
    };

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
    };

    var GetSelectedProductCategory = function () {
        return CommonServices.GetSelectedProductCategory($scope.data.productCategories);
    };

    $scope.OnOpportunityTypeChange = function () { GetProjectSummaryData(false, true); };

    $scope.IsLoading = function () {
        return $scope.data.projectSummaryLoading
        || $scope.data.downloadingReport;
    };

    $scope.GetProjectDetailLink = function (drNo, projName) {
        var url = __RootUrl + "/Projects/ProjectDetail2.aspx";
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate,
            "drNo": drNo,
            "projName": projName
        }
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }
        return url + "#?" + $.param(p);
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
    }


    $scope.OnProductCategoryCheckChange = function (d) {
        var tempProCtg = $scope.data.productCategories[d];
        if (tempProCtg.value == "All") {
            $.each($scope.data.productCategories, function (k, v) { v.IsChecked = tempProCtg.IsChecked; });
        } else {
            if (tempProCtg.IsChecked == false) {
                var tempProCtg = $($scope.data.productCategories).filter(function (index) { return this.value === "All"; });
                if (tempProCtg.length > 0) { tempProCtg[0].IsChecked = false; }
            }
        }
    };

    $scope.OnProductCategoryClick = function ($event) {
        var ele = $($event.currentTarget);
        var popup = $("div#divProductCtg");
        $(popup).dialog({
            autoOpen: false,
            modal: true,
            width: 950,
            //height: 530,
            title: "Select Product Category",
            dialogClass: "Popup",
            position: { my: 'top', at: 'top+20' },
            buttons: {
                "OK": function () {
                    GetProjectSummaryData(false, true);
                    $(this).dialog("close");
                }
            }
        });

        $(popup).dialog("open");
    };

    $scope.$on("AccountSearch_ClearAccount", function (evt, data) {
        GetAccount().then(function () { GetProjectSummaryData(true, true); });
    });

    $scope.$on("AccountSearch_SearchAccount", function (evt, data) {
        GetAccount().then(function () { GetProjectSummaryData(true, true); });
    });

    $scope.$on("DateSelect_DateChanged", function (evt, data) { GetProjectSummaryData(true, true); });
    $scope.$on("Pagination_OnChange", function (evt, data) { GetProjectSummaryData(false, false); });

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
        CommonServices.SetDefaultPagination($scope);
        GetAccount().then(function () { GetProjectSummaryData(true, true); });
    });
});

