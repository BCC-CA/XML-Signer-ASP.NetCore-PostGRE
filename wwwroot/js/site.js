// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

var temp;
$(document).ready(function () {
    $.support.cors = true;

    $(".sign_file").click(function () {
        var id = $(this).attr('file_id');

        var token = $.ajax({
            type: "GET",
            url: "/api/XmlFiles/token/" + id,
            async: false
        }).responseText;
        var baseUrl = window.location.protocol + "//" + window.location.host + "/";

        $.ajax({
            type: "POST",
            crossdomain: true,
            //contentType: "application/json; charset=utf-8",
            contentType: 'text/plain',
            accepts: 'application/json',
            url: "http://localhost:5050/",
            dataType: 'jsonp',
            async: false,
            data: {
                id: id,
                token: token,
                downloadUrl: baseUrl + "api/XmlFiles/" + token + "/" + id,
                uploadUrl: baseUrl + "api/XmlFiles"
            },
            success: function (data) {
                alert(data);
                console.log(data);
            },
            error: function(xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                console.log(ajaxOptions);
                console.log(thrownError);
            }
        });
    });
});
