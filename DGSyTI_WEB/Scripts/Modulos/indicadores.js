
function mostarCriterios() {
    $q("#idTablaStatusByIntitucion_criterios_form").show()
    $q("#mostrar_criterios").hide()
    $q("#ocultar_criterios").show()
}
function ocultarCriterios() {
    $q("#idTablaStatusByIntitucion_criterios_form").hide()
    $q("#mostrar_criterios").show()
    $q("#ocultar_criterios").hide()
}
function cargarCriteriosGrafica() {
    
    $q.ajax({

        type: "POST",
        data: { pantalla:"graficas"},
        url: '/Home/criterios',
        success: function (view) {
            jQuery("#idCriteriosGraficas").html(view);
            CargarGraficas();
        },
        error: function (jqXHR, textStatus, errorThrown) { }
    });

}
function CargarGraficas() {
    revisarchecarSesion() 
    GfByStatusByCarrera()
    GfByModalisdadTitulosByInstCarrera();
}
function mostrarListadoIstituciones() {
  
    $q("#idTablaStatusByCarrerasIntitucion_barraAccionesGrid").html("");
    $q("#listadoStatusCarrerasInst").slideToggle();
    $q("#listadoStatusInst").slideToggle();
    $q("#divFormCriterios").slideToggle();
    
}
function mostrarListadoIntitucionCarrerasStatus(conId, docInstitucionClave, docInstitucionNombre) {
    
    $q("#divLoading").show();
    conId: $q("#conId").val(conId);
    docInstitucionClave: $q("#docInstitucionClave").val(docInstitucionClave);
    docInstitucionNombre: $q("#docInstitucionNombre").val(docInstitucionNombre);
    cargarDatosinstitucionConsultada(conId, docInstitucionClave, docInstitucionNombre);
}

function cargarDatosListadoIntitucionCarrerasStatus(conId, docInstitucionClave, docInstitucionNombre) {
   
    $q.ajax({
        type: 'POST',
        url: "Home/ListadoIntitucionCarrerasStatus",
        data: { filtro: "{conId:'" + conId + "',docInstitucionClave:'" + docInstitucionClave + "',docInstitucionNombre:'" + docInstitucionNombre + "'}"},
        success: function (result) {
            $q("#idTablaStatusByCarrerasIntitucion_lista").html(result);
            $q("#divLoading").hide();
            iniciarTabla("idTablaStatusByCarrerasIntitucion", "Home/criteriosInst");
            $q("#listadoStatusCarrerasInst").slideToggle();
            $q("#listadoStatusInst").slideToggle();
            $q("#divFormCriterios").slideToggle();
            $q("#TablaAccionesSolicitud").html('<button onclick="mostrarListadoIstituciones()" class="btnAtras colorWhite tit-EstiloBtnAncla">Regresar</button>')
        },
        error: function () {
        }
    });
}
function cargarDatosinstitucionConsultada(conId, docInstitucionClave, docInstitucionNombre) {
    $q.ajax({
        type: 'POST',
        url: "Home/datosInstitucion",
        data: { conId: conId, docInstitucionClave: docInstitucionClave, docInstitucionNombre: docInstitucionNombre },
        success: function (result) {
            $q("#datosSolicitud").html(result);
            cargarDatosListadoIntitucionCarrerasStatus(conId, docInstitucionClave, docInstitucionNombre)
        },
        error: function () {
        }
    });
}
function getFiltros(idTabla) {
    if (idTabla == "idTablaStatusByIntitucion") {
        return {
            
            fechaInicial: $q("#fechaInicial").val(),
            fechaFinal: $q("#fechaFinal").val(),
            conIdConsultar: $q("#conIdConsultar").val(),
            docInstitucionClaveConsultar: $q("#comboInstituciones").val(),
            docInstitucionNombreConsultar: $q("#docInstitucionNombreConsultar").val(),
            urlController: "Home/ListadoInstitucionesStatus"
        }
    }
    if (idTabla == "idTablaStatusByCarrerasIntitucion") {
        return {
            fechaInicial: $q("#fechaInicial").val(),
            fechaFinal: $q("#fechaFinal").val(),
            conId: $q("#conId").val(),
            docInstitucionClave: $q("#docInstitucionClave").val(),
            docInstitucionNombre: $q("#docInstitucionNombre").val(),
            docCarreraClave: $q("#docCarreraClaveConsultar").val(),
            docCarreraNombre: $q("#docCarreraNombreConsultar").val(),
            urlController: "Home/ListadoIntitucionCarrerasStatus"
        }
            
        }
    }
function GfByStatusByCarrera() {
    $q.ajax({
        type: "POST",
        data: { fechaini: $q("#fechaInicial").val(), fechaFin: $q("#fechaFinal").val(), institucion: $q("#comboInstituciones").val() },
        url: '/Home/GraficaByStatusByCarrera',
        success: function (datos) {
            GraficaByStatusByCarrera(datos);
        },
        error: function (jqXHR, textStatus, errorThrown) { }
    });
}
function GfByModalisdadTitulosByInstCarrera() {
    $q.ajax({
        type: "POST",
        data: { fechaini: $q("#fechaInicial").val(), fechaFin: $q("#fechaFinal").val(), institucion: $q("#comboInstituciones").val() },
        url: '/Home/GraficaByModalisdadTitulosByInstCarrera',
        success: function (datos) {
            GraficaByModalisdadTitulosByInstCarrera(datos);
        },
        error: function (jqXHR, textStatus, errorThrown) { }
    });
}
function GraficaByStatusByCarrera(datos) {
    Highcharts.setOptions({
        colors: getColores(datos.series.length, datos.drilldown.length, 3)
    });
    $q('#container').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Total documentos por estatus'
        },
        subtitle: {
            text: 'Haga clic en la aplicación para desglosar los estatus por carrera.'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                }
            }
        },
        series: [{
            name: 'Estatus',
            colorByPoint: true,
            data: datos.series
        }]
        ,
        drilldown: {
            series: datos.drilldown
        }
    })
}

function GraficaByModalisdadTitulosByInstCarrera(datos) {
    Highcharts.setOptions({
        colors: getColores(datos.series.length, datos.drilldown.length, 5)
    });
    $q('#container_2').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Títulos emitidos por modalidad de titulación'
        },
        subtitle: {
            text: 'Haga clic en la modalidad para desglosar por carrera.'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total'
            }
        },
        legend: {
            enabled: false
        },

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                }
            }
        },
        series: [{
            name: 'Modalidad',
            colorByPoint: true,
            data: datos.series
        }]
        ,
        drilldown: {
            series: datos.drilldown
        }
    })

}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function getColores(numeroElementosNivel1, numeroElementosNivel2, arreloDeColores) {
    var coloresInst;
    switch (arreloDeColores) {
        case 1:
            coloresInst = ['#96c11f', '#7a429e', '#00497f', '#0262c9', '#4455a6', '#2461ab', '#00a5e2', '#2f8ce1', '#b7c3e4', '#021f84', '#3291ff', '#0059bc', '#e6358a', '#f27221'];
            break;
        case 2:
            coloresInst = ['#2461ab', '#3291ff', '#4455A6', '#e6358a', '#00a5e2', '#b7c3e4', '#00497f', '#2f8ce1', '#021f84', '#96C11F', '#0059BC', '#f27221', '#7A429E', '#0262c9'];
            break;
        case 3:
            coloresInst = ['#b7c3e4', '#f27221', '#00497f', '#0059BC', '#021f84', '#96C11F', '#3291ff', '#4455A6', '#2461ab', '#7A429E', '#2f8ce1', '#e6358a,', '#0262c9', '#00a5e2'];
            break;
        case 4:
            coloresInst = ['#00a5e2', '#e6358a', '#7A429E', '#4455A6', '#96C11F', '#0059BC', '#f27221', '#b7c3e4', '#00497f', '#021f84', '#3291ff', '#2461ab', '#2f8ce1', '#0262c9'];
            break;
        case 5:
            coloresInst = ['#7A429E', '#4455A6', '#96C11F', '#0059BC', '#f27221', '#b7c3e4', '#00497f', '#021f84', '#3291ff', '#2461ab', '#2f8ce1', '#00a5e2', '#e6358a', '#0262c9'];
            break;
        default:
            coloresInst = ['#96c11f', '#7a429e', '#00497f', '#0262c9', '#4455a6', '#2461ab', '#00a5e2', '#2f8ce1', '#b7c3e4', '#021f84', '#3291ff', '#0059bc', '#e6358a', '#f27221'];
    }

    if (numeroElementosNivel1 > 14 || numeroElementosNivel2 > 14) {
        var numeroMayor = numeroElementosNivel1;
        if (numeroElementosNivel2 > numeroElementosNivel1) {
            numeroMayor = numeroElementosNivel2
        }
        var coloresFaltantes = numeroMayor - coloresInst.length;
        var totalNumItemsArray = coloresInst.length + coloresFaltantes;
        while (coloresInst.length < totalNumItemsArray) {
            var color = getRandomColor();
            if (!coloresInst.includes(color)) {
                coloresInst.push(color);
            }
        }
    }
    return coloresInst;
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
        var redirect = result.toString().split("'");
        window.location.replace(redirect[1]);
        return false;
    } 
}