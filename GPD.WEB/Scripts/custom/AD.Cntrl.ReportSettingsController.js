//=================================================
angular.module('ReportSettings').controller('ReportSettingsController', function ($scope, $http, ReportSettingsServices) {
    $scope.data.leadEmailSetting = { "MonthlyAlertInd": false, "SweetsAccNumbers": [], "AltEmail": "", "errorMessage": "" };

    var OpenReportSettingPopup = function () {
        GetLeadEmailSettingData();
        var popup = $("div#divReportSetting");
        $(popup).dialog({
            autoOpen: false,
            modal: true,
            width: 560,
            height: 400,
            title: "Monthly Report Settings",
            dialogClass: "Popup",
            buttons: {
                "Save": function () {
                    SaveLeadEmailSettings(popup);
                },
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        $(popup).dialog("open");
    };

    var GetLeadEmailSettingData = function () {
        ReportSettingsServices.GetLeadEmailSettingsForUser()
        .then(function (payload) {
            $scope.data.leadEmailSetting = payload;
        });
    };

    var SaveLeadEmailSettings = function (popup) {
        $scope.$apply(function () { $scope.data.leadEmailSetting.errorMessage = ""; })
        var altMonthlyEmail = $scope.data.leadEmailSetting.AltEmail;
        var emailToTest = $.trim(altMonthlyEmail);
        var regex = /^(\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*[,])*\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*$/;
        if (emailToTest == '' || regex.test(emailToTest)) {
            var postData = new Object();
            postData.dailyAlertInd = "N";
            postData.monthlyAlertInd = $scope.data.leadEmailSetting.MonthlyAlertInd == true ? 'M' : 'N';
            postData.altDailyEmail = "";
            postData.altMonthlyEmail = emailToTest;

            var itemHorizonNumber = [];

            $($scope.data.leadEmailSetting.SweetsAccNumbers)
            .filter(function (index) { return this.IsChecked == true; })
            .each(function () { itemHorizonNumber.push(this.HorizonNumber); });


            postData.chkBoxListItems = itemHorizonNumber.join(",");

            return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/SaveLeadEmailAlertSettings", JSON.stringify(postData))
                .success(function (data, status, headers, config) {
                    $scope.data.leadEmailSetting.errorMessage = "";
                    $(popup).dialog("close");
                    return true;
                })
                .error(function (data, status, header, config) {
                    $scope.data.leadEmailSetting.errorMessage = "Server Error!";
                    return false;
                });
        } else {
            $scope.$apply(function () { $scope.data.leadEmailSetting.errorMessage = "Please enter valid email address."; })
            return false;
        }
    };

    $scope.OnAccountCheckChange = function (d) {
        var tempAccNo = $scope.data.leadEmailSetting.SweetsAccNumbers[d];
        if (tempAccNo.IsChecked == false) {
            var tempAccNos = $($scope.data.leadEmailSetting.SweetsAccNumbers).filter(function (index) { return this.IsChecked == true; });
            if (tempAccNos.length == 0) { $scope.data.leadEmailSetting.MonthlyAlertInd = false; }
        }
        else {
            $scope.data.leadEmailSetting.MonthlyAlertInd = true;
        }
    };

    $scope.OnFlagCheckChange = function () {
        $.each($scope.data.leadEmailSetting.SweetsAccNumbers, function (k, v) { v.IsChecked = $scope.data.leadEmailSetting.MonthlyAlertInd; });
    };

    $scope.OnReportSettingClick = function ($event) {
        OpenReportSettingPopup();
    };

    angular.element(document).ready(function () { });
});