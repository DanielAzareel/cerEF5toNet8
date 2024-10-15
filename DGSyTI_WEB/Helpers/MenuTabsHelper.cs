using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessModel.Models.Personalizados;

namespace Helpers
{
    public static class MenuTabsHelper
    {
        public static MvcHtmlString crearTabsMenu(this HtmlHelper helper, List<TabML> tabs, int numeroNiveles = 1)
        {
            String tabsNivel1 = "<div id='contenedor' class='contenedorTabs'><div id='tabsNiv1' class='tabs'>";
            String tabsNivel2 = "";
            string divContentView = "";
            try
            {
                int idTab = 1;
                int numTabs = (tabs.Count() == 0 )? 1 : tabs.Count();
                int numTabsActivos = tabs.Where(c => c.activa == true).Count();
                decimal widthTab =/* 16;//*/Decimal.Divide(100, numTabs);

                foreach (TabML tab in tabs)
                {
                    int numSubTabs = tab.subTabs != null && tab.activa ? tab.subTabs.Count : 0;

                    TagBuilder inputTag = new TagBuilder("input");
                    inputTag.Attributes.Add("name", "tabsNiv1");
                    inputTag.Attributes.Add("type", "radio");
                    inputTag.ToString(TagRenderMode.SelfClosing);

                    TagBuilder labelTag = new TagBuilder("label");
                    String onClick = "";

                    if (tab.activa)
                    {
                        inputTag.Attributes.Add("id", "tab-" + idTab);

                        onClick = (numSubTabs > 0) || (numeroNiveles > 1 && numSubTabs > 0) ?
                        "clickTab(" + numSubTabs + "," + idTab + ")" :
                        "GuardarUltimoTabActivo(" + idTab + "); " +
                        "CrearFiltroDinamico('"+ tab.idTablaFiltrado +"');" +
                        "try{setTabCustomParams();}catch(err){} " +
                        "obtenerContentView('" + tab.controller + "', '" + tab.metodo +
                        "','" + tab.parametro + "','" + numTabsActivos + "','" + numSubTabs + "','" + idTab + "','0') ";
                        labelTag.Attributes.Add("onclick", onClick);

                        labelTag.Attributes.Add("for", "tab-" + idTab);
                        labelTag.Attributes.Add("id", "labelTab-" + idTab);

                    }
                    else
                    {

                        labelTag.Attributes.Add("class", "tabDisabled");
                        labelTag.Attributes.Add("Title", tab.title);
                    }
                    labelTag.Attributes.Add("style", "width: " + widthTab + "%;");
                    //labelTag.InnerHtml = "<img src='Images/Compartidas/Iconos/cancelado.png'/>" + " " + tab.label;
                    labelTag.SetInnerText(tab.label);


                    tabsNivel1 += inputTag.ToString() + labelTag.ToString();
                    if (numSubTabs > 0)
                    {
                        decimal widthSubTab = Decimal.Divide(100, numSubTabs);
                        int idSubTab = 1;
                        tabsNivel2 += "<div class='subTabs' id='subTab" + idTab + "' inactive>";

                        foreach (TabML subTab in tab.subTabs)
                        {
                            TagBuilder inputSubTag = new TagBuilder("input");
                            TagBuilder labelSubTag = new TagBuilder("label");

                            inputSubTag.Attributes.Add("id", "subTabsNiv" + idTab + "-" + idSubTab);
                            inputSubTag.Attributes.Add("name", "subTabsNiv" + idTab);
                            inputSubTag.Attributes.Add("type", "radio");
                            inputSubTag.ToString(TagRenderMode.SelfClosing);


                            onClick = "obtenerContentView('" + subTab.controller + "','" + subTab.metodo + "','" + subTab.parametro + "','" + numTabsActivos + "','" + numSubTabs + "','" + idTab + "','" + idSubTab + "')";
                            labelSubTag.Attributes.Add("id", "labelSubTabsNiv" + idTab + "-" + idSubTab);
                            labelSubTag.Attributes.Add("for", "subTabsNiv" + idTab + "-" + idSubTab);
                            labelSubTag.Attributes.Add("style", "width: " + widthSubTab + "%;");
                            labelSubTag.Attributes.Add("onclick", onClick);
                            labelSubTag.SetInnerText(subTab.label);

                            tabsNivel2 += inputSubTag.ToString() + labelSubTag.ToString();
                            idSubTab++;
                        }
                        tabsNivel2 += "</div>";


                    }

                    if (tab.activa) { idTab++; }
                }

                tabsNivel1 += "</div>";

                divContentView = "<div id = 'contentView' class='contentView'> " +
                "<div id='contentViewResult'></div><div id='divLodingTabs' style='display:none;' class='sk-fading-circle'>" +
                "<div class='sk-circle1 sk-circle'></div> " +
                "<div class='sk-circle2 sk-circle'></div> " +
                "<div class='sk-circle3 sk-circle'></div> " +
                "<div class='sk-circle4 sk-circle'></div> " +
                "<div class='sk-circle5 sk-circle'></div> " +
                "<div class='sk-circle6 sk-circle'></div> " +
                "<div class='sk-circle7 sk-circle'></div> " +
                "<div class='sk-circle8 sk-circle'></div> " +
                "<div class='sk-circle9 sk-circle'></div> " +
                "<div class='sk-circle10 sk-circle'></div>" +
                "<div class='sk-circle11 sk-circle'></div>" +
                "<div class='sk-circle12 sk-circle'></div></div></div>";
            }
            catch (Exception e)
            {
                Console.Write("Error en helper tabs:" + e);
            }
            string result = tabsNivel1 + tabsNivel2 + divContentView + "</div>";
            return new MvcHtmlString(result);

        }

        public static MvcHtmlString crearTabsMenu2(this HtmlHelper helper, List<TabML> tabs, int numeroNiveles = 1)
        {
            String tabsNivel1 = "<div id='contenedor2' class='contenedorTabs'><div id='tabs2Niv1' class='tabs' height='40px'>";
            String tabsNivel2 = "";
            string divContentView = "";
            try
            {
                int idTab = 1;
                int numTabs = tabs.Count();
                int numTabsActivos = tabs.Where(c => c.activa == true).Count();
                decimal widthTab = 20;//Decimal.Divide(100, numTabs);

                foreach (TabML tab in tabs)
                {
                    int numSubTabs = tab.subTabs != null && tab.activa ? tab.subTabs.Count : 0;
                    TagBuilder inputTag = new TagBuilder("input");
                    TagBuilder labelTag = new TagBuilder("label");
                    String onClick = "";

                    inputTag.Attributes.Add("name", "tabs2Niv1");
                    inputTag.Attributes.Add("type", "radio");
                    inputTag.ToString(TagRenderMode.SelfClosing);

                    if (tab.activa)
                    {
                        inputTag.Attributes.Add("id", "tab2-" + idTab);

                        onClick = (numSubTabs > 0) || (numeroNiveles > 1 && numSubTabs > 0) ?
                        "clickTab(" + numSubTabs + "," + idTab + ")" :
                        "try{setTabCustomParams2();}catch(err){} " +
                        "obtenerContentView2('" + tab.controller + "', '" + tab.metodo +
                        "','" + tab.parametro + "','" + numTabsActivos + "','" + numSubTabs + "','" + idTab + "','0') ";
                        labelTag.Attributes.Add("onclick", onClick);

                        labelTag.Attributes.Add("for", "tab2-" + idTab);
                        labelTag.Attributes.Add("id", "labelTab2-" + idTab);

                    }
                    else
                    {

                        labelTag.Attributes.Add("class", "tabDisabled");
                        labelTag.Attributes.Add("Title", tab.title);
                    }
                    labelTag.Attributes.Add("style", "width: " + widthTab + "%;");
                    labelTag.SetInnerText(tab.label);


                    tabsNivel1 += inputTag.ToString() + labelTag.ToString();
                    if (numSubTabs > 0)
                    {
                        decimal widthSubTab = Decimal.Divide(100, numSubTabs);
                        int idSubTab = 1;
                        tabsNivel2 += "<div class='subTabs' id='subTab2" + idTab + "' inactive>";

                        foreach (TabML subTab in tab.subTabs)
                        {
                            TagBuilder inputSubTag = new TagBuilder("input");
                            TagBuilder labelSubTag = new TagBuilder("label");

                            inputSubTag.Attributes.Add("id", "subTabs2Niv" + idTab + "-" + idSubTab);
                            inputSubTag.Attributes.Add("name", "subTabs2Niv" + idTab);
                            inputSubTag.Attributes.Add("type", "radio");
                            inputSubTag.ToString(TagRenderMode.SelfClosing);


                            onClick = "obtenerContentView2('" + subTab.controller + "','" + subTab.metodo + "','" + subTab.parametro + "','" + numTabsActivos + "','" + numSubTabs + "','" + idTab + "','" + idSubTab + "')";
                            labelSubTag.Attributes.Add("id", "labelSubTabs2Niv" + idTab + "-" + idSubTab);
                            labelSubTag.Attributes.Add("for", "subTabs2Niv" + idTab + "-" + idSubTab);
                            labelSubTag.Attributes.Add("style", "width: " + widthSubTab + "%;");
                            labelSubTag.Attributes.Add("onclick", onClick);
                            labelSubTag.SetInnerText(subTab.label);

                            tabsNivel2 += inputSubTag.ToString() + labelSubTag.ToString();
                            idSubTab++;
                        }
                        tabsNivel2 += "</div>";


                    }

                    if (tab.activa) { idTab++; }
                }

                tabsNivel1 += "</div>";

                divContentView = "<div id='contentView2' class='contentView'> " +
                "<div id='contentViewResult2'></div><div id='divLodingTabs2' style='display:none;' class='sk-fading-circle'>" +
                "<div class='sk-circle1 sk-circle'></div> " +
                "<div class='sk-circle2 sk-circle'></div> " +
                "<div class='sk-circle3 sk-circle'></div> " +
                "<div class='sk-circle4 sk-circle'></div> " +
                "<div class='sk-circle5 sk-circle'></div> " +
                "<div class='sk-circle6 sk-circle'></div> " +
                "<div class='sk-circle7 sk-circle'></div> " +
                "<div class='sk-circle8 sk-circle'></div> " +
                "<div class='sk-circle9 sk-circle'></div> " +
                "<div class='sk-circle10 sk-circle'></div>" +
                "<div class='sk-circle11 sk-circle'></div>" +
                "<div class='sk-circle12 sk-circle'></div></div></div>";
            }
            catch (Exception e)
            {
                Console.Write("Error en helper tabs:" + e);
            }
            string result = tabsNivel1 + tabsNivel2 + divContentView + "</div>";
            return new MvcHtmlString(result);

        }
    }
}