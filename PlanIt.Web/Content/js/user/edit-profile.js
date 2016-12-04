$('#save').click(function () {
    var form = $("#editProfileForm");
    var url = form.attr("action");
    var formData = form.serialize();
    $.post(url, formData, function (data) {
        $("#msg").html(data);
    });
})