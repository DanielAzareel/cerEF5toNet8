using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using System.Web.Routing;
namespace Helpers
{
    public static class GridHelper
    {
        public static MvcHtmlString paginacion(){

             
            string result = "&lt;table class='barraPaginacion' style=&quot;text-align: center&quot;" +
                "&gt; &lt;tbody&gt; &lt;tr&gt; &lt;td style=&quot;text-align: center&quot; aling=&quot;" +
                "center&quot;&gt; &lt;center&gt; &lt;table&gt;  &lt;tr&gt; &lt;td style=&quot;font-size: 12px;" +
                "&quot;&gt; &lt;a href='#' id=&quot;btnInicio&quot;&gt; &lt;div id=&quot;imgInicio&quot;&gt;&lt;/div&gt;" +
                " &lt;/a&gt;&amp;nbsp;&amp;nbsp;&amp;nbsp; &lt;/td&gt; &lt;td style=&quot;font-size: 12px;&quot;&gt; " +
                "&lt;a href='#' id=&quot;btnAnterior&quot;&gt; &lt;div id=&quot;imgAnterior&quot;&gt;&lt;/div&gt;" +
                " &lt;/a&gt;&amp;nbsp;&amp;nbsp;&amp;nbsp; &lt;/td&gt; &lt;td style=&quot;font-size: 12px;&quot;" +
                "&gt; &lt;div id=&quot;htmlTotalRegistro&quot;&gt; &lt;/div&gt; &lt;/td&gt; &lt;" +
                "td style=&quot;font-size: 12px;&quot;&gt; &lt;a href='#' id=&quot;btnSiguiente&quot;&gt; &lt;div id=&quot;" +
                "imgSiguiente&quot;&gt;&lt;/div&gt; &lt;/a&gt;&amp;nbsp;&amp;nbsp;&amp;nbsp; &lt;/td&gt; &lt;td style=&quot;" +
                "font-size: 12px;&quot;&gt; &lt;a href='#' id=&quot;btnFin&quot;&gt; &lt;div id=&quot;imgFin&quot;&gt" +
                ";&lt;/div&gt; &lt;/a&gt; &lt;/td&gt; &lt;/tr&gt; &lt;/table&gt; &lt;/center&gt; &lt;/td&gt; &lt;/tr&gt; " +
                "&lt;/tbody&gt; &lt;/table&gt; ";


            return new MvcHtmlString(HttpUtility.HtmlDecode(result));

        }
        /// <summary>
        /// codificación de la sección de criterios de búsqueda, la codificación se realiza bajo el esquema de Base64
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString seccionBusqueda(string tabla)
        {
            //string result = "PHRhYmxlIGNsYXNzPSd0YWJsYUNyaXRlcmlvcyc+CiAgICA8dGhlYWQ+CiAgICAgICAgPHRyPgogICAgICAgICAgICA8dGQgY29sc3Bhbj0nMic+CiAgICAgICAgICAgICAgICA8dGFibGUgc3R5bGU9J3dpZHRoOjEwMCU7IHRleHQtYWxpZ246cmlnaHQ7Jz4KICAgICAgICAgICAgICAgICAgICA8dGJvZHk+CiAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCBjbGFzcz0nY3JpdGVyaW9zX21vc3RyYXInIHN0eWxlPSd0ZXh0LWFsaWduOmxlZnQ7Jz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgY2xhc3M9J2NyaXRlcmlvc190aXR1bG8nIHN0eWxlPSd0ZXh0LWFsaWduOmNlbnRlcjsnPgo8ZGl2Pgo8aW1nIHNyYz0nL0ltYWdlcy9sYXlvdXQvaW1hZ2VuZXNfZXN0aWxvcy9kb3duLnBuZycgaWQ9J2ljb19tb3N0cmFyXzEnIHdpZHRoPScyNHB4JyBoZWlnaHQ9JzI0cHgnIGJvcmRlcj0nMCcgYWx0PSdNb3N0cmFyIGNyaXRlcmlvcycKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHN0eWxlPSdkaXNwbGF5OiBub25lOycgLz4gPGltZyBzcmM9Jy9JbWFnZXMvbGF5b3V0L2ltYWdlbmVzX2VzdGlsb3MvdXAucG5nJwogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlkPSdpY29fb2N1bHRhcl8xJyB3aWR0aD0nMjRweCcgaGVpZ2h0PScyNHB4JyBib3JkZXI9JzAnIGFsdD0nT2N1bHRhciBjcml0ZXJpb3MnIC8+CkNyaXRlcmlvcyBkZSBiw7pzcXVlZGEKPC9kaXY+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGNsYXNzPSdjcml0ZXJpb3NfYXBsaWNhZG9zJz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGlkPSd0YmxDcml0ZXJpb3MnPjwvZGl2PgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgPC90Ym9keT4KICAgICAgICAgICAgICAgIDwvdGFibGU+CiAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgPC90cj4KICAgIDwvdGhlYWQ+CiAgICA8dGJvZHkgaWQ9J2NyaXRlcmlvc19mb3JtJyBzdHlsZT0nZGlzcGxheTo7Jz4KICAgICAgICA8dHI+CiAgICAgICAgICAgIDx0ZCBjb2xzcGFuPScyJz4KICAgICAgICAgICAgICAgIDxkaXYgaWQ9J3pvbmFDcml0ZXJpb3MnPjwvZGl2PgogICAgICAgICAgICA8L3RkPgogICAgICAgIDwvdHI+CiAgICAgICAgPHRyIGNsYXNzPSIiPgogICAgICAgICAgICA8dGQgY29sc3Bhbj0nMicgYWxpZ249J3JpZ2h0JyA7Pgo8ZGl2IGNsYXNzPSdiYXJyYUFjY2lvbmVzJz4KICAgICAgICAgICAgICAgIDxhIGhyZWY9JyMnIGlkPSdidG5MaW1waWFyJyBjbGFzcz0nbGlua0xpbXBpYXInPkxpbXBpYXI8L2E+Jm5ic3A7Jm5ic3A7PGltZyBzcmM9Ii4uL0ltYWdlcy9Db21wYXJ0aWRhcy9JY29ub3MvaW1nX2JhcnJhX2dyaXMucG5nIiBzdHlsZT0iYm9yZGVyOjBweDsiIC8+Jm5ic3A7Jm5ic3A7PGEgaHJlZj0nIycgdGl0bGU9JwogICAgICAgICAgICAgICAgQnVzY2FyJyBpZD0nYnRuQnVzY2FyJyBjbGFzcz0nbGlua0J1c2Nhcic+QnVzY2FyPC9hPgo8L2Rpdj4KICAgICAgICAgICAgPC90ZD4KICAgICAgICA8L3RyPgogICAgPC90Ym9keT4KPC90YWJsZT4KPGRpdiBpZD0nc2VwYXJhZG9yJz48L2Rpdj4=";
            //string result1 = DGSyTI_WEB.CodeBase64.decodeVariable(result);
            //return new MvcHtmlString(DGSyTI_WEB.CodeBase64.decodeVariable(result));
            string result = "&lt;table class=&quot;tablaCriterios&quot;&gt;&lt;thead&gt;&lt;tr&gt;&lt;td colspan=&quot;2&quot;&gt;" +
                "&lt;table style=&quot;width:100%; text-align:right;&quot;&gt;\n&lt;tbody&gt;&lt;tr&gt;&lt;td class=&quot;criterios_mostrar&quot;" +
                " style=&quot;text-align:left;&quot;&gt;&lt;/td&gt;\n&lt;td class=&quot;criterios_titulo&quot; style=&quot;text-align:center;" +
                "&quot;&gt;&lt;div&gt;\n&lt;img src=&quot;/Images/layout/imagenes_estilos/down.png&quot; id=&quot;"+tabla+"_ico_mostrar_1&quot; width=&quot;24px&quot;" +
                " height=&quot;24px&quot; border=&quot;0&quot; alt=&quot;Mostrar criterios&quot;\nstyle=&quot;display: none;&quot; /&gt; &lt;img " +
                "src=&quot;/Images/layout/imagenes_estilos/up.png&quot;\nid=&quot;"+tabla+"_ico_ocultar_1&quot; width=&quot;24px&quot; height=&quot;24px&quot; " +
                "border=&quot;0&quot; alt=&quot;Ocultar criterios&quot; /&gt;Criterios de búsqueda&lt;/div&gt;&lt;/td&gt;\n&lt;" +
                "td class=&quot;criterios_aplicados&quot;&gt;&lt;div id=&quot;"+tabla+"_tblCriterios&quot;&gt;&lt;/div&gt;&lt;/td&gt;&lt;/tr&gt;" +
                "&lt;/tbody&gt;&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/thead&gt;&lt;tbody id=&quot;"+tabla+"_criterios_form&quot; style=&quot;display:;" +
                "&quot;&gt;&lt;tr&gt;&lt;td colspan=&quot;2&quot;&gt;&lt;div id=&quot;"+tabla+"_zonaCriterios&quot;&gt;&lt;/div&gt;&lt;/td&gt;&lt;/tr&gt;\n&lt;" +
                "tr class=&quot;&quot;&gt;&lt;td colspan=&quot;2&quot; align=&quot;right&quot; ;&gt;&lt;div class=&quot;barraAcciones&quot;&gt;&lt;" +
                "a href=&quot;#&quot; id=&quot;"+tabla+"_btnLimpiar&quot; class=&quot;linkLimpiar&quot;&gt;Limpiar&lt;/a&gt;&nbsp;&nbsp;&lt;" +
                "img src=&quot;../Images/Compartidas/Iconos/img_barra_gris.png&quot; style=&quot;border:0px;&quot; /&gt;&nbsp;&nbsp;&lt;" +
                "a href=&quot;#&quot; title=&quot;\nBuscar&quot; id=&quot;"+tabla+"_btnBuscar&quot; class=&quot;linkBuscar&quot;&gt;Buscar&lt;/a&gt;&lt;" +
                "/div&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/tbody&gt;&lt;/table&gt;&lt;div id=&quot;separador&quot;&gt;&lt;/div&gt;";
            return new MvcHtmlString(HttpUtility.HtmlDecode(result));
        }


        public static MvcHtmlString seccionAcciones()
        {
            string result = "&lt;table class=&quot;" +
                "TablaAcciones&quot;&gt; &lt;tr&gt; &lt;td colspan=&quot;4&quot;&gt; &lt;" +
                "div id=&quot;barraAcciones&quot;&gt;&lt;/div&gt; &lt;/td&gt;&lt;td colspan=&quot;1&quot;" +
                " id=&quot;totalR&quot; align=&quot;right&quot;&gt;&lt;td&gt; &lt;td colspan=&quot;1&quot; " +
                "align=&quot;right&quot;&gt; Paginar cada: &lt;select name=&quot;page_size&quot; id=&quot;page_size&quot;" +
                "&gt; &lt;option&gt;&lt;/option&gt; &lt;/select&gt; &lt;/td&gt; &lt;/tr&gt; &lt;/table&gt;";
           
            return new MvcHtmlString(HttpUtility.HtmlDecode(result));
        }

        public static MvcHtmlString seccionPaginacion()
        {
            string result = "PHRhYmxlIGNsYXNzPSdiYXJyYVBhZ2luYWNpb24nIHN0eWxlPSJ0ZXh0LWFsaWduOiBjZW50ZXIiPgogICAgPHRib2R5PgogICAgICAgIDx0cj4KICAgICAgICAgICAgPHRkIHN0eWxlPSJ0ZXh0LWFsaWduOiBjZW50ZXIiIGFsaW5nPSJjZW50ZXIiPgogICAgICAgICAgICAgICAgPGNlbnRlcj4KICAgICAgICAgICAgICAgICAgICA8dGFibGU+CiAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCBzdHlsZT0iZm9udC1zaXplOiAxMnB4OyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGEgaHJlZj0nIycgaWQ9ImJ0bkluaWNpbyIgb25jbGljaz0icGFnaW5hcignSU5JQ0lPJyk7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGltZyBzcmM9In4vSW1hZ2VzL0NvbXBhcnRpZGFzL0ljb25vcy9pY29faW5pY2lvLnBuZyIgd2lkdGg9IjI1IiBoZWlnaHQ9IjI1IiBib3JkZXI9JzAnIGFsdD0nSW5pY2lvJyAvPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBJbmljaW8KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2E+Jm5ic3A7Jm5ic3A7Jm5ic3A7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIHN0eWxlPSJmb250LXNpemU6IDEycHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YSBocmVmPScjJyBpZD0iYnRuQW50ZXJpb3IiIG9uY2xpY2s9InBhZ2luYXIoJ0FOVEVSSU9SJyk7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGltZyBzcmM9In4vSW1hZ2VzL0NvbXBhcnRpZGFzL0ljb25vcy9pY29fYW50ZXJpb3IucG5nIiB3aWR0aD0iMjUiIGhlaWdodD0iMjUiIGJvcmRlcj0nMCcgYWx0PSdBbnRlcmlvcicgLz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgQW50ZXJpb3IKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2E+Jm5ic3A7Jm5ic3A7Jm5ic3A7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIHN0eWxlPSJmb250LXNpemU6IDEycHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGlkPSJodG1sVG90YWxSZWdpc3RybyI+CgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCBzdHlsZT0iZm9udC1zaXplOiAxMnB4OyIgb25jbGljaz0icGFnaW5hcignU0lHVUlFTlRFJyk7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAmbmJzcDsmbmJzcDsmbmJzcDs8YSBocmVmPScjJyBpZD0iYnRuU2lndWllbnRlIj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgU2lndWllbnRlCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbWcgc3JjPSJ+L0ltYWdlcy9Db21wYXJ0aWRhcy9JY29ub3MvaWNvX2FkZWxhbnRlLnBuZyIgd2lkdGg9IjI1IiBoZWlnaHQ9IjI1IiBib3JkZXI9JzAnIGFsdD0nU2lndWllbnRlJyAvPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvYT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgc3R5bGU9ImZvbnQtc2l6ZTogMTJweDsiPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICZuYnNwOyZuYnNwOyZuYnNwOzxhIGhyZWY9JyMnIGlkPSJidG5GaW4iIG9uY2xpY2s9InBhZ2luYXIoJ0ZJTicpOyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIEZpbmFsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbWcgc3JjPSJ+L0ltYWdlcy9Db21wYXJ0aWRhcy9JY29ub3MvaWNvX3VsdGltby5wbmciIHdpZHRoPSIyNSIgaGVpZ2h0PSIyNSIgYm9yZGVyPScwJyBhbHQ9J0ZpbmFsJyAvPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvYT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgPC90YWJsZT4KICAgICAgICAgICAgICAgIDwvY2VudGVyPgogICAgICAgICAgICA8L3RkPgogICAgICAgIDwvdHI+CiAgICA8L3Rib2R5Pgo8L3RhYmxlPg==";

            return new MvcHtmlString(DGSyTI_WEB.CodeBase64.decodeVariable(result));
        }
        
    }

}