function sharePlan(planId) {
    var url = '/UserInteractions/SharePlan/';
    var post = {};
    var chooseUserAutocompleteId = "#chooseUserAutocomplete-" + planId;
    if ($(chooseUserAutocompleteId).val() === "") {
        if ($(chooseUserAutocompleteId).prev(".validation").length === 0) {
            $(chooseUserAutocompleteId).before("<div class='validation' style='color:red; margin-botton: 2px;'>Please enter existing user email or choose from list: </div>");
        }
    }
    else {
        $(chooseUserAutocompleteId).prev(".validation").remove();
        $(chooseUserAutocompleteId).attr("data-dismiss", "modal");

        post.planId = planId;
        post.toUserEmail = $(chooseUserAutocompleteId).val();
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(post),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data && data.success) {
                    noty({ text: data.message, layout: 'topCenter', type: 'success', timeout: 5000, maxVisible: 1 });
                } else {
                    noty({ text: data.message, layout: 'topCenter', type: 'error', timeout: 5000, maxVisible: 1 });
                }
            }
        });
    }
}