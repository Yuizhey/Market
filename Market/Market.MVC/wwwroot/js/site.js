const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationhub")
    .build();

connection.on("ReceiveBroadcast", function (message) {
    showToast(`[Broadcast] ${message}`);
});

connection.on("ReceiveByRole", function (message) {
    showToast(`[Role message] ${message}`);
});

connection.on("ReceiveMessage", function (message) {
    showToast(`[Personal message] ${message}`);
});

connection.on("ReceiveUnauthenticated", function (message) {
    showToast(`[Guest message] ${message}`);
});

connection.start().then(function () {
    console.log("Connected to SignalR");
}).catch(function (err) {
    console.error(err.toString());
});

function showToast(message) {
    const toast = document.createElement("div");
    toast.textContent = message;
    toast.style.cssText = `
        background-color: #333;
        color: white;
        padding: 12px 20px;
        margin-top: 10px;
        border-radius: 4px;
        box-shadow: 0 2px 6px rgba(0,0,0,0.3);
        animation: fadein 0.5s, fadeout 0.5s 3s;
        max-width: 300px;
        word-wrap: break-word;
    `;

    document.getElementById("toast-container").appendChild(toast);

    setTimeout(() => toast.remove(), 5000);
}
