$(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        var query = $('#query').val();
        $('tbody').load('/Categories/Search?query=' + query);
    });

});