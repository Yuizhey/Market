const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationhub")
    .withAutomaticReconnect([0, 2000, 5000, 10000, 20000]) // Попытки переподключения через 0, 2, 5, 10 и 20 секунд
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

// Обработка состояния подключения
connection.onreconnecting((error) => {
    console.log("Переподключение к SignalR...", error);
    showToast("Переподключение к серверу...");
});

connection.onreconnected((connectionId) => {
    console.log("Переподключено к SignalR", connectionId);
    showToast("Соединение восстановлено");
});

connection.onclose((error) => {
    console.log("Соединение закрыто", error);
    showToast("Соединение потеряно");
});

// Функция для установки соединения
async function startConnection() {
    try {
        await connection.start();
        console.log("Connected to SignalR");
    } catch (err) {
        console.error("Ошибка подключения к SignalR:", err);
        showToast("Ошибка подключения к серверу");
        // Повторная попытка через 5 секунд
        setTimeout(startConnection, 5000);
    }
}

// Запуск соединения
startConnection();

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
        animation: fadein 0.5s, fadeout 0.5s 29.5s;
        max-width: 300px;
        word-wrap: break-word;
        opacity: 0;
        transition: opacity 0.5s ease-in-out;
    `;

    document.getElementById("toast-container").appendChild(toast);
    
    // Плавное появление
    setTimeout(() => {
        toast.style.opacity = "1";
    }, 10);

    // Плавное исчезновение и удаление
    setTimeout(() => {
        toast.style.opacity = "0";
        setTimeout(() => toast.remove(), 500);
    }, 29500);
}

// Добавляем стили для анимации
const style = document.createElement('style');
style.textContent = `
    @keyframes fadein {
        from { opacity: 0; }
        to { opacity: 1; }
    }
    @keyframes fadeout {
        from { opacity: 1; }
        to { opacity: 0; }
    }
`;
document.head.appendChild(style);
