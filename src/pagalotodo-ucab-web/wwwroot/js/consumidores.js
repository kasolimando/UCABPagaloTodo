
// Datos de ejemplo
var consumidores = [
    { username: "miaRenaldi", nombre: "Mia Termopolis", correo: "genovia@gmail.com", documento: "v-1234", direccion: "Genovia", estatus: "Activo" },
    { username: "badBunny", nombre: "Benito", correo: "bellaca@gmail.com", documento: "v-456", direccion: "Miami", estatus: "Inactivo" },
    { username: "miaRenaldita", nombre: "Mia Termopolis", correo: "genovia@gmail.com", documento: "v-1234", direccion: "Genovia", estatus: "Activo" },

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
        enlaceNombre.href = "#detalleUserModal";
        enlaceNombre.setAttribute("data-toggle", "modal");
        enlaceNombre.textContent = user.username;
        enlaceNombre.addEventListener("click", function (event) {
            // Obtener la referencia al modal y sus elementos
            var modal = document.getElementById("detalleUserModal");
            var modalTitle = document.getElementById("detalleUserModalLabel");
            var modalInfo = document.getElementById("detalleUserInfo");


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
        botonEditar.href = "#modalModificarConsu";
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
