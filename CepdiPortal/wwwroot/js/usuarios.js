$(document).ready(function () {

    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = '/Home/Index';
    }

    $.ajaxSetup({
        beforeSend: function (xhr) {
            const token = localStorage.getItem('token'); 
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });

    $('#usuariosTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: function (data, callback, settings) {
            const page = Math.floor(data.start / data.length) + 1; 
            const pageSize = data.length;

            $.ajax({
                url: `https://localhost:7074/api/Usuarios`,
                type: 'GET',
                data: {
                    page: page,
                    pageSize: pageSize,
                    filtro: data.search.value || null 
                },
                success: function (response) {
                    callback({
                        draw: data.draw, 
                        recordsTotal: response.totalCount, 
                        recordsFiltered: response.totalCount, 
                        data: response.data 
                    });
                },
                error: function (xhr) {
                    console.error('Error al cargar los datos:', xhr.responseJSON);
                }
            });
        },
        columns: [
            { data: 'idUsuario' },
            { data: 'nombre' },
            { data: 'usuario' },
            {
                data: 'estatus',
                render: function (data) {
                    return data === 1 ? 'Activo' : 'Inactivo';
                }
            },
            {
                data: 'idPerfil',
                render: function (data) {
                    return data === 1 ? 'Administrador' : 'Usuario';
                }
            },
            {
                data: null,
                render: function (data) {
                    return `
                    <button onclick="verUsuario(${data.idUsuario})" class="btn btn-info">Ver</button>
                    <button onclick="editarUsuario(${data.idUsuario})" class="btn btn-warning">Editar</button>
                    <button onclick="eliminarUsuario(${data.idUsuario})" class="btn btn-danger">Eliminar</button>
                `;
                }
            }
        ],
        pageLength: 10, 
        lengthMenu: [10, 25, 50, 100], 
        language: {
            paginate: {
                previous: "Anterior",
                next: "Siguiente"
            },
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros por página"
        }
    });

    $('<button>')
        .text('Nuevo Usuario')
        .addClass('btn btn-primary mb-3')
        .click(function () {
            mostrarFormularioNuevoUsuario();
        })
        .prependTo('#usuariosTable_wrapper .dataTables_filter');
});


function mostrarFormularioNuevoUsuario() {
    $('#nuevoUsuarioModal').modal('show');
}

function verUsuario(id) {
    $.ajax({
        url: `https://localhost:7074/api/Usuarios/${id}`,
        type: 'GET',
        success: function (usuario) {
            $('#verNombre').val(usuario.nombre);
            $('#verUsuario').val(usuario.usuario);
            $('#verIdPerfil').val(usuario.idPerfil === 1 ? 'Administrador' : 'Usuario');
            $('#verEstatus').val(usuario.estatus === 1 ? 'Activo' : 'Inactivo');

            $('#verUsuarioModal').modal('show');
        },
        error: function (xhr) {
            alert(`Error al obtener los datos del usuario: ${xhr.responseJSON.message}`);
        }
    });
}


function editarUsuario(id) {
    $.ajax({
        url: `https://localhost:7074/api/Usuarios/${id}`,
        type: 'GET',
        success: function (usuario) {
            $('#idUsuario').val(usuario.idUsuario);
            $('#editarNombre').val(usuario.nombre);
            $('#editarUsuario').val(usuario.usuario);
            $('#editarPassword').val(''); // Contraseña vacía para que el usuario la reingrese si desea cambiarla
            $('#editarIdPerfil').val(usuario.idPerfil);
            $('#editarEstatus').val(usuario.estatus);

            $('#editarUsuarioModal').modal('show');
        },
        error: function (xhr) {
            alert(`Error al obtener los datos del usuario: ${xhr.responseJSON.message}`);
        }
    });
}

function modificarUsuario() {
    const id = $('#idUsuario').val();
    const usuarioModificado = {
        nombre: $('#editarNombre').val(),
        usuario: $('#editarUsuario').val(),
        password: $('#editarPassword').val(),
        idPerfil: parseInt($('#editarIdPerfil').val(), 10),
        estatus: parseInt($('#editarEstatus').val(), 10)
    };

    $.ajax({
        url: `https://localhost:7074/api/Usuarios/${id}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(usuarioModificado),
        success: function () {
            alert("Usuario modificado exitosamente.");
            $('#editarUsuarioModal').modal('hide');
            $('#usuariosTable').DataTable().ajax.reload();
        },
        error: function (xhr) {
            alert(`Error al modificar el usuario: ${xhr.responseJSON.message}`);
        }
    });
}


function eliminarUsuario(id) {
    if (confirm("¿Está seguro de que desea eliminar este usuario? Esta acción no se puede deshacer.")) {
        $.ajax({
            url: `https://localhost:7074/api/Usuarios/${id}`, 
            type: 'DELETE',
            success: function () {
                alert("Usuario eliminado exitosamente.");
                $('#usuariosTable').DataTable().ajax.reload(); 
            },
            error: function (xhr) {
                alert(`Error al eliminar el usuario: ${xhr.responseJSON.message}`);
            }
        });
    }
}



function crearUsuario() {
    const nuevoUsuario = {
        nombre: $('#nombre').val(),
        usuario: $('#usuario').val(),
        password: $('#password').val(),
        idPerfil: parseInt($('#idPerfil').val(), 10) 
    };

    $.ajax({
        url: 'https://localhost:7074/api/Usuarios',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(nuevoUsuario),
        success: function () {
            alert("Usuario creado exitosamente.");
            $('#nuevoUsuarioModal').modal('hide');
            $('#usuariosTable').DataTable().ajax.reload();
        },
        error: function (xhr) {
            alert(`Error: ${xhr.responseJSON.message}`);
        }
    });
}
