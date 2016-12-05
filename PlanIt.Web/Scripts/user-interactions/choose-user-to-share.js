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

$('#chooseUserAutocomplete').autocomplete({
    serviceUrl: '/Share/ChooseUser',
    onSelect: function (suggestion) {
        alert('You have shared plan with user: ' + suggestion.value + ', ' + suggestion.data);
    }
});

$('#autocomplete').autocomplete({
    paramName: 'searchString',
    transformResult: function (response) {
        return {
            suggestions: $.map(response.myData, function (dataItem) {
                return { value: dataItem.valueField, data: dataItem.dataField };
            })
        };
    }
})
