function deleteProduct(id) {
    if (confirm('Вы уверены, что хотите удалить этот продукт?')) {
        fetch(`/Admin/Products/Delete/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            if (response.ok) {
                window.location.reload();
            } else {
                alert('Произошла ошибка при удалении продукта');
            }
        });
    }
}