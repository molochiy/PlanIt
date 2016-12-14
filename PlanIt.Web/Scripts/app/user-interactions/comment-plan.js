$(document).ready(function () {
    var notificationHub = $.connection.notificationHub;

    notificationHub.client.addNewCommentToList = function (data) {
        addNewCommentToList(data);
    }
});

function commentPlan(planId) {
    var commentTextElementId = "#new-comment-text-" + planId;
    var commentTextElement = $(commentTextElementId);

    var commentText = commentTextElement.val();

    if (commentText === "") {
        alert("Plesea write a comment!");
        commentTextElement.focus();
    } else {
        var commentData = {
            text: commentText,
            planId: planId
        };

        $.ajax({
            type: "POST",
            url: "/Plan/CommentPlan/",
            data: commentData,
            success: function (data) {
                if (data && !data.success) {
                    noty({ text: data.message, layout: 'topCenter', type: 'error', timeout: 5000, maxVisible: 1 });
                }
                commentTextElement.val("");
                $('#no-comment').remove();
            }
        });
    };
};

function addNewCommentToList(data) {
    var listCommentsElementId = "#list-comments-" + data.Data.PlanId;
    var listCommentsElement = $(listCommentsElementId);

    var newCommentHtml = '<div class="panel panel-info"> ' +
        '<div class="panel-heading"> ' +
        data.Data.UserEmail +
        '--' +
        data.Data.CreatedTime +
        '</div> ' +
        '<div class="panel-body"> ' +
        data.Data.Text +
        '</div>' +
        '</div>';

    listCommentsElement.append(newCommentHtml);
};