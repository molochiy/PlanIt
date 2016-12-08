$('#chooseUserAutocomplete')
    .keyup(function () {
        var inputField = $('#chooseUserAutocomplete');
        var partOfEmail = inputField[0].value;
            $.ajax({
                url: '/UserInteractions/GetUsersByPartOfEmailsExceptCurrentUser?partOfEmail=' + partOfEmail,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    inputField.autocomplete({
                        source: data,
                        appendTo: ".modal-body"
                    })
                }
            });
    });

/*
$('#chooseUserAutocomplete')
    .keyup(function () {
        var inputField = $('#chooseUserAutocomplete');
        var partOfEmail = inputField[0].value;
        if (partOfEmail.length > 4) {
            $.ajax({
                url: '/UserInteractions/GetUsersByPartOfEmailsExceptCurrentUser?partOfEmail=' + partOfEmail,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    inputField.autocomplete({
                        source: data,
                        appendTo: ".modal-body"
                    })
                }
            });
        }
    });
*/