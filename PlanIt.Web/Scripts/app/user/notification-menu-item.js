﻿$(document).ready(function updateCountNotifications() {
    $.ajax({
        url: '/UserInteractions/GetNumberOfNotifications',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data && data.numberOfNotification > 0) {
                $('#noti_Counter').text(data.numberOfNotification).css('opacity', '1');

                $('#noti_Counter :hidden').show();
            } else {
                $('#noti_Counter :visible').hide();
            }
            setTimeout(updateCountNotifications, 5000);
        }
    });
});