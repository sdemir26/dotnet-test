// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Modal form submit işlemlerini AJAX ile yap
document.addEventListener('DOMContentLoaded', function () {
    // Tüm modal formlarını seç
    const modalForms = document.querySelectorAll('.modal form');

    modalForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault(); // Normal form submit'i engelle

            const formData = new FormData(this);
            const action = this.getAttribute('action');
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;

            // Buton metnini değiştir
            submitBtn.innerHTML = '<i class="bi bi-hourglass-split"></i> Kaydediliyor...';
            submitBtn.disabled = true;

            // AJAX ile form gönder
            fetch(action, {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        // Başarılı ise modal'ı kapat
                        const modal = this.closest('.modal');
                        const modalInstance = bootstrap.Modal.getInstance(modal);
                        if (modalInstance) {
                            modalInstance.hide();
                        }

                        // Başarı mesajı göster
                        showSuccessMessage('Değişiklik başarıyla kaydedildi!');

                        // Sayfayı yenile (sadece içeriği)
                        setTimeout(() => {
                            window.location.reload();
                        }, 1000);
                    } else {
                        alert('Bir hata oluştu. Lütfen tekrar deneyin.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Bir hata oluştu. Lütfen tekrar deneyin.');
                })
                .finally(() => {
                    // Buton metnini geri al
                    submitBtn.innerHTML = originalText;
                    submitBtn.disabled = false;
                });
        });
    });

    // Modal'ların her zaman ortalanması için
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.addEventListener('show.bs.modal', function () {
            // Modal açılırken body'ye overflow hidden ekle
            document.body.style.overflow = 'hidden';

            // Modal dialog'u ortala
            const modalDialog = this.querySelector('.modal-dialog');
            if (modalDialog) {
                modalDialog.style.margin = 'auto';
                modalDialog.style.display = 'flex';
                modalDialog.style.alignItems = 'center';
                modalDialog.style.minHeight = '100vh';
            }
        });

        modal.addEventListener('hidden.bs.modal', function () {
            // Modal kapandığında body overflow'u geri al
            document.body.style.overflow = '';
        });
    });
});

// Başarı mesajı göster
function showSuccessMessage(message) {
    // Mevcut mesajları temizle
    const existingAlerts = document.querySelectorAll('.alert-success');
    existingAlerts.forEach(alert => alert.remove());

    // Yeni mesaj oluştur
    const alertDiv = document.createElement('div');
    alertDiv.className = 'alert alert-success alert-dismissible fade show position-fixed';
    alertDiv.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    alertDiv.innerHTML = `
        <i class="bi bi-check-circle"></i> ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    // Mesajı sayfaya ekle
    document.body.appendChild(alertDiv);

    // 3 saniye sonra otomatik kapat
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 3000);
}
