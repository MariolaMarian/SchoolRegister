"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").configureLogging(signalR.LogLevel.Information).build();

async function start() {
    try {
        await connection.start();
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();

function encodeText(text) {
    if (text !== undefined && text !== null) {
        text = text.replace(/&/g, "&apm;").replace(/</g, "&gt;");
        const encodedHtml = $("<div/>").text(text).html();
        return encodedHtml;
    }
    return "";
}

connection.on("ShowMessage", (message) => {
    const li = $("<li>");
    li.html(encodeText(message.authorName) + " : " + encodeText(message.content));
    $("#discussion").append(li);
});

$("#sendMessage").click(function (event) {
    event.preventDefault();
    if ($("#message").val() === undefined || $("#message").val() === "") {
        alert("Type a message");
        return;
    }
    const message = {
        Content: $("#message").val(),
        RecipientName: $("#typeChoice select").val() === "User" ? $("#users select option:selected").text() : $("#groups select option:selected").text(),
        RecipientId: $("#typeChoice select").val() === "User" ? $("#users select option:selected").val() : $("#groups select option:selected").val()
    };

    if ($("#typeChoice select").val() === "User") {
        if (message.RecipientName.trim() == "All students".trim()) {
            connection.invoke("SendMessageToEveryUser", message);
        } else {
            connection.invoke("SendMessageToSingleUser", message);
        }
    } else {
        connection.invoke("SendMessageToGroup", message);
    }
    $("#message").val("").focus();
});

function hideOnLoad() {
    $("#usersDiv").show();
    $("#chatGroupDiv").hide();
}

$(document).ready(function () {
    hideOnLoad();
    $("#typeChoice select").change(function () {
        var value = $(this).val();
        if (value == "User") {
            hideOnLoad();
        } else {
            $("#usersDiv").hide();
            $("#chatGroupDiv").show();
        }
    })
});