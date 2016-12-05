$('#sharePlanBtn').click(function () {
    var url = '/Plan/AddPlan/';
    var post = {};
    post.title = $('#Title').val();
    post.description = $('#Description').val();
    post.startDate = $('#StartDate').val();
    post.endDate = $('#EndDate').val();
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(post),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
        }
    })
})
