"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function () {
    $("#notifications").load(window.location.href + " #notifications", function () {
        $("#notifications").fadeIn("3000");
    });
    $.ajax({
        url: rootPath + "/Notification/UserNotificationsPartial",
        type: "GET",
        success: function (partialView) {
            $("#notificationsPartial").attr("data-content", partialView);
            var popover = $("#notificationsPartial").data("popover");
            popover.setContent();
            popover.$tip.addClass(popover.options.placement);
        }
    });
});

connection.start()
    .catch(err => console.error(err.toString()));

document.getElementById("submit").addEventListener("click", function (event) {
    var projectCode = document.getElementById("projectCode").value;
    var userRole = document.getElementById("userRole").value;
    connection.invoke("SendNotification", projectCode, userRole).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});