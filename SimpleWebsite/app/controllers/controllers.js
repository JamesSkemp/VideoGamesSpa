app.controller('HomeController', function ($scope) {
	init();

	function init() {
	}
});

app.controller('GamesController', function ($scope, $http) {
	$scope.currentPage = 0;
	$scope.pageSize = 10;
	$scope.games = [];
	$http.get('/Content/json/VideoGames.json').success(function (data) {
		$scope.games = data.VideoGames;
	});
	$scope.numberOfPages = function () {
		return Math.ceil($scope.games.length / $scope.pageSize);
	}
	// TODO not working
	//$scope.orderProp = 'System';
	//console.log($scope);
});

app.controller('ProfileController', function ($scope, $http, $window) {
	$scope.psnProfile = [];
	$scope.xblProfile = [];
	$http.get('Content/json/_psnProfile.xml.json').success(function (data) {
		$scope.psnProfile = data.PsnProfile;
		$scope.psnPointsPercent = 100 * data.PsnProfile.Points / data.PsnProfile.PossiblePoints;
		$scope.psnTrophiesPercent = 100 * data.PsnProfile.Trophies / data.PsnProfile.PossibleTrophies;
		$scope.psnTrophiesBronzePercent = 100 * data.PsnProfile.TrophiesBronze / data.PsnProfile.PossibleTrophiesBronze;
		$scope.psnTrophiesSilverPercent = 100 * data.PsnProfile.TrophiesSilver / data.PsnProfile.PossibleTrophiesSilver;
		$scope.psnTrophiesGoldPercent = 100 * data.PsnProfile.TrophiesGold / data.PsnProfile.PossibleTrophiesGold;
		$scope.psnTrophiesPlatinumPercent = 100 * data.PsnProfile.TrophiesPlatinum / data.PsnProfile.PossibleTrophiesPlatinum;
	});
	$http.get('Content/json/_xblProfile.xml.json').success(function (data) {
		$scope.xblProfile = data.XblProfile;
		$scope.xblPointsPercent = 100 * data.XblProfile.GamerScore / data.XblProfile.PossibleGamerScore;
		$scope.xblAchievementsPercent = 100 * data.XblProfile.Achievements / data.XblProfile.PossibleAchievements;
	});
	// todo remove once these are handled by the controller/model
	$scope.Math = window.Math;

	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
});

app.controller('NavbarController', function ($scope, $location) {
	$scope.getClass = function (path) {
		if ($location.path().substr(0, path.length) == path) {
			return true
		} else {
			return false;
		}
	}
});

// From http://jsfiddle.net/2ZzZB/56/
app.filter('startFrom', function () {
	return function (input, start) {
		start = +start; //parse to int
		return input.slice(start);
	}
});