app.controller('HomeController', function ($scope) {
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
