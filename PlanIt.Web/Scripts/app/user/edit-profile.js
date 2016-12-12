$('#save').click(function () {
    var form = $("#editProfileForm");
    var url = form.attr("action");
    var formData = form.serialize();
    $.post(url, formData, function (data) {
        $("#msg").html(data);
    });
});

$(function () {
    jQuery.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[a-zA-Zа-яА-ЯіІїЇєЄґҐ]+$/i.test(value);
    });
    $("#editProfileForm").validate({
        rules: {
            FirstName: {
                required:false,
                lettersonly: true
            },
            LastName: {
                required: false,
                lettersonly: true
            },
            PhoneNumber: {
                digits: true,
                required: false,
                minlength: 12,
                maxlength: 12
            }
        },
        messages: {
            FirstName: {
                lettersonly: "Only letters required"
            },
            LastName: {
                lettersonly: "Only letters required"
            },
            PhoneNumber: {
                digits: "Only digits required",
                minlength: "Incorrect number format (ex: 38 098 765 43 21)",
                maxlength: "Incorrect number format (ex: 38 098 765 43 21)"
            }
        },
        submitHandler: function (form) {
            form.submit();
        },
        errorClass: 'field-validation-error',
        highlight: function (element) {
            $(element).addClass('input-validation-error');
        },
        unhighlight: function (element) {
            $(element).removeClass('input-validation-error');
        },
    });
});