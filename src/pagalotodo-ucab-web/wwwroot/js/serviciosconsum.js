let mostrador = document.getElementById("mostrador");
let seleccion = document.getElementById("seleccion");
let servicioseleccionado = document.getElementById("servicio");
let descripseleccionada = document.getElementById("descripcionModal");
let precioseleccionado = document.getElementById("precioModal");
let inputServicio = document.getElementById("servicioSeleccionado");

function cargar(item,servicio) {
    quitarBordes();
    mostrador.style.width = "100%"; // Cambia el ancho del mostrador para ocupar todo el espacio
    seleccion.style.display = "block"; // Muestra el modal
    item.style.border = "2px solid rgb(255, 167, 15)";
    servicioseleccionado.innerHTML = item.getElementsByClassName("descripcion")[0].innerHTML;
    descripseleccionada.innerHTML = "<label class='popServicio'>Descripción:</label> " + item.getElementsByClassName("nombre")[0].innerHTML;
    precioseleccionado.innerHTML = "<label class='popServicio'>Monto:</label> " + item.getElementsByClassName("monto")[0].innerHTML;
    precioseleccionado.style.color = "black";
    descripseleccionada.style.color = "black";
    var precioInput = document.getElementById("Pago.Monto");
    inputServicio.value = servicio;
    if (item.getElementsByClassName("tipo")[0].innerHTML === "confirmacion") {
        var montoNumerico = parseFloat(item.getElementsByClassName("monto")[0].innerHTML); // 10.5
        precioInput.value = montoNumerico;
        precioInput.readOnly = false;
    } else {
        precioInput.readOnly = false;
    }
    seleccion.style.zIndex = 1;
}

function cerrar() {
    mostrador.style.width = "100%";
    seleccion.style.display = "none"; // Oculta el modal
    quitarBordes();
}


function quitarBordes() {
    var items = document.getElementsByClassName("item");
    for (i = 0; i < items.length; i++) {
        items[i].style.border = "none";
    }
}


