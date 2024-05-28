(() => {
    'use strict'
    const form = document.querySelector('form')
    form.addEventListener('submit', event => {
        event.preventDefault()
        event.stopPropagation()
        const password = $("#password").val();
        const confirmPassword = $("#confirm-password").val();
        form.classList.add('was-validated')
        if (password !== confirmPassword) {
            $("#confirm-password").addClass("is-invalid");
            return;
        }
        if (!form.checkValidity()) {
            return;
        }
        $("#confirm-password").removeClass("is-invalid");
        const email = $('#email').val();
        const username = $('#username').val();
        const fullName = $('#full-name').val();
        const phoneNumber = $('#phone').val();

        const user = {
            email,
            username,
            password,
            fullName,
            phoneNumber
        }

        requestToServer('Users/CreateUser', 'post', user, (result) => {
            if (result) {
                redirectToPage('../login');
            } else {
                showMessage("فشل انشاء مستخدم")
            }
        });
    }, false)
})()