

function validaNumerosYComas(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros
    patron = /[0-9,]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function validaSoloNumeros(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros
    patron = /[0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function validaNumerosLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toString();
    //Se define todo lo que se quiere que se muestre
    caracter = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789";
    especiales = [8, 6];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }
    if (caracter.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

//Funcion que ejecuta los controladores al precionar un Tab de la vista.
function obtenerContentView(controller, metodo, idTab, numTabs, numSubTabs, tabActivo, subTabActivo) {
    pag_evento = "";
    var customParams = "";

    $q('#contentViewResult').html("");
    $q("#divLodingTabs").show(10);
    $q('label[for*=tab-]').attr("class", "bloqueaTab");
    $q('input[id*=tab-]').attr("class", "bloqueaTab");
    if (numSubTabs == 0) {
        $q('.subTabs').removeAttr('active');
    }

    //Agregamos un parámetro más a la petición para enviar datos adicionales que se requieran.
    if ($q("#tapCustomParamsJson") != null) {
        customParams = $q("#tapCustomParamsJson").val();
    }

    $q.ajax({
        type: 'POST',
        url: '/' + controller + '/' + metodo,
        data: { tab: idTab, customParamsJson: customParams },
        success: function (result) {
            setTimeout(function () {
                $q("#divLodingTabs").hide();
                $q('label[for*=tab-]').removeAttr("class");
                $q('input[id*=tab-]').removeAttr("class");
                $q('#contentViewResult').html(result);
            }, 500);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $q('#contentView').html("");
        }
    });
    pag_numTabs = numTabs;
    pag_tabActivo = tabActivo;
    pag_subTabActivo = subTabActivo;
    pag_numSubTabs = numSubTabs;
    pag_idTabActivo = idTab;
}

function obtenerContentView2(controller, metodo, idTab, numTabs, numSubTabs, tabActivo, subTabActivo) {
    pag_evento = "";
    var customParams = "";

    $q('#contentViewResult2').html("");
    $q("#divLodingTabs2").show(10);
    $q('label[for*=tab2-]').attr("class", "bloqueaTab");
    $q('input[id*=tab2-]').attr("class", "bloqueaTab");
    if (numSubTabs == 0) {
        $q('.subTabs').removeAttr('active');
    }

    //Agregamos un parámetro más a la petición para enviar datos adicionales que se requieran.
    if ($q("#tapCustomParamsJson2") != null) {
        customParams = $q("#tapCustomParamsJson2").val();
    }

    $q.ajax({
        type: 'POST',
        url: '/' + controller + '/' + metodo,
        data: { tab: idTab, customParamsJson: customParams },
        success: function (result) {
            setTimeout(function () {
                $q("#divLodingTabs2").hide();
                $q('label[for*=tab2-]').removeAttr("class");
                $q('input[id*=tab2-]').removeAttr("class");
                $q('#contentViewResult2').html(result);
            }, 500);
        },
        error: function () {
            $('#contentView2').html("");
        }
    });
    pag_numTabs = numTabs;
    pag_tabActivo = tabActivo;
    pag_subTabActivo = subTabActivo;
    pag_numSubTabs = numSubTabs;
    pag_idTabActivo = idTab;
}

//Variable donde se almacena el nombre de la cookie de la pagina actual.
var varNombreDinamicoCookie = "";

//Metodo que se ejecuta al inicio de cada carga de una vista que contenga tabs
//para guardar el ultimo tab seleccionado de la pagina por primera vez.
function CrearMiCookieTab(pNombreCookie) {
    var valorCookie = ObtenerCookie(pNombreCookie);
    varNombreDinamicoCookie = pNombreCookie;

    if (valorCookie == "") {
        CrearCookie(pNombreCookie, "1", 1);
        valorCookie = ObtenerCookie(pNombreCookie);
    }

    ClickUltimoTabActivo(valorCookie);
}

//Funcion que selecciona en vista el tab guardado en la cookie
function ClickUltimoTabActivo(idTabulador) {
    if ($q("#labelTab-" + idTabulador).length) {
        $q("#labelTab-" + idTabulador).click();
    } else {
        $q("#labelTab-1").click();
    }
}

//Funcion que almacena en una cookie el tab seleccionado.
function GuardarUltimoTabActivo(idTab) {
    CrearCookie(varNombreDinamicoCookie, idTab, 1);
}

//Metodo para crear una cookie
function CrearCookie(pNombre, pValor, pDias) {
    var fecha = new Date();
    fecha.setTime(fecha.getTime() + ((pDias * 24 * 60 * 60 * 1000) / 4));
    var expires = "expires=" + fecha.toUTCString();
    document.cookie = pNombre + "=" + pValor + ";" /*+ expires*/ + ";path=/";
}

//Metodo para obtener el valor de una cookie (si no existe regresa una cadena vacia)
function ObtenerCookie(pNombreCookie) {
    var nombre = pNombreCookie + "=";
    var decodificarCookie = decodeURIComponent(document.cookie);
    var ArregloCookie = decodificarCookie.split(';');
    for (var i = 0; i < ArregloCookie.length; i++) {
        var cookie = ArregloCookie[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(nombre) == 0) {
            return cookie.substring(nombre.length, cookie.length);
        }
    }
    return "";
}

//Elimina todas las cookies existentes en una pagina
function EliminarCookiesPag() {
    document.cookie.split(";").forEach(function (c) {
        document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
    });
}

function CrearFiltroDinamico(idTabla) {
/*    $q.ajax({
        type: "POST",
        datatype: "JSON",
        url: "../Titulo/SeccionFiltros",
        data: { idTabla: idTabla },
        success: function (filtros) {
            $q("#divFormCriterios").html(filtros);
            $q("#divFormCriterios").attr("style", "display:display");
        }
    });*/
}