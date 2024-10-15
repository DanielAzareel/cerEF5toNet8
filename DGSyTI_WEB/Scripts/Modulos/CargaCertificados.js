var textoTotalRegistrosGrid = "";
var htmlAcciones = "";
function ValidarFormularioValidarLayout() {

    if ($q('#formValidarLayout').valid()) {

       

        var FileSize = $q("#txtCargaFileLayout")[0].files[0].size; // in MB
        if (FileSize > 2000000) {
            alertify.alert('No se pueden subir archivos mayores a 2MB.').setHeader('<section class="headerAlert text-left" warning >Atención</section>');
                // $(file).val(''); //for clearing with Jquery
            } else {




                $q("#divLoading").show();
                $q.ajax({
                    url: '/CargaCertificadosTerminacion/ValidarLayout',
                    type: 'POST',
                    data: new FormData($q("#formValidarLayout")[0]),//permite pasar el formulario y el input type=file
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                    },
                    success: function (result) {

                        checarSesion(result)
                        $q("#divLoading").hide();
                        if (result[0] == "True") {
                            $q("#txtCargaFileLayout").val('')
                            $q("#carId").val(result[1]);
                            $q("#idTablaListadoValidacionCarga_barraAccionesGrid").html('')

                            CargarListadoValidacion();

                        } else {

                            onSuccessMensajeFlotanteTabs(result, 2);
                        }

                    },
                    error: function () {

                    }
                });
            }
        
    }
}

function getFiltros(idTabla) {

    if (idTabla == "idTablaListadoValidacionCarga") {
        return {
            registrosConError: $q('#registrosConError').prop('checked'),
            registrosConObservavion: $q('#registrosConObservavion').prop('checked'),
            idDeCarga: $q("#carId").val(),
            tipoDocumento: $q("#tipoDocumento").val(),
            registrosCorrectos: $q('#registrosCorrectos').prop('checked'),
            urlController: "/CargaCertificadosTerminacion/ListadoResultadosValidacionCarga"
        }

    }

}

function cargarModalCargaArchivo() {
  
    $q("#lbl1").text($q("#totalRsinErrores").val());
    $q("#lbl2").text($q("#totalRconObservaciones").val());
    $q("#lbl3").text($q("#totalRconErrores").val());

    if ($q("#totalRconObservaciones").val() != "0") {
        $q("#cargarConObservaciones").show();
    }
    $q("#idModalCancelar").modal("show");
}
function cargarRegistros() {
    $q.ajax({
        type: "POST",
        cache: false,
        data: { idcarga: $q("#carId").val(), cargaConObservaciones: $q("#checkSubirConObservaciones").prop('checked') },
        url: "/CargaCertificadosTerminacion/cargarArchivo",
        success: function (result) {

            checarSesion(result)
            $q("#divLoading").hide();
            $q("#idModalCancelar").modal("hide");
            $q("#idTablaListadoValidacionCarga_btnBuscar").click()

            onSuccessMensajeFlotanteTabs(result, 2);


        },
        error: function () {

        }
    });
}

function modalObservaciones(lista, dato) {
    $q("#tituloModalObservaciones").text(dato);
    $q("#lstObservaciones").html('<ul style="font-size: 12px;">' + lista + '</ul>');
    $q("#idModalObservaciones").modal("show");
}
////////////////////////



