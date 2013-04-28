﻿app.controller('HomeController', function ($scope) {
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

app.controller('ProfileController', function ($scope) {
	init();

	function init() {
		$scope.profile = '';
	}
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