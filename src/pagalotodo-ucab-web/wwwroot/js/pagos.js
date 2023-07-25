// Datos de ejemplo
var pagos = [
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023", aprobado: "Aprobado", cierre: "Realizado" },
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023", aprobado: "Aprobado", cierre: "Realizado" },
    { servicio: "Servicio 2", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023", aprobado: "Aprobado", cierre: "Realizado" },
    { servicio: "Servicio 1", consumidor: "Juana Patricia", monto: "69", fecha: "19/06/2023", aprobado: "No aprobado", cierre: "Realizado" }
];

// Obtener la referencia a la tabla
var tabla = document.getElementById("tablaPagos");
// Obtener la referencia al botón de búsqueda
var botonBuscar = document.getElementById("btn_buscar");

// Agregar un listener de evento al botón
botonBuscar.addEventListener("click", function (event) {
    event.preventDefault(); // Evitar el comportamiento predeterminado del enlace

    // Llamar a la función buscarPagos
    buscarPagos();
});

function buscarPagos() {
    // Obtener las fechas ingresadas
    var fechaInicio = document.getElementById("FInicio").value;
    var fechaFinal = document.getElementById("FFinal").value;

    // Filtrar los pagos según las fechas
    //var pagosFiltrados = pagos.filter(function(pago) {
    //   return pago.fecha >= fechaInicio && pago.fecha <= fechaFinal;
    //});

    // Obtener la referencia al cuerpo de la tabla
    var tbody = tabla.getElementsByTagName("tbody")[0];
    // Limpiar el contenido actual de la tabla
    tbody.innerHTML = "";

    // Iterar sobre los pagos filtrados y generar las filas de la tabla
    for (var i = 0; i < pagos.length; i++) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // Crear las celdas y asignarles el contenido
        var celdaServicio = document.createElement("td");
        celdaServicio.textContent = pagos[i].servicio;

        var celdaConsumidor = document.createElement("td");
        celdaConsumidor.textContent = pagos[i].consumidor;

        var celdaMonto = document.createElement("td");
        celdaMonto.textContent = pagos[i].monto;

        var celdaFecha = document.createElement("td");
        celdaFecha.textContent = pagos[i].fecha;

        var celdaAprobado = document.createElement("td");
        celdaAprobado.textContent = pagos[i].aprobado;

        var celdaCierre = document.createElement("td");
        celdaCierre.textContent = pagos[i].cierre;

        // Agregar las celdas a la fila
        fila.appendChild(celdaServicio);
        fila.appendChild(celdaConsumidor);
        fila.appendChild(celdaMonto);
        fila.appendChild(celdaFecha);
        fila.appendChild(celdaAprobado);
        fila.appendChild(celdaCierre);

        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    }
}
