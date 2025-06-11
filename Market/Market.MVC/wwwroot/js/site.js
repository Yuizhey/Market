const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationhub")
    .withAutomaticReconnect([0, 2000, 5000, 10000, 20000])
    .build();

connection.on("ReceiveBroadcast", function (message) {
    showToast(message);
});

connection.on("ReceiveByRole", function (message) {
    showToast(message);
});

connection.on("ReceiveMessage", function (message) {
    showToast(message);
});

connection.on("ReceiveUnauthenticated", function (message) {
    showToast(message);
});

// Обработка состояния подключения
connection.onreconnecting((error) => {
    console.log("Переподключение к SignalR...", error);
});

connection.onreconnected((connectionId) => {
    console.log("Переподключено к SignalR", connectionId);
});

connection.onclose((error) => {
    console.log("Соединение закрыто", error);
});

// Функция для установки соединения
async function startConnection() {
    try {
        await connection.start();
        console.log("Connected to SignalR");
    } catch (err) {
        console.error("Ошибка подключения к SignalR:", err);
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

    const container = document.getElementById("toast-container");
    if (!container) {
        console.error("Контейнер для уведомлений не найден!");
        return;
    }

    container.appendChild(toast);
    
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
