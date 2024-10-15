using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Helpers
{
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 06Junio204
    /// Descripción: Clase estática que provee una serie de métodos sobrecargados para la construcción del tag de html de imagen, conforme a los 
    ///             valores de los atributos que se vayan proporcionando..
    /// </summary>
    /// <remarks>Construido por Eduardo Jaramillo Olvera</remarks>
    public static class ImageTagHelper
    {
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método que permitira cosntruir el tag de imagen de html en base a los valores de los atriubutos que se este proporcionando.
        /// </summary>    
        /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
        /// <param name="id">Parámetro que identificará el tag de imagen de html</param>
        /// <param name="url">Parámetro que idicará la ruta URL de donde se obtendrá la imagen</param>
        /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
        public static MvcHtmlString ImageTag(this HtmlHelper helper, string id, string url)
        {
            return ImageTag(helper, id, url, "", "", "", null);
        }
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método que permitira cosntruir el tag de imagen de html en base a los valores de los atriubutos que se este proporcionando.
        /// </summary>        
        /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
        /// <param name="id">Parámetro que identificará el tag de imagen de html</param>
        /// <param name="url">Parámetro que idicará la ruta URL de donde se obtendrá la imagen</param>
        /// <param name="classCSS">Parámetro que proveerá el nombre de la clase de estilo para aplicarse al tag de imagen.</param>
        /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
        public static MvcHtmlString ImageTag(this HtmlHelper helper, string id, string url, string classCSS)
        {
            return ImageTag(helper, id, url, classCSS, "", "", null);
        }
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método que permitira cosntruir el tag de imagen de html en base a los valores de los atriubutos que se este proporcionando.
        /// </summary>    
        /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
        /// <param name="id">Parámetro que identificará el tag de imagen de html</param>
        /// <param name="url">Parámetro que idicará la ruta URL de donde se obtendrá la imagen</param>
        /// <param name="classCSS">Parámetro que proveerá el nombre de la clase de estilo para aplicarse al tag de imagen.</param>
        /// <param name="style">Parámetro que permitirá incluir los estilos en el tag de imagen.</param>
        /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
        public static MvcHtmlString ImageTag(this HtmlHelper helper, string id, string url, string classCSS, string style)
        {
            return ImageTag(helper, id, url, classCSS, style, "", null);
        }
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método que permitira cosntruir el tag de imagen de html en base a los valores de los atriubutos que se este proporcionando.
        /// </summary>    
        /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
        /// <param name="id">Parámetro que identificará el tag de imagen de html</param>
        /// <param name="url">Parámetro que idicará la ruta URL de donde se obtendrá la imagen</param>
        /// <param name="classCSS">Parámetro que proveerá el nombre de la clase de estilo para aplicarse al tag de imagen.</param>
        /// <param name="style">Parámetro que permitirá incluir los estilos en el tag de imagen.</param>
        /// <param name="alternateText">Parámetro que permitirá incluir el texto alterno al tag de imagen.</param>
        /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
        public static MvcHtmlString ImageTag(this HtmlHelper helper, string id, string url, string classCSS, string style, string alternateText)
        {
            return ImageTag(helper, id, url, classCSS, style, alternateText, null);
        }
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método que permitira cosntruir el tag de imagen de html en base a los valores de los atriubutos que se este proporcionando.
        /// </summary>    
        /// <param name="helper">Parámetro de extendiende la funcionalidad del helper Html al momento de ser llamado desde la vista</param>
        /// <param name="id">Parámetro que identificará el tag de imagen de html</param>
        /// <param name="url">Parámetro que idicará la ruta URL de donde se obtendrá la imagen</param>
        /// <param name="classCSS">Parámetro que proveerá el nombre de la clase de estilo para aplicarse al tag de imagen.</param>
        /// <param name="style">Parámetro que permitirá incluir los estilos en el tag de imagen.</param>
        /// <param name="alternateText">Parámetro que permitirá incluir el texto alterno al tag de imagen.</param>
        /// <param name="htmlAttributes">Paramétro que permitirá incluir otros atributos de html al tag de imagen.</param>
        /// <returns>Devuelve una objeto de tipo MvcHtmlString correspondiente a la estrutura Html del tag de imagen.</returns>
        public static MvcHtmlString ImageTag(this HtmlHelper helper, string id, string url, string classCSS, string style, string alternateText, object htmlAttributes)
        {
            if (url.Trim().Length == 0) return null;
            TagBuilder img = new TagBuilder("img");
            if(id.Trim().Length > 0) img.GenerateId(id);
            img.MergeAttribute("src", url);
            if (classCSS.Trim().Length > 0 ) img.AddCssClass(classCSS.Trim());
            if (style.Trim().Length > 0 && classCSS.Trim().Length == 0) img.MergeAttribute("style", style.Trim());
            if (alternateText.Trim().Length > 0) img.MergeAttribute("alt", alternateText.Trim());
            if (htmlAttributes != null) img.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(img.ToString(TagRenderMode.SelfClosing));
        }

    }
}