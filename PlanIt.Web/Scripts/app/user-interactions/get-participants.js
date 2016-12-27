function getParticipants(planId) {
    var dropdown = $('#getParticipantsDropDown-' + planId);
    dropdown.html('');
    $.getJSON('/UserInteractions/GetReceiversEmailsOfCurrentPlan?planId=' + planId, function (data) {
        $.each(data, function (index, item) {
            dropdown.append(
                $('<li role="presentation"></li>')).append(
                $('<a role="menuitem" tabindex="-1" href="#" onclick="removeParticipant(&quot;' + item + '&quot;,' + planId + ')"></a>').val(item).html(item)).append(
                $('<span class="glyphicon glyphicon-remove pull-right"></span>'));
        });
    });
}