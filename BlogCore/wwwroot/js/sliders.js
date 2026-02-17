var dataTable;

document.addEventListener('DOMContentLoaded', function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblSliders").DataTable({
        // DataTable hace un GET al endpoint "/admin/categorias/GetAll"
        // y espera un JSON con la información de las categorías.
        // Por ejemplo:
        // {
        //     "data": [
        //         { "id": 1, "nombre": "A", "orden": 1 },
        //         { "id": 2, "nombre": "B", "orden": 2 }
        //     ]
        // }
        "ajax": {
            "url": "/admin/sliders/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        // Definimos las columnas que se mostrarán en la tabla.
        // El orden debe coincidir con el JSON recibido y con el HTML de la tabla en la vista.
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "20%" },
            {
                "data": "estado",
                "width": "10%",
                "render": function (estado) {
                    if (estado) {
                        return "Activo"
                    }
                    else {
                        return "Inactivo"
                    }
                }
            },
            {
                "data": "urlImagen",
                "width": "20%",
                "render": function (imagen) {
                    return `<img src="..${imagen}" width="120">`
                }
            },
            // La última columna no viene del JSON, sino que se genera dinámicamente con botones de acción.
            // 
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Sliders/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="bi bi-pencil-square"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Sliders/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer;">
                                <i class="bi bi-trash"></i> Borrar
                                </a>
                          </div>
                         `;
                }, "width": "30%"
            }
        ],
        // Configuración de idioma para mostrar los textos en español.
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });
}

function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}