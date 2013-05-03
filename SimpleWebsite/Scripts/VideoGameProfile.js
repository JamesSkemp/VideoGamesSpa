(function ($) {
	// Enable functionality for profile header.
	$.fn.miniProfileHeader = function () {
		var profileHeader = $('#profileHeader');
		if (profileHeader.length > 0) {
			$.ajax({
				url: '/Projects/VideoGames/MiniProfile'
			}).done(function (data) {
				profileHeader.html(data).slideDown('slow');
			});
		}
	};
})(jQuery);