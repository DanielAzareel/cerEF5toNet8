$(document).ready(function () {
    $.ajax({
        type: 'POST',
        url: "/DescargaCertificado/cargaBotones",
        success: function (result) {
            jQuery("#botonesPortalDescarga").html(result)
        },
        error: function () {
        }
    });
})

function onSuccessDescarga(result) {
    if (result[0] == "True") {
        jQuery.ajax({
            type: 'POST',
            url: "/DescargaCertificado/mostrarListado",
            success: function (result) {
                jQuery("#idSeccionDescarga").html(result)
            },
            error: function () {
            }
        });
    } else {
        jQuery("#errorToken").html(result[1])
        jQuery("#errorToken").removeAttr("hidden")
    }
}

function nuevoToken() {
    jQuery.ajax({
        type: 'POST',
        url: "/DescargaCertificado/formCorreo",
        success: function (result) {
            jQuery("#idSeccionDescarga").html(result)
        },
        error: function () {
        }
    });
}
function onSuccessCorreo(result) {
    if (result[0] == "True") {
        alertify.alert("", 'Un nuevo token de descarga fue enviado a su correo electrónico.', function () {
            window.location.href = result[1];
        }).setHeader('<section class="headerAlert headerSuccess" success>Envío</section> ');
    } else if (result[0] == "False") {
        alertify.alert("", result[1], function () {
        }).setHeader('<section class="headerAlert" warning>Envío</section> ');
    } else {
        jQuery("#errorToken").html(result[1])
        jQuery("#errorToken").removeAttr("hidden")
    }
}
function eliminaCookie() {
    document.cookie = "pToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    document.cookie = "pId=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}
function quitaMensaje() {
    jQuery("#errorToken").html("")
    jQuery("#errorToken").attr("hidden")
}
function Autenticar() {
    $.ajax({
        type: 'POST',
        url: "/DescargaCertificado/modalAutenticacion",
        success: function (result) {
            jQuery("#idSeccionDescarga").html(result)
        },
        error: function () {
        }
    });
}