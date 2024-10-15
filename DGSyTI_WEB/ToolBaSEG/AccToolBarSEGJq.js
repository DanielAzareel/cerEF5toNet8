// Variables para identificar el navegador.
var chromeTB = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
var firefoxTB = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;

//Variables para nombres de cookies.
var cookieZoomTB = 'zoomTB';
var cookieCursorTB = 'cursorTB';
var cookieLinkTB = 'linkTB';
var cookieModoNocturnoTB = 'modoNocturnoTB';
var cookieBtnAccionesTB = 'btnAccionesTB';

//Variables para mostrar tipo de cursor por etiqueta.
var etiquetasFechaTB = 'body, select, textarea';
var etiquetasManoTB = 'a, button, ol, ul, li, img, input';

//Variables auxiliares para almacenar estatus de acciones.
var cursoresActivosTB = '';
var linksActivosTB = '';
var modoNocturnoActivoTB = false;
var btnAccionesActivoTB = '';

//Variables que almacenan caracteres especiales.
var aTB = '\u00E1', eTB = '\u00E9', iTB = '\u00ED', oTB = '\u00F3', uTB = '\u00FA', nTB = '\u00F1';

//Variables con imágenes de los botones del toolbarSEG.
var imagenInicioTB = '../ToolBaSEG/IconosToolBar/configuraciones.png';
var imagenCerrarModalTB = '../ToolBaSEG/IconosToolBar/error.png';
var imagenZoomMasTB = '../ToolBaSEG/IconosToolBar/zoom.png';
var imagenZoomMenosTB = '../ToolBaSEG/IconosToolBar/alejar.png';
var imagenCursorTB = '../ToolBaSEG/IconosToolBar/cursor.png';
var imagenLinksTB = '../ToolBaSEG/IconosToolBar/enlace-roto.png';
var imagenTextoVozTB = '../ToolBaSEG/IconosToolBar/oido.png';
var imagenModoNocturnoTB = '../ToolBaSEG/IconosToolBar/luna.png';
var imagenReiniciarTodoTB = '../ToolBaSEG/IconosToolBar/flecha-hacia-atras.png';
var imagenInformacionTB = '../ToolBaSEG/IconosToolBar/info.png';
var imagenCerrarAccionesTB = '../ToolBaSEG/IconosToolBar/error.png';

//Función principal para crear ToolBarSEG.
AgregarJSRequeridosToolBar();

function AgregarJSRequeridosToolBar() {
    //var elemetoScriptDrag = document.createElement('script');
    //elemetoScriptDrag.type = 'text/javascript';
    //elemetoScriptDrag.id = 'idJqueryDragDropToolBarSEG';
    //elemetoScriptDrag.src = 'https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js';

    //document.head.appendChild(elemetoScriptDrag);

    var elemetoScript = document.createElement('script');
    elemetoScript.type = 'text/javascript';
    elemetoScript.id = 'idJqueryToolBarSEG';
    elemetoScript.src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js';

    document.head.appendChild(elemetoScript);
}

window.onload = function () {
    if (document.getElementById('idJqueryToolBarSEG')) {
        $j = jQuery.noConflict(true);
        CrearBtnToolBar();
    }

    //$j('#idNavFlotanteSEG').draggable({ containment: "window" });
    //$j('#idAccionesBtnFlotanteSEG').draggable({ containment: "window" });
};

function CrearBtnToolBar() {
    CrearEstilosBtnFlotanteToolBar();
    CrearBotonFlotanteToolBar();
    CrearModalToolBar();
    CreaBotonesToolBar();
    CrearAccionesBotonesToolBar();
    CrearCookiesFuncionalidadToolBar();
    EjecutarFuncionesToolBar();
}

function CrearEstilosBtnFlotanteToolBar() {
    var cuerpoEstilo = '<style type="text/css" class="classEstilosbtnFlotante">';

    //Estilos para barra de accesibilidad
    cuerpoEstilo = cuerpoEstilo + '.floating-menuToolBarSEG { border-radius: 50px !important; z-index: 9999 !important; padding-top: 10px !important; padding-bottom: 10px !important; left: 1% !important; position: fixed !important; display: inline-block; bottom: 5% !important; }';
    cuerpoEstilo = cuerpoEstilo + ' .menu-bgToolBarSEG { background-image: -webkit-linear-gradient(top, rgb(0, 100, 201) , rgb(0, 100, 201)  100%)  !important; background-image: -o-linear-gradient(top, rgb(0, 100, 201) , rgb(0, 100, 201)  100%) !important; background-image: -webkit-gradient(linear, left top, left bottom, from(rgb(0, 100, 201) ), to(rgb(0, 100, 201) )) !important; background-image: linear-gradient(to bottom, rgb(0, 100, 201) , rgb(0, 100, 201)  100%) !important; background-repeat: repeat-x !important; position: absolute !important; width: 100% !important; height: 100% !important; border-radius: 50px !important; z-index: -1 !important; top: 0 !important; right: 2% !important; }';
    cuerpoEstilo = cuerpoEstilo + ' .btnVacioToolBarSEG { background: none !important; border: 0 !important; background-color: none !important; }';
    cuerpoEstilo = cuerpoEstilo + ' .main-menuToolBarSEG { list-style: none !important; padding: 0 !important; }';

    //Estilos para el modal
    cuerpoEstilo = cuerpoEstilo + ' .modalToolBarSEG-contenido { background-color: white !important; width:70% !important; padding: 10px 20px !important; margin: 10% auto !important; position: relative !important; border-radius: 20px !important;}';
    cuerpoEstilo = cuerpoEstilo + ' .modalToolBarSEG{ background-color: rgba(0,0,0,.8) !important; position: fixed !important; top: 0 !important; right: 0 !important; bottom: 0 !important; left: 0 !important; transition: all 1s !important; z-index:9998 !important;} ';

    //Estilos para seleccionar las anclas
    cuerpoEstilo = cuerpoEstilo + ' .linksToolBarSEG { background-color: yellow !important; color:black !important; text-decoration: underline !important; }';

    //Estilos para el cursos en flecha y mano
    cuerpoEstilo = cuerpoEstilo + ' .cursorFlechaToolBarSEG { cursor: url("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACEAAAAzCAYAAAAZ+mH/AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RDA1OTE2NURCQzkyMTFFN0IwODJCQjE5QzZFMDg2QjYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RDA1OTE2NUVCQzkyMTFFN0IwODJCQjE5QzZFMDg2QjYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpEMDU5MTY1QkJDOTIxMUU3QjA4MkJCMTlDNkUwODZCNiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpEMDU5MTY1Q0JDOTIxMUU3QjA4MkJCMTlDNkUwODZCNiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Phwph8YAAAWrSURBVHjavFldSGxVFF7zq+N4/RtTuY1SU2SWoqUW/iAZhL1UFD4kVBD02Jv45os/+Psi+CCU9hRYkGVF1kOUmEYZpmGJEpqJ4Ev5e/XqzDi7tU5rz92zx7nqzBwXfBxn73P2/va311pnnS0AwDuI3xG34H9zIGwMC8NUsyIOEU8iphAexDnCzn2mE5AkrPx3PRPJZiJSEavZiqgkyJ5BfInIQQSZiOmKXDRBDSuSc1OKxFplJWISkasQMU2RiIF9Ph+kpqbKn88ivmAiIYTTLEVUfzAIeL1ecLlcsulpxKdmKxIxYFpaGrS0tEBOTg44nU7VWT83W5G3EIJQWVkpyAYGBkRBQYFAZYTsQ/yM8JJgxDfZqoRJVFRUiGAwaBDp6uoS+fn5AhVRiSwoRNxK5CSsSAQJv98vpPX19Ym8vLwbUSQmiZtU5L4kVEVSUlJMU+RSElKR3Nxc4XA4TFHkSiTIent7hcfjMUWRK5OQihCRZCtyLRJmKXJtElIRzKxJUyQuElKR7OxsPXzjUiRuEmSdnZ0GkUQVSYhEshRJmIRUJCsrK25FkkKCrKenR2RmZsalSNJIkHV0dIiMjAxht9uvpUhSScSrSNJJXKLIgxoRm2kkyPr7+w0imiI/MZEUScSeSCESCoXg9PQULJboqKO21tZW2Nvbg7GxMeOKVZtaxb+E+DdhEoeHh1BbWwv7+/sxidhsNkB14fz8XO2SVfxrRORKJI6OjoyJsPgFzAPhdrfbbUyws7MTzxqkIq9YL7uzu7sbsAqHkpISWFpaitqOsrIyQOeLV0z60hu779PoWDA8PAy7u7uGnFjmwcTERLgfX+XQ1tYGk5OThvToi9T8B+JDzgdB/lYJ8ceT/DvIvwOI7SgSVqs1rAARoG1gh4KFhQWYnZ2F+vr6yOWgUouLi5IE2TziH46GAE94rhChq5/7QhHbQU5EGBwchKGhITg4ODD2XNrW1haMj49HECDHbGxsNJ5jowOXF3i1enq2cJuNv+RSOVfcyxNVVVWivb39ooI2jObmZrG9vR2RD3C7RGFhoXrfPqIC8RjiIcRtRB5/Snr42IGQhUgnRuWIV4kNJhaYn583YlpVAO2uZLyysgINDQ1QXFwcDkvyDdqy6elpw1k5EZ0hvmf5z1j6gOIPQcVn7ilB3xZadiN8gHhZ/qb+8vLyqOw4MzNj9KNPyee+46On23x1MzknL8jBZ2P2CCWOj4/VpLKMGER8hjhA0HlBOfXTyskJa2pqIhLTxsYGrK6uhtMI4hfEX+wLAc05Q3JhsfIEhdm7iK/5YUqvi6qD0oSqFRUVQVNTE2AVLpvIB15n59MdVFcb3tQafuVzK/LyUkQx4mHEUwhapsBVi9LSUrG8vBy1LT6fz+hXxitmQrd4O2x6QaMr8RvibY5xku2YQV76J+ITkpG2Ym1tDaampiAQCIQfPjk5gerqasPB2fycngXvvy1WjfmGUnQ8TsoiHuVrgRJSHn4F79L9FMK0at0wmYn09HRVjTlW4gEKR3bMiO0hZnWIR/jVesRee8bwK2FFA95hvEihSMdKlC3JH1TfoCw7Nzcnmyg61tmnbJpTGkYSzSC+ReyxR9/lmwJKLAO3+fk+2irb+vo6jI6OQl1dHZydncHIyAhsbm4a+UJNxIhMmWeUA1yhErGyRJmcwTJYNpd22O5kkuTtP8icQNkV07yRbb1e74VZlsk/weO7lS0Jm1Op+dJ48hStELWyai5Gs5zA5XIZH8daRKggZd/jbfFofhEhl13LYvq/GiyKYum8oh9jTCoU//kK8TyHuJffHVFK2Hmv9bAR2hUUvwjxvyfe53yiP0eVz0cc5tM8oUV5Xwh9XHuMyWKZ4MFoFX8zGZkUyME/5lrijqx7tEiTL6+I+a57yCVrAQcP+BznFLJvlC1Vixa/gqDy/ggr8p8AAwB38ep+f+/fmwAAAABJRU5ErkJggg=="), url("../ToolBaSEG/IconosToolBar/cursorMano.cur"),url("../ToolBaSEG/IconosToolBar/cursorFlecha.cur"), url("../ToolBaSEG/IconosToolBar/cursorFlecha.cur"), auto !important; } ';
    cuerpoEstilo = cuerpoEstilo + ' .cursorManoToolBarSEG { cursor: url("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACkAAAAtCAYAAAAz8ULgAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RkE0QzFBMjdCQzkyMTFFNzg4RDE5NkYzNkM0MDkwNzAiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RkE0QzFBMjhCQzkyMTFFNzg4RDE5NkYzNkM0MDkwNzAiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpGQTRDMUEyNUJDOTIxMUU3ODhEMTk2RjM2QzQwOTA3MCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpGQTRDMUEyNkJDOTIxMUU3ODhEMTk2RjM2QzQwOTA3MCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PnfOpO8AAAhdSURBVHjaxFlpTBVX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"), url("../ToolBaSEG/IconosToolBar/cursorMano.cur"), auto !important; } ';

    //Estilos para modo nocturno
    cuerpoEstilo = cuerpoEstilo + ' .modoNocturnoToolBarSEG > :not(.classDivToolBarSEG) { filter: invert(20%) !important; background-color: #fff !important; }';
    cuerpoEstilo = cuerpoEstilo + ' .modoNocturnoBodyToolBarSEG > :not(.classDivToolBarSEG) { background-color: #fff !important; }';

    //Estilos para modo nocturno 2
    cuerpoEstilo = cuerpoEstilo + ' .modoNocturnoDosToolBarSEG > :not(.classDivToolBarSEG) { filter: grayscale(1) !important; background-color: #fff !important; }';
    cuerpoEstilo = cuerpoEstilo + ' .modoNocturnoDosBodyToolBarSEG > :not(.classDivToolBarSEG) { background-color: #fff !important; }';

    //Estilos para tamaño fuente
    cuerpoEstilo = cuerpoEstilo + ' .fontSizeToolBarSEG > :not(.classDivToolBarSEG) {  }';

    cuerpoEstilo = cuerpoEstilo + '</style>';
    $j('head').append(cuerpoEstilo);
}

function CrearBotonFlotanteToolBar() {
    $j('html').append('<div id="idDivToolBarSEG" class="classDivToolBarSEG"></div>');

    $j('#idDivToolBarSEG').append('<nav id="idNavFlotanteSEG" class="floating-menuToolBarSEG"></nav>');
    $j('#idNavFlotanteSEG').append('<ul id="idUlNavBtnFlotante" class="main-menuToolBarSEG">');
    $j('#idUlNavBtnFlotante').append('<li><button id="idBtnOpenToolBarSEG" class="btnVacioToolBarSEG"><img src="' + imagenInicioTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlNavBtnFlotante').append('<div class="menu-bgToolBarSEG"></div>');
}

function CrearModalToolBar() {
    $j('html').append('<div id="idMiModalInfoToolBar" class="modalToolBarSEG" style="display:none !important"></div>');
    $j('#idMiModalInfoToolBar').append('<div id="idModalContenidoToolBar" class="modalToolBarSEG-contenido"></div>');

    var contenidoTituloModal = '<button id="idBtnCerrarModalInfoToolBar" class="btnVacioToolBarSEG" title="Cerrar"><img src="' + imagenCerrarModalTB + '" height="24px" width="24px"></button>';
    contenidoTituloModal = contenidoTituloModal + '<h2 style="color:black !important;">Informaci' + oTB + 'n sobre la barra de accesibilidad.</h2>';
    $j('#idModalContenidoToolBar').append(contenidoTituloModal);

    var contenidoModal = '<div class="row"> <div class="col-12"> <table border="1" align = "center" width = "100%"> <thead style="background-color:#2461AB !important; color: white !important;"> <tr>';
    contenidoModal = contenidoModal + '<th>Imagen</th>';
    contenidoModal = contenidoModal + '<th>Acci' + oTB + 'n</th>';

    contenidoModal = contenidoModal + '</tr> </thead> <tbody> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenZoomMasTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Aumenta la cantidad predeterminada de ampliaci' + oTB + 'n de los elementos.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenZoomMenosTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Disminuye la cantidad predeterminada de ampliaci' + oTB + 'n de los elementos.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenCursorTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Aumenta tama\u00F1o del cursor.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenLinksTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Subraya y resalta en color amarillo los enlaces dentro de la p' + aTB + 'gina web.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenTextoVozTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Lee el texto seleccionado a voz.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenModoNocturnoTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Modo nocturno con dos configuraciones; opaco y blanco/negro.</td>';

    contenidoModal = contenidoModal + '</tr> <tr>';
    contenidoModal = contenidoModal + '<td><img src="' + imagenReiniciarTodoTB + '" height="24px" width="24px"></td>';
    contenidoModal = contenidoModal + '<td style = "text-align: left !important;">Restablecer configuraci' + oTB + 'n predeterminada.</td>';

    contenidoModal = contenidoModal + '</tr> </tbody> </table> </div> </div>';

    $j('#idModalContenidoToolBar').append(contenidoModal);
}

function CreaBotonesToolBar() {
    $j('#idDivToolBarSEG').append('<nav id="idAccionesBtnFlotanteSEG" class="floating-menuToolBarSEG" style="display:none; !important"></nav>');
    $j('#idAccionesBtnFlotanteSEG').append('<ul id="idUlBtnFlotante" class="main-menuToolBarSEG">');

    //Botones
    $j('#idUlBtnFlotante').append('<li><button id="idZoomMasToolBarSEG" class="btnVacioToolBarSEG" title="Aumentar"><img src="' + imagenZoomMasTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idZoomMenosToolBarSEG" class="btnVacioToolBarSEG" title="Disminuir"><img src="' + imagenZoomMenosTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idCursorToolBarSEG" class="btnVacioToolBarSEG" title="Cursor"><img src="' + imagenCursorTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idEnlacesToolBarSEG" class="btnVacioToolBarSEG" title="Marcar enlaces"><img src=" ' + imagenLinksTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idTextoToolBarSEG" class="btnVacioToolBarSEG" title="Escuchar"><img src="' + imagenTextoVozTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idModoNocturnoToolBarSEG" class="btnVacioToolBarSEG" title="Modo nocturno"><img src="' + imagenModoNocturnoTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idReiniciarToolBarSEG" class="btnVacioToolBarSEG" title="Reiniciar cambios"><img src="' + imagenReiniciarTodoTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idAyudaToolBarSEG" class="btnVacioToolBarSEG" title="Ayuda"><img src="' + imagenInformacionTB + '" height="24px" width="24px"></button></li>');
    $j('#idUlBtnFlotante').append('<li><button id="idCerrarToolBarSEG" class="btnVacioToolBarSEG" title="Cerrar"><img src="' + imagenCerrarAccionesTB + '" height="24px" width="24px"></button></li>');

    //Para mostrar fondo
    $j('#idUlBtnFlotante').append('<div class="menu-bgToolBarSEG"></div>');
}

function CrearAccionesBotonesToolBar() {
    $j('#idBtnOpenToolBarSEG').click(function () { ToggleToolBar(); });
    $j('#idZoomMasToolBarSEG').click(function () { ZoomPageToolBar('+'); });
    $j('#idZoomMenosToolBarSEG').click(function () { ZoomPageToolBar('-'); });
    $j('#idCursorToolBarSEG').click(function () { CursorToolBar(); });
    $j('#idEnlacesToolBarSEG').click(function () { MostrarLinksToolBar(); });
    $j('#idTextoToolBarSEG').click(function () { TextoAVozToolBar(); });
    $j('#idModoNocturnoToolBarSEG').click(function () { ModoNocturnoToolBar(); });
    $j('#idReiniciarToolBarSEG').click(function () { ReiniciarTodoToolBar(); });
    $j('#idAyudaToolBarSEG').click(function () { ToggleModalInfoToolBar(); });
    $j('#idCerrarToolBarSEG').click(function () { ToggleToolBar(); });
    $j('#idBtnCerrarModalInfoToolBar').click(function () { ToggleModalInfoToolBar(); });
}

function CrearCookiesFuncionalidadToolBar() {
    if (ObtenerCookieToolBar(cookieZoomTB) === false) CrearCookieToolBar(cookieZoomTB, 1.0);
    if (ObtenerCookieToolBar(cookieCursorTB) === false) CrearCookieToolBar(cookieCursorTB, false);
    if (ObtenerCookieToolBar(cookieLinkTB) === false) CrearCookieToolBar(cookieLinkTB, false);
    if (ObtenerCookieToolBar(cookieModoNocturnoTB) === false) CrearCookieToolBar(cookieModoNocturnoTB, '');
    if (ObtenerCookieToolBar(cookieBtnAccionesTB) === false) CrearCookieToolBar(cookieBtnAccionesTB, false);
}

function EjecutarFuncionesToolBar() {
    ZoomPageToolBar("cookie");
    CursorToolBar();
    MostrarLinksToolBar();
    ModoNocturnoToolBar();
    ToggleToolBar();
}

function ToggleModalInfoToolBar() {
    $j("#idMiModalInfoToolBar").toggle();
}

function CerrarModalInfoToolBar() {
    $j("#idMiModalInfoToolBar").hide();
}

function ToggleToolBar() {
    if (btnAccionesActivoTB === "") {
        if (ObtenerCookieToolBar(cookieBtnAccionesTB) !== false) btnAccionesActivoTB = ObtenerCookieToolBar(cookieBtnAccionesTB);
        else {
            CrearCookiesFuncionalidadToolBar();
            btnAccionesActivoTB = 'false';
        }
    } else btnAccionesActivoTB = btnAccionesActivoTB === 'true' ? 'false' : 'true';

    switch (btnAccionesActivoTB) {
        case '':
            ActualizarCrearCookieToolBar(cookieBtnAccionesTB, 'false');
            break;
        case 'false':
            $j('#idNavFlotanteSEG').show("slow");
            $j('#idAccionesBtnFlotanteSEG').hide("slow");
            ActualizarCrearCookieToolBar(cookieBtnAccionesTB, 'false');
            break;
        case 'true':
            $j('#idNavFlotanteSEG').hide("slow");
            $j('#idAccionesBtnFlotanteSEG').show("slow");
            ActualizarCrearCookieToolBar(cookieBtnAccionesTB, 'true');
            break;
    }
}

function ZoomPageToolBar(accion) {
    var auxZoom = 1.0;

    if (accion !== "") {
        if (ObtenerCookieToolBar(cookieZoomTB) === false) CrearCookiesFuncionalidadToolBar();
        else auxZoom = parseFloat(ObtenerCookieToolBar(cookieZoomTB));

        switch (accion) {
            case '+': auxZoom += 0.1; break;
            case '-': auxZoom -= 0.1; break;
            case 'cookie': auxZoom = auxZoom; break;
        }
    }

    if (auxZoom > 1.0 && auxZoom <= 1.5) {
        zoomActual = auxZoom;
        if (chromeTB) document.body.style.zoom = zoomActual;
        if (firefoxTB) document.body.style.transform = 'scale(' + zoomActual + ')';
        ActualizarCrearCookieToolBar(cookieZoomTB, zoomActual);
    } else if (auxZoom === 1.0) {
        document.body.style.zoom = 0;
        document.body.style.transform = '';
        ActualizarCrearCookieToolBar(cookieZoomTB, auxZoom);
    }

}

function CursorToolBar() {
    if (cursoresActivosTB === "") {
        if (ObtenerCookieToolBar(cookieCursorTB) !== false) cursoresActivosTB = ObtenerCookieToolBar(cookieCursorTB);
        else {
            CrearCookiesFuncionalidadToolBar();
            cursoresActivosTB = 'false';
        }
    } else cursoresActivosTB = cursoresActivosTB === 'true' ? 'false' : 'true';

    switch (cursoresActivosTB) {
        case 'true':
            $j(etiquetasFechaTB).addClass('cursorFlechaToolBarSEG');
            $j(etiquetasManoTB).addClass('cursorManoToolBarSEG');
            ActualizarCrearCookieToolBar(cookieCursorTB, true);
            break;
        case 'false':
            $j(etiquetasFechaTB).removeClass('cursorFlechaToolBarSEG');
            $j(etiquetasManoTB).removeClass('cursorManoToolBarSEG');
            ActualizarCrearCookieToolBar(cookieCursorTB, false);
            break;
    }
}

function ReiniciarCursorToolBar() {
    $j(etiquetasFechaTB).removeClass('cursorFlechaToolBarSEG');
    $j(etiquetasManoTB).removeClass('cursorManoToolBarSEG');
    ActualizarCrearCookieToolBar(cookieCursorTB, false);
    cursoresActivosTB = 'false';
}

function MostrarLinksToolBar() {
    if (linksActivosTB === "") {
        if (ObtenerCookieToolBar(cookieLinkTB) !== false) linksActivosTB = ObtenerCookieToolBar(cookieLinkTB);
        else {
            CrearCookiesFuncionalidadToolBar();
            linksActivosTB = 'false';
        }
    } else linksActivosTB = linksActivosTB === 'true' ? 'false' : 'true';

    switch (linksActivosTB) {
        case 'true':
            $j('a').addClass('linksToolBarSEG');
            ActualizarCrearCookieToolBar(cookieLinkTB, true);
            break;
        case 'false':
            $j('a').removeClass('linksToolBarSEG');
            ActualizarCrearCookieToolBar(cookieLinkTB, false);
            break;
    }
}

function ReiniciarLinksToolBar() {
    $j('a').removeClass('linksToolBarSEG');
    ActualizarCrearCookieToolBar(cookieLinkTB, false);
    linksActivosTB = 'false';
}

function TextoAVozToolBar() {
    var textoSeleccionadoToolBar = document.getSelection().toString();
    speechSynthesis.speak(new SpeechSynthesisUtterance(textoSeleccionadoToolBar));
}

function ModoNocturnoToolBar() {
    if (modoNocturnoActivoTB === false) {
        if (ObtenerCookieToolBar(cookieModoNocturnoTB) !== false) modoNocturnoActivoTB = ObtenerCookieToolBar(cookieModoNocturnoTB);
        else {
            CrearCookiesFuncionalidadToolBar();
        }
    } else {
        modoNocturnoActivoTB = modoNocturnoActivoTB === '' ? 'modoNocturnoToolBarSEG' :
            modoNocturnoActivoTB === 'modoNocturnoToolBarSEG' ? 'modoNocturnoDosToolBarSEG' :
                modoNocturnoActivoTB === 'modoNocturnoDosToolBarSEG' ? '' : false;
    }

    switch (modoNocturnoActivoTB) {
        case false:
        case "":
            $j('html').removeClass('modoNocturnoDosToolBarSEG');
            $j('body').removeClass('modoNocturnoDosBodyToolBarSEG');
            ActualizarCrearCookieToolBar(cookieModoNocturnoTB, '');
            break;
        case 'modoNocturnoToolBarSEG':
            $j('html').addClass('modoNocturnoToolBarSEG');
            $j('body').addClass('modoNocturnoBodyToolBarSEG');
            ActualizarCrearCookieToolBar(cookieModoNocturnoTB, 'modoNocturnoToolBarSEG');
            break;
        case 'modoNocturnoDosToolBarSEG':
            $j('html').removeClass('modoNocturnoToolBarSEG');
            $j('body').removeClass('modoNocturnoBodyToolBarSEG');

            $j('html').addClass('modoNocturnoDosToolBarSEG');
            $j('body').addClass('modoNocturnoDosBodyToolBarSEG');
            ActualizarCrearCookieToolBar(cookieModoNocturnoTB, 'modoNocturnoDosToolBarSEG');
            break;
    }
}

function ReiniciarModoNocturnoToolBar() {
    $j('html').removeClass('modoNocturnoToolBarSEG');
    $j('body').removeClass('modoNocturnoBodyToolBarSEG');

    $j('html').removeClass('modoNocturnoDosToolBarSEG');
    $j('body').removeClass('modoNocturnoDosBodyToolBarSEG');

    ActualizarCrearCookieToolBar(cookieModoNocturnoTB, '');
    modoNocturnoActivoTB = '';
}

function ReiniciarTodoToolBar() {
    ZoomPageToolBar("");
    ReiniciarCursorToolBar();
    CerrarModalInfoToolBar();
    ReiniciarLinksToolBar();
    ReiniciarModoNocturnoToolBar();
}

function CrearCookieToolBar(pNombre, pValor) {
    var fecha = new Date();
    fecha.setTime(fecha.getTime() + 365 * 24 * 60 * 60 * 1000 / 4);
    var expires = "expires=" + fecha.toUTCString();
    document.cookie = pNombre + "=" + pValor + ";" /*+ expires*/ + ";path=/";
}

function ActualizarCrearCookieToolBar(pNombre, pValor) {
    var fecha = new Date();
    fecha.setTime(fecha.getTime() + 365 * 24 * 60 * 60 * 1000 / 4);
    var expires = "expires=" + fecha.toUTCString();
    document.cookie = pNombre + "=" + pValor + ";" /*+ expires*/ + ";path=/";
}

function ObtenerCookieToolBar(pNombreCookie) {
    var nombre = pNombreCookie + "=";
    var decodificarCookie = decodeURIComponent(document.cookie);
    var ArregloCookie = decodificarCookie.split(';');
    for (var i = 0; i < ArregloCookie.length; i++) {
        var cookie = ArregloCookie[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(nombre) === 0) {
            return cookie.substring(nombre.length, cookie.length);
        }
    }
    return false;
}

function EliminarCookiesToolBar() {
    document.cookie.split(";").forEach(function (c) {
        document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
    });
}

