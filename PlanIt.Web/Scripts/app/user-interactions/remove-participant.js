function removeParticipant(email, planId) {
    var url = '/UserInteractions/RemoveParticipant/';
    var post = {};
    post.participantEmail = email;
    post.planId = planId;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(post),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data && data.success) {
                noty({ text: data.message, layout: 'topCenter', type: 'info', timeout: 5000, maxVisible: 1 });
            } else {
                noty({ text: data.message, layout: 'topCenter', type: 'error', timeout: 5000, maxVisible: 1 });
            }
        }
    });
    getParticipants(planId);
}