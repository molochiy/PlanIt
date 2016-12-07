$(document).ready(function updateCountNotifications() {
    $.ajax({
        url: '/UserInteractions/GetNumberOfNotifications',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data && data.numberOfNotification > 0) {
                $('#noti_Counter')
                    .css({ opacity: 0 })
                    .text(data.numberOfNotification)
                    .css({ top: '10px', right: '10px' })
                    .animate({ top: '10px', opacity: 1 }, 500);
            } else {
                $('#noti_Counter').remove();
            }
            setTimeout(updateCountNotifications, 5000);
        }
    });
});