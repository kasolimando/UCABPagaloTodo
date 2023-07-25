// Datos de ejemplo
var pagos = [
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 2", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 2", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", fecha: "12/06/2023" },
    { servicio: "Servicio 3", consumidor: "Juana Patricia", monto: "69", fecha: "19/06/2023" }
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
    // Obtener el valor seleccionado del servicio
    var selectServicio = document.getElementById("selectServicio");
    var servicioSeleccionado = selectServicio.value;

    // Obtener las fechas ingresadas
    var fechaInicio = document.getElementById("FInicio").value;
    var fechaFinal = document.getElementById("FFinal").value;

    // Filtrar los pagos según el servicio y las fechas
    var pagosFiltrados = pagos.filter(function (pago) {
        var cumpleServicio = servicioSeleccionado === "" || pago.servicio === servicioSeleccionado;
        //var cumpleFechas = pago.fecha >= fechaInicio && pago.fecha <= fechaFinal;
        return cumpleServicio; //&& cumpleFecha
    });

    // Obtener la referencia al cuerpo de la tabla
    var tbody = tabla.getElementsByTagName("tbody")[0];
    // Limpiar el contenido actual de la tabla
    tbody.innerHTML = "";

    // Iterar sobre los pagos filtrados y generar las filas de la tabla
    for (var i = 0; i < pagosFiltrados.length; i++) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // Crear las celdas y asignarles el contenido
        var celdaServicio = document.createElement("td");
        celdaServicio.textContent = pagosFiltrados[i].servicio;

        var celdaConsumidor = document.createElement("td");
        celdaConsumidor.textContent = pagosFiltrados[i].consumidor;

        var celdaMonto = document.createElement("td");
        celdaMonto.textContent = pagosFiltrados[i].monto;

        var celdaFecha = document.createElement("td");
        celdaFecha.textContent = pagosFiltrados[i].fecha;


        // Agregar las celdas a la fila
        fila.appendChild(celdaServicio);
        fila.appendChild(celdaConsumidor);
        fila.appendChild(celdaMonto);
        fila.appendChild(celdaFecha);

        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    }
}



//Cargar el archivo de conciliacion
function cargarArchivo() {
    var archivoInput = document.getElementById("archivoInput");
    var archivo = archivoInput.files[0];

    if (archivo) {
        var lector = new FileReader();

        lector.onload = function (evento) {
            var contenido = evento.target.result;

            console.log(contenido);
        };

        lector.readAsText(archivo);
    }
}
