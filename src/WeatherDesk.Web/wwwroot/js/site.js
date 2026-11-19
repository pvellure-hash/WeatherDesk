(function () {
  'use strict';

  if (!window.jQuery) {
    document.getElementById('libwarn').hidden = false;
    return;
  }
  var $ = window.jQuery;

  function errorText(xhr) {
    if (xhr && xhr.responseJSON && xhr.responseJSON.error) {
      return xhr.responseJSON.error;
    }
    if (xhr && xhr.responseJSON && xhr.responseJSON.detail) {
      return xhr.responseJSON.detail;
    }
    return 'Something went wrong. Please try again.';
  }

  function loadRecent() {
    $.getJSON('/api/recent').done(function (items) {
      var list = $('#recent').empty();
      $.each(items, function (_, name) {
        list.append($('<li>').text(name));
      });
      $('#recentEmpty').toggle(items.length === 0);
    });
  }

  function showWeather(w) {
    $('#lookupError').hide();
    $('#rCity').text(w.city + (w.country ? ', ' + w.country : ''));
    $('#rTemp').text(w.temperatureC + ' \u00B0C / ' + w.temperatureF + ' \u00B0F');
    $('#rWind').text(w.windKmh + ' mph (' + w.windDescription + ')');
    $('#rComfort').text(w.comfort);
    $('#rObserved').text(w.observedAt);
    $('#result').show();
  }

  function lookup() {
    var params = { city: $('#city').val() };
    var country = $('#country').val();
    if (country) {
      params.country = country;
    }
    $.getJSON('/api/weather', params)
      .done(function (w) {
        showWeather(w);
        loadRecent();
      })
      .fail(function (xhr) {
        $('#result').hide();
        $('#lookupError').text(errorText(xhr)).show();
      });
  }

  function compare() {
    $.getJSON('/api/compare', { cities: $('#cities').val() })
      .done(function (c) {
        $('#compareError').hide();
        $('#cWarmest').text(c.warmest.city + ' (' + c.warmest.temperatureC + ' \u00B0C)');
        $('#cColdest').text(c.coldest.city + ' (' + c.coldest.temperatureC + ' \u00B0C)');
        $('#cAverage').text(c.averageC + ' \u00B0C across ' + c.cityCount + ' cities');
        $('#compareResult').show();
        loadRecent();
      })
      .fail(function (xhr) {
        $('#compareResult').hide();
        $('#compareError').text(errorText(xhr)).show();
      });
  }

  if ($.fn.validate) {
    $('#lookupForm').validate({
      rules: { city: { required: true, minlength: 2 } },
      messages: { city: { required: 'Please enter a city name.', minlength: 'At least 2 characters.' } },
      submitHandler: function () { lookup(); }
    });
  } else {
    $('#lookupForm').on('submit', function (e) { e.preventDefault(); lookup(); });
  }

  $('#compareForm').on('submit', function (e) {
    e.preventDefault();
    compare();
  });

  loadRecent();
})();
