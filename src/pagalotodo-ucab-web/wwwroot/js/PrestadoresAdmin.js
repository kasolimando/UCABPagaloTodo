function seleccionarPrestador(elemento) {
    // Obtener los valores de los campos de la fila seleccionada
    var fila = elemento.parentNode.parentNode;
    var nombre = fila.querySelector("td:nth-child(2)").textContent;
    var docIdentidad = fila.querySelector("td:nth-child(3)").textContent.substring(2);
    var correo = fila.querySelector("td:nth-child(4)").textContent;
    var direccion = fila.querySelector("td:nth-child(5)").textContent;
    var estatus = fila.querySelector("td:nth-child(6)").textContent;

    var spanNombre = document.querySelector("#spanDetalleNombre");
    var spanCorreo = document.querySelector("#spanDetalleCorreo");
    var spanDoc = document.querySelector("#spanDetalleDocIdentidad");
    var spanDeireccion = document.querySelector("#spanDetalleDireccion");
    var spanEstatus = document.querySelector("#spanDetalleEstatus");

    spanNombre.innerHTML = nombre;
    spanCorreo.innerHTML = correo;
    spanDoc.innerHTML = docIdentidad;
    spanDeireccion.innerHTML = direccion;
    spanEstatus.innerHTML = estatus;

    // Mostrar los detalles del prestador seleccionado
    $("#detallePrestadorModal").modal("show");
}

function modificarPrestador(elemento) {
    // Obtener los valores de los campos de la fila seleccionada
    var fila = elemento.parentNode.parentNode;
    var username = fila.querySelector("a").getAttribute("data-nombre");
    var nombre = fila.querySelector("td:nth-child(2)").textContent;
    var docIdentidad = fila.querySelector("td:nth-child(3)").textContent.substring(2);
    var correo = fila.querySelector("td:nth-child(4)").textContent;
    var direccion = fila.querySelector("td:nth-child(5)").textContent;

    var inputUsername = document.querySelector("#inputUsername");
    var inputNombre = document.querySelector("#inputNombre");
    var inputApellido = document.querySelector("#inputApellido");
    var tipoDoc = document.querySelector("#tipoDoc");
    var inputIdentidad = document.querySelector("#inputIdentidad");
    var inputCorreo = document.querySelector("#inputCorreo");
    var inputDireccion = document.querySelector("#inputDireccion");

    inputUsername.value = username;
    var nombreSeparado = nombre.split(" ");
    var docIdentidadSeparado = docIdentidad.split("-");
    inputNombre.value = nombreSeparado[0];
    inputApellido.value = nombreSeparado[1];;
    tipoDoc.value = docIdentidadSeparado[0];
    inputIdentidad.value = docIdentidadSeparado[1];
    inputCorreo.value = correo;
    inputDireccion.value = direccion;

    // Mostrar los detalles del prestador seleccionado
    $("#modalModificarPrestador").modal("show");
}


function cerrar() {
    mostrador.style.width = "100%";
    seleccion.style.display = "none"; // Oculta el modal
    quitarBordes();
}