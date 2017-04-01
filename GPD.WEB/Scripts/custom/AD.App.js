
(function () {
    'use strict';
    angular.module('ReportSettings', []);
    angular.module('Download', []);
    angular.module('AccountSearch', []);
    angular.module('AccountSelect', []);
    angular.module('DateSelect', []);
    angular.module('Pagination', []);

    angular.module('Dashboard', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect', 'ReportSettings']);

    angular.module('ProjectSummary', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect', 'Pagination']);

    angular.module('ProjectDetail', ['Download']);

    angular.module('WebsiteActivity', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect']);

    angular.module('RequestDetail', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect', 'Pagination']);

    angular.module('ProductDetail', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect', 'Pagination']);

    angular.module('NewsLetterActivity', ['AccountSearch', 'AccountSelect', 'Download', 'DateSelect']);

})();



