document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form[data-validate="true"]');
    
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            if (!validateForm(form)) {
                e.preventDefault();
                e.stopPropagation();
            }
        });

        const inputs = form.querySelectorAll('input[required], select[required]');
        inputs.forEach(input => {
            input.addEventListener('blur', function () {
                validateField(input);
            });

            input.addEventListener('input', function () {
                if (input.classList.contains('is-invalid')) {
                    validateField(input);
                }
            });
        });
    });
});

function validateForm(form) {
    let isValid = true;
    const inputs = form.querySelectorAll('input[required], select[required]');
    
    inputs.forEach(input => {
        if (!validateField(input)) {
            isValid = false;
        }
    });

    return isValid;
}

function validateField(field) {
    let isValid = true;
    let errorMessage = '';

    if (field.hasAttribute('required') && !field.value.trim()) {
        isValid = false;
        errorMessage = 'This field is required';
    }

    if (field.type === 'text' && field.value.trim()) {
        const minLength = field.getAttribute('minlength');
        const maxLength = field.getAttribute('maxlength');
        const length = field.value.trim().length;

        if (minLength && length < parseInt(minLength)) {
            isValid = false;
            errorMessage = `Minimum ${minLength} characters required`;
        }
        if (maxLength && length > parseInt(maxLength)) {
            isValid = false;
            errorMessage = `Maximum ${maxLength} characters allowed`;
        }
    }

    if (field.type === 'number' && field.value) {
        const min = field.getAttribute('min');
        const max = field.getAttribute('max');
        const value = parseInt(field.value);

        if (min && value < parseInt(min)) {
            isValid = false;
            errorMessage = `Minimum value is ${min}`;
        }
        if (max && value > parseInt(max)) {
            isValid = false;
            errorMessage = `Maximum value is ${max}`;
        }
    }

    if (isValid) {
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
        removeErrorMessage(field);
    } else {
        field.classList.remove('is-valid');
        field.classList.add('is-invalid');
        showErrorMessage(field, errorMessage);
    }

    return isValid;
}

function showErrorMessage(field, message) {
    removeErrorMessage(field);
    
    let errorDiv = field.parentNode.querySelector('.invalid-feedback');
    if (!errorDiv) {
        errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        field.parentNode.appendChild(errorDiv);
    }
    
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';
    errorDiv.setAttribute('data-error-for', field.id || field.name);
}

function removeErrorMessage(field) {
    const existingError = field.parentNode.querySelector('.invalid-feedback');
    if (existingError) {
        existingError.remove();
    }
}

function clearFormValidation(form) {
    const inputs = form.querySelectorAll('input, select');
    inputs.forEach(input => {
        input.classList.remove('is-valid', 'is-invalid');
        removeErrorMessage(input);
    });
}
