(function ($) {
    $.fn.extend({
        NumericTextBox: function () {//Funcion para solo admitir numeros

            return this.each(function () {
                var Digitos = $(this).attr("Digitos");
                //$(this).val('0.' + ('' + "00000000000000").slice(0, Digitos));
                $(this).keydown(function (e) {
                    var key = e.charCode || e.keyCode || 0;
                    return (
                        key == 8 ||
                        key == 9 ||
                        key == 13 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
                });
                if (!Digitos)
                    Digitos = 2;

                $(this).blur(function () {


                    var valor = $(this).val();
                    if (valor) {
                        if (Digitos)
                            valor = +(Math.round((valor) + "e+" + Digitos) + ("e-" + Digitos));
                        else
                            valor = +(Math.round((valor) + "e+2") + ("e-2"));

                        $(this).val(addCommas(valor, Digitos));
                    }
                    //else
                    //{ $(this).val('0.' + ('' + "00000000000000").slice(0, Digitos)); }
                    //$(this).formatNumber({ format: "#,###.00", locale: "us" });
                });
                $(this).focus(function () {
                    if (Number($(this).val()) == 0)
                        $(this).val('');
                    else
                        $(this).val($(this).val().replace(/,/g, ''));

                });




            });
        }
       , DecimalFormat: function (Option) {//Funcion para solo admitir numeros

           return this.each(function () {
               // var Digitos = $(this).attr("Digitos");
               //$(this).val('0.' + ('' + "00000000000000").slice(0, Digitos));
               debugger;
               var Digitos = 2;



               var valor = $(this).val();
               if (!valor)
                   valor = $(this).text();
               valor = valor.replace(/,/g, '');
               if (valor && !isNaN(valor)) {

                   if (Digitos)
                       valor = +(Math.round((valor) + "e+" + Digitos) + ("e-" + Digitos));
                   else
                       valor = +(Math.round((valor) + "e+2") + ("e-2"));
                   if (this.nodeName == "SPAN" || this.nodeName == "TD" || this.nodeName == "DIV")
                       $(this).text(addCommas(valor, Digitos));
                   else
                       $(this).val(addCommas(valor, Digitos));
               }






           });
       }
         , NumericFormat: function () {//Funcion para solo admitir numeros

             return this.each(function () {
                 // var Digitos = $(this).attr("Digitos");
                 //$(this).val('0.' + ('' + "00000000000000").slice(0, Digitos));

                 var Digitos = 0;



                 var valor = $(this).val();
                 if (!valor)
                     valor = $(this).text();
                 valor = valor.replace(/,/g, '');
                 if (valor && !isNaN(valor)) {

                     //  if (Digitos)
                     valor = +(Math.round((valor) + "e+" + Digitos) + ("e-" + Digitos));
                     //else
                     //    valor = +(Math.round((valor) + "e+2") + ("e-2"));
                     if (this.nodeName == "SPAN" || this.nodeName == "TD" || this.nodeName == "DIV")
                         $(this).text(addCommas(valor, Digitos));
                     else
                         $(this).val(addCommas(valor, Digitos));
                 }






             });
         }
         , Autocomplete_Ing: function (Url, Limpiar) {
             if (!Limpiar)
                 Limpiar = false;
             var autocomp_opt = {

                 source: function (request, response) {
                     $.ajax(
                         {
                             url: Url,
                             dataType: "json",
                             type: "post",
                             contentType: "application/json; charset=utf-8",
                             dataFilter: function (data) { return data; },
                             data: "{'maxRows':'15','term':'" + request.term + "'}",
                             success: function (data) {
                                 if (data.d.Result == "OK") {
                                     response($.map(data.d.Options, function (item) {
                                         return {
                                             label: item.DisplayText,
                                             value: item.DisplayText,
                                             ID: item.Value
                                         }
                                     }))
                                 }
                             },

                             error: function (a, b, c) {
                                 alert(a);
                             }
                         }

                     )
                 },
                 minLength: 2,
                 select: function (event, ui) {
                     $(this).attr("valueSelected", ui.item.ID)
                 },
                 response: function (event, ui) {
                     if (!ui.content.length) {
                         var noResult
                         noResult = { value: event.target.value, label: "No se encontraron resultados." };
                         ui.content.push(noResult);
                     }
                 }
                 , change: function (event, ui) {

                     if (ui.item == null || ui.item == undefined) {
                         $(this).removeAttr("valueSelected");
                         if (Limpiar)
                             $(this).val('');
                     }

                 }
             };
             this.autocomplete(autocomp_opt).focus(function () {

             });
         }
          , Fn_AutoComplete_Input: function (Options) {
              //inicio autocomplete   
              var $input = this;
              Options = $.extend({
                  urlAutocomplete: 'SELECT_AUTOCOMPLETE',
                  Autocomplete_clear: true,
                  Change: null,
                  MaxRows: 15,
                  ParamNameMaxRow: 'maxRows',
                  ParamNameTerm: 'term',
                  MinLength: 2,
                  NoResultMessage: 'No se encontraron resultados.',
                  Columns: null
              }, Options);
              var url = Options.urlAutocomplete;
              var limpiar = Options.Autocomplete_clear;
              var Change = Options.Change;

              var autocomp_opt = {

                  source: function (request, response) {
                      var Success = function (data) {
                          if (data.d.Result == "OK") {
                              response($.map(data.d.Options, function (item) {
                                  var Obj = new Object();
                                  if (Options.Columns !== null)
                                      for (i = 0; i < Options.Columns.length; i++) {
                                          Obj[Options.Columns[i]] = item[Options.Columns[i]];
                                      }
                                  return {
                                      label: item.DisplayText,
                                      value: item.DisplayText,
                                      ID: item.Value,
                                      Columnas: Obj

                                  }
                              }))
                          }
                      }
                      $._EjecutaAJAX(url, "{'" + Options.ParamNameMaxRow + "':'" + Options.MaxRows + "','" + Options.ParamNameTerm + "':'" + request.term + "'}", Success);
                  },
                  minLength: Options.MinLength,
                  select: function (event, ui) {
                      $(this).attr("valueSelected", ui.item.ID)
                  },
                  response: function (event, ui) {
                      if (!ui.content.length) {
                          var noResult
                          noResult = { value: event.target.value, label: Options.NoResultMessage };
                          ui.content.push(noResult);
                      }
                  },
                  change: function (event, ui) {
                      var _limpiar = limpiar;
                      if (ui.item == null || ui.item == undefined) {
                          if (_limpiar === undefined || _limpiar == true)
                              $(this).val('');
                          $(this).removeAttr("valueSelected");
                      }
                      if (Options && Options.Change && ($.type(Options.Change) === 'function'))
                          Options.Change();
                  }
              };
              $input.autocomplete(autocomp_opt).focus(function () {
                  $(this).autocomplete("search", "");
              });



              //fin autocomplete
          }
        ,
        Fn_PreventCopy: function () {
            //$(this).bind("cut copy paste", function (e) {
            //    e.preventDefault();
            //});
            $(this).live('input propertychange', function () {
                maxLength(this, 50);
            });


            function maxLength(input, maxChar) {

                var len = $(input).val().length;
                var TipoInput = $(input).attr("TipoInput");

                if (input.maxLength !== undefined && input.maxLength > 0)
                    maxChar = input.maxLength;
                //Elimina caracteres diferentes a numeros , letras y caracteres permitidos
                // $(input).val($(input).val().replace(/[^a-zA-Z0-9áéíóúÁÉÍÓÚ@,.:;\- _]/g, ''));
                var regex = new RegExp("[^a-zA-Z0-9áéíóúÁÉÍÓÚ@ñÑ,.:;\-_\s ]", "g");
                if (TipoInput !== undefined && TipoInput == "URL")
                    regex = new RegExp("[^a-zA-Z0-9áéíóúÁÉÍÓÚ@ñÑ,.:;\-_/#&?=:\s -+]", "g");
                //  if (TipoInput !== undefined && TipoInput != "Todos")
                if (regex.test($(input).val()))
                    $(input).val($(input).val().replace(regex, ''));
                //Verifica si la longitud de la cadena es mayor a la especificada en el input.
                if (len > maxChar) {
                    $(input).val($(input).val().substring(0, maxChar));

                }

            }
        },
        bindFirst: function (name, fn) {
            var elem, handlers, i, _len;
            this.bind(name, fn);
            for (i = 0, _len = this.length; i < _len; i++) {
                elem = this[i];
                handlers = jQuery._data(elem).events[name.split('.')[0]];
                handlers.unshift(handlers.pop());
            }
        }
    });
    $.extend({
        _EjecutaAJAX: function (WEBMETHOD, DATA, OPTIONS, DONE, FAIL, ALWAYS) {
            OPTIONS = $.extend({
                url: WEBMETHOD,
                type: 'POST',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: DATA,
                async: false,
            }, OPTIONS);
            $("#cargando").css("display", "inherit");
            $.ajax(OPTIONS).done(function (data, textStatus, jqXHR) {
                if (DONE !== undefined && DONE != null && $.type(DONE) === "function") {
                    DONE(data, textStatus, jqXHR);
                }
            })
              .fail(function (jqXHR, textStatus, errorThrown) {
                  if (FAIL !== undefined && FAIL != null && $.type(FAIL) === "function") {
                      FAIL(jqXHR, textStatus, errorThrown);
                  }
              })
              .always(function (a, textStatus, b) {
                  if (ALWAYS !== undefined && ALWAYS != null && $.type(ALWAYS) === "function") {
                      ALWAYS(a, textStatus, b);
                  }
              });

        }

    });
    function addCommas(nStr, Digitos) {

        var subCadena = nStr;//.replace(/,/g, '')
        subCadena += '';
        var x = subCadena.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : Digitos > 0 ? '.' : '';
        if (Digitos > 0)
            x2 = (x2 + "00000000000000").slice(0, Number(Digitos) + 1);
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }
    //$.fn.bindFirst = function (name, fn) {
    //    var elem, handlers, i, _len;
    //    this.bind(name, fn);
    //    for (i = 0, _len = this.length; i < _len; i++) {
    //        elem = this[i];
    //        handlers = jQuery._data(elem).events[name.split('.')[0]];
    //        handlers.unshift(handlers.pop());
    //    }
    //};
})(jQuery)
function deleteAllCookies() {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;";
    }
}
function setCookie(cname, cvalue, exdays, path) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=" + path;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
function DeleteCookie(cname) {

    document.cookie = cname + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
}

var search = function () {
    var s = window.location.search.substr(1),
        p = s.split(/\&/),
        l = p.length,
        kv, r = {};
    if (l === 0) { return false; }
    while (l--) {
        kv = p[l].split(/\=/);
        r[kv[0]] = kv[1] || true;
    }
    return r;
}();
function searchQS(QS) {
    if (!QS)
        return false;
    QS = QS.replace("?", "");

    var s = QS,
        p = s.split(/\&/),
        l = p.length,
        kv, r = {};
    if (l === 0) { return false; }
    while (l--) {
        kv = p[l].split(/\=/);
        r[kv[0]] = kv[1] || true;
    }
    return r;
};
function _dateToYMD(date) {

    if (date.getDate) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + y + '/' + (m <= 9 ? '0' + m : m) + '/' + (d <= 9 ? '0' + d : d);
    }
    else {
        var dateVal = new Date(date);

        if (dateVal instanceof Date)//&& !  isNaN(dateVal.valueOf))​)
            return date.replace(/(\d{2})[- /.](\d{2})[- /.](\d{4})/, '$3-$2-$1').replace(/-/g, '/');
        else
            return "";
    }
}
function _dateToDMY(date) {

    if (date.getDate) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + (d <= 9 ? '0' + d : d) + '/' + (m <= 9 ? '0' + m : m) + '/' + y;
    }
    else {
        var dateVal = new Date(date);

        if (dateVal instanceof Date)//&& !  isNaN(dateVal.valueOf))​)
            return date.replace(/(\d{2})[- /.](\d{2})[- /.](\d{4})/, '$3-$2-$1').replace(/-/g, '/');
        else
            return "";
    }
}
function _OmitirAcentos(text) {
    text = decodeURI(text);
    var acentos = "ÃÀÁÄÂÈÉËÊÌÍÏÎÒÓÖÔÙÚÜÛãàáäâèéëêìíïîòóöôùúüûÑñÇç";
    var original = "AAAAAEEEEIIIIOOOOUUUUaaaaaeeeeiiiioooouuuunncc";
    for (var i = 0; i < acentos.length; i++) {
        text = text.replace(acentos.charAt(i), original.charAt(i));
    }
    return text;

}
function _Fn_parseDate(dateString) {

    if (dateString.indexOf('Date') >= 0) { //Format: /Date(1320259705710)/
        return new Date(
            parseInt(dateString.substr(6), 10)
        );
    } else if (dateString.length == 10) { //Format: 2011-01-01
        return new Date(
            parseInt(dateString.substr(0, 4), 10),
            parseInt(dateString.substr(5, 2), 10) - 1,
            parseInt(dateString.substr(8, 2), 10)
        );
    } else if (dateString.length == 19) { //Format: 2011-01-01 20:32:42
        return new Date(
            parseInt(dateString.substr(0, 4), 10),
            parseInt(dateString.substr(5, 2), 10) - 1,
            parseInt(dateString.substr(8, 2, 10)),
            parseInt(dateString.substr(11, 2), 10),
            parseInt(dateString.substr(14, 2), 10),
            parseInt(dateString.substr(17, 2), 10)
        );
    } else {
        this._logWarn('Given date is not properly formatted: ' + dateString);
        return 'format error!';
    }
}
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }
}
function permiteCadenaV2(elEvento, permitidos, adicionales) {

    // Variables que definen los caracteres permitidos
    var numeros = "0123456789";
    var caracteres = " abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜ";
    var numeros_caracteres = numeros + caracteres;
    var carurl = "/#&?=:-*";
    var teclas_especiales = [8, 9, 46, 11];
    // 8 = BackSpace, 46 = Supr, 37 = flecha izquierda, 39 = flecha derecha


    // Seleccionar los caracteres a partir del parámetro de la función
    switch (permitidos) {
        case 'num':
            permitidos = numeros;
            break;
        case 'alfa':
            permitidos = caracteres;
            break;
        case 'alfanum':
            permitidos = numeros_caracteres;
            break;
        case 'url':
            permitidos = numeros_caracteres + carurl;
            break;
    }
    if (adicionales)
        permitidos = permitidos + ",\"@";

    //¿?!¡()[],-_:*\"\'.;";
    // Obtener la tecla pulsada 
    var evento = elEvento || window.event;
    var codigoCaracter = evento.charCode || evento.keyCode;
    var caracter = String.fromCharCode(codigoCaracter);

    // Comprobar si la tecla pulsada es alguna de las teclas especiales
    // (teclas de borrado y flechas horizontales)
    var tecla_especial = false;
    for (var i in teclas_especiales) {
        if (codigoCaracter == teclas_especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    // Comprobar si la tecla pulsada se encuentra en los caracteres permitidos
    // o si es una tecla especial

    //if (caracter == '%') {
    //    return false;
    //}
    //else {
    //    return permitidos.indexOf(caracter) != -1 || tecla_especial;
    //}

    if ($(elEvento)[0].key == "'" || $(elEvento)[0].key == "%") {
        return false;
    }
    else {
        return permitidos.indexOf(caracter) != -1 || tecla_especial;
    }

}

function permiteCadena(elEvento, permitidos, adicionales) {

    // Variables que definen los caracteres permitidos
    var numeros = "0123456789.";
    var caracteres = " abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜ";
    var numeros_caracteres = numeros + caracteres;
    var carurl = "/#&?=:-*";
    var teclas_especiales = [8, 9, 46, 11, 58, 37, 39];
    // 8 = BackSpace, 46 = Supr, 37 = flecha izquierda, 39 = flecha derecha

    //debugger;
    // Seleccionar los caracteres a partir del parámetro de la función
    switch (permitidos) {
        case 'num':
            permitidos = numeros;
            break;
        case 'alfa':
            permitidos = caracteres;
            break;
        case 'alfanum':
            permitidos = numeros_caracteres;
            break;
        case 'url':
            permitidos = numeros_caracteres + carurl;
            break;
    }
    if (adicionales)
        permitidos = permitidos + ",.\"-_;@";

    //¿?!¡()[],-_:*\"\'.;";
    // Obtener la tecla pulsada 
    var evento = elEvento || window.event;
    var codigoCaracter = evento.charCode || evento.keyCode;
    var caracter = String.fromCharCode(codigoCaracter);

    // Comprobar si la tecla pulsada es alguna de las teclas especiales
    // (teclas de borrado y flechas horizontales)
    var tecla_especial = false;
    for (var i in teclas_especiales) {
        if (codigoCaracter == teclas_especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    // Comprobar si la tecla pulsada se encuentra en los caracteres permitidos
    // o si es una tecla especial

    //if (caracter == '%') {
    //    return false;
    //}
    //else {
    //    return permitidos.indexOf(caracter) != -1 || tecla_especial;
    //}

    if ($q(elEvento)[0].key == "'" || $q(elEvento)[0].key == "%") {
        return false;
    }
    else {
        return permitidos.indexOf(caracter) != -1 || tecla_especial;
    }

}
function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return expr.test(email);

}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}






