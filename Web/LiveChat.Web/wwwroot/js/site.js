let connection = null;

setupConnection = () => {
    connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

    connection.on("NewMessage",
        function (message) {
            var chatInfo = `<div id=${message.id}>[${message.user}] ${escapeHtml(message.text)} [${message.dateOfMessage}]
                                <button class="btn btn-primary delete">Delete Message </button>
                                </div>`;
            $("#messagesList").append(chatInfo);
        });

    connection.on("Notification",
        function (message, username) {
            alert(`You Have new message: ${message} from: ${username}`);;
        });

    connection.on("UsersList",
        function (user) {
            var currentUser = `<div>[UserName: ][${user}]</div>`;
            $("#notificationsList").append(currentUser);
        });

    $("#sendButton").click(function () {
        var message = $("#messageInput").val();
        fetch("/Chat/Notification",
            {
                method: "POST",
                body: JSON.stringify(message),
                headers: {
                    'content-type': 'application/json'
                }
            }).then(connection.invoke("Send", message));

        $("#messageInput").val("");
    });

    $("#delete").click(function (e) {
        var messageId = this.parentElement.id;
        connection.invoke("DeleteMessage", messageId);
        $("#messageId").remove();
    });

    $("#loadHistoryButton").click(function () {
        connection.invoke("LoadHistory");
    });

    $("#getAllParticipants").click(function () {
        connection.invoke("LoadParticipants");
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
};

setupConnection();