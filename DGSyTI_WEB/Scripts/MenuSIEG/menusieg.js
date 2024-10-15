var $q = jQuery.noConflict();
//$q.ajaxSetup({ cache: false });

$q(document).ready(function () {


    $q(".openDialog").on("click", function (e) {
        e.preventDefault();
        var ancho = $q(window).width();
        var alto = $q(document).height();

        $q("<div></div>")
            .addClass("dialog")
            .attr("id", $q(this)
                .attr("data-dialog-id"))
            .appendTo("body")
            .dialog({
                title: $q(this).attr("data-dialog-title"),
                close: function () { $q(this).remove(); },
                modal: true,
                height: $q(this).attr("data-dialog-height"),
                width: $q(this).attr("data-dialog-width"),
                top: '30%',


                resizable: false,
                dialogClass: 'main-dialog-class'

            })
            .load(this.href);

    });

    $q(".close").on("click", function (e) {
        e.preventDefault();
        $q(this).closest(".dialog").dialog("close");
    });

    
    var cookieMenu = null;// readCookie("cookieMenu");
    //alert(cookieMenu);
    
    if (cookieMenu==null || cookieMenu==0) {

        item = $q("#divMenu");
        $q("#mostrar_menu").hide();
        $q("#ocultar_menu").show();
        $q(item).css({ "display": "inline-table" });
        $q(item).removeClass('invisible');
        $q(item).addClass('visible');
        //$q(item).slideDown('slow');
        $q("#mostrarHover_menu").hide();

    }
    else
    {
        if (cookieMenu==1) {
            //alert('1');
            item = $q("#divMenu");
            $q(item).removeClass('visible');
            //// cambiamos su estado
            $q(item).addClass('invisible');
            $q("#ocultar_menuHover").hide();
            $q("#mostrar_menu").show();
            $q("#ocultar_menu").hide();
            $q(item).css({ "display": "none" });
            //// animamos
            //$q(item).slideUp('slow');
        }
    }

});

function oculta(elemento) {
    ///// capturamos el elemento
    item = $q("#" + elemento);
  
    ///// verificamos su estado
    if ($q(item).hasClass('visible')) {
        //alert('ocultar');
        $q(item).removeClass('visible');
        //// cambiamos su estado
        $q(item).addClass('invisible');
        $q("#ocultar_menuHover").hide();
        $q("#mostrar_menu").show();
        //// animamos
        $q(item).slideUp('slow');
        //document.cookie = "cookieMenu=false;";
        $q("#contentMenu").attr("style", "width:0%");
        $q("#contentPrincipal").attr("style", "width:99%");
        createCookie("cookieMenu", 1);
    } else {
        //alert('mostrar');
        $q("#mostrar_menu").hide();
        $q(item).css({ "display": "inline-table" });
        $q(item).removeClass('invisible');
        $q(item).addClass('visible');
        $q(item).slideDown('slow');
        $q("#ocultar_menu").show();
        $q("#mostrarHover_menu").hide();
        $q("#contentMenu").attr("style", "width:15%");
        $q("#contentPrincipal").attr("style", "width:84%");
        //document.cookie = "cookieMenu=true;";
        createCookie("cookieMenu", 0);
    }
}

function mouseHoverOculta() {
    item = $q("#divMenu");

    if ($q(item).hasClass('visible')) {

        $q("#ocultar_menu").hide();
        $q("#ocultar_menuHover").show();
    } else {
        $q("#mostrar_menu").hide();
        $q("#mostrarHover_menu").show();
    }
}

function mouseOutOculta() {
    if ($q(item).hasClass('visible')) {
        $q("#ocultar_menuHover").hide();
        $q("#ocultar_menu").show();

    } else {
        $q("#mostrarHover_menu").hide();
        $q("#mostrar_menu").show();

    }
}