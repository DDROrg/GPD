angular.module('ProjectDetail')
.controller('ProjectDetailController', function ($scope, $http, $location, $q, CommonServices, ProjectDetailServices) {
    CommonServices.SetDefaultData($scope, $location);
    var qp = $location.search();
    $scope.data.projectDetailLoading = false;

    $scope.data.sort = [
        { column: 'taggedUrlPresent', descending: true },
        { column: 'mnfPresent', descending: true },
        { column: 'manufacturer', descending: false },
        { column: 'model', descending: false },
        { column: 'quantity', descending: true }
    ];

    $scope.data.reports = [];
    $scope.data.projectName = qp.projName ? qp.projName : "";
    $scope.data.productCategories = [];

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

    var GetProjectDetailData = function (includeProdCtg) {
        $scope.data.projectDetailLoading = true;
        var prodCtg = includeProdCtg == true ? "All" : GetSelectedProductCategory();
        return ProjectDetailServices.GetProjectDetailData($scope.data.drNumber, prodCtg, includeProdCtg)
        .then(function (payload) {
            $scope.data.reports = payload.projectDetails;
            $scope.data.taggedImage = payload.taggedImage;
            $scope.data.dodgeProjectUrl = payload.dodgeProjectUrl;
            if (includeProdCtg == true) {
                $scope.data.productCategories = [{ value: "All", IsChecked: true }];
                $.each(payload.productCategories, function (k, v) {
                    $scope.data.productCategories.push({ value: v, IsChecked: true });
                });
            }
            $scope.data.projectDetailLoading = false;
        });
    };

    var GetSelectedProductCategory = function () {
        return CommonServices.GetSelectedProductCategory($scope.data.productCategories);
    };

    $scope.OnOpportunityTypeChange = function () { GetProjectDetailData(false); };

    $scope.IsLoading = function () {
        return $scope.data.projectDetailLoading
        || $scope.data.downloadingReport;
    };

    $scope.OnChangeSorting = function (col) {
        var t = { column: col, descending: true };
        if ($scope.data.sort.length > 0) {
            if ($scope.data.sort[0].column == col) {
                t.descending = !$scope.data.sort[0].descending;
            } else {
                t.descending = true;
            }
        }
        $scope.data.sort = [t];
    };

    $scope.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($scope.data.sort && $scope.data.sort.length == 1 && column == $scope.data.sort[0].column) {
            if ($scope.data.sort[0].descending) { retVal = "fa fa-sort-down"; }
            else { retVal = "fa fa-sort-up"; }
        }
        return retVal;
    };

    $scope.ColumnSortOrder = function () {
        var retVal = [];
        $.each($scope.data.sort, function (k, v) { retVal.push((v.descending ? "-" : "") + v.column); });
        return retVal;
    };

    $scope.SetBreadCrumbValue = function (type) {
        var url = "";
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate
        };
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }

        if (type == "Summary") {
            url = __RootUrl + "/LeadsDashboard.aspx";
        }
        else {
            url = __RootUrl + "/Projects/ProjectSummary2.aspx";
            p.OppId = $scope.data.selectedOpportunityTypes;
        }

        return url + "#?" + $.param(p);
    };

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
            //height: 300,
            title: "Select Product Category",
            dialogClass: "Popup",
            position: { my: 'top', at: 'top+100' },
            buttons: {
                "OK": function () {
                    GetProjectDetailData(false);
                    $(this).dialog("close");
                }
            }
        });

        $(popup).dialog("open");
    };

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
        GetAccount();
        GetAccount().then(function () { GetProjectDetailData(true); });       
    });
});

