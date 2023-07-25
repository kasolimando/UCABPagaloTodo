
// Datos de ejemplo
var servicios = [
    { nombre: "Servicio 1", prestador: "Manolo Perez", descripcion: "Descripción del servicio 1", estatus: "activo", categoria: "Alimento", TipoPago: "Confirmacion" },
    { nombre: "Servicio 2", descripcion: "Descripción del servicio 2" },
    { nombre: "Servicio 3", descripcion: "Descripción del servicio 3" }
];

// Obtener la referencia a la tabla
var tabla = document.getElementById("tablaServicios");

// Obtener la referencia al cuerpo de la tabla
var tbody = tabla.getElementsByTagName("tbody")[0];

// Iterar sobre los datos y generar las filas de la tabla
for (var i = 0; i < servicios.length; i++) {
    (function (servicio) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // ...

        // Crear la celda de nombre y agregar el enlace para abrir el modal
        var celdaNombre = document.createElement("td");
        var enlaceNombre = document.createElement("a");
        enlaceNombre.href = "#detalleServicioModal";
        enlaceNombre.setAttribute("data-toggle", "modal");
        enlaceNombre.textContent = servicio.nombre;
        enlaceNombre.addEventListener("click", function (event) {
            // Obtener la referencia al modal y sus elementos
            var modal = document.getElementById("detalleServicioModal");
            var modalTitle = document.getElementById("detalleServicioModalLabel");
            var modalInfo = document.getElementById("detalleServicioInfo");
            var btnFormato = document.getElementById("btnFormato");

            // Establecer el título y la información detallada en el modal
            modalTitle.textContent = servicio.nombre;
            modalInfo.innerHTML = "<span class='atributosModalServicioParti'>Prestador:</span> " + servicio.prestador + "<br>" +
                "<span class='atributosModalServicioParti'>Descripcion:</span> " + servicio.descripcion + "<br>" +
                "<span class='atributosModalServicioParti'>Estatus:</span> " + servicio.estatus + "<br>" +
                "<span class='atributosModalServicioParti'>Categoria:</span> " + servicio.categoria + "<br>" +
                "<span class='atributosModalServicioParti'>Tipo de Pago:</span> " + servicio.TipoPago;


            // Lógica para el botón "Formato"
            btnFormato.addEventListener("click", function () {
                // Lógica para abrir el formato o realizar alguna acción adicional
                console.log("Abrir formato para el servicio con nombre: " + servicio.nombre);
            });
        });

        celdaNombre.appendChild(enlaceNombre);


        var celdaPrestador = document.createElement("td");
        celdaPrestador.textContent = servicio.prestador;

        var celdaDescripcion = document.createElement("td");
        celdaDescripcion.textContent = servicio.descripcion;

        var celdaEstatus = document.createElement("td");
        celdaEstatus.textContent = servicio.estatus;

        var celdaCategoria = document.createElement("td");
        celdaCategoria.textContent = servicio.categoria;

        var celdaTipoDePago = document.createElement("td");
        celdaTipoDePago.textContent = servicio.TipoPago;

        // Crear la celda de acciones y agregar los botones
        var celdaAcciones = document.createElement("td");

        var botonEditar = document.createElement("a");
        botonEditar.href = "#editServicioModal";
        botonEditar.textContent = "Modificar"
        botonEditar.className = "btn";
        botonEditar.style.backgroundColor = "rgb(43, 89, 195,1)";
        botonEditar.style.color = "white";
        botonEditar.style.marginRight = "10px";
        botonEditar.setAttribute("data-toggle", "modal");
        //botonEditar.innerHTML = '<i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i>';

        var botonEliminar = document.createElement("a");
        botonEliminar.href = "#cambiarEstServicioModal";
        botonEliminar.textContent = "Activar/Desactivar"
        botonEliminar.className = "btn btn-danger";
        //botonEliminar.style.backgroundColor = "rgb(255, 167, 15)";
        botonEliminar.style.color = "white";
        botonEliminar.setAttribute("data-toggle", "modal");
        // botonEliminar.innerHTML = '<i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i>';

        celdaAcciones.appendChild(botonEditar);
        celdaAcciones.appendChild(botonEliminar);


        // Agregar las celdas a la fila
        fila.appendChild(celdaNombre);
        fila.appendChild(celdaPrestador);
        fila.appendChild(celdaDescripcion);
        fila.appendChild(celdaEstatus);
        fila.appendChild(celdaCategoria);
        fila.appendChild(celdaTipoDePago);
        fila.appendChild(celdaAcciones)
        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    })(servicios[i]);
}


//formato
document.getElementById("btnGuardar").addEventListener("click", function () {
    var tiles = document.getElementsByClassName("tile");
    for (var i = 0; i < tiles.length; i++) {
        tiles[i].classList.add("locked");
    }
});

document.getElementById("btnModificar").addEventListener("click", function () {
    var tiles = document.getElementsByClassName("tile");
    for (var i = 0; i < tiles.length; i++) {
        tiles[i].classList.remove("locked");
    }
});


document.getElementById("btnEstatusServ").addEventListener("click", function () {

    var popup = document.getElementById("popup");


    popup.classList.remove("d-none"); // Quitar la clase "d-none" para mostrar el popup
    popup.style.display = "block";

    // Cerrar el popup después de 3 segundos (3000 milisegundos)
    setTimeout(function () {
        popup.style.display = "none";
    }, 3000);

});

function seleccionarServicio(elemento) {
    // Obtener los valores de los campos de la fila seleccionada
    var fila = elemento.parentNode.parentNode;
    var prestador = fila.querySelector("td:nth-child(2)").textContent;
    var descripcion = fila.querySelector("td:nth-child(3)").textContent;
    var estatus = fila.querySelector("td:nth-child(4)").textContent;
    var categoria = fila.querySelector("td:nth-child(5)").textContent;
    var tipoPago = fila.querySelector("td:nth-child(6)").textContent;

    var spanPrestador = document.querySelector("#spanDetallePrestador");
    var spanDescripcion = document.querySelector("#spanDetalleDecripcion");
    var spanEstatus = document.querySelector("#spanDetalleEstatus");
    var spanCategoria = document.querySelector("#spanDetalleCategoria");
    var spanTipoPago = document.querySelector("#spanDetalleTipoPago");

    spanPrestador.innerHTML = prestador;
    spanDescripcion.innerHTML = descripcion;
    spanEstatus.innerHTML = estatus;
    spanCategoria.innerHTML = categoria;
    spanTipoPago.innerHTML = tipoPago;

    // Mostrar los detalles del prestador seleccionado
    $("#detalleServicioModal").modal("show");
};

function modificarServicio(elemento) {
    // Obtener los valores de los campos de la fila seleccionada
    var fila = elemento.parentNode.parentNode;
    var nombre = fila.querySelector("td:nth-child(1)").textContent;
    var prestador = fila.querySelector("td:nth-child(2)").textContent;
    var descripcion = fila.querySelector("td:nth-child(3)").textContent;
    var estatus = fila.querySelector("td:nth-child(4)").textContent;
    var categoria = fila.querySelector("td:nth-child(5)").textContent;
    var tipoPago = fila.querySelector("td:nth-child(6)").textContent;

    var inputNombre = document.querySelector("#inputNombre");
    var inputPrestador = document.querySelector("#inputPrestador");
    var inputDecripcion = document.querySelector("#inputDecripcion");
    var inputEstatus = document.querySelector("#estatus");
    var inputCategoria = document.querySelector("#inputCategoria");
    var tipoPago = document.querySelector("#tipoPago");

    inputNombre.value = nombre;
    inputPrestador.value = prestador;
    inputDecripcion.value = descripcion;
    inputEstatus.value = estatus;
    inputCategoria.value = categoria;
    tipoPago.value = tipoPago;

    // Mostrar los detalles del prestador seleccionado
    $("#editServicioModal").modal("show");
};


function activarServicio(elemento) {
    // Obtener los valores de los campos de la fila seleccionada
    var fila = elemento.parentNode.parentNode;
    var nombre = fila.querySelector("td:nth-child(1)").textContent;

    var inputNombre = document.querySelector("#inputCambioEstatus");

    inputNombre.value = nombre;

    // Mostrar los detalles del prestador seleccionado
    $("#cambiarEstServicioModal").modal("show");
};
