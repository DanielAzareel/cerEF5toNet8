jQuery(this).ready(function () {
    $q(document).ajaxStart(bloqueo).ajaxStop(desbloqueo);
    iniciarTabla("idTablaSolicitudes", "/Titulo/criterios");
});

function mostrarListadoTitulos(solId) {
    $q("#divLoading").show();
    $q("#idSolicitud").val(solId);
    cargarDatosSolicitud(solId);
}
function mostrarListadoSolicitudes() {
    $q("#listadoTitulos").slideToggle();
    $q("#listadoSolicitudes").slideToggle();
    $q("#divFormCriterios").slideToggle();
    $q("#idTablaSolicitudes_btnBuscar").click();
    $q("#idTablaTitulos_barraAccionesGrid").html("")
}

function cargarDatosSolicitud(solId) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/datosSolicitud",
        data: { solId: solId },
        success: function (result) {
            $q("#datosSolicitud").html(result);
            cargarListadoTitulos(solId);
        },
        error: function () {
        }
    });
}


function bloqueo() {
    $q("#divLoading").show();
}

function desbloqueo() {
    $q("#divLoading").hide();
}


function getFiltros(idTabla) {
    if (idTabla == "idTablaSolicitudes")
        return {
            listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControl").val(),
            curp: $q("#curp").val(),
            urlController: "/Titulo/gridSolicitudesProcesoSEP"
        }

    if (idTabla == "idTablaTitulos")
        return {
            //listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            //listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControlTitulo").val(),
            curp: $q("#curpTitulo").val(),
            idSolicitud: $q("#idSolicitud").val(),
            estatus: $q("#comboEstatus").val(),
            urlController: "/Titulo/gridTitulosMonitoreo"
        }
}
function cargarListadoTitulos(solId) {
    var estatus = '3,4,5,6,7,8'
    $q.ajax({
        type: 'POST',
        url: "/Titulo/gridTitulosMonitoreo",
        data: {
            filtro: "{idSolicitud:'" + solId + "', estatus:'" + estatus + "'}", pagina: 1, bloque: 10
        },
        success: function (result) {
            $q("#idTablaTitulos_lista").html(result);
            iniciarTabla("idTablaTitulos", "/Titulo/criteriosTitulos", { estatus: estatus });
            $q("#divLoading").hide();
            $q("#listadoTitulos").slideToggle();
            $q("#listadoSolicitudes").slideToggle();
            $q("#divFormCriterios").slideToggle();
            $q("#TablaAccionesSolicitud").html('<button onclick="mostrarListadoSolicitudes()" class="btnAtras colorWhite tit-EstiloBtnAncla">Regresar</button>')
        },
        error: function (result) {
            alertify.success(result);
        }
    });
}

function mostrarModalTitulo() {
    $q('#divModalTitulo').modal('show');
}
function procesarSolicitud(idSolicitud) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/ProcesarSolicitud",
        data: { solId: idSolicitud },
        success: function (result) {
            $q("#datosSolicitud").html(result);
            document.getElementById('idTablaSolicitudes_btnBuscar').click();
        },
        error: function () {
        }
    });
}

function MostrarModalConcluirSolicitud() {
    $q('#idModalConcluirSolicitud').modal('show');
}

function ConcluirDocumento(docId) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/ConcluirDocumento",
        data: { sDocId: docId },
        success: function (result) {
            if (result) {
                alertify.success('Se concluyó el documento correctamente.');
                document.getElementById('idTablaTitulos_btnBuscar').click();
            } else {
                alertify.success('Ocurrió un erro al concluir el documento.');
            }
        },
        error: function () {
        }
    });
}

function ConcluirSolicitud(solId) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/ConcluirSolicitud",
        data: { sSolId: solId },
        success: function (result) {
            if (result) {
                alertify.success('Se concluyó la solicitud correctamente.');
                document.getElementById('idTablaSolicitudes_btnBuscar').click();
            } else {
                alertify.success('Ocurrió un erro al concluir la solicitud.');
            }
        },
        error: function () {
        }
    });
}

function ResolverSolicitud(idSolicitud) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/ResolverSolicitud",
        data: { sSolId: idSolicitud },
        success: function (result) {
            $q("#idModalResolverSolicitudDetalle").html(result);
            $q("#idModalResolverSolicitud").modal("show");

        },
        error: function () {
        }
    });
}

function OnSuccesResolverSolicitud(data) {
    if (data) {
        alertify.success("Se realizó la acción correctamente.");
        $q("#btnCerrarREsolverSolicitud").click();
        document.getElementById('idTablaSolicitudes_btnBuscar').click();
    } else {
        alertify.error("Ocurrió un error.");
    }
}

function ValidarRadioButton() {
    if ($q('input[name="radioOpcion"]').is(':checked')) {
        $q("#dvMensajeValidación").hide();
        document.getElementById("btnSubmitResolverSolicitud").click();
    } else {
        $q("#dvMensajeValidación").show();
        
    }
}

function SeleccionOpcionResolverSolicitud() {
    if ($q('input[name="radioOpcion"]').is(':checked')) {
        $q("#dvMensajeValidación").hide();
    } else {
        $q("#dvMensajeValidación").show();
    }
}