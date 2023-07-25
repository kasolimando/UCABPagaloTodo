
// Datos de ejemplo
var consumidores = [
    { username: "wasakaka", nombre: "Mia Termopolis", correo: "a@gmail.com", documento: "v-1234", direccion: "Genovia", estatus: "Activo" },
    { username: "eloeoeoeo", nombre: "Benito", correo: "b@gmail.com", documento: "v-456", direccion: "Miami", estatus: "Inactivo" },
    { username: "pantufla", nombre: "Mia Termopolis", correo: "c@gmail.com", documento: "v-1234", direccion: "Genovia", estatus: "Activo" },

];


// Obtener la referencia a la tabla
var tabla = document.getElementById("tablaConsumidores");

// Obtener la referencia al cuerpo de la tabla
var tbody = tabla.getElementsByTagName("tbody")[0];


// Iterar sobre los datos y generar las filas de la tabla
for (var i = 0; i < consumidores.length; i++) {
    (function (user) {
        // Crear una nueva fila
        var fila = document.createElement("tr");

        // ...

        // Crear la celda de nombre y agregar el enlace para abrir el modal
        var celdaNombre = document.createElement("td");
        var enlaceNombre = document.createElement("a");
        enlaceNombre.href = "#detallePrestadorModal";
        enlaceNombre.setAttribute("data-toggle", "modal");
        enlaceNombre.textContent = user.username;
        enlaceNombre.addEventListener("click", function (event) {
            // Obtener la referencia al modal y sus elementos
            var modal = document.getElementById("detallePrestadorModal");
            var modalTitle = document.getElementById("detallePrestadorModal");
            var modalInfo = document.getElementById("detallePrestadorModal");


            // Establecer el título y la información detallada en el modal
            modalTitle.textContent = user.username;
            modalInfo.innerHTML = "<span class='atributosModalServicioParti'>Nombre y Apellido:</span> " + user.nombre + "<br>" +
                "<span class='atributosModalServicioParti'>Correo:</span> " + user.correo + "<br>" +
                "<span class='atributosModalServicioParti'>Documento:</span> " + user.documento + "<br>" +
                "<span class='atributosModalServicioParti'>Direccion:</span> " + user.direccion + "<br>" +
                "<span class='atributosModalServicioParti'>Estatus:</span> " + user.estatus;

        });

        celdaNombre.appendChild(enlaceNombre);


        var celdaNombreA = document.createElement("td");
        celdaNombreA.textContent = user.nombre;

        var celdaCorreo = document.createElement("td");
        celdaCorreo.textContent = user.correo;

        var celdaDocumento = document.createElement("td");
        celdaDocumento.textContent = user.documento;

        var celdaDireccion = document.createElement("td");
        celdaDireccion.textContent = user.direccion;

        var celdaEstatus = document.createElement("td");
        celdaEstatus.textContent = user.estatus;

        // Crear la celda de acciones y agregar los botones
        var celdaAcciones = document.createElement("td");

        var botonEditar = document.createElement("a");
        botonEditar.href = "#modalModificarPrestador";
        botonEditar.textContent = "Modificar"
        botonEditar.className = "btn";
        botonEditar.style.backgroundColor = "rgb(43, 89, 195,1)";
        botonEditar.style.color = "white";
        botonEditar.style.marginRight = "10px";
        botonEditar.setAttribute("data-toggle", "modal");
        //botonEditar.innerHTML = '<i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i>';


        celdaAcciones.appendChild(botonEditar);


        // Agregar las celdas a la fila
        fila.appendChild(celdaNombre);
        fila.appendChild(celdaNombreA);
        fila.appendChild(celdaCorreo);
        fila.appendChild(celdaDocumento);
        fila.appendChild(celdaDireccion);
        fila.appendChild(celdaEstatus);
        fila.appendChild(celdaAcciones)
        // Agregar la fila al cuerpo de la tabla
        tbody.appendChild(fila);
    })(consumidores[i]);
}

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
