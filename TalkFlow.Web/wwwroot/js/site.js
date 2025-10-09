const { Subject, ReplaySubject, Observable, BehaviorSubject, Subscription } = rxjs;

function CallToast(message, type = 'info') {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) {
        console.error('Toast container not found');
        return;
    }

    const toastId = `toast-${Date.now()}`;
    const iconMap = {
        'success': '<i class="fas fa-check-circle me-2"></i>',
        'error': '<i class="fas fa-exclamation-circle me-2"></i>',
        'warning': '<i class="fas fa-exclamation-triangle me-2"></i>',
        'info': '<i class="fas fa-info-circle me-2"></i>'
    };
    
    const bgColorMap = {
        'success': 'bg-success',
        'error': 'bg-danger',
        'warning': 'bg-warning',
        'info': 'bg-info'
    };

    const icon = iconMap[type] || iconMap['info'];
    const bgColor = bgColorMap[type] || bgColorMap['info'];

    const toastHTML = `
        <div id="${toastId}" class="toast align-items-center text-white ${bgColor} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    ${icon}${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `;

    toastContainer.insertAdjacentHTML('beforeend', toastHTML);
    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement, { delay: 4000 });
    toast.show();

    toastElement.addEventListener('hidden.bs.toast', function () {
        toastElement.remove();
    });
}

function showLoading(message = 'Loading...') {
    const loadingHTML = `
        <div id="global-loading" class="position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center" style="background: rgba(0,0,0,0.7); z-index: 9999;">
            <div class="text-center">
                <div class="spinner-border text-light" role="status" style="width: 3rem; height: 3rem;">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <div class="text-white mt-3">${message}</div>
            </div>
        </div>
    `;
    document.body.insertAdjacentHTML('beforeend', loadingHTML);
}

function hideLoading() {
    const loading = document.getElementById('global-loading');
    if (loading) {
        loading.remove();
    }
}