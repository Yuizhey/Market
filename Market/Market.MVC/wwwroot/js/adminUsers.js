function createAdmin(event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    fetch('/Admin/Admins/Create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ email, password }),
        credentials: 'same-origin'
    }).then(response => {
        if (response.ok) {
            window.location.reload();
        } else {
            alert('Ошибка при создании администратора');
        }
    }).catch(error => {
        console.error('Ошибка:', error);
        alert('Ошибка при создании администратора');
    });
}

function deleteAdmin(id) {
    if (confirm('Вы уверены, что хотите удалить этого администратора?')) {
        fetch(`/Admin/Admins/Delete/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            credentials: 'same-origin'
        }).then(response => {
            if (response.ok) {
                window.location.reload();
            } else {
                alert('Ошибка при удалении администратора');
            }
        }).catch(error => {
            console.error('Ошибка:', error);
            alert('Ошибка при удалении администратора');
        });
    }
}