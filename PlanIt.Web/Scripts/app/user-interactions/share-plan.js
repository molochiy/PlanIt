$('#sharePlanBtn').click(function () {
    var url = '/UserInteractions/SharePlan/';
    var post = {};
    if ($('#chooseUserAutocomplete').val() === "") {
        if ($("#chooseUserAutocomplete").prev(".validation").length === 0) {
            $("#chooseUserAutocomplete").before("<div class='validation' style='color:red; margin-botton: 2px;'>Please enter existing user email or choose from list: </div>");
        }
    }
    else {
        $("#chooseUserAutocomplete").prev(".validation").remove();
        $("#chooseUserAutocomplete").attr("data-dismiss", "modal");

        post.planId = $('#planId').val();
        post.toUserEmail = $('#chooseUserAutocomplete').val();
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(post),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                noty({ text: data.message, layout: 'topCenter', type: 'success', timeout: 5000, maxVisible: 1 });
            }
        });
    }
})