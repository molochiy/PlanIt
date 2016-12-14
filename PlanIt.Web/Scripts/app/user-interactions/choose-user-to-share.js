function chooseUserAutocomplete(planId) {
    var inputField = $('#chooseUserAutocomplete-' + planId);
    var partOfEmail = inputField.val();
    $.ajax({
        url: '/UserInteractions/GetUsersByPartOfEmailsExceptCurrentUser?partOfEmail=' + partOfEmail,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            inputField.autocomplete({
                source: data,
                appendTo: ".modal-body"
            });
        }
    });
}