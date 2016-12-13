$(document)
    .ready(function () {
        function commentPlan(planId) {
            var commentTextElementId = "#new-comment-text-" + planId;
            var commentTextElement = $(commentTextElementId);

            var commentText = commentTextElement.val();

            if (commentText == "") {
                alert("Plesea write a comment!");
                commentTextElement.focus();
            } else {
                var data = {
                    text: commentText,
                    planId: planId
                };

                $.ajax({
                    type: "POST",
                    url: "/Plan/CommentPlan/",
                    data: data,
                    success: function (commentData) {
                        addNewCommentToList(commentData);
                        commentTextElement.html("");
                    }
                });
                return false;
            }
        }

        function addNewCommentToList(data) {
            var listCommentsElementId = "#list-comments-" + data.PlanId;
            var listCommentsElement = $(listCommentsElementId);

            var newCommentHtml = '<div class="panel panel-info"> ' +
                '<div class="panel-heading"> ' +
                data.UserEmail +
                '--' +
                data.CreatedTime +
                '</div> ' +
                '<div class="panel-body"> ' +
                data.Text +
                '</div>' +
                '</div>';

            listCommentsElement.append(newCommentHtml);
        }

        var notificationHub = $.connection.notificationHub;

        $.connection.hub.start()
            .done(function () {
                console.log('Notification hub started');
            });

        notificationHub.client.addNewCommentToList = function (data) {
            addNewCommentToList(data);
        }
    });