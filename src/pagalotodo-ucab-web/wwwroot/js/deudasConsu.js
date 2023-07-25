// Datos de ejemplo
var deudas = [
    { servicio: "Servicio 1", monto: "40" },
    { servicio: "Servicio 1", monto: "40" },
    { servicio: "Servicio 2", monto: "40" },
    { servicio: "Servicio 2", monto: "40" },
    { servicio: "Servicio 3", monto: "40" },
    { servicio: "Servicio 3", monto: "40" },
    { servicio: "Servicio 3", monto: "40" },
    { servicio: "Servicio 3", monto: "69" }
];

// Obtener la referencia a la tabla
var tabla = document.getElementById("tablaDeudas");

// Función para llenar la tabla de deudas
function llenarTablaDeudas() {
    // Obtener la referencia al cuerpo de la tabla
    var tbody = tabla.getElementsByTagName("tbody")[0];
    // Limpiar el contenido actual de la tabla
    tbody.innerHTML = "";

    // Iterar sobre las deudas y generar las filas de la tabla
    for (var i = 0; i < deudas.length; i++) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // Crear las celdas y asignarles el contenido
        var celdaServicio = document.createElement("td");
        celdaServicio.textContent = deudas[i].servicio;

        var celdaMonto = document.createElement("td");
        celdaMonto.textContent = deudas[i].monto;


        // Agregar las celdas a la fila
        fila.appendChild(celdaServicio);
        fila.appendChild(celdaMonto);

        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    }
}

// Llamar a la función para llenar la tabla de deudas cuando se carga la página
llenarTablaDeudas();
