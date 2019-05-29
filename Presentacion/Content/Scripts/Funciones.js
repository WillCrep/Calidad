/*$("#guardar").click(function (e) {
    swal("OK");
})*/

function go() {
    swal({
        title: "Guardado",
        text: "Se guardó el préstamo del cliente y el cronograma de pagos",
        icon: "success",
        button: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                //redirect
                var url = '@Url.Action("GuardarCronograma","Prestamo")';
                window.location.href = "/Prestamo/GuardarCronograma/";
            }
        });
}
/*$("#bus").click(function (e) {
    swal({
        title: "Deseas ser redirigido",
        text: "",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                //redirect
                window.location.href = "http://www.w3schools.com";
            } else {
                swal("Operacion cancelada !!");
            }
        });
});*/