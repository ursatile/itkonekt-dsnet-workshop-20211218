// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(connectToSignalR);

function connectToSignalR() {
    console.log("Connecting to SignalR");
    var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    connection.on("DisplayAutobarnNotification", displayNotification);
    connection.start()
        .then(function () {
            console.log("SignalR Connected! Hooray!");
        })
        .catch(function (ex) {
            console.log(ex);
        });
}

function displayNotification(user, json) {
    let $notificationsDiv = $("#signalr-notifications");
    var data = JSON.parse(json);
    var $div = $(`<div>New car alert! 
        ${data.manufacturerName} ${data.modelName} (${data.color}, ${data.year}). 
        Price ${data.price} ${data.currencyCode}.
        <a href="/vehicles/details/${data.registration}">Click here for more...</a></div>`);
    $notificationsDiv.prepend($div);
    window.setTimeout(function () { $div.fadeOut(2000, function () { $div.remove(); }) }, 5000);
}