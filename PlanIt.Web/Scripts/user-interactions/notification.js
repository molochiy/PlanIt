$(function () {
    // Initialize the connection to the server
    var realtimeNotifier = $.connection.realtimeNotifierHub;

    // Preparing a client side function 
    // called sendMessage that will be called from the server side
    realtimeNotifier.client.sendMessage = function (message) {
        showOrUpdateSuccessMessage(message, false);
    };

    // Establish the connection to the server. When done, sets the click of the button
    $.connection.hub.start().done(function () {
        $('#sharePlanBtn').click(function () {
            // When the button is clicked, 
            // call the method DoLongOperation defined in the Hub
            realtimeNotifier.server.doLongOperation();
        });
    });
});

// Helper code that updates the noty notification bar
var n;
function showOrUpdateSuccessMessage(message, timeout) {
    if (n == null) {
        n = noty({ text: message, type: 'success', timeout: timeout, maxVisible: 1 });
    }
    else {
        n.setText(message);
    }
}