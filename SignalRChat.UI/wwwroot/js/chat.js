$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    var messageCount = 0;
    var messageLimit = 0;

    $("#sendButton").disabled = true;

    connection.on("SetUsers", function (usersList) {
        $("#loggedUsers").html("");
        usersList.forEach(function (item) {
            $("#loggedUsers").append($("<li>").text(item));
        })
    })

    connection.on("SetMessageLimit", function (limit) {
        messageLimit = limit;
    })

    connection.start().then(function () {
        $("#sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveMessage", function (user, message) {
        messageCount++;

        setMessage(user, message);

        if (messageCount > messageLimit)
            $("#messagesList > .chat-entry").first().remove();
    });

    $(document).on("click", "#sendButton" ,function (event) {
        var message = $("#messageInput").val();
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });

    function setMessage(user, message) {
        var container = $("<div>").addClass("chat-entry");
        container.append($("<span>").addClass("user").text(user));
        container.append($("<span>").text(message));

        $("#messagesList").append(container);
        $("#messagesList").animate({ scrollTop: $('#messagesList').prop("scrollHeight") }, 1000);
    }
});