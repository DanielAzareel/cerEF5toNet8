
function cerrarModalConcentradora() {
    $q('#idModalConcentradora').modal('hide')
    $q("#divLoading").show()
}

function onSuccessConcentradora(result) {
    $q('#idModalConcentradora').modal('hide')
    limpiarCamposConcentradora()
    actualizarListado()
    onSuccessMensajeFlotanteTabs(result, 2)
}

// Esta función se manda llamar desde la propiedad onclick del botón del form modal.
// De esta forma se ejecuta antes del form onBegin y onSuccess, para moder asignar valor en encryptAES()
// También por esta función mandamos el form con el input file de plantilla
function procesar() {

    procesarFormulario();

    encryptAES();
}

function CancelarCaptura() {
    $q("#divFormularioEditarPlantilla").html("");
}

function procesarFormulario() {
    if ($q('#formConcentradora').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'Concentradora/GuardarConfigConcentradora',
            type: 'POST',
            data: new FormData($q("#formConcentradora")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            beforeSend: function () {
            },
            success: function (result) {
                $q("#divLoading").hide();
                cerrarModalConcentradora();
                limpiarCamposConcentradora()
                actualizarListado();
                onSuccessMensajeFlotanteTabs(result, 2);
            },
            error: function () {

            }
        });

    } //valid
}


function actualizarPlantilla() {
    if ($q('#formDatosPlantilla').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'Concentradora/ActualizarPlantilla',
            type: 'POST',
            data: new FormData($q("#formDatosPlantilla")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            success: function (result) {
                $q("#divLoading").hide();

                onSuccessMensajeFlotanteTabs(result, 2);

                if (result[0] === "True") {
                    ObtenerPlantillaPredeterminada();
                    $q("#divFormularioEditarPlantilla").html("");
                    $q("#idTablaPlantillas_btnInicio").click();
                }
            },
            error: function () {

            }
        });

    } //valid
}


function agregarPlantilla() {
    if ($q('#formDatosPlantilla').valid()) {
        $q("#divLoading").show();
        $q.ajax({
            url: 'Concentradora/Agregarplantilla',
            type: 'POST',
            data: new FormData($q("#formDatosPlantilla")[0]),//permite pasar el formulario y el input type=file
            processData: false,
            contentType: false,
            success: function (result) {
                $q("#divLoading").hide();
                $q("#idTablaPlantillas_btnInicio").click();
                onSuccessMensajeFlotanteTabs(result, 2);
                $q("#divFormularioEditarPlantilla").html("");
            },
            error: function () {

            }
        });

    } //valid
}



function actualizarListado(opcion = 0) {

    $q.ajax({
        type: 'POST',
        url: "Concentradora/ActualizarListado",
        //data: { dato: "ssss" },
        //success: function (result) { alert(result)},
        success: function (result) {
            //$q('#tipAcuerdoAnterior').attr('value', acuerdo)
            //$q('#usuarioAnterior').attr('value', usuario)
            //$q('#docAnterior').attr('value', claveDoc)
            $q('#seccionListadoConcentradoras').html(result)
            //$q('#tipAcuerdoAnterior').attr("Value", acuerdo)
            //$q('#estatusAnterior').attr("Value", estatus)
            $q("#divLoading").hide(10)
            //$q("#TablaLista").attr("data-Actual", $q("#pagina").val());
            //cargaPaginacion()
        },
        error: function () {
            $q("#divLoading").hide(10)
        }
    });
}



function obtenerNuevoGUID(id) {
    $q.ajax({
        contentType: 'text/plain; charset=utf-8',
        url: 'Concentradora/GenerarGUID',
        dataType: "text",
        success: function (result) {
            $q("#" + id).val(result);
        },
    }).done(
    );
}

function encryptAES() {
    var txtpassword = $q('#conContrasenaWS').val();
    if (txtpassword !== "12345678") {
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

        var encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtpassword), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        $q('#conContrasenaWSEncript').val(encryptedPassword);
    }
}

function limpiarCamposConcentradora() {
    $q("#conId").val("");
    $q("#conFolio").val("");
    $q("#conUsuarioWS").val("");
    $q("#conContrasenaWS").val("");
    $q("#conTokenSeguridad").val("");
    $q("#conFirmaInsitucion").val("");
    $q("#conNotificacionProfesionista").val("");

}

function ValidarArchivo(inputArchivoId) {
    var bBandera = true;
    if ($q("#" + inputArchivoId).val() !== "") {
        if ($q("#" + inputArchivoId)[0].name !== "") {
            var ext = $q("#" + inputArchivoId).val().split('.').pop();
            switch (ext) {
                case 'docx':
                    var fileSize = $q("#" + inputArchivoId)[0].files[0].size;
                    if (fileSize > 2097152) {
                        alertify.error("El archivo no debe superar 2 MB.");
                        $q("#" + inputArchivoId).val('');
                        bBandera = false;
                    }
                    break;
                default:
                    alertify.error('Solo se permiten archivos con extensión ".docx".');
                    $q("#" + inputArchivoId).val('');
                    bBandera = false;
            }
        }
    } else {
        alertify.error('Favor de seleccionar un archivo con extensión ".docx".');
        bBandera = false;
    }
    return bBandera;
}
function DefinirPlantillaPredeterminada(id) {
    $q.ajax({
        method: "POST",
        url: 'Concentradora/DefinirPlantillaPredeterminada',
        data: { plantilla: id },
        success: function (result) {
            onSuccessMensajeFlotanteTabs(result, 2);
            if (result[0] === "True") {
                $q('#idTablaPlantillas_lista input[type="checkbox"]').prop("checked", false);
                $q('#' + id).prop("checked", true);
                $q("#idTablaPlantillas_btnInicio").click();
                ObtenerPlantillaPredeterminada();
            } else {
                $q('#' + id).prop("checked", false);
            }
        }
    }).done(
    );
}


function EliminarPlantilla(id) {

    alertify.confirm("Confirmar", "Continuar con la eliminación de la plantilla.",
        function () {
            $q.ajax({
                method: "POST",
                url: 'Concentradora/EliminarPlantilla',
                data: { plantilla: id },
                success: function (result) {
                    onSuccessMensajeFlotanteTabs(result, 2);
                    $q("#idTablaPlantillas_btnInicio").click();
                }
            }).done(
            );

        }, function () {
        }).setHeader('<section class="headerAlert" warning>Atención </section> ');


}


function editardatosPlantilla(id) {
    $q.ajax({
        method: "POST",
        url: 'Concentradora/DatosPlantilla',
        data: { idPlantilla: id },
        success: function (result) {
            $q("#divFormularioEditarPlantilla").html(result);

        }
    }).done(
    );
}

function ObtenerPlantillaPredeterminada() {
    $q.ajax({
        method: "POST",
        url: 'Concentradora/PlantillaPredeterminada',
        success: function (result) {
            $q("#aPlantillaPredeterminada").text(result[0]);
            $q("#aPlantillaPredeterminada").attr("href", "/Concentradora/DescargarPlantilla?plantillaId=" + result[1]);
        }
    }).done(
    );
}

function validaCambioFlujo() {
    $q.ajax({
        method: "POST",
        url: 'Concentradora/TitulosPendientes',
        success: function (result) {
            
            if (result) {
                
                $q('#conFirmaInsitucion').prop("checked", !$q('#conFirmaInsitucion').is(':checked'));
                alertify.error("Hay registros pendientes por sellar con la configuración actual.");
            }
        }
    }).done(
    );
}

function OnCompleteGuardarConfigConcentradora(result) {

    onSuccessMensajeFlotanteTabs(result, 2);
}