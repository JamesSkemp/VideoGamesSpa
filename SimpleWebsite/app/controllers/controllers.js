app.controller('HomeController', function ($scope) {
	// todo not sure what I want on the page yet, since we didn't have it before
});

app.controller('GamesController', function ($scope, $http, JsonCache) {
	$scope.currentPage = 0;
	$scope.pageSize = 10;
	$scope.games = [];
	$scope.consoles = [];
	$scope.purchasePlaces = [];
	$http.get('Content/json/VideoGames.json', { cache: JsonCache }).success(function (data) {
		$scope.games = data.VideoGames;

		for (var g = 0; g < data.VideoGames.length; g++) {
			var console = data.VideoGames[g].System;
			if ($scope.consoles.indexOf(console) == -1) {
				$scope.consoles.push(console);
			}
			var purchasePlace = data.VideoGames[g].Place;
			if (purchasePlace != '' && $scope.purchasePlaces.indexOf(purchasePlace) == -1) {
				$scope.purchasePlaces.push(purchasePlace);
			}
		}
		$scope.consoles.sort(function (a, b) {
			return a.toLowerCase().localeCompare(b.toLowerCase());
		});
		$scope.purchasePlaces.sort(function (a, b) {
			return a.toLowerCase().localeCompare(b.toLowerCase());
		});
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
			$scope.psnPointsGoal1 = $scope.percentGoal(data.PsnProfile.Points, data.PsnProfile.PossiblePoints, 1);
			$scope.psnPointsGoal2 = $scope.percentGoal(data.PsnProfile.Points, data.PsnProfile.PossiblePoints, 2);
			$scope.psnTrophiesGoal1 = $scope.percentGoal(data.PsnProfile.Trophies, data.PsnProfile.PossibleTrophies, 1);
			$scope.psnTrophiesGoal2 = $scope.percentGoal(data.PsnProfile.Trophies, data.PsnProfile.PossibleTrophies, 2);
		});
	};
	var getXblData = function () {
		$http.get(xblDataUrl, { cache: JsonCache }).success(function (data) {
			$scope.xblProfile = [];
			$scope.xblProfile = data.XblProfile;
			$scope.xblPointsPercent = 100 * data.XblProfile.GamerScore / data.XblProfile.PossibleGamerScore;
			$scope.xblAchievementsPercent = 100 * data.XblProfile.Achievements / data.XblProfile.PossibleAchievements;
			$scope.xblGamerscoreGoal1 = $scope.percentGoal(data.XblProfile.GamerScore, data.XblProfile.PossibleGamerScore, 1);
			$scope.xblGamerscoreGoal2 = $scope.percentGoal(data.XblProfile.GamerScore, data.XblProfile.PossibleGamerScore, 2);
			$scope.xblAchievementsGoal1 = $scope.percentGoal(data.XblProfile.Achievements, data.XblProfile.PossibleAchievements, 1);
			$scope.xblAchievementsGoal2 = $scope.percentGoal(data.XblProfile.Achievements, data.XblProfile.PossibleAchievements, 2);
		});
	};

	$scope.psnProfile = [];
	$scope.xblProfile = [];
	getPsnData();
	getXblData();
	$scope.percentClass = function (percent) {
		return { percent0: percent >= 0, percent25: percent >= 25, percent50: percent >= 50, percent75: percent >= 75, percent100: percent >= 100 }
	};
	$scope.quantityToPercent = function (obtained, total, percent, outputType) {
		if (total * percent / 100 <= obtained) {
			return "";
		} else {
			var numberAtPercent = Math.ceil(total * percent / 100);
			var trophyInfo = "";
			if (outputType == "psnPoints") {
				trophyInfo = $scope.trophiesToPointsGoal(numberAtPercent - obtained);
			}
			return percent + "% = " + numberAtPercent + " (" + (numberAtPercent - obtained) + trophyInfo + ")";
		}
	};
	$scope.percentGoal = function (obtained, total, goal) {
		var currentPercent = Math.floor(obtained / total * 100);
		var goalPercent = 0;
		if (goal == 1) {
			// Goal one is the next percent.
			for (var i = 1; i <= 100; i = i + 1) {
				if (currentPercent < i) {
					goalPercent = i;
					break;
				}
			}
		} else {
			// Goal two is the next percent evently divisible by 5.
			currentPercent = currentPercent + 1;
			for (var i = 5; i <= 100; i = i + 5) {
				if (currentPercent < i) {
					goalPercent = i;
					break;
				}
			}
		}
		return goalPercent;
	};
	$scope.trophiesToPointsGoal = function (goal) {
		var output = "";
		if (goal > 0) {
			var bronze = 15, silver = 30, gold = 90;
			var totalBronze = Math.round(goal / bronze);
			var totalSilver = Math.round(goal / silver);
			var totalGold = Math.round(goal / gold);
			if (totalBronze > 0) {
				output += " / " + totalBronze + " bronze";
				if (totalSilver > 0) {
					output += " | " + totalSilver + " silver";
					if (totalGold > 0) {
						output += " | " + totalGold + " gold";
					}
				}
			}
		}
		return output;
	};

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
app.controller('PsnBasicsController', function ($scope, $http, JsonCache, $window) {
	$scope.psnGamesBasic = [];
	$http.get('Content/json/_psnGamesBasic.xml.json', { cache: JsonCache }).success(function (data) {
		$scope.psnGamesBasic = data.PsnGamesBasic;
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
app.controller('XblBasicsController', function ($scope, $http, JsonCache, $window) {
	$scope.xblGamesBasic = [];
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
					$scope.platform = $scope.game.Platform.replace("psp2", "PlayStation Vita").replace("ps3", "PlayStation 3").replace("ps4", "PlayStation 4").replace(",", ", ");
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

app.controller('StatsController', function ($scope) { });

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