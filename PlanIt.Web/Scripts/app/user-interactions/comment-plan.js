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
    var listCommentsElementId = "#list-comments-" + data.Data.PlanId + "> .chat";
    var listCommentsElement = $(listCommentsElementId);
    var name = usrName;
    var newCommentHtml;
    if (data.Data.UserEmail && data.Data.UserEmail == usrName) {
        newCommentHtml = '<li class="right clearfix">' +
                                    '<span class="chat-img pull-right">' +
                                        '<img src="~/Content/img/me.png" alt="User Avatar" class="img-circle" />' +
                                    '</span>' +
                                    '<div class="chat-body clearfix">' +
                                        '<div class="header">' +
                                            '<small class="text-muted"><span class="glyphicon glyphicon-time"></span>' + data.Data.CreatedTime + '</small>' +
                                            '<strong class="pull-right primary-font">' + data.Data.UserEmail + '</strong>' +
                                        '</div>' +
                                        '<p>' +
                                            data.Data.Text +
                                        '</p>' +
                                    '</div>' +
                                '</li>';
    } else {
        newCommentHtml = '<li class="left clearfix">' +
                                    '<span class="chat-img pull-left">' +
                                        '<img src="~/Content/img/user.png" alt="User Avatar" class="img-circle" />' +
                                    '</span>' +
                                    '<div class="chat-body clearfix">' +
                                        '<div class="header">' +
                                            '<strong class="primary-font">' + data.Data.UserEmail + '</strong> <small class="pull-right text-muted">' +
                                                '<span class="glyphicon glyphicon-time"></span>' + data.Data.CreatedTime +
                                            '</small>' +
                                        '</div>' +
                                        '<p>' +
                                            data.Data.Text +
                                        '</p>' +
                                    '</div>' +
                                '</li>';
    }

    listCommentsElement.append(newCommentHtml);
};