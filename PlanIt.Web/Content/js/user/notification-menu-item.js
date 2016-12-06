$(document).ready(function () {
    $('#noti_Counter')
        .css({ opacity: 0 })
        .text('7')
        .css({ top: '10px', right: '10px' })
        .animate({ top: '10px', opacity: 1 }, 500);

    $('#noti_Button').click(function () {
        $('#noti_Counter').fadeOut('slow');
    });
    if ($('#noti_Counter').is(':hidden')) {
    }
});