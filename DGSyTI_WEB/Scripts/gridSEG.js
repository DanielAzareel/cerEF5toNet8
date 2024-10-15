
/////////función para construir los registros del combo paginación

function iniciarTabla(tabla, urlController = "", datosCriterios = "") {
    $q("#" + tabla).attr("data-Actual", "1");

    cargaBarraAcciones(tabla);
    cargaPaginacion(tabla, datosCriterios);
    jQuery("#" + tabla + "_page_size").change(function () {
        $q("#" + tabla).attr("data-Actual", "1");

        paginarGrid('INICIO', tabla, datosCriterios);

    });
    jQuery(this).ready(function () {
        jQuery("#" + tabla + "_btnBuscar").click(function () {

            _ = $q("[aria-invalid='true']").length == 0 ? paginarGrid("INICIO", tabla, datosCriterios) : " ";

        })
    });


    jQuery(this).ready(function () {
        jQuery("#" + tabla + "_btnLimpiar").click(function () {

            //jQuery("#divLoading").show();
            limpiarSeccion("#" + tabla + "_criterios_form", datosCriterios);
            //paginar("INICIO");
            paginarGrid("INICIO", tabla, datosCriterios);

        })
    });

    jQuery(this).ready(function () {

        //if (readCookie("mostrarCriterios") != null) {

        //    var mostrarCriterios = readCookie("mostrarCriterios");
        //    //alert(mostrarCriterios);
        //    if (mostrarCriterios=="true") {
        //        jQuery("#ico_mostrar_1").hide();
        //        jQuery("#ico_ocultar_1").show();
        //        jQuery("#criterios_form").show();
        //    }
        //    else {
        //        jQuery("#ico_mostrar_1").show();
        //        jQuery("#ico_ocultar_1").hide();
        //        jQuery("#criterios_form").hide();
        //    }

        //}

        jQuery("#" + tabla + "_ico_mostrar_1").click(function () {
            jQuery("#" + tabla + "_ico_mostrar_1").hide(); jQuery("#" + tabla + "_ico_ocultar_1").show(); jQuery("#" + tabla + "_criterios_form").show();
            if (readCookie(tabla + "mostrarCriterios") != null) {
                createCookie(tabla + "mostrarCriterios", true, expira);
            } else {

                createCookie(tabla + "mostrarCriterios", true, expira);
            }
        });
        jQuery("#" + tabla + "_ico_ocultar_1").click(function () {
            jQuery("#" + tabla + "_ico_mostrar_1").show(); jQuery("#" + tabla + "_ico_ocultar_1").hide(); jQuery("#" + tabla + "_criterios_form").hide();
            if (readCookie(tabla + "mostrarCriterios") != null) {

                createCookie(tabla + "mostrarCriterios", false, expira);

            } else {

                createCookie(tabla + "mostrarCriterios", false, expira);
            }
        });
        //$q("#TablaLista").attr("data-Actual","1");
        //listaPaginar(10, 1);
        //cargaPaginacion();
        //jQuery("#page_size").change(function () {
        //    $q("#TablaLista").attr("data-Actual", "1");

        //        paginarGrid('INICIO');

        //});
        if (urlController != "") {
            try {
                jQuery.ajax({
                    type: "POST",
                    cache: false,
                    url: urlController,
                    data: { filtro: JSON.stringify(datosCriterios) }
                }).done(function (partialViewResult) {
                    jQuery("#" + tabla + "_zonaCriterios").html(partialViewResult);
                    // jQuery(".barraAcciones").html("<a href='#' id='btnLimpiar' class='linkLimpiar'>Limpiar</a>&nbsp;&nbsp;<img src='../Images/Compartidas/Iconos/img_barra_gris.png' style='border:0px;'>&nbsp;&nbsp;<a href='#' title='Buscar' id='btnBuscar' class='linkBuscar'>Buscar</a>")

                });
            } catch (ex) { }
        }

    });

}

function listaPaginar(Seleccionado, tabla) {
    jQuery('#' + tabla + '_page_size').empty();
    var strCombo = "<option value='10' data-id=1 " + (Seleccionado == 1 ? "selected" : "") + ">10 registros</option>"
    var strCombo = strCombo + "<option value='20' data-id=2 " + (Seleccionado == 2 ? "selected" : "") + ">20 registros</option>"
    var strCombo = strCombo + "<option value='50' data-id=3 " + (Seleccionado == 3 ? "selected" : "") + ">50 registros</option>"
    jQuery('#' + tabla + '_page_size').html(strCombo);

}

function paginarGrid(accion, tabla, datosCriterios = "") {
    jQuery("#divLoading").show();
    var bloque1 = jQuery('#' + tabla + '_page_size').val();
    var datos = getFiltros(tabla);

    var urlController = "";
    if (datos.urlController) {
        urlController = datos.urlController;
        delete datos.urlController;
    }

    datos.estatus = datos.estatus != "" && datos.estatus != null ? datos.estatus : datosCriterios.estatus != "" && datosCriterios.estatus != null ? datosCriterios.estatus : datos.estatus;

    var datosFiltro = JSON.stringify(datos);
    var paginaActual = $q("#" + tabla).attr("data-Actual");
    var filtro = datosFiltro;
    var paginar = jQuery("#" + tabla).attr("data-Paginar");

    var totalRegistros = jQuery("#" + tabla).attr("data-Total");
    jQuery("#" + tabla + "_totalR").html(' Se encontraron ' + totalRegistros + ' registros.</div>');
    paginaActual = $q("#" + tabla).attr("data-Actual");
    //var totalPaginas = (totalRegistros / parseInt(jQuery("#page_size option:selected").val())).toFixed(0);
    var totalPaginas = Math.ceil(totalRegistros / parseInt(jQuery("#" + tabla + "_page_size option:selected").val()));
    if (totalRegistros < 10) {
        totalPaginas = 1;
    }

    switch (accion) {
        case "INICIO":
            paginaActual = 1;
            break
        case "ANTERIOR":
            paginaActual = (Number(paginaActual) - 1) < 1 ? 1 : (Number(paginaActual) - 1);
            break
        case "SIGUIENTE":
            paginaActual = (Number(paginaActual) + 1) > totalPaginas ? totalPaginas : (Number(paginaActual) + 1);
            break
        case "FIN":
            paginaActual = totalPaginas;
            break
    }

    jQuery.ajax({
        type: "POST",
        cache: false,
        data: { filtro: filtro, pagina: paginaActual, bloque: bloque1 },
        url: urlController
    }).done(function (partialViewResult) {
        
        jQuery("#" + tabla + "_lista").html(partialViewResult);
        $q("#" + tabla).attr("data-Actual", paginaActual);
        
        cargaPaginacion(tabla);
        jQuery("#divLoading").hide();
    });
}



function irPagina(pagina, tabla, datosCriterios = "") {
    jQuery("#divLoading").show();
    var bloque1 = jQuery('#' + tabla + '_page_size').val();
    var datos = getFiltros(tabla);

    var urlController = "";
    if (datos.urlController) {
        urlController = datos.urlController;
        delete datos.urlController;
    }

    datos.estatus = datos.estatus != "" && datos.estatus != null ? datos.estatus : datosCriterios.estatus != "" && datosCriterios.estatus != null ? datosCriterios.estatus : datos.estatus;

    var datosFiltro = JSON.stringify(datos);

    var paginaActual = pagina; //$q("#TablaLista").attr("data-Actual");
    var filtro = datosFiltro;
    var paginar = jQuery("#" + tabla).attr("data-Paginar");

    var totalRegistros = jQuery("#" + tabla).attr("data-Total");
    jQuery("#" + tabla + "_totalR").html(' Se encontraron ' + totalRegistros + ' registros.</div>');
    paginaActual = pagina; //$q("#TablaLista").attr("data-Actual");
    //var totalPaginas = (totalRegistros / parseInt(jQuery("#page_size option:selected").val())).toFixed(0);
    var totalPaginas = Math.ceil(totalRegistros / parseInt(jQuery("#" + tabla + "_page_size option:selected").val()));
    if (totalRegistros < 10) {
        totalPaginas = 1;
    }

    /*switch (accion) {
        case "INICIO":
            paginaActual = 1;
            break
        case "ANTERIOR":
            paginaActual = (Number(paginaActual) - 1) < 1 ? 1 : (Number(paginaActual) - 1);
            break
        case "SIGUIENTE":
            paginaActual = (Number(paginaActual) + 1) > totalPaginas ? totalPaginas : (Number(paginaActual) + 1);
            break
        case "FIN":
            paginaActual = totalPaginas;
            break
    }*/

    jQuery.ajax({
        type: "POST",
        cache: false,
        data: { filtro: filtro, pagina: paginaActual, bloque: bloque1 },
        url: urlController
    }).done(function (partialViewResult) {
        jQuery("#" + tabla + "_lista").html(partialViewResult);
        $q("#" + tabla).attr("data-Actual", paginaActual);
        cargaPaginacion(tabla);
        
        jQuery("#divLoading").hide();
    });
}


function limpiarSeccion(seccion, datosCriterios = "") {
    /* Se encarga de leer todas las etiquetas input del formulario*/
    jQuery(seccion).find('input').each(function () {
        switch (this.type) {
            case 'password':
            case 'text':
            case 'hidden':
                jQuery(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
                if ($q(this).val() == -1) {
                    this.checked = true;
                }
                if ($q(this).val() == 0) {
                    this.checked = true;
                }
        }
        jQuery(this).attr('class', 'valid');
    });

    /* Se encarga de leer todas las etiquetas select del formulario */
    jQuery(seccion).find('select').each(function () {
        var lista = "#" + this.id + " option";
        if (jQuery(lista).length > 1) {
            jQuery("#" + this.id).val("");
            jQuery(lista).attr("selected", false);
            jQuery(lista + "[value=0]").attr("selected", true);
            //jQuery(lista + "[value='']").attr("selected", true);
            jQuery(lista + "[value='"+datosCriterios.estatus+"']").attr("selected", true);
        }
        jQuery("#" + this.id).change();
    });
    /* Se encarga de leer todas las etiquetas textarea del formulario */
    jQuery(seccion).find('textarea').each(function () {
        jQuery(this).val('');
    });

    jQuery(seccion).find('radio').each(function () {
        if (jQuery(this).val() == -1) {
            this.checked = true;
        }
    });

    $q("[class='field-validation-error']").attr("class", "field-validation-valid");
    $q("[aria-invalid='true']").attr("aria-invalid", false);
}

function cargaPaginacion(tabla, datosCriterios = "") {
    var paginaActual = $q("#" + tabla).attr("data-Actual");
    var totalRegistros = jQuery("#" + tabla).attr("data-Total");
    jQuery("#" + tabla + "_totalR").html(totalRegistros < 2 ? totalRegistros == 1 ? 'Se encontró ' + totalRegistros + ' registro.' : '' : ' Se encontraron ' + totalRegistros + ' registros.</div>');
    if (totalRegistros < parseInt(jQuery("#" + tabla + "_page_size option:selected").val())) {
        totalPaginas = 1;
        paginaActual = 1;
    }
    else {
        var totalPaginas = Math.ceil(totalRegistros / parseInt(jQuery("#" + tabla + "_page_size option:selected").val()));
    }
    //listaPaginar(jQuery("#" + tabla + "_page_size option:selected").attr("data-id"), tabla)
    jQuery("#" + tabla + "_htmlTotalRegistro").html(paginaActual + "/" + totalPaginas)
    jQuery("#" + tabla + "_barraPaginacion").append("<a href='#' id='" + tabla + "_btnInicio'><img src='" + $q("#urlRepositorio").val() + "/Iconos/ico_inicio.png' width='25' height='25' border='0' alt='Inicio'/>Inicio</a>&nbsp;&nbsp;&nbsp;");
    jQuery("#" + tabla + "_barraPaginacion").append("<a href='#' id='" + tabla + "_btnAnterior'><img src='" + $q("#urlRepositorio").val() + "/Iconos/ico_anterior.png' width='25' height='25' border='0' alt='Anterior'/>Anterior</a>&nbsp;&nbsp;&nbsp;");
    //jQuery("#barraPaginacion").append("&nbsp;" + paginaActual + "/" + totalPaginas + "&nbsp;&nbsp;&nbsp;");

    var limiteInferior = 0;
    var paginaControl = paginaActual;
    var totalControl = 6;
    var limiteSuperior = 0;

    limiteSuperior = parseInt(paginaControl) + 3;
    limiteInferior = parseInt(paginaControl) - 3;
    if (limiteInferior <= 1) {
        paginaControl = 1;
    }
    else {
        if (limiteSuperior >= totalPaginas) {
            paginaControl = totalPaginas - 5;
        } else {
            paginaControl = limiteInferior;
        }
    }
    if (totalPaginas < totalControl) {
        totalControl = totalPaginas;
    }
    if (paginaControl == 0) {
        paginaControl = 1;
    }
    var i;
    //alert(paginaControl);
    jQuery("#" + tabla + "_barraPaginacion").append("<font style='color:#00497F'>");
    for (i = 1; i <= totalControl; i++) {
        //if (paginaControl == paginaActual) {
        jQuery("#" + tabla + "_barraPaginacion").append((paginaControl == paginaActual ? "<b>" : "<a href='#' onclick='irPagina(" + paginaControl + ",\"" + tabla + "\"," + JSON.stringify(datosCriterios) + ")'>") + paginaControl + (paginaControl == paginaActual ? "</b>" : "</a>") + "&nbsp;");
        //} else {
        //    jQuery("#barraPaginacion").append("<a href='#'" + paginaControl + " onclick='paginar(" + paginaControl + ")'/>" + "&nbsp;");
        //}
        paginaControl += 1;
    }
    jQuery("#" + tabla + "_barraPaginacion").append("</font>");

    jQuery("#" + tabla + "_barraPaginacion").append("&nbsp<a href='#' id='" + tabla + "_btnSiguiente'>Siguiente<img src='" + $q("#urlRepositorio").val() + "/Iconos/ico_adelante.png' width='25' height='25' border='0' alt='Siguiente'/></a>&nbsp;&nbsp;&nbsp;");
    jQuery("#" + tabla + "_barraPaginacion").append("<a href='#' id='" + tabla + "_btnFIN'>Final<img src='" + $q("#urlRepositorio").val() + "/Iconos/ico_ultimo.png' width='25' height='25' border='0' alt='Final'/></a>");

    jQuery("#" + tabla + "_btnInicio").click(function () {
        paginarGrid('INICIO', tabla, datosCriterios);

    });
    jQuery("#" + tabla + "_btnAnterior").click(function () {
        paginarGrid('ANTERIOR', tabla, datosCriterios);

    });
    jQuery("#" + tabla + "_btnSiguiente").click(function () {
        paginarGrid('SIGUIENTE', tabla, datosCriterios);

    });
    jQuery("#" + tabla + "_btnFIN").click(function () {
        paginarGrid('FIN', tabla, datosCriterios);

    });

    if ($q("#"+tabla).find('tbody tr').length == 0) {
        var numeroDeColum = numeroColumnas(tabla);
        
        jQuery("#" + tabla).prepend("<tr class='row1'><td colspan='" + numeroDeColum + "' style='text-align: center;'>No se encontraron resultados.</td></tr>");
    }

}
function numeroColumnas(idTabla) {
    var cols = $q("#" + idTabla).find("tr:first th");
    var count = 0;
    for (var i = 0; i < cols.length; i++) {
        var colspan = cols.eq(i).attr("colspan");
        if (colspan && colspan > 1) {
            count += parseInt( colspan);
        } else {
            count++;
        }
    }
    return count;
}

function paginar(accion, tabla) {

    jQuery("#divLoading").show();
    var bloque1 = jQuery('#' + tabla + '_page_size').val();
    var datos = getFiltros(tabla);
    var datosFiltro = JSON.stringify(datos);
    var filtro = datosFiltro;//jQuery.base64.encode(datosFiltro);
    var paginar = jQuery("#" + tabla).attr("data-Paginar");
    if (parseInt(jQuery("#" + tabla + "_page_size option:selected").attr("data-id")) == 4) {
        bloque1 = jQuery("#" + tabla).attr("data-total");
        bloque1 = "-1";
    }

    jQuery.ajax({
        type: "POST",
        cache: false,
        data: { filtro: filtro, pag: paginar, bloque: bloque1, accion: accion },
        url: controller + '/paginar'
    }).done(function (partialViewResult) {
        jQuery("#" + tabla + "_lista").html(partialViewResult);
        var html = jQuery("#" + tabla).attr("data-Actual") + "/" + jQuery("#" + tabla).attr("data-Paginas");
        jQuery("#" + tabla + "_htmlTotalRegistro").html(html);
        jQuery("#divLoading").hide();
    });
}

function cargaBarraAcciones(tabla) {
    var barraAccionesGrid = "<div class='TablaAcciones'><div class='row'><div class='col-4' style='padding-top:5px'><div id='" + tabla + "_barraAcciones'>" + $q("#" + tabla + "_barraAccionesGrid").html()+"</div></div><div class='col-3' style='padding-top:10px' id='" + tabla + "_totalR' align='left'> </div><div class='col-5' align='right'> Paginar cada: <select name='" + tabla + "_page_size' id='" + tabla + "_page_size' style='min-width:100px;max-width:200px'><option value='10' data-id='1' selected=''>10 registros</option><option value='20' data-id='2'>20 registros</option><option value='50' data-id='3'>50 registros</option></select> </div> </div></div>";

    jQuery("#" + tabla + "_barraAccionesGrid").html(barraAccionesGrid);
}
