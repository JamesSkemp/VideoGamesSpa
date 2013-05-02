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

	// Enable experimentation functionality for individual games.
	$.fn.detailsExperimentation = function () {
		var accomplishmentsBlock = $('#accomplishments');
		if (accomplishmentsBlock.length > 0 && accomplishmentsBlock.find('.unearned').length > 0) {
			var unearnedItems = accomplishmentsBlock.find('.unearned li');
			var existingData = {
				earnedPoints: accomplishmentsBlock.attr('data-points') * 1,
				totalPoints: accomplishmentsBlock.attr('data-totalpoints') * 1,
				earnedItems: accomplishmentsBlock.attr('data-quantity') * 1,
				totalItems: accomplishmentsBlock.attr('data-totalquantity') * 1,
				testPoints: 0,
				testItems: 0,
				pointPercentage: function () {
					return Math.round(this.earnedPoints / this.totalPoints * 100 * 1000) / 1000;
				},
				itemPercentage: function () {
					return Math.round(this.earnedItems / this.totalItems * 100 * 1000) / 1000;
				},
				testPointPercentage: function () {
					return Math.round(this.testPoints / this.totalPoints * 100 * 1000) / 1000;
				},
				testItemPercentage: function () {
					return Math.round(this.testItems / this.totalItems * 100 * 1000) / 1000;
				}
			};

			// reset - TODO move into function
			existingData.testPoints = existingData.earnedPoints;
			existingData.testItems = existingData.earnedItems;

			//accomplishmentsBlock.append('<a href="#" id="experimentationLink">Experimentation</a>');

			//accomplishmentsBlock.on('click', '#experimentationLink', function (e) {
			accomplishmentsBlock.append('<h3>Experimentation</h3><p>Points percentage: ' + existingData.pointPercentage() + '% &gt; <span id="pointsPercentage">' + existingData.testPointPercentage() + '</span>% <span id="pointsChange"></span><br />Accomplishments: ' + existingData.itemPercentage() + '% &gt; <span id="itemsPercentage">' + existingData.testItemPercentage() + '</span>% <span id="itemsChange"></span></p>');
				//$(this).hide();
				//e.preventDefault();
				unearnedItems.each(function () {
					var unearnedItem = $(this);
					unearnedItem.on('click', function (e) {
						var accomplishment = $(this);
						if (accomplishment.hasClass('enabled')) {
							existingData.testPoints -= (accomplishment.attr('data-points') * 1);
							existingData.testItems -= 1;
						} else {
							existingData.testPoints += (accomplishment.attr('data-points') * 1);
							existingData.testItems += 1;
						}
						accomplishment.toggleClass('enabled');
						$('#pointsPercentage').html(existingData.testPointPercentage());
						var pointsChange = existingData.testPoints - existingData.earnedPoints;
						var itemsChange = existingData.testItems - existingData.earnedItems;
						if (pointsChange > 0) {
							$('#pointsChange').html("(+" + pointsChange + ")");
						} else {
							$('#pointsChange').html("");
						}
						if (itemsChange > 0) {
							$('#itemsChange').html("(+" + itemsChange + ")");
						} else {
							$('#itemsChange').html("");
						}
						$('#itemsPercentage').html(existingData.testItemPercentage());
						//console.log(existingData);
					});
				});
			//});
			//console.log(existingData);
		}
	};
})(jQuery);