

var strURLImage;
var strLogin;
var strRedirectExit;
var strUsrName;
var strEmail;
var strMenu;

try {

    jQuery(document).ready(function () {
        strUsrName = jQuery("#nombre").val();
        strEmail = jQuery("#email").val();
        strLogin = jQuery("#login").val();
        strMenu = jQuery("#menu").val();
        strRedirectExit = ("http://172.17.1.109:7001/sieg");
        strAccesoLiveedu = jQuery("#strAccesoLiveedu").val();
    });

} catch (e) { alert("Error:" + e.message); }



