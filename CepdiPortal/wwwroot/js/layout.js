$(document).ready(function () {
    const token = localStorage.getItem('token');


    if (token) {
        $('#logoutItem').removeClass('d-none'); 
    } else {
        $('#logoutItem').addClass('d-none'); 
    }

    if (token && window.location.pathname === '/') {
        window.location.href = '/Usuarios';
    }

    $('#logout').on('click', function (e) {
        e.preventDefault();

        localStorage.removeItem('token');
        window.location.href = '/Home/Index';
    });
});

