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

// Funci�n para llenar la tabla de deudas
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

        var celdaConsumidor = document.createElement("td");
        celdaConsumidor.textContent = deudas[i].consumidor;

        var celdaMonto = document.createElement("td");
        celdaMonto.textContent = deudas[i].monto;

        var celdaEstatus = document.createElement("td");
        celdaEstatus.textContent = deudas[i].estatus;

        // Agregar las celdas a la fila
        fila.appendChild(celdaServicio);
        fila.appendChild(celdaConsumidor);
        fila.appendChild(celdaMonto);
        fila.appendChild(celdaEstatus);

        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    }
}

// Llamar a la funci�n para llenar la tabla de deudas cuando se carga la p�gina
llenarTablaDeudas();
