$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault(); // Prevenir el envío por defecto del formulario
        login();
    });
});

function login() {
    const loginRequest = {
        usuario: $('#usuario').val(),
        password: $('#password').val()
    };

    $.ajax({
        url: 'https://localhost:7074/api/Usuarios/login',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(loginRequest),
        success: function (response) {
            // Almacenar el token en localStorage
            localStorage.setItem('token', response.token);
            $('#loginMessage').html('<div class="alert alert-success">Login exitoso. Bienvenido.</div>');
            console.log('Token recibido y almacenado:', response.token);

            // Redirigir al dashboard u otra página
            window.location.href = '/Usuarios';
        },
        error: function (xhr) {
            $('#loginMessage').html('<div class="alert alert-danger">Error: ' + xhr.responseJSON.message + '</div>');
        }
    });
}
