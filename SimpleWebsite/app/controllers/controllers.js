app.controller('HomeController', function ($scope) {
	// todo not sure what I want on the page yet, since we didn't have it before
});

app.controller('GamesController', function ($scope, $http, JsonCache) {
	$scope.currentPage = 0;
	$scope.pageSize = 10;
	$scope.games = [];
	$http.get('Content/json/VideoGames.json', { cache: JsonCache }).success(function (data) {
		$scope.games = data.VideoGames;
	});

	$scope.numberOfPages = function () {
		return Math.ceil($scope.games.length / $scope.pageSize);
	};
});

app.controller('ProfileController', function ($scope, $route, $http, JsonCache, $templateCache, $window) {
	var psnDataUrl = "Content/json/_psnProfile.xml.json";
	var xblDataUrl = "Content/json/_xblProfile.xml.json";

	var getPsnData = function () {
		$http.get(psnDataUrl, { cache: JsonCache }).success(function (data) {
			$scope.psnProfile = [];
			$scope.psnProfile = data.PsnProfile;
			$scope.psnPointsPercent = 100 * data.PsnProfile.Points / data.PsnProfile.PossiblePoints;
			$scope.psnTrophiesPercent = 100 * data.PsnProfile.Trophies / data.PsnProfile.PossibleTrophies;
			$scope.psnTrophiesBronzePercent = 100 * data.PsnProfile.TrophiesBronze / data.PsnProfile.PossibleTrophiesBronze;
			$scope.psnTrophiesSilverPercent = 100 * data.PsnProfile.TrophiesSilver / data.PsnProfile.PossibleTrophiesSilver;
			$scope.psnTrophiesGoldPercent = 100 * data.PsnProfile.TrophiesGold / data.PsnProfile.PossibleTrophiesGold;
			$scope.psnTrophiesPlatinumPercent = 100 * data.PsnProfile.TrophiesPlatinum / data.PsnProfile.PossibleTrophiesPlatinum;
		});
	};
	var getXblData = function () {
		$http.get(xblDataUrl, { cache: JsonCache }).success(function (data) {
			$scope.xblProfile = [];
			$scope.xblProfile = data.XblProfile;
			$scope.xblPointsPercent = 100 * data.XblProfile.GamerScore / data.XblProfile.PossibleGamerScore;
			$scope.xblAchievementsPercent = 100 * data.XblProfile.Achievements / data.XblProfile.PossibleAchievements;
		});
	};

	$scope.psnProfile = [];
	$scope.xblProfile = [];
	getPsnData();
	getXblData();
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

	//console.log($route);
	//console.log($templateCache.info());
});

app.controller('BasicsController', function ($scope, $http, JsonCache, $window) {
	$scope.psnGamesBasic = [];
	$scope.xblGamesBasic = [];
	$http.get('Content/json/_psnGamesBasic.xml.json', { cache: JsonCache }).success(function (data) {
		$scope.psnGamesBasic = data.PsnGamesBasic;
	});
	$http.get('Content/json/_xblGamesBasic.xml.json', { cache: JsonCache }).success(function (data) {
		$scope.xblGamesBasic = data.XblGamesBasic;
	});
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		if (date.length > 6) {
			var uglyDate = new Date(parseInt(date.substr(6)));
			return uglyDate;
		}
		return "";
	}
});

app.controller('PsnGameController', function ($scope, $http, JsonCache, $routeParams) {
	$scope.test = [];
	$scope.test.enabled = [];
	var gameId = ($routeParams.gameId) ? ($routeParams.gameId) : "";
	if (gameId != "") {
		$http.get('Content/json/_psnGames.xml.json', { cache: JsonCache }).success(function (data) {
			for (var i = 0; i < data.PsnGames.length; i++) {
				if (data.PsnGames[i].Id == gameId) {
					$scope.game = data.PsnGames[i];
					$scope.test.additionalPoints = 0;
					$scope.test.additionalAccomplishments = 0;
					$scope.test.newPointsPercent = $scope.game.PointsPercentage;
					$scope.test.newItemsPercent = $scope.game.TrophyPercentage;
					$scope.graphicUrl = 'Content/images/psn/' + $scope.game.Id + '.png';
					break;
				}
			}
		});
	}
	$scope.testEnabled = function (itemId) {
		if ($scope.test.enabled.indexOf(itemId) == -1) {
			return "";
		} else {
			return "enabled";
		}
	};
	$scope.testItem = function (trophy) {
		if ($scope.test.enabled.indexOf(trophy.Id) == -1) {
			$scope.test.additionalPoints += 1 * $scope.trophyTypeToPoints(trophy.Type);
			$scope.test.additionalAccomplishments += 1;
			$scope.test.enabled.push(trophy.Id);
		} else {
			$scope.test.additionalPoints -= 1 * $scope.trophyTypeToPoints(trophy.Type);
			$scope.test.additionalAccomplishments -= 1;
			$scope.test.enabled.splice($scope.test.enabled.indexOf(trophy.Id), 1);
		}
		$scope.test.newPointsPercent = Math.round(($scope.test.additionalPoints + $scope.game.EarnedPoints) / $scope.game.PossiblePoints * 100 * 1000) / 1000;
		$scope.test.newItemsPercent = Math.round(($scope.test.additionalAccomplishments + $scope.game.EarnedTrophies) / $scope.game.PossibleTrophies * 100 * 1000) / 1000;
	};
	$scope.newPoints = function () {
		if ($scope.test.additionalPoints == 0) {
			return "";
		} else {
			return "(+" + $scope.test.additionalPoints + ")";
		}
	};
	$scope.newItems = function () {
		if ($scope.test.additionalAccomplishments == 0) {
			return "";
		} else {
			return "(+" + $scope.test.additionalAccomplishments + ")";
		}
	};
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		if (date != null) {
			var uglyDate = new Date(parseInt(date.substr(6)));
			return uglyDate;
		}
		return "";
	};
	$scope.trophyTypeToPoints = function (trophyType) {
		switch (trophyType) {
			case "BRONZE":
				return 15;
			case "SILVER":
				return 30;
			case "GOLD":
				return 90;
			case "PLATINUM":
				return 180;
			default:
				return 0;
		}
	};
	$scope.wasEarned = function (accomplishment) {
		return accomplishment.Earned != null;
	};
	$scope.notEarned = function (accomplishment) {
		return accomplishment.Earned == null;
	};
});

app.controller('XblGameController', function ($scope, $http, JsonCache, $routeParams) {
	$scope.test = [];
	$scope.test.enabled = [];
	var gameId = ($routeParams.gameId) ? ($routeParams.gameId) : "";
	if (gameId != "") {
		$http.get('Content/json/_xblGames.xml.json', { cache: JsonCache }).success(function (data) {
			for (var i = 0; i < data.XblGames.length; i++) {
				if (data.XblGames[i].Id == gameId) {
					$scope.game = data.XblGames[i];
					$scope.test.additionalPoints = 0;
					$scope.test.additionalAccomplishments = 0;
					$scope.test.newPointsPercent = $scope.game.GamerscorePercentage;
					$scope.test.newItemsPercent = $scope.game.AchievementPercentage;
					$scope.graphicUrl = 'Content/images/xbl/' + $scope.game.Id + '.jpg';
					break;
				}
			}
		});
	}
	$scope.testEnabled = function (itemId) {
		if ($scope.test.enabled.indexOf(itemId) == -1) {
			return "";
		} else {
			return "enabled";
		}
	};
	$scope.testItem = function (achievement) {
		if ($scope.test.enabled.indexOf(achievement.Id) == -1) {
			$scope.test.additionalPoints += 1 * achievement.GamerScore;
			$scope.test.additionalAccomplishments += 1;
			$scope.test.enabled.push(achievement.Id);
		} else {
			$scope.test.additionalPoints -= 1 * achievement.GamerScore;
			$scope.test.additionalAccomplishments -= 1;
			$scope.test.enabled.splice($scope.test.enabled.indexOf(achievement.Id), 1);
		}
		$scope.test.newPointsPercent = Math.round(($scope.test.additionalPoints + $scope.game.EarnedGamerScore) / $scope.game.PossibleGamerScore * 100 * 1000) / 1000;
		$scope.test.newItemsPercent = Math.round(($scope.test.additionalAccomplishments + $scope.game.EarnedAchievements) / $scope.game.PossibleAchievements * 100 * 1000) / 1000;
	};
	$scope.newPoints = function () {
		if ($scope.test.additionalPoints == 0) {
			return "";
		} else {
			return "(+" + $scope.test.additionalPoints + ")";
		}
	};
	$scope.newItems = function () {
		if ($scope.test.additionalAccomplishments == 0) {
			return "";
		} else {
			return "(+" + $scope.test.additionalAccomplishments + ")";
		}
	};
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.extractDate = function (date) {
		if (date != null) {
			var uglyDate = new Date(parseInt(date.substr(6)));
			return uglyDate;
		}
		return "";
	}
	$scope.secretTitle = function (accomplishment) {
		return accomplishment.Title.length > 0 ? accomplishment.Title : "Secret achievement";
	};
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
	};
});

// From http://jsfiddle.net/2ZzZB/56/
app.filter('startFrom', function () {
	return function (input, start) {
		start = +start; //parse to int
		return input.slice(start);
	};
});