function hideDivLoading() {
    $q("#divLoadingMensaje").text("")
    $q("#divLoading").hide(10)
}

function onSuccessMensajeFlotanteTabs(result, numNiveles) {

    if (result[0] === "True") {
        alertify.success(result[1])
    } else {
        if (result[1].length > 1) {
            alertify.error(result[1])
        } else {
            alertify.error('No se realizó la acción.');
        }
    }
    if (numNiveles === 1) {
        $q(".ajs-top").css("top", "195px");
    } else if (numNiveles === 2) {
        $q(".ajs-top").css("top", "237px");
    } else if (numNiveles == 0) {
        $q(".ajs-top").css("top", "5px");
    }

    hideDivLoading();
}
function CambiarPerfil() {
    var sValor = $q("#cboRoles").val();
    $q.ajax({
        type: 'POST',
        url: "/Home/CambiarPerfil",
        data: { sValor: sValor },
        success: function (result) {
            if (result) {
                document.getElementById('btnInicio').click();
            } else {
                alertify.error('No se realizó la acción.');
            }
        },
        error: function (result) {
            alertify.success(result);
        }
    });
}

function checarSesion(result) {
    if (result.toString().search('window.location.replace') > 0) {
        // alert('Finalizó tu sesión')
        var redirect = result.toString().split("'");
        alertify.dismissAll();
        alertify.alert('Finalizó tu sesión.', function () { window.location.replace(redirect[1]); }).setHeader('<section style="font-size: 14px" class="colorAzulGto" warning >Atención</section> ');
        alertify.dismissAll();
        return false;
    } else {
        $q("#idFormSelladoConcentradora").submit();
    }
}