// Datos de ejemplo
var deudas = [
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", estatus: "Activo" },
    { servicio: "Servicio 3", consumidor: "Juanito Perez", monto: "40", estatus: "Activo" },
    { servicio: "Servicio 1", consumidor: "Manolo Perez", monto: "40", estatus: "Inactivo" },
    { servicio: "Servicio 2", consumidor: "Manolo Perez", monto: "40", estatus: "Inactivo" },
    { servicio: "Servicio 2", consumidor: "Manolo Perez", monto: "40", estatus: "Inactivo" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", estatus: "Inactivo" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", estatus: "Activo" },
    { servicio: "Servicio 3", consumidor: "Manolo Perez", monto: "40", estatus: "Activo" },
    { servicio: "Servicio 3", consumidor: "Juana Patricia", monto: "69", estatus: "Activo" }
];

// Obtener la referencia a la tabla
var tabla = document.getElementById("tablaDeudas");
// Obtener la referencia al botón de búsqueda
var botonBuscar = document.getElementById("btn_buscar");

// Agregar un listener de evento al botón
botonBuscar.addEventListener("click", function (event) {
    event.preventDefault(); // Evitar el comportamiento predeterminado del enlace

    // Llamar a la función buscarPagos
    buscarDeudas();
});

function buscarDeudas() {
    // Obtener el valor seleccionado del servicio
    var selectServicio = document.getElementById("selectServicio");
    var servicioSeleccionado = selectServicio.value;

    // Filtrar los pagos según el servicio y las fechas
    var deudasFiltradas = deudas.filter(function (deuda) {
        var cumpleServicio = servicioSeleccionado === "" || deuda.servicio === servicioSeleccionado;

        return cumpleServicio;
    });

    // Obtener la referencia al cuerpo de la tabla
    var tbody = tabla.getElementsByTagName("tbody")[0];
    // Limpiar el contenido actual de la tabla
    tbody.innerHTML = "";

    // Iterar sobre las deudasFiltradas y generar las filas de la tabla
    for (var i = 0; i < deudasFiltradas.length; i++) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // Crear las celdas y asignarles el contenido
        var celdaServicio = document.createElement("td");
        celdaServicio.textContent = deudasFiltradas[i].servicio;

        var celdaConsumidor = document.createElement("td");
        celdaConsumidor.textContent = deudasFiltradas[i].consumidor;

        var celdaMonto = document.createElement("td");
        celdaMonto.textContent = deudasFiltradas[i].monto;

        var celdaEstatus = document.createElement("td");
        celdaEstatus.textContent = deudasFiltradas[i].estatus;


        // Agregar las celdas a la fila
        fila.appendChild(celdaServicio);
        fila.appendChild(celdaConsumidor);
        fila.appendChild(celdaMonto);
        fila.appendChild(celdaEstatus);

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
