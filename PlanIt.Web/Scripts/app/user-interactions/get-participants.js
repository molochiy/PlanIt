function getParticipants(planId) {
    var dropdown = $('#getParticipantsDropDown-' + planId);
    dropdown.html('');
    $.getJSON('/UserInteractions/GetReceiversEmailsOfCurrentPlan?planId=' + planId, function (data) {
        $.each(data, function (index, item) {
            dropdown.append($('<li role="presentation"><a href="#" onclick="removeParticipant(&quot;' + item + '&quot;,' + planId + ')">' + item + '</a></li>'));
        });
    });
}