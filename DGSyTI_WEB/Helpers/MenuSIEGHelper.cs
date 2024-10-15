using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DGSyTI_WEB;
using BusinessModel.Business;

namespace Helpers
{
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 06Junio204
    /// Descripción: Clase estática que construye el menú izquierdo de la plantilla principal conforme a los permisos y privilegios definidos de un usuario,
    ///             Esta clase analiza el contenido de las acciones asignadas al usuario.
    /// </summary>
    /// <remarks>Construido por Eduardo Jaramillo Olvera, en base al diseño correspondiente del SIIEg por parte de Julio Rangel y Jorge Tapia</remarks>
    public static class MenuSIEGHelper
    {
       /// <summary>
       /// Autor: Secretaría de Educación de Guanajuato
       /// Fecha de Creación: 06Junio204
       /// Descripción: Método que permitira cosntruir el menú izquierdo en formato html en base a los valores a los parametrización de las acciones en el SIIEg
       ///             para un usuario específico.
       /// </summary>  
       /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
       /// <param name="user">Parámetro que porveerá la información del usuario que está autenticado en la sesión del aplicativo, dicho
       /// objeto cuenta con la lista de acciones que le fueron asignadas en el Sistema de Control de Seguridad.</param>
       /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
       public static MvcHtmlString GetMenu(this HtmlHelper helper, Usuario user) 
       {
           string result = "";
           
           if (user != null && user.ListaAcciones != null && user.ListaAcciones.Count > 0)
           {
               TagBuilder tag = new TagBuilder("ul");
               tag.AddCssClass("ul-menu");
               using (List<Acciones>.Enumerator ac = user.ListaAcciones.GetEnumerator())
               {                   
                   while (ac.MoveNext())
                   {
                       TagBuilder itemTag = new TagBuilder("li");
                       if (ac.Current.Nivel == 1)
                       {
                           itemTag.AddCssClass("itemHeader");
                           itemTag.SetInnerText(ac.Current.AccNombre);
                       }
                       if (ac.Current.Nivel == 2 && ac.Current.AccURL!=null && ac.Current.AccURL.Trim().Length == 0) 
                       {
                           itemTag.AddCssClass("itemHeader2");
                           itemTag.SetInnerText(ac.Current.AccNombre);

                       }
                        //Modificación : 2014-09-25
                        //Autor: Jorge Tapia
                        //Descripción: Se agrego en la validación siguiente la condición "ac.Current.AccURL!=null", ya que ocasionaba problemas
                        //           al trabajar en estas opciones del menú por no encontrarse la ruta "AccURL"
                        var sImg = AppConfig.AppEnviroment.menuAccion;
                        string sAccesoURL = "";
                        string sNombreMenu = "";
                        if ((ac.Current.Nivel == 2 || ac.Current.Nivel == 3) && ((ac.Current.AccURL != null && ac.Current.AccURL.Trim().Length > 0)))
                       {
                           //itemTag.AddCssClass(((ac.Current.Nivel == 2)?"item":"itemSub"));
                           //MvcHtmlString link = LinkExtensions.ActionLink(helper, ac.Current.AccNombre, "index", ac.Current.AccURL);
                           //MvcHtmlString img = ImageTagHelper.ImageTag(helper, ac.Current.AccId.ToString(), UrlHelper.GenerateContentUrl(AppConfig.AppEnviroment.repositorioImagenes+ "Iconos/MenuLateral/" + ac.Current.AccNombre+".png", new HttpContextWrapper(HttpContext.Current)), "", "width:24px; height:24px;");
                           //itemTag.InnerHtml = img.ToString()+" " + link.ToString()  ;

                            itemTag.AddCssClass(((ac.Current.Nivel == 2) ? "item" : "itemSub"));

                            string[] sURL = ac.Current.AccURL.Split(';');
                            if (sURL.Length > 1)
                            {
                                sImg = sURL[0];
                                sAccesoURL = sURL[1];
                            }
                            else
                            {
                                sImg = AppConfig.AppEnviroment.menuAccion;
                                sAccesoURL = ac.Current.AccURL;
                            }

                            string[] array = sAccesoURL.Split('?');
                            sAccesoURL = sAccesoURL.Split('?')[0];
                            string p = (array.Length > 1 ? (array[1] != null ? array[1] : "") : "");
                            MvcHtmlString link = LinkExtensions.ActionLink(helper, ac.Current.AccNombre, "index", sAccesoURL, new { pr = p }, null);

                            MvcHtmlString img = ImageTagHelper.ImageTag(helper, ac.Current.AccId.ToString(), UrlHelper.GenerateContentUrl(sImg, new HttpContextWrapper(HttpContext.Current)), "", "width:20px; height:20px;");
                            itemTag.AddCssClass(sNombreMenu.Replace(" ", ""));
                            itemTag.InnerHtml = img.ToString() + link.ToString();

                        }
                       // Importante: Deberá tomar en cuenta que el estandar del menú izquierdo es en base a tres niveles, por lo que
                       //           en caso de configurar más niveles del menú, el programador deberá incorporar las rutinas para su despliegue.
                       tag.InnerHtml += itemTag.ToString();
                   }
               }
               string js = Scripts.Render(new string[] { AppConfig.AppEnviroment.jsMenuSieg }).ToString();
               result = js + tag.ToString();
               result += string.Format("<input type=\"hidden\" id=\"login\" value=\"{0}\" />", user.Login);
               result += string.Format("<input type=\"hidden\" id=\"nombre\" value=\"{0}\" />", user.NombreCompleto);
               result += string.Format("<input type=\"hidden\" id=\"email\" value=\"{0}\" />", user.Email);
              // result += string.Format("<input type=\"hidden\" id=\"menu\" value=\"{0}\" />", user.Menu);
           }
           return new MvcHtmlString(result);
       }

        public static MvcHtmlString GetMenuHorizontal(this HtmlHelper helper, Usuario user)
        {

            string result = "<div class='panel-body' style='width:99%;'>";
                result += "<div class='row' style='margin-left:0px !important;'>";

            //foreach (var opcion in EstadisticasBL.getAccionesbyUsuario(user.Login))
            //{
            //    string opcionMenu = "";
            //    //foreach (var opcion in user.ListaAcciones.Where(c=>c.AccOrden==999)) {
            //    result += "" +
            //    //"<ul class='nav navbar-nav'>" +
            //    //"<li>
            //    "<div class='col-md-1' style='height:90px; background-color:white !important; padding-right:0px !important; padding-left:0px;'>" +
            //        "<div class='panel hoverDiv' style='border: 0px; border-style:solid; border-color:white; width: 99.5%;'>" +
            //            "<div class='panel-heading backgroundGris4' style='min-height:55px; border-radius: 3px; border-style: solid; border-width: 0px 0px 4px; text-align:center; width: 98%;'>";


            //     if (opcion.linkNavegacion == "")
            //    {
            //        opcionMenu = "<a href='/" + opcion.estPantalla + "'  style='text-decoration:none;'>" +
            //            "<div class='tituloRes'>" +
            //                //"<img src = 'http://repositorioimagenes/cemsysi/Iconos/MenuHorizontal/" + opcion.menuNombre + ".png' style='width: 30px;'>" +
            //                "&nbsp;" + opcion.menuNombre +
            //                (opcion.menuIcono != "" && opcion.menuIcono != null ? "<br/><img src = 'http://repositorioimagenes/cemsysi/Iconos/MenuHorizontal/" + opcion.menuIcono + ".png' style='width: 40px;'>" : "<br/><img src = 'http://repositorioimagenes/cemsysi/Iconos/MenuHorizontal/Default.png' style='width: 40px;'>") +
            //        //"<br/><img src = 'http://repositorioimagenes/cemsysi/Iconos/MenuHorizontal/" + opcion.menuNombre + ".png' style='width: 40px;'>" +                                "</div>" +
            //        "</a>";
            //    }
            //    else
            //    {
            //        opcionMenu = "<div class='tituloRes'>" + opcion.linkNavegacion+"</div>";
            //    }
            //    result= result + opcionMenu+
            //    //"" + opcion.menuNombre + "</a>"+
            //            "</div>" +
            //        "</div>"+
            //    "</div>";
            //    //"</li>" +
            //    //"</ul>";
            //}
            result += "</div></div>";

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString GetMenuAyuda(this HtmlHelper helper, Usuario user)
        {
            string result = "";
            result += "<div id='divCategorias' style='display: block; width: 60%;'>";

            if (user != null && user.ListaAcciones != null && user.ListaAcciones.Count > 0)
            {
                //TagBuilder tag = new TagBuilder("ul");
                //tag.AddCssClass("ul-menu");
                result += "<div id='cat'>";
                result += "Contenido<br /><br />";
                result += "<button class='collapsible'><b>Consideraciones generales</b></button>";
                result += "<div class='content'>";
                result += "<p style='margin-top:10px;'>" +
                            "<a href='#' onclick='consultaTema(\"Introducción\");'>Introducción</a><br/>" +
                            "</p>";
                result += "</div>";


                using (List<Acciones>.Enumerator ac = user.ListaAcciones.GetEnumerator())
                {
                    int menuA = 0;
                    while (ac.MoveNext())
                    {

                        //TagBuilder itemTag = new TagBuilder("li");
                        if (ac.Current.Nivel == 1)
                        {
                            if (menuA == 1)
                            {
                                result += "</div>";
                            }
                            menuA = 1;
                            //itemTag.AddCssClass("itemHeader");
                            //itemTag.SetInnerText(ac.Current.AccNombre);
                            result += "<button class='collapsible'><b>" + ac.Current.AccNombre + "</b></button>" +
                                "<div class='content'>";
                        }
                        /*if (ac.Current.Nivel == 2 && ac.Current.AccURL != null && ac.Current.AccURL.Trim().Length == 0)
                        {
                            //itemTag.AddCssClass("itemHeader2");
                            //itemTag.SetInnerText(ac.Current.AccNombre);
                            result += "<p>" +
                            "<a href='#' onclick='consultaTema(" + ac.Current.AccNombre + ");'> " + ac.Current.AccNombre + "</a><br/>" +
                            "</p>";
                        }*/
                        //Modificación : 2014-09-25
                        //Autor: Jorge Tapia
                        //Descripción: Se agrego en la validación siguiente la condición "ac.Current.AccURL!=null", ya que ocasionaba problemas
                        //           al trabajar en estas opciones del menú por no encontrarse la ruta "AccURL"
                        if ((ac.Current.Nivel == 2 || ac.Current.Nivel == 3) && ((ac.Current.AccURL != null && ac.Current.AccURL.Trim().Length > 0)))
                        {
                            //itemTag.AddCssClass(((ac.Current.Nivel == 2) ? "item" : "itemSub"));
                            //MvcHtmlString link = LinkExtensions.ActionLink(helper, ac.Current.AccNombre, "index", ac.Current.AccURL);
                            //MvcHtmlString img = ImageTagHelper.ImageTag(helper, ac.Current.AccId.ToString(), UrlHelper.GenerateContentUrl(AppConfig.AppEnviroment.repositorioImagenes + "Iconos/MenuLateral/" + ac.Current.AccNombre + ".png", new HttpContextWrapper(HttpContext.Current)), "", "width:24px; height:24px;");
                            //itemTag.InnerHtml = img.ToString() + " " + link.ToString();
                            result += "<p style='margin-top:10px;'>" +
                            "<a href='#' onclick='consultaTema(\"" + ac.Current.AccNombre + "\");'>" + ac.Current.AccNombre + "</a><br/>" +
                            "</p>";

                        }
                        // Importante: Deberá tomar en cuenta que el estandar del menú izquierdo es en base a tres niveles, por lo que
                        //           en caso de configurar más niveles del menú, el programador deberá incorporar las rutinas para su despliegue.
                        //tag.InnerHtml += itemTag.ToString();
                    }
                }
                result += "</div>";
                //string js = Scripts.Render(new string[] { AppConfig.AppEnviroment.jsMenuSieg }).ToString();
                //result = js + tag.ToString();
                //result += string.Format("<input type=\"hidden\" id=\"login\" value=\"{0}\" />", user.Login);
                //result += string.Format("<input type=\"hidden\" id=\"nombre\" value=\"{0}\" />", user.NombreCompleto);
                //result += string.Format("<input type=\"hidden\" id=\"email\" value=\"{0}\" />", user.Email);
                // result += string.Format("<input type=\"hidden\" id=\"menu\" value=\"{0}\" />", user.Menu);
            }
            result += "</div>";
            return new MvcHtmlString(result);
        }

    }
}