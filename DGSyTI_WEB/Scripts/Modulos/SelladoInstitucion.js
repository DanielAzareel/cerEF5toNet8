

jQuery(this).ready(function () {
    iniciarTabla("idTablaTitulos", "../Titulo/criterios");

    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaTitulos_barraAcciones").html(htmlAcciones);
    $q("#idDivAccionesTabla").remove();
});

function getFiltros(idTabla) {
    return {

        listInstitucionBusqueda: [$q("#comboInstitucion").val()],
        listCarrerasBusqueda: $q("#comboCarrera").val(),
        folioControl: $q("#folioControl").val(),
        curp: $q("#curp").val(),
        urlController: "../Titulo/FiltroTitulosInstitucion"
    };
}

//Script utilizado para envio de formulario de sellado.
$q(document).ready(function () {
    $q("#idFormSelladoInstitucion").submit(function (event) {
        var form = $q(this);
        $q('#divLoading').show();
        $q('#idFiltros').val(JSON.stringify(getFiltros()));

        form.ajaxSubmit({
            dataType: 'JSON',
            type: 'POST',
            url: form.attr('action'),
            success: function (mensaje) {
                if (mensaje[0] === "true") {
                    LimpiarCamposSellado();
                    $q("#idModalSelladoInstitucion").modal("hide");
                    $q("body").toggleClass("modal-open");
                    $q("div.modal-backdrop").toggleClass("modal-backdrop fade show");

                    alertify.alert(mensaje[1]).setHeader('<section class="headerAlert text-left" warning >Atención</section>');
                    CrearMiCookieTab("TabsEtapasSellado");
                } else {
                    alertify.alert(mensaje[1]).setHeader('<section class="headerAlert text-left" warning >Atención</section>');
                }

                $q('#divLoading').hide();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "_>" + textStatus + "_>" + errorThrown);
                $q('#divLoading').hide();
            }
        });

        return false;
    });
});

function LimpiarCamposSellado() {
    $q("#idArchivoKey").val("");
    $q("#idContrasenia").val("");
}

$q('body').click(function (event) {
    if ($q(event.target).closest('#idModalSelladoInstitucion').length && $q(event.target).is('#idModalSelladoInstitucion')) {
        LimpiarCamposSellado();
    }
});

function revisarchecarSesion() {

    $q.ajax({
        async: true,
        type: "POST",

        url: '/Home/ChecarSesionAjax',
        success: function (result) {

            checarSesion(result);

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
        $q("#idFormSelladoInstitucion").submit();
    }
}