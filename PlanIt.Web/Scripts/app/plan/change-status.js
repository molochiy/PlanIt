function changePlanStatus(planId) {
    var wellClassNotStarted = 'well';
    var wellClassDone = 'well status-done';
    var wellClassInProgress = 'well status-in-progress';

    var planPanelClassNotStarted = 'panel-heading plan-heading';
    var planPanelClassDone = 'panel-heading status-done plan-heading';
    var planPanelClassInProgress = 'panel-heading status-in-progress plan-heading';

    var url = '/Plan/ShangeStatus/';
    var post = {};
    var checkBoxId = '#plan-checkbox-' + planId;
    var wellId = '#well-' + planId;
    var planPanelId = '#plan-panel-' + planId;
    var planBeginId = '#plan-begin-' + planId;

    if ($(checkBoxId)[0].checked) {
        $(wellId).attr('class', wellClassDone);
        $(planPanelId).attr('class', planPanelClassDone);
    } else {

        var planBeginPartOfDate = $(planBeginId).val().split('/');
        var planBegin = planBeginPartOfDate[1] + '/' + planBeginPartOfDate[0] + '/' + planBeginPartOfDate[2];
        var planBeginDate = new Date(planBegin);
        var now = new Date(Date.now());
        if (!planBeginDate || planBeginDate < now) {
            $(wellId).attr('class', wellClassInProgress);
            $(planPanelId).attr('class', planPanelClassInProgress);
        } else {
            $(wellId).attr('class', wellClassNotStarted);
            $(planPanelId).attr('class', planPanelClassNotStarted);
        }
    }
}