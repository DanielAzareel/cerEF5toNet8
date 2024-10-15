jQuery(this).ready(function () {
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaSellado_barraAcciones").html(htmlAcciones);
    $q("#idDivAccionesTabla").remove();
    $q.ajax({
        type: "POST",
        cache: false,
        data: { filtro: getFiltros(), pagina: 1, bloque: 10 },
        url: "/Certificado/Paginar"
    }).done(function (partialViewResult) {
        $q("#idTablaSolicitudes_lista").html(partialViewResult);
        $q("#divLoading").hide();
        iniciarTabla("idTablaSolicitudes", "/Certificado/IniciarCriteriosConsulta");
    });
});

function getFiltros(idTabla) {
    if (idTabla == "idTablaSolicitudes") {
        return {
            lstTipoCertificado: $q("#listSLTipoCertificado").val(),
            lstEstatus: $q("#listSLEstatus").val(),
            lstPlan: $q("#listSLPlan").val(),
            sFolioControl: $q("#sFolioControl").val(),
            sCURP: $q("#sCURP").val(),
            sNombre: $q("#sNombre").val(),
            urlController: "/Certificado/Paginar"
        };
    } else if (idTabla == "idTablaPlantillas") {
        return {
            sIdDocumento: $q("#sIdDocumentoPlantilla").val(),
            sIdPlan: $q("#sIdPlanPlantilla").val(),
            sIdTipoCertificado: $q("#sIdTipoDocumento").val(),
            urlController: "/Certificado/ListadoPlantillas"
        };
    }
    else {
        return {
            sSolId: $q("#txtSolId").val(),
            lstTipoCertificado: $q("#listSLTipoCertificadoCer").val(),
            lstEstatus: $q("#listSLEstatusCer").val(),
            lstPlan: $q("#listSLPlanCer").val(),
            sFolioControl: $q("#sFolioControlCer").val(),
            sCURP: $q("#sCURPCer").val(),
            sNombre: $q("#sNombreCer").val(),
            urlController: "/Certificado/gridCertificadosMonitoreo"
        };
    }
}

function getFiltrosCertificados(solId) {

    return {
        sSolId: $q("#txtSolId").val(),
        lstTipoCertificado: $q("#listSLTipoCertificadoCer").val(),
        lstEstatus: $q("#listSLEstatusCer").val(),
        lstPlan: $q("#listSLPlanCer").val(),
        sFolioControl: $q("#sFolioControl").val(),
        sCURP: $q("#sCURP").val(),
        sNombre: $q("#sNombre").val(),
        urlController: "/Certificado/gridCertificadosMonitoreo"
    };
}

function MostrarListadoCertificados(solId) {
    $q("#txtSolId").val(solId);
    $q("#divLoading").show();
    $q("#idSolicitud").val(solId);
    CargarDatosSolicitud(solId);
}

function CargarDatosSolicitud(solId) {
    $q.ajax({
        type: 'POST',
        url: "/Certificado/DatosSolicitud",
        data: { solId: solId },
        success: function (result) {
            $q("#datosSolicitud").html(result);
            CargarListadoCertificados(solId);
        },
        error: function () {
        }
    });
}

function CargarListadoCertificados(solId) {
    var estatus = '';

    $q.ajax({
        type: 'POST',
        url: "/Certificado/gridCertificadosMonitoreo",
        data: {
            filtro: getFiltros(), pagina: 1, bloque: 10, sSolId: solId
        },
        success: function (result) {
            $q("#idTablaCertificados_lista").html(result);
            iniciarTabla("idTablaCertificados", "/Certificado/CriteriosBusquedaCertificados", { filtro: getFiltrosCertificados(solId), pagina: 100 });
            $q("#divLoading").hide();
            $q("#listadoCertificiados").slideToggle();
            $q("#listadoSolicitudes").slideToggle();
            $q("#divFormCriterios").slideToggle();
            $q("#TablaAccionesSolicitud").html('<button onclick="mostrarListadoSolicitudes()" class="btnAtras colorWhite tit-EstiloBtnAncla">Regresar</button>');
        },
        error: function (result) {
        }
    });
}

function mostrarListadoSolicitudes() {
    $q("#listadoCertificiados").slideToggle();
    $q("#listadoSolicitudes").slideToggle();
    $q("#divFormCriterios").slideToggle();
    $q("#idTablaSolicitudes_btnBuscar").click();
    $q("#idTablaCertificados_barraAccionesGrid").html("");
}

function procesarSolicitud(idSolicitud) {
    $q.ajax({
        type: 'POST',
        url: "/Certificado/ProcesarSolicitud",
        data: { solId: idSolicitud },
        success: function (result) {
            $q("#datosSolicitud").html(result);
            document.getElementById('idTablaSolicitudes_btnBuscar').click();
        },
        error: function () {
        }
    });
}

function onSuccessCancelado(result) {
    $q("#idModalCancelarRegistro").modal("hide");
    onSuccessMensajeFlotanteTabs(result, 2);
    document.getElementById('idTablaCertificados_btnBuscar').click();

}


function setCamposPlantilla(sIdDocumento, sIdPlan, sIdTipoDocumento) {
    $q("#idTablaPlantillas_barraAccionesGrid").empty();
    $q("#idTablaPlantillas_barraAccionesGrid").append($q("#dvAgregarPlantilla").html());
    $q("#sIdDocumentoPlantilla").val(sIdDocumento);
    $q("#sIdPlanPlantilla").val(sIdPlan);
    $q("#sIdTipoDocumento").val(sIdTipoDocumento);
}

function iniciarTablaPlantilla() {
    crearBarrayPaginadoPlantillas1();

}
function crearBarrayPaginadoPlantillas1() {
    iniciarTabla("idTablaPlantillas", "");
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaPlantillas_barraAcciones").html(htmlAcciones);
}
