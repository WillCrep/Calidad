import Swal from "./sweetalert2";

/*$("#guardar").click(function (e) {
    swal("OK");
})*/

function go2() {
   
    swal({
        title: "Pagado",
        text: "Pago realizado exitosamente.",
        icon: "success",
        button: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                //redirect
                var efectivo = $('#tEfectivo').val();
                var vuelto = $('#tVuelto').val();
                window.location.href = "/Pago/PagarDeuda/?efec="+  encodeURIComponent(efectivo) + "&vuelt="+ encodeURIComponent(vuelto);
            }
        });
};

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
};

function val1() {
    var myJs = $('#to').val();
    var tefec = $('#tEfectivo').val();
    if (tefec >= myJs) {
        $('#pDeu').attr("disabled", false);
        document.getElementById('errorEfe').style.display = 'none';
        var vuel = Math.round((tefec - myJs) * 100) / 100;
        $('#tVuelto').val(vuel);

    } else {
        $('#pDeu').attr("disabled", true);
        document.getElementById('errorEfe').style.display = 'block';
    }
};

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