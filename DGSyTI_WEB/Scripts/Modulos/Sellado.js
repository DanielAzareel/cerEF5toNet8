

$q(this).ready(function () {

     
    var htmlAcciones = $q("#idDivAccionesTabla").html();
    $q("#idTablaSellado_barraAcciones").html(htmlAcciones);
    $q("#idDivAccionesTabla").remove();

    $q.ajax({
        type: "POST",
        cache: false,
        data: { filtro: getFiltros(), pagina: 1, bloque: 10 },
        url: "/Sellado/Paginar"
    }).done(function (partialViewResult) {
        $q("#idTablaSellado_lista").html(partialViewResult);
        $q("#divLoading").hide();
        iniciarTabla("idTablaSellado", "/Sellado/IniciarCriteriosConsulta");
    });
});

function getFiltros(idTabla) {

    
    return { 
        listaPlanEstudios: $q("#comboPlanEstudios").val(),
        listaTipoDocumento: $q("#comboTipoDocumento").val(),
        folioControl: $q("#folioControl").val(),
        curp: $q("#curp").val(),
        nombreFiltro: $q("#nombreFiltro").val(),
        urlController: "/Sellado/Paginar"
    };
}
 
function LimpiarCamposSellado() {
    $q("#idArchivoKey").val("");
    $q("#idContrasenia").val("");
}

$q('body').click(function (event) {
    if ($q(event.target).closest('#idModalSellado').length && $q(event.target).is('#idModalSellado')) {
        LimpiarCamposSellado();
    }
});


function CancelarDocumento(id) {


    alertify.confirm("Confirmar","Al cancelar el registro se perderá su información, ¿deseas continuar?", function () {
        $q.ajax({
            type: "POST",
            cache: false,
            data: { documentoId: id },
            url: "/Sellado/CancelarRegistro"
        }).done(function (resultado) {
            onSuccessMensajeFlotanteTabs(resultado, 2);
            $q("#idTablaSellado_btnInicio").click();
        });
    }, function () { }                
    ).setHeader('<section class="headerAlert" warning>Atención </section> ');
    
}

function TerminarSellado() {
    document.getElementById('idTablaSellado_btnInicio').click()

    $q("#idModalSellado").modal("hide");
    $q("body").toggleClass("modal-open");
    $q("div.modal-backdrop").toggleClass("modal-backdrop fade show");

}