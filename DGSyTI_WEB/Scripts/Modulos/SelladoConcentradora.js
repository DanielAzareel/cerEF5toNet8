

var totalDocumentos = "";

//Script para mostrar u ocultar las tablas de documentos y solicitudes.
function ToogleTablas() {
    $q("#divFormCriterios").slideToggle();
    $q("#idDivTablaPrincipal").slideToggle();
    $q("#idDivMostrarDetalleSolicitud").slideToggle();
    $q("#idTablaTitulos_barraAccionesGrid").html("")
}

//Actualiza la cantidad de documentos por solicitud y manda a llamar la funcion ToogleTablas.
function ToogleTablasRegresar() {
    //var registrosSolicitud = $q('#idTablaTitulos tbody tr').length;
    //$q("#idTdTotalDocumentos-" + solicitudSeleccionada).html(totalSolicitudesActual);

    ToogleTablas();
}


function dataTableDocumentos() {
    //Script para crear el datatable de documentos
    iniciarTabla("idTablaTitulos", "../Titulo/criteriosTitulos", { estatus: '2' });

    var htmlAcciones = $q("#idDivBtnRegresar").html();
    $q("#TablaAccionesSolicitud").html(htmlAcciones);
    $q("#idDivBtnRegresar").hide();

    $q("#divLoading").hide();
}

//Script para crear el datatable.
jQuery(this).ready(function () {
    iniciarTabla("idTablaSolicitudes", "../Titulo/criterios");
});

function getFiltros(idTabla) {
    if (idTabla === "idTablaSolicitudes")
        return {
            listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControl").val(),
            curp: $q("#curp").val(),
            urlController: "../Titulo/FiltroSolicitudesConcentradora"
        };

    if (idTabla === "idTablaTitulos")
        return {
            listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControlTitulo").val(),
            curp: $q("#curpTitulo").val(),
            idSolicitud: $q("#idSolicitud").val(),
            estatus: $q("#comboEstatus").val(),
            urlController: "../Titulo/FiltroTitulosConcentradora"
        };
}

//Funcion que inserta el folio en un hidden del registro seleccionado.
function PasarSolicitud(folio) {
    $q("#IdSolicitud").val(folio);
}

//Script utilizado para envio de formulario de sellado.
$q(document).ready(function () {
    $q("#idFormSelladoConcentradora").submit(function (event) {
        var form = $q(this);
        $q('#divLoading').show();
        $q('#idFiltros').val(JSON.stringify(getFiltros("idTablaSolicitudes")));


        form.ajaxSubmit({
            dataType: 'JSON',
            type: 'POST',
            url: form.attr('action'),
            success: function (mensaje) {
                if (mensaje[0] === "true") {
                    LimpiarCamposSellado();

                    $q("#idModalSelladoConcentradora").modal("hide");
                    $q("body").toggleClass("modal-open");
                    $q("div.modal-backdrop").toggleClass("modal-backdrop fade show");

                    alertify.alert(mensaje[1]).setHeader('<section style="text-align: left;font-size: 14px" class="colorAzulGto headerWarning">Atención</section>');
                    CrearMiCookieTab("TabsEtapasSellado");
                } else {
                    alertify.alert(mensaje[1]).setHeader('<section style="text-align: left;font-size: 14px" class="colorAzulGto headerWarning">Atención</section>');
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
    if ($q(event.target).closest('#idModalSelladoConcentradora').length && $q(event.target).is('#idModalSelladoConcentradora')) {
        LimpiarCamposSellado();
    }
});

function cargarDatosSolicitud(solicitud) {
    $q.ajax({
        type: 'POST',
        url: "../Titulo/datosSolicitud",
        data: { solId: solicitud },
        success: function (result) {
            $q("#datosSolicitud").html(result);

        },
        error: function () {
        }
    });
}

function MostrarDetalleSolicitud(solicitud) {
    $q('#divLoading').show();
    solicitudSeleccionada = solicitud;
    $q("#idSolicitud").val(solicitud);
    cargarDatosSolicitud(solicitud);
    //totalDocumentos = $q("#idTdTotalDocumentos-" + solicitudSeleccionada).html();
}
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
        $q("#idFormSelladoConcentradora").submit();
    }
}