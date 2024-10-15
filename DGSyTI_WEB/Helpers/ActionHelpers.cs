using System.Runtime.Remoting.Messaging;
using DGSyTI_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessModel.Business;
namespace DGSyTI_WEB.Helpers
{
    public enum ActionType
    {
        Add, Save, Return, ExportToExcel, ExportToPdf, Accept, Delete, Edit, Next,
        Back, Seguimiento, Asign, Canalizacion, CargaDescarga, Descartar, AddSeguimiento, AddObservacion, Imprimir
    }

    public static class ActionHelpers
    {

        private static string GetPath(ActionType type)
        {
            string img = "";
            switch (type)
            {
                case ActionType.Add:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/add.png");
                    break;
                case ActionType.Save:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/save.png");
                    break;
                case ActionType.Return:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/return.png");
                    break;
                case ActionType.ExportToExcel:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/ms-excel.png");
                    break;
                case ActionType.ExportToPdf:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/pdf.png");
                    break;
                case ActionType.Accept:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/confirm.png");
                    break;
                case ActionType.Delete:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/delete.png");
                    break;
                case ActionType.Edit:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/edit.png");
                    break;
                case ActionType.Next:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/icono_siguiente.png");
                    break;
                case ActionType.Back:
                    img = VirtualPathUtility.ToAbsolute("~/Images/layout/imagenes_estilos/icono_atras.png");
                    break;
                case ActionType.Seguimiento:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/seguimiento.png");
                    break;
                case ActionType.Asign:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/asignar.png");
                    break;
                case ActionType.Canalizacion:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/canalizacion.png");
                    break;
                case ActionType.CargaDescarga:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/cargaDescarga.png");
                    break;
                case ActionType.Descartar:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/descartar.png");
                    break;
                case ActionType.AddSeguimiento:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/addSeguimiento.png");
                    break;
                case ActionType.AddObservacion:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/addObservacion.png");
                    break;
                case ActionType.Imprimir:
                    img = VirtualPathUtility.ToAbsolute("~/Images/actions/pdf.png");
                    break;
            }
            return img;
        }

        public static MvcHtmlString ActionLinkImg(this HtmlHelper helper, string text, string action, ActionType type)
        {
            return ActionLinkImg(helper, text, action, type, null, null);
        }

        public static MvcHtmlString ActionLinkImg(this HtmlHelper helper, string text, string action, string imgUrl)
        {
            return ActionLinkImg(helper, text, action, imgUrl, null, null);
        }

        public static MvcHtmlString ActionLinkImg(this HtmlHelper helper, string text, string action, ActionType type, object routeValues)
        {
            return ActionLinkImg(helper, text, action, GetPath(type), routeValues, null);
        }

        public static MvcHtmlString ActionLinkImg(this HtmlHelper helper, string text, string action, ActionType type, object routeValues, object htmlAttributes)
        {
            return ActionLinkImg(helper, text, action, GetPath(type), routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkForm(this HtmlHelper helper,string text,string controller, string action, string urlImg, object routeValues, object htmlAttributes)
        {
            return ActionLinkFormtag(helper, text, controller, action, urlImg, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkForm(string text, string controller, string action, string urlImg, object routeValues, object htmlAttributes)
        {
            return ActionLinkFormtag(text, controller, action, urlImg, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkImg(this HtmlHelper helper, string text, string action, string imgUrl, object routeValues, object htmlAttributes)
        {
            var controller = helper.ViewContext.RouteData.GetRequiredString("controller");
            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = "<img src='" + imgUrl + "' border='0' height='20' width='20'>&nbsp;" + text;

            string dir = VirtualPathUtility.ToAbsolute("~/" + controller + "/" + action);
            if (routeValues != null)
            {
                RouteValueDictionary routes = new RouteValueDictionary(routeValues);
                for (int i = 0; i < routes.Count; i++)
                {
                    var element = routes.ElementAt(i);
                    if (i == 0)
                    {
                        dir += "/" + element.Value;
                    }
                    else if (i == 1)
                    {
                        dir += "?" + element.Key + "=" + element.Value;
                    }
                    else
                    {
                        dir += "&" + element.Key + "=" + element.Value;
                    }
                }
            }
            builder.Attributes.Add("href", dir);
            builder.Attributes.Add("class", "button-link");
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes((htmlAttributes ?? new Object())));

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ActionLinkFormtag(this HtmlHelper helper, string text,string controller, string action, string imgUrl, object routeValues, object htmlAttributes,bool registraNavegacion=false)
        {
            string dir = VirtualPathUtility.ToAbsolute("~/" + controller +(action.Equals("")?"":"/"+ action));
            Random r = new Random();
            Random seed = new Random(DateTime.Now.Millisecond);
            int aleatorionuevo = r.Next(1, 1000);
            int aleatorio = r.Next(aleatorionuevo, 500000);
            Guid miGuid = Guid.NewGuid();
            string token = Convert.ToBase64String(miGuid.ToByteArray());
            string camposOcultos = "";
            //builder.Append("" + helper.AntiForgeryToken() + "");
            string value = "";
            if (routeValues != null)
            {
                RouteValueDictionary routes = new RouteValueDictionary(routeValues);
                for (int i = 0; i < routes.Count; i++)
                {
                    var element = routes.ElementAt(i);
                    camposOcultos+="<input type='hidden' id='" + element.Key + "' name='" + element.Key + "' value='" + element.Value + "'/>";
                    value+= element.Value;
                }
            }

            string nombreFormulario = "frm"+ token + controller + seed.Next() + DateTime.Now.Millisecond.ToString()+value;
            StringBuilder builder = new StringBuilder("<form class='formAccion' method='post' id='" + nombreFormulario + "' action='" + dir + "'>");
            builder.Append(camposOcultos);
            //builder.Append("<button class='button-link white linkAction " + htmlAttributes + "' name='btn" + action + "' title='' type='submit' value=''>");
            string link=@"<a href=&#& id=&submit& onclick=&document.getElementById('"+ nombreFormulario + "').submit();& class=&" + htmlAttributes + "&>";

            builder.Append(link.Replace("&","\""));


            if (imgUrl != null && !imgUrl.Equals(""))
            {
                builder.Append("<img style='vertical-align:text-bottom' src='" + imgUrl + "' />");
            }
            builder.Append("" + text + "</a>");
            builder.Append("</form>");

            
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ActionLinkFormtag(string text, string controller, string action, string imgUrl, object routeValues, object htmlAttributes, bool registraNavegacion = false)
        {
            string dir = VirtualPathUtility.ToAbsolute("~/" + controller + (action.Equals("") ? "" : "/" + action));
            Random r = new Random();
            int aleatorio = r.Next(0, 500000);
            string nombreFormulario = "frm" + aleatorio.ToString().Trim();
            StringBuilder builder = new StringBuilder("<form class='formAccion' method='post' id='" + nombreFormulario + "' action='" + dir + "'>");
            //builder.Append("" + helper.AntiForgeryToken() + "");

            if (routeValues != null)
            {
                RouteValueDictionary routes = new RouteValueDictionary(routeValues);
                for (int i = 0; i < routes.Count; i++)
                {
                    var element = routes.ElementAt(i);
                    builder.Append("<input type='hidden' id='" + element.Key + "' name='" + element.Key + "' value='" + element.Value + "'/>");
                }
            }

            //builder.Append("<button class='button-link white linkAction " + htmlAttributes + "' name='btn" + action + "' title='' type='submit' value=''>");
            string link = @"<a href=&#& id=&submit& onclick=&document.getElementById('" + nombreFormulario + "').submit();& class=&" + htmlAttributes + "&>";

            builder.Append(link.Replace("&", "\""));


            if (imgUrl != null && !imgUrl.Equals(""))
            {
                builder.Append("<img style='vertical-align:text-bottom' src='" + imgUrl + "' />");
            }
            builder.Append("" + text + "</a>");
            builder.Append("</form>");


            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString SubmitLinkImg(this HtmlHelper helper, string text, string name, ActionType type)
        {
            return SubmitLinkImg(helper, text, name, type, null, null);
        }

        public static MvcHtmlString SubmitLinkForm(this HtmlHelper helper, string text, string name, ActionType type, string cssClass)
        {
            return new MvcHtmlString(BuildTag(text, name, null, type, cssClass, null).ToString());
        }

        public static MvcHtmlString SubmitLinkImg(this HtmlHelper helper, string text, string name, ActionType type, string cssClass)
        {
            return new MvcHtmlString(BuildTag(text, name, null, type, cssClass, null).ToString());
        }

        public static MvcHtmlString SubmitLinkImg(this HtmlHelper helper, string text, string name, string value, ActionType type, string cssClass)
        {
            return new MvcHtmlString(BuildTag(text, name, value, type, cssClass, null).ToString());
        }

        public static MvcHtmlString SubmitLinkImg(this HtmlHelper helper, string text, string name, ActionType type, string cssClass, string titulo)
        {
            return new MvcHtmlString(BuildTag(text, name, null, type, cssClass, titulo).ToString());
        }

        private static TagBuilder BuildTag(string text, string name, string value, ActionType type, string cssClass, string title)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = "<img src='" + GetPath(type) + "' border='0' height='20' width='20'>&nbsp;" + text;
            builder.Attributes.Add("type", "submit");
            builder.Attributes.Add("name", name);
            builder.Attributes.Add("value", value);
            builder.Attributes.Add("class", "button-link " + (cssClass ?? String.Empty));
            builder.Attributes.Add("title", (title ?? String.Empty));
            return builder;
        }




        public static MvcHtmlString ActionBar(this HtmlHelper helper, params HtmlString[] str)
        {
            var controller = helper.ViewContext.RouteData.GetRequiredString("controller");
            TagBuilder builder = new TagBuilder("div");
            StringBuilder sb = new StringBuilder();
            foreach (var item in str)
            {
                sb.Append(item);
            }
            builder.InnerHtml = "<div class='content'>"
                + "<div class='left'>" + sb.ToString() + "</div></div>";
            builder.Attributes.Add("class", "acciones");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ActionSpanImg(this HtmlHelper helper, string text, string action, ActionType type,
            object htmlAttributes)
        {
            return ActionSpanImg(helper, text, action, GetPath(type), htmlAttributes);
        }

        public static MvcHtmlString ActionSpanImg(this HtmlHelper helper, string text, string action, string imgUrl, object htmlAttributes)
        {
            var builder = new TagBuilder("span")
            {
                InnerHtml = "<img src='" + imgUrl + "' border='0' height='20' width='20'>&nbsp;" + text
            };
            builder.Attributes.Add("class", "button-link");
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes((htmlAttributes ?? new Object())));
            return new MvcHtmlString(builder.ToString());
        }
    }
}