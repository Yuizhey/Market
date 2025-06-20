async function resolveRequest(id) {
    try {
        const response = await fetch('@Url.Action("Delete", "Contact", new { area = "Admin" })', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ id: id })
        });

        const result = await response.json();

        if (result.success) {
            const row = document.getElementById(`contact-${id}`);
            row.style.transition = 'opacity 0.3s';
            row.style.opacity = '0';
            setTimeout(() => row.remove(), 300);
        } else {
            alert('Произошла ошибка при удалении запроса');
        }
    } catch (error) {
        alert('Произошла ошибка при удалении запроса');
    }
}