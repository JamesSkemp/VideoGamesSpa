app.controller('HomeController', function ($scope) {
	// todo not sure what I want on the page yet, since we didn't have it before
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
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.quantityToPercent = function (obtained, total, percent) {
		if (total * percent / 100 <= obtained) {
			return "";
		} else {
			//50% = {{Math.round(xblProfile.PossibleAchievements * .50)}} (56)
			var numberAtPercent = $window.Math.round(total * percent / 100);
			return percent + "% = " + numberAtPercent + " (" + (numberAtPercent - obtained) + ")";
		}
	};

	// todo remove once I figure out what I'm doing with # of bronze trophies to percent display
	$scope.Math = window.Math;
});

app.controller('BasicsController', function ($scope, $http, $window) {
	$scope.psnGamesBasic = [];
	$scope.xblGamesBasic = [];
	$http.get('Content/json/_psnGamesBasic.xml.json').success(function (data) {
		$scope.psnGamesBasic = data.PsnGamesBasic;
	});
	$http.get('Content/json/_xblGamesBasic.xml.json').success(function (data) {
		$scope.xblGamesBasic = data.XblGamesBasic;
	});
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		var uglyDate = new Date(parseInt(date.substr(6)));
		return uglyDate;
	}
});

app.controller('PsnGameController', function ($scope, $http, $routeParams) {
	var gameId = ($routeParams.gameId) ? ($routeParams.gameId) : "";
	if (gameId != "") {
		$http.get('Content/json/_psnGames.xml.json').success(function (data) {
			for (var i = 0; i < data.PsnGames.length; i++) {
				if (data.PsnGames[i].Id == gameId) {
					$scope.game = data.PsnGames[i];
					break;
				}
			}
		});
	}
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		var uglyDate = new Date(parseInt(date.substr(6)));
		return uglyDate;
	}
	$scope.wasEarned = function (accomplishment) {
		return accomplishment.Earned != null;
	};
	$scope.notEarned = function (accomplishment) {
		return accomplishment.Earned == null;
	};
});

app.controller('XblGameController', function ($scope, $http, $routeParams) {
	var gameId = ($routeParams.gameId) ? ($routeParams.gameId) : "";
	if (gameId != "") {
		$http.get('Content/json/_xblGames.xml.json').success(function (data) {
			for (var i = 0; i < data.XblGames.length; i++) {
				if (data.XblGames[i].Id == gameId) {
					$scope.game = data.XblGames[i];
					break;
				}
			}
		});
	}
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		var uglyDate = new Date(parseInt(date.substr(6)));
		return uglyDate;
	}
	$scope.wasEarned = function (accomplishment) {
		return accomplishment.Earned != null;
	};
	$scope.notEarned = function (accomplishment) {
		return accomplishment.Earned == null;
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