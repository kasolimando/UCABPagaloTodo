// Datos de ejemplo
var servicios = [
    { nombre: "Servicio 1", descripcion: "Descripción del servicio 1", estatus: "activo", categoria: "Alimento", TipoPago: "Confirmacion" },
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
            modalInfo.innerHTML = "<span class='atributosModalServicioParti'>Servicio:</span> " + servicio.nombre + "<br>" +
                "<span class='atributosModalServicioParti'>Descripcion:</span> " + servicio.descripcion + "<br>" +
                "<span class='atributosModalServicioParti'>Estatus:</span> " + servicio.estatus + "<br>" +
                "<span class='atributosModalServicioParti'>Categoria:</span> " + servicio.categoria + "<br>" +
                "<span class='atributosModalServicioParti'>Tipo de Pago:</span> " + servicio.TipoPago;

        });

        celdaNombre.appendChild(enlaceNombre);


        var celdaDescripcion = document.createElement("td");
        celdaDescripcion.textContent = servicio.descripcion;

        var celdaEstatus = document.createElement("td");
        celdaEstatus.textContent = servicio.estatus;

        var celdaCategoria = document.createElement("td");
        celdaCategoria.textContent = servicio.categoria;

        var celdaTipoDePago = document.createElement("td");
        celdaTipoDePago.textContent = servicio.TipoPago;

        // Agregar las celdas a la fila
        fila.appendChild(celdaNombre);
        fila.appendChild(celdaDescripcion);
        fila.appendChild(celdaEstatus);
        fila.appendChild(celdaCategoria);
        fila.appendChild(celdaTipoDePago);

        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    })(servicios[i]);
}
