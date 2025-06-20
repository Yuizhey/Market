document.addEventListener('DOMContentLoaded', function() {
    const deleteButtons = document.querySelectorAll('.delete-product');

    deleteButtons.forEach(button => {
        button.addEventListener('click', async function() {
            if (confirm('Вы уверены, что хотите удалить этот товар?')) {
                const productId = this.getAttribute('data-product-id');
                try {
                    const response = await fetch(`/Items/Delete/${productId}`, {
                        method: 'DELETE'
                    });

                    if (response.ok) {
                        // Удаляем строку таблицы с товаром
                        this.closest('tr').remove();
                    } else {
                        alert('Произошла ошибка при удалении товара');
                    }
                } catch (error) {
                    console.error('Ошибка:', error);
                    alert('Произошла ошибка при удалении товара');
                }
            }
        });
    });
});