//AccountSearchController =================================
angular.module('AccountSearch').controller('AccountSearchController', function ($scope, $location, $q, CommonServices) {
    $scope.ClearAccount = function ($event) {
        $scope.data.searchedAccountNo = "";
        $("input#search_account").val("");
        $scope.data.errorFilterSelection = "";
        $scope.data.ac.isVisible = false;
        $scope.$emit("AccountSearch_ClearAccount", "");
    };

    $scope.ValidateAccountNumber = function ($event) {
        $scope.data.errorFilterSelection = "";
        $scope.data.ac.isVisible = false;
        var data = $("input#search_account").val();
        CommonServices.ValidateAccountNumber(data)
        .then(function (payload) {
           if (payload.isValid == true) {
               $scope.data.searchedAccountNo = payload.accountNo;
               $scope.$emit("AccountSearch_SearchAccount", "");
           } else {
               $scope.data.errorFilterSelection = "Invalid account number.";
           };
       });

    };

    $scope.GetAccounts = function (term) {
        console.log(term);
        var regex = /^\w(\w|\s|.){2,}/;
        if (regex.test(term)) {
            CommonServices.GetAccounts(term).then(function (payload) {
                $scope.data.ac.accounts = payload;
                $scope.data.ac.isVisible = payload.length > 0 ? true : false;
            });
        } else {
            $scope.data.ac.accounts = [];
            $scope.data.ac.isVisible = false;
        }
    };

    $scope.isVisible = function () {
        return $scope.data.ac.isVisible;
    };

    $scope.SelectAccount = function (d) {
        $("input#search_account").val(d.value);
        $scope.data.searchedAccountNo = d.value;
        $scope.data.ac.accounts = [];
        $scope.data.ac.isVisible = false;
        $scope.$emit("AccountSearch_SearchAccount", "");
    };

    angular.element(document).ready(function () { });
})
.directive('keyboardPoster', function ($parse, $timeout) {
    return {
        scope: {
            postFn: '&'
        },
        link: function (scope, elem, attr) {
            var DELAY_TIME_BEFORE_POSTING = 500;
            var currentTimeout = null;
            elem.bind("input", function (event) {
                if (currentTimeout) {
                    $timeout.cancel(currentTimeout);
                }
                currentTimeout = $timeout(function () {
                    var t = elem.val();
                    scope.postFn({ term: t });
                }, DELAY_TIME_BEFORE_POSTING);
            });
        }
    };
});