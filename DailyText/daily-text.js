$(function () {
    //$('#header-text').text('Daily Text');

    $.getJSON("http://assignit.apphb.com/DailyText/Json", null, function (result) {
        var textDate = $('#text-date');
        var textheader = $('#text-header');
        var textBody = $('#text-body');

        textDate.append(result.DateLine);
        textheader.append("<i>" + result.Header + "</i>");
        textBody.append(result.Body);

    });
});