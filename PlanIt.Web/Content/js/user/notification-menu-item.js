$(document).ready(function () {
    $.ajax({
        url: '/UserInteractions/GetNumberOfIncommingPlansWithPendingStatus',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data)
            if (data[0] > 0)
            {
                $('#noti_Counter')
                    .css({ opacity: 0 })
                    .text(data)
                    .css({ top: '10px', right: '10px' })
                    .animate({ top: '10px', opacity: 1 }, 500);
            }
        }
    });

    $('#noti_Button').click(function () {
        $('#noti_Counter').fadeOut('slow');
    });
    if ($('#noti_Counter').is(':hidden')) {
    }
});