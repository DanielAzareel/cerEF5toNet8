function OnCompleteGuardarConfigInstitucion(result) {
    onSuccessMensajeFlotanteTabs(result, 2);
}

function getFiltros(idTabla) {

    if (idTabla == "idTablaEtiquetas") {
        return {
            sIdInstitucion: $q("#txtIdInstitucion").val(),
            sIdTipoDocumento: $q("#listSLTipoDocumento").val(),
            urlController: "/ConfiguracionInstitucion/ListadoEtiquetas"
        };
    } else if (idTabla == "idTablaPlantillas") {
        return {
            sIdInstitucion: $q("#txtIdInstitucion").val(),
            sIdTipoDocumento: $q("#listSLTipoDocumento").val(),
            sIdPlan: $q("#listSLPlan").val(),
            urlController: "/ConfiguracionInstitucion/ListadoPlantillas"
        };
    }
    else if (idTabla == "idTablaFirmantes") {
        return {
            urlController: "/ConfiguracionInstitucion/ListadoFirmantes"
        };
    }


}

function getFiltrosEtiquetas() {
    return {
        sIdTipoDocumento: $q("#listSLTipoDocumento").val(),
        sIdInstitucion: $q("#txtIdInstitucion").val(),
        urlController: "/ConfiguracionInstitucion/ListadoEtiquetas"
    };
}

function getFiltrosPlantillas() {
    return {
        sIdTipoDocumento: $q("#listSLTipoDocumento").val(),
        sIdPlan: $q("#listSLPlan").val(),
        sIdInstitucion: $q("#txtIdInstitucion").val(),
        urlController: "/ConfiguracionInstitucion/ListadoEtiquetas"
    };
}

function getFiltrosFirmantes() {
    return {
        sIdTipoDocumento: $q("#listSLTipoDocumento").val(),
        sIdPlan: $q("#listSLPlan").val(),
        sIdInstitucion: $q("#txtIdInstitucion").val(),
        urlController: "/ConfiguracionInstitucion/ListadoFirmantes"
    };
}
function iniciarTablaFirmantes() {
    $q('#modalFirmantes').modal('show');
    crearBarrayPaginadoFirmantes();
}

function iniciarTablaPlantillas() {
    $q('#modalPlantillas').modal('show');
    crearBarrayPaginadoPlantillas();

}

function iniciarTablaEtiquetas() {
    $q('#modalEtiquetas').modal('show');
    crearBarrayPaginado();
}

function crearBarrayPaginado() {
    iniciarTabla("idTablaEtiquetas", "/ConfiguracionInstitucion/CriteriosBusquedaEtiquetas");
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaEtiquetas_barraAcciones").html(htmlAcciones);
}

function crearBarrayPaginadoPlantillas() {
    iniciarTabla("idTablaPlantillas", "/ConfiguracionInstitucion/CriteriosBusquedaPlantilla");
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaPlantillas_barraAcciones").html(htmlAcciones);
}

function crearBarrayPaginadoFirmantes() {
    iniciarTabla("idTablaFirmantes", "");
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaFirmantes_barraAcciones").html(htmlAcciones);
}

function onHideModalEtiquetas() {
    $q("#idTablaEtiquetas_barraAccionesGrid").html('');

}

function onHideModalPlantillas() {
    $q("#idTablaPlantillas_barraAccionesGrid").empty();
    $q("#idTablaPlantillas_barraAccionesGrid").append($q("#dvAgregarPlantilla").html());
}

function onHideModalFirmantes() {
    $q("#idTablaFirmantes_barraAccionesGrid").empty();
    $q("#idTablaFirmantes_barraAccionesGrid").append($q("#dvAgregarFirmantesHide").html());
}

function EditarEtiqueta(id, valor) {
    $q("#lbl" + id).hide();
    $q("." + id).show();
    $q("." + id).val(valor);

}

function CancelarEdicionEtiqueta(id) {
    $q("." + id).hide();
    $q("#lbl" + id).show();
    $q("." + id + "MsjValidacion").empty();
    $q("." + id).removeClass('input-validation-error');

}

function successEditarEtiqueta(result) {
    if (result[0] == "True") {
        $q("#idTablaEtiquetas_btnBuscar").click();
    }
    onSuccessMensajeFlotanteTabs(result, 2);
}

function MostrarAgregarPlantilla() {
    $q("#idModalBodyConcentradora").hide('swing');
    $q("#dvEditarPlantilla").show('swing');

    $q("#btnCancelarPlantilla").show();
    $q("#btnGuardarPlantilla").show();
    $q("#btnCerrarModal").hide();

    $q("#lblTituloModalPlantillas").text("Configuración de plantillas > Agregar plantilla");
}

function MostrarAgregarFirmante() {
    $q("#idModalBodyFirmantes").hide('swing');
    $q("#dvEditarFirmantes").show('swing');

    $q("#btnCancelarFirmante").show();
    $q("#btnGuardarFirmante").show();
    $q("#btnCerrarModalFirmante").hide();

    $q("#lblTituloModalFirmantes").text("Configuración de firmantes > Agregar firmante");
}

function successAgregarPlantilla(result) {
    //if (result[0] == "True") {
    //    $q("#idTablaEtiquetas_btnBuscar").click();
    //}
    onSuccessMensajeFlotanteTabs(result, 2);
}

function agregarPlantilla() {
    if ($q('#formDatosPlantilla').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'ConfiguracionInstitucion/AgregarPlantilla',
            type: 'POST',
            data: new FormData($q("#formDatosPlantilla")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            success: function (result) {
                $q("#divLoading").hide();
                if (result[0] == "True") {
                    RegresarAgregarPlantilla();
                    $q("#idTablaPlantillas_btnInicio").click();
                }
                onSuccessMensajeFlotanteTabs(result, 2);
            },
            error: function () {

            }
        });
    }
}

function EditarPlantilla() {
    if ($q('#formDatosPlantilla').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'ConfiguracionInstitucion/AgregarPlantilla',
            type: 'POST',
            data: new FormData($q("#formDatosPlantilla")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            success: function (result) {
                $q("#divLoading").hide();
                if (result[0] == "True") {
                    SuccessEditarPlantilla();
                    $q("#idTablaPlantillas_btnInicio").click();
                }
                onSuccessMensajeFlotanteTabs(result, 2);
            },
            error: function () {

            }
        });
    }
}

function EditarFirmante() {
    if ($q('#formDatosFirmante').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'ConfiguracionInstitucion/AgregarEditarFirmante',
            type: 'POST',
            data: new FormData($q("#formDatosFirmante")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            success: function (result) {
                $q("#divLoading").hide();
                if (result[0] == "True") {
                    SuccessEditarFirmante();
                    $q("#idTablaFirmantes_btnInicio").click();
                }
                onSuccessMensajeFlotanteTabs(result, 2);
            },
            error: function () {

            }
        });
    }
}

function RegresarAgregarPlantilla() {
    $q("#divFormularioEditarPlantilla").hide('swing');
    $q("#idModalBodyConcentradora").show('swing');

    $q("#btnCancelarPlantilla").hide();
    $q("#btnGuardarPlantilla").hide();
    $q("#btnCerrarModal").show();

    $q("#lblTituloModalPlantillas").text("Configuración de plantillas");


}

function successViewEditarPlantilla() {
    $q("#idModalBodyConcentradora").hide('swing');
    $q("#dvEditarPlantilla").show('swing');

    $q("#btnCancelarPlantilla").show();
    $q("#btnGuardarPlantilla").show();
    $q("#btnCerrarModal").hide();

    $q("#lblTituloModalPlantillas").text("Configuración de plantillas > Editar plantilla");
}

function successViewEditarFirmante() {
    $q("#idModalBodyFirmantes").hide('swing');
    $q("#dvEditarFirmantes").show('swing');

    $q("#btnCancelarFirmante").show();
    $q("#btnGuardarFirmante").show();
    $q("#btnCerrarModalFirmante").hide();

    $q("#lblTituloModalFirmantes").text("Configuración de firmantes > Editar firmante");
}

function SuccessEditarPlantilla() {
    $q("#dvEditarPlantilla").hide('swing');
    $q("#idModalBodyConcentradora").show('swing');

    $q("#btnCancelarPlantilla").hide();
    $q("#btnGuardarPlantilla").hide();
    $q("#btnCerrarModal").show();

    $q("#lblTituloModalPlantillas").text("Configuración de plantillas");

}

function SuccessEditarFirmante() {
    $q("#dvEditarFirmantes").hide('swing');
    $q("#idModalBodyFirmantes").show('swing');

    $q("#btnCancelarFirmante").hide();
    $q("#btnGuardarFirmante").hide();
    $q("#btnCerrarModalFirmante").show();

    $q("#lblTituloModalFirmantes").text("Configuración de firmantes");

}

function successEliminarPlantilla(result) {
    if (result[0] == "True") {
        $q("#idTablaPlantillas_btnInicio").click();
    }
    onSuccessMensajeFlotanteTabs(result, 2);
}

function successEliminarFirmante(result) {
    if (result[0] == "True") {
        $q("#idTablaFirmantes_btnInicio").click();
    }

    onSuccessMensajeFlotanteTabs(result, 2);
}

function SuccessAsignarPlantilla(result) {
    if (result[0] == "True") {
        $q("#idTablaPlantillas_btnInicio").click();
    }
    onSuccessMensajeFlotanteTabs(result, 2);
}

function SuccessAsignarFirmante(result) {
    if (result[0] == "True") {
        $q("#idTablaFirmantes_btnInicio").click();
        GetFirmantePredeterminado();
    }
    onSuccessMensajeFlotanteTabs(result, 2);
}

function obtenerNuevoGUID(id) {
    $q.ajax({
        contentType: 'text/plain; charset=utf-8',
        url: 'ConfiguracionInstitucion/GenerarGUID',
        dataType: "text",
        success: function (result) {
            $q("#" + id).val(result);
        },
    }).done(
    );
}
function successGuardarConfigInstitucion(result) {
    onSuccessMensajeFlotanteTabs(result, 2);
}

function GetFirmantePredeterminado() {
    $q.ajax({
        url: 'ConfiguracionInstitucion/ObtenerFirmantePredeterminado',
        type: 'POST',
        data: {},
        processData: false,
        contentType: false,
        success: function (result) {
            $q("#lblFirmantePredeterminado").html('<a class="cer-EstiloBtnAncla btnDescargar" download="download" href="/ConfiguracionInstitucion/DescargarCertificado?firId=' + result[2] + '&amp;insId=' + result[1] + '" title="Descargar certificado">' + result[0] + '</a>');
            
        },
        error: function () {

        }
    });
}