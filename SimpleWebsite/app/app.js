var app = angular.module('videoGamesApp', []);
// Configure routes and controllers and views associated with them.
app.config(function ($routeProvider) {
	$routeProvider
		.when('/home', {
			controller: 'HomeController',
			templateUrl: 'app/partials/home.html'
		})
		.when('/games', {
			//controller: 'GamesController',
			templateUrl: 'app/partials/games.html'
		})
		.when('/profile', {
			//controller: 'ProfileController',
			templateUrl: 'app/partials/profile.html'
		})
		.when('/basics/all', {
			//controller: 'BasicsController',
			templateUrl: 'app/partials/basics/all.html'
		})
		.when('/basics/psn', {
			//controller: 'BasicsController',
			templateUrl: 'app/partials/basics/psn.html'
		})
		.when('/basics/xbl', {
			//controller: 'BasicsController',
			templateUrl: 'app/partials/basics/xbl.html'
		})
		.when('/psnGame/:gameId', {
			//controller: 'PsnGameController',
			templateUrl: 'app/partials/psnGame.html'
		})
		.when('/xblGame/:gameId', {
			//controller: 'XblGameController',
			templateUrl: 'app/partials/xblGame.html'
		})
		.when('/stats', {
			templateUrl: 'app/partials/stats/home.html'
		})
		.when('/stats/date', {
			templateUrl: 'app/partials/stats/date.html'
		})
		.when('/stats/dayOfWeek', {
			templateUrl: 'app/partials/stats/dayOfWeek.html'
		})
		.when('/stats/hour', {
			templateUrl: 'app/partials/stats/hour.html'
		})
		.when('/stats/type', {
			templateUrl: 'app/partials/stats/type.html'
		})
		.when('/stats/yearMonth', {
			templateUrl: 'app/partials/stats/yearMonth.html'
		})
		.when('/admin', {
			templateUrl: 'app/partials/admin/home.html'
		})
		.otherwise({ redirectTo: '/home' });
});
app.factory('JsonCache', function ($cacheFactory) {
	return $cacheFactory('jsonCache');
});