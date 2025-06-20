document.addEventListener('DOMContentLoaded', function() {
    const removeButtons = document.querySelectorAll('.remove-item');

    removeButtons.forEach(button => {
        button.addEventListener('click', async function() {
            const cartId = this.dataset.cartId;
            const productId = this.dataset.productId;

            try {
                const response = await fetch('/Cart/RemoveItem', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        cartId: cartId,
                        productId: productId
                    })
                });

                if (response.ok) {
                    // Удаляем элемент из DOM
                    this.closest('li').remove();

                    // Обновляем счетчик товаров
                    const badge = document.querySelector('.badge');
                    const currentCount = parseInt(badge.textContent);
                    badge.textContent = currentCount - 1;

                    // Если корзина пуста, обновляем страницу
                    if (currentCount - 1 === 0) {
                        window.location.reload();
                    }
                } else {
                    alert('Ошибка при удалении товара');
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Произошла ошибка при удалении товара');
            }
        });
    });
});