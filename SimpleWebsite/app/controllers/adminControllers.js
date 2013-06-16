app.controller('AdminHomeController', function ($scope, JsonCache, $templateCache) {
	$scope.jsonCache = JsonCache;
	$scope.templateCache = $templateCache;
	//console.log($scope.jsonCache);
	//console.log($scope.jsonCache.info());
	$scope.dataFiles = [];
	$scope.dataFiles.push('Content/json/VideoGames.json');
	$scope.dataFiles.push('Content/json/_psnProfile.xml.json');
	$scope.dataFiles.push('Content/json/_xblProfile.xml.json');
	$scope.dataFiles.push('Content/json/_psnGamesBasic.xml.json');
	$scope.dataFiles.push('Content/json/_xblGamesBasic.xml.json');
	$scope.dataFiles.push('Content/json/_psnGames.xml.json');
	$scope.dataFiles.push('Content/json/_xblGames.xml.json');

	$scope.templateFiles = [];
	$scope.templateFiles.push('app/partials/home.html');
	$scope.templateFiles.push('app/partials/basics/all.html');
	$scope.templateFiles.push('app/partials/basics/psn.html');
	$scope.templateFiles.push('app/partials/basics/xbl.html');
	$scope.templateFiles.push('app/partials/games.html');
	$scope.templateFiles.push('app/partials/profile.html');
	$scope.templateFiles.push('app/partials/psnGame.html');
	$scope.templateFiles.push('app/partials/xblGame.html');
	$scope.templateFiles.push('app/partials/stats/date.html');
	$scope.templateFiles.push('app/partials/stats/dayOfWeek.html');
	$scope.templateFiles.push('app/partials/stats/home.html');
	$scope.templateFiles.push('app/partials/stats/hour.html');
	$scope.templateFiles.push('app/partials/stats/type.html');
	$scope.templateFiles.push('app/partials/stats/yearMonth.html');
	$scope.templateFiles.push('app/partials/admin/home.html');

	//console.log($scope.templateFiles);
	//console.log($templateCache);
	//console.log($route);
	//console.log($templateCache.info());

	$scope.emptyDataCache = function () {
		JsonCache.removeAll();
	};

	$scope.emptyTemplateCache = function () {
		$scope.templateCache.removeAll();
	};
});
