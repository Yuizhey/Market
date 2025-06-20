document.querySelectorAll('.download-files').forEach(button => {
    button.addEventListener('click', async function () {
        const productId = this.getAttribute('data-product-id');
        try {
            const response = await fetch(`/Items/DownloadAdditionalFiles?productId=${productId}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/zip'
                }
            });

            if (!response.ok) {
                throw new Error('Не удалось загрузить ZIP-архив');
            }

            const blob = await response.blob();
            if (blob.size === 0) {
                alert('Файлы для скачивания отсутствуют.');
                return;
            }

            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `AdditionalFiles_${productId}.zip`;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Ошибка при скачивании файлов:', error);
            alert('Произошла ошибка при скачивании ZIP-архива.');
        }
    });
});