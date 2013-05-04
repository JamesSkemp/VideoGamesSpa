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
		.when('/basics', {
			//controller: 'BasicsController',
			templateUrl: 'app/partials/basics.html'
		})
		.when('/psnGame/:gameId', {
			//controller: 'PsnGameController',
			templateUrl: 'app/partials/psnGame.html'
		})
		.when('/xblGame/:gameId', {
			//controller: 'XblGameController',
			templateUrl: 'app/partials/xblGame.html'
		})
		.otherwise({ redirectTo: '/home' });
});
app.factory('JsonCache', function ($cacheFactory) {
	return $cacheFactory('jsonCache');
});