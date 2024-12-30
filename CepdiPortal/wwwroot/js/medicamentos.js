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

    $('#medicamentosTable').DataTable({
        serverSide: true, // Habilitar procesamiento en el servidor
        processing: true, // Mostrar indicador de carga
        ajax: function (data, callback, settings) {
            const page = Math.floor(data.start / data.length) + 1; // Número de página actual
            const pageSize = data.length; // Número de registros por página

            // Llamada AJAX a la API
            $.ajax({
                url: 'https://localhost:7074/api/Medicamentos',
                type: 'GET',
                data: {
                    page: page, // Enviar número de página
                    pageSize: pageSize, // Enviar tamaño de la página
                    filtro: data.search.value || null // Enviar filtro de búsqueda si está presente
                },
                success: function (response) {
                    // Procesar la respuesta del servidor y devolverla al DataTable
                    callback({
                        draw: data.draw, // Identificador único de la petición
                        recordsTotal: response.totalCount, // Total de registros sin filtrar
                        recordsFiltered: response.totalCount, // Total de registros filtrados
                        data: response.data // Datos de la página actual
                    });
                },
                error: function (xhr) {
                    console.error('Error al cargar los datos:', xhr.responseJSON);
                }
            });
        },
        columns: [
            { data: 'idMedicamento' },
            { data: 'nombre' },
            { data: 'concentracion' },
            { data: 'formaFarmaceuticaNombre', defaultContent: 'No disponible' },
            { data: 'precio', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'stock' },
            { data: 'presentacion' },
            { data: 'bHabilitado', render: function (data) { return data === 1 ? 'Sí' : 'No'; } },
            {
                data: null,
                render: function (data) {
                    return `
                        <button class="btn btn-info btn-sm" onclick="verMedicamento(${data.idMedicamento})">Ver</button>
                        <button class="btn btn-warning btn-sm" onclick="editarMedicamento(${data.idMedicamento})">Editar</button>
                        <button class="btn btn-danger btn-sm" onclick="eliminarMedicamento(${data.idMedicamento})">Eliminar</button>
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

    // Lógica para abrir el modal de crear medicamento
    $('#btnCrearMedicamento').on('click', function () {
        $('#medicamentoForm')[0].reset();
        $('#medicamentoId').val('');
        $('#medicamentoModalLabel').text('Crear Medicamento');
        $('#submitMedicamento').off('click').on('click', crearMedicamento);
        $('#medicamentoModal').modal('show');
    });

    // Cargar las formas farmacéuticas cuando se abra el modal de creación
    $('#btnCrearMedicamento').on('click', function () {
        $('#medicamentoForm')[0].reset();
        $('#medicamentoId').val('');
        $('#medicamentoModalLabel').text('Crear Medicamento');
        cargarFormasFarmaceuticas();
        $('#medicamentoModal').modal('show');
    });

});

// Función para cargar las formas farmacéuticas
function cargarFormasFarmaceuticas(selectedId) {
    $.ajax({
        url: 'https://localhost:7074/api/Medicamentos/formas-farmaceuticas',
        type: 'GET',
        success: function (data) {
            const select = $('#idFormaFarmaceutica');
            select.empty(); // Limpiar las opciones anteriores
            select.append('<option value="">Seleccione una opción</option>');
            data.forEach(item => {
                select.append(`<option value="${item.idFormaFarmaceutica}">${item.nombre}</option>`);
            });

            // Seleccionar la forma farmacéutica correspondiente
            if (selectedId) {
                select.val(selectedId);
            }
        },
        error: function (xhr) {
            console.error('Error al cargar las formas farmacéuticas:', xhr.responseJSON);
            alert('No se pudieron cargar las formas farmacéuticas. Intente nuevamente.');
        }
    });
}

// Función para crear un medicamento
function crearMedicamento(e) {
    e.preventDefault();
    const medicamento = obtenerDatosFormulario();
    console.log(medicamento);
    $.ajax({
        url: 'https://localhost:7074/api/Medicamentos',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(medicamento),
        success: function () {
            alert('Medicamento creado exitosamente');
            $('#medicamentoModal').modal('hide');
            $('#medicamentosTable').DataTable().ajax.reload();
        },
        error: function (xhr) {
            alert(`Error: ${xhr.responseJSON.message}`);
        }
    });
}

// Función para obtener datos del formulario
function obtenerDatosFormulario() {
    return {
        idMedicamento: $('#medicamentoId').val() || null,
        nombre: $('#nombre').val(),
        concentracion: $('#concentracion').val(),
        idFormaFarmaceutica: $('#idFormaFarmaceutica').val(),
        precio: parseFloat($('#precio').val()),
        stock: parseInt($('#stock').val()),
        presentacion: $('#presentacion').val(),
        bHabilitado: parseInt($('#bHabilitado').val())
    };
}

function verMedicamento(id) {
    $.ajax({
        url: `https://localhost:7074/api/Medicamentos/${id}`,
        type: 'GET',
        success: function (data) {
            // Llenar los campos del modal con los datos del medicamento
            $('#verIdMedicamento').text(data.idMedicamento);
            $('#verNombre').text(data.nombre);
            $('#verConcentracion').text(data.concentracion);
            $('#verFormaFarmaceutica').text(data.formaFarmaceutica ? data.formaFarmaceuticaNombre : 'No disponible');
            $('#verPrecio').text(`$${data.precio.toFixed(2)}`);
            $('#verStock').text(data.stock);
            $('#verPresentacion').text(data.presentacion);
            $('#verHabilitado').text(data.bHabilitado === 1 ? 'Sí' : 'No');

            // Mostrar el modal
            $('#verMedicamentoModal').modal('show');
        },
        error: function (xhr) {
            console.error('Error al obtener los detalles del medicamento:', xhr.responseJSON);
            alert('No se pudieron cargar los detalles del medicamento. Intente nuevamente.');
        }
    });
}


function editarMedicamento(id) {
    // Obtener los datos del medicamento desde la API
    $.ajax({
        url: `https://localhost:7074/api/Medicamentos/${id}`,
        type: 'GET',
        success: function (data) {
            // Llenar los campos con los datos del medicamento
            $('#medicamentoId').val(data.idMedicamento);
            $('#nombre').val(data.nombre);
            $('#concentracion').val(data.concentracion);
            $('#precio').val(data.precio);
            $('#stock').val(data.stock);
            $('#presentacion').val(data.presentacion);
            $('#bHabilitado').val(data.bHabilitado);

            // Cargar las formas farmacéuticas y seleccionar la correspondiente
            cargarFormasFarmaceuticas(data.idFormaFarmaceutica);

            // Cambiar el título del modal
            $('#medicamentoModalLabel').text('Editar Medicamento');

            // Cambiar la función del botón de envío
            $('#submitMedicamento').off('click').on('click', function (e) {
                e.preventDefault();
                actualizarMedicamento();
            });

            // Mostrar el modal
            $('#medicamentoModal').modal('show');
        },
        error: function (xhr) {
            console.error('Error al obtener los detalles del medicamento:', xhr.responseJSON);
            alert('No se pudieron cargar los detalles del medicamento. Intente nuevamente.');
        }
    });
}

function actualizarMedicamento() {
    const medicamento = obtenerDatosFormulario();

    // Enviar la solicitud PUT a la API
    $.ajax({
        url: `https://localhost:7074/api/Medicamentos/${medicamento.idMedicamento}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(medicamento),
        success: function () {
            alert('Medicamento actualizado exitosamente');
            $('#medicamentoModal').modal('hide');
            $('#medicamentosTable').DataTable().ajax.reload();
        },
        error: function (xhr) {
            console.error('Error al actualizar el medicamento:', xhr.responseJSON);
            alert('No se pudo actualizar el medicamento. Intente nuevamente.');
        }
    });
}

// Función para eliminar medicamento
function eliminarMedicamento(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este medicamento?')) {
        $.ajax({
            url: `https://localhost:7074/api/Medicamentos/${id}`,
            type: 'DELETE',
            success: function () {
                alert('Medicamento eliminado exitosamente.');
                $('#medicamentosTable').DataTable().ajax.reload();
            },
            error: function (xhr) {
                alert(`Error al eliminar el medicamento: ${xhr.responseJSON.message}`);
            }
        });
    }
}
