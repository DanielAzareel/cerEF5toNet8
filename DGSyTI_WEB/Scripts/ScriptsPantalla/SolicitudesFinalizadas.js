function mostrarListadoTitulos(solId) {
    $q("#divLoading").show()
    $q("#idSolicitud").val(solId)
    cargarDatosSolicitud(solId)
}
function mostrarListadoSolicitudes() {
    $q("#listadoTitulos").slideToggle()
    $q("#listadoSolicitudes").slideToggle()
    $q("#divFormCriterios").slideToggle()
    $q("#idTablaTitulos_barraAccionesGrid").html("")
}

jQuery(this).ready(function () {
    iniciarTabla("idTablaSolicitudes", "/Titulo/criterios");
});

function getFiltros(idTabla) {
    if (idTabla == "idTablaSolicitudes")
        return {
            listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControl").val(),
            curp: $q("#curp").val(),
            urlController: "/Titulo/gridSolicitudes"
        }

    if (idTabla == "idTablaTitulos")
        return {
            //listInstitucionBusqueda: [$q("#comboInstitucion").val()],
            //listCarrerasBusqueda: $q("#comboCarrera").val(),
            folioControl: $q("#folioControlTitulo").val(),
            curp: $q("#curpTitulo").val(),
            idSolicitud: $q("#idSolicitud").val(),
            estatus: $q("#comboEstatus").val(),
            urlController: "/Titulo/gridTitulos"
        }
}
function cargarListadoTitulos(solId) {
    var estatus = '4,5,6,7,8'
    $q.ajax({
        type: 'POST',
        url: "/Titulo/gridTitulos",
        data: {
            filtro: "{idSolicitud:'" + solId + "', estatus:'" + estatus +"'}", pagina: 1, bloque: 10
        },
        success: function (result) {
            $q("#idTablaTitulos_lista").html(result)
            iniciarTabla("idTablaTitulos", "/Titulo/criteriosTitulos", { estatus: estatus })
            $q("#divLoading").hide();
            $q("#listadoTitulos").slideToggle()
            $q("#listadoSolicitudes").slideToggle()
            $q("#divFormCriterios").slideToggle()
            $q("#TablaAccionesSolicitud").html('<button onclick="mostrarListadoSolicitudes()" class="btnAtras colorWhite tit-EstiloBtnAncla">Regresar</button>')
        },
        error: function (result) {
            alertify.success(result);
        }
    });
}
function cargarDatosSolicitud(solId) {
    $q.ajax({
        type: 'POST',
        url: "/Titulo/datosSolicitud",
        data: { solId: solId },
        success: function (result) {
            $q("#datosSolicitud").html(result)
            cargarListadoTitulos(solId)
        },
        error: function () {
        }
    });
}

function onSuccessCancelado(result) {
    $q("#idModalCancelarRegistro").modal("hide")
    onSuccessMensajeFlotanteTabs(result, 2)
}

function onSuccessCorreo(result) {
    $q("#idModalCorreo").modal("hide")
    onSuccessMensajeFlotanteTabs(result, 2)
}
function onSuccessPlantilla(result) {
    $q("#idModalCambiarPlantilla").modal("hide")
    onSuccessMensajeFlotanteTabs(result, 2)
}