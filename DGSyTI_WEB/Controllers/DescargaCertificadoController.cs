using BusinessModel.Business;
using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGSyTI_WEB.Models;
using System.Globalization;
using CertificadosEletronicosMS.Controllers;

namespace DGSyTI_WEB.Controllers
{
    public class DescargaCertificadoController : Controller
    {
        // GET: DescargaCertificado
        public ActionResult Index(string id = "")
        {
            HttpResponse.RemoveOutputCacheItem("/caching/CacheForever.aspx");
            CertificadoViewModel documento = new CertificadoViewModel();
            //titulo.observaciones = "Ocurrió algo inesperado al momento de consultar sus documentos, por favor intente más tarde.";
            ViewBag.urlRedirect = "";
            ViewBag.error = false;
            ViewBag.autenticado = false;
            try
            {
                string[] datos = getDatos(id);
                cerConfiguracionInstitucion oConfigInstitucion = new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(datos[1]);
                ViewBag.urlRedirect = oConfigInstitucion.insBotonSalir;


                CultureInfo provider = new CultureInfo("en-US");
                DateTime dateTime = DateTime.ParseExact(datos[2], "yyyy-MM-dd HH:mm:ss", provider);

                if (dateTime.AddHours(24) > DateTime.Now) //El último acceso no ha excedido las 24 horas.
                {
                    if (Request.Cookies.Count >= 1)
                    {
                        if (verificaCookie())

                        {
                            string[] datosCookie = getDatos(Request.Cookies["pId"].Value);


                            if (datos[0] == datosCookie[0])//Se verifica que la última cookie corresponde a la misma curp de lo contrario se envía para que se autentique nuevamente.
                            {

                                documento = obtenerListado(datos[0], datos[1]);
                                ViewBag.autenticado = true;
                                creaCookieId(id);

                            }
                            else
                            {
                                documento = obtenerListado(datos[0], datos[1]);
                                ViewBag.autenticado = false;
                                creaCookieNoAutenticada(id);
                            }
                        }
                        else
                        {

                            documento = obtenerListado(datos[0], datos[1]);
                            ViewBag.autenticado = false;
                            creaCookieNoAutenticada(id);

                        }
                    }
                    else  
                    {

                        documento = obtenerListado(datos[0], datos[1]);
                        ViewBag.autenticado = true;
                        creaCookieId(id);


                    }

                    documento.docAlumnoCurp = datos[0];

                }
                else

                {

                    documento.docAlumnoCurp = datos[0];

                }
            }
            catch (Exception ex)
            {
                ViewBag.error = true;
                Console.WriteLine(ex);
            }

            return View(documento);
        }
        public ActionResult mostrarListado()
        {
            cerDocumento documento = new cerDocumento();
            string[] datos = getDatos(Request.Cookies["pId"].Value);
            documento = obtenerListado(datos[0], datos[1]);

            return PartialView("ListadoDescarga", documento);
        }
        public ActionResult verificarToken(string idToken)
        {
            string[] datos = getDatos(Request.Cookies["pIdNoAutenticada"].Value);
            string[] result = new CertificadosBL().verificaTokenDescarga(datos[0], idToken);
            if (result[0] == "True")
            {
                new CerTokenDescargaDAL().inactivaTokens(datos[0], idToken);
                HttpCookie _oCookie2 = new HttpCookie("pToken", EncriptarAES.EncryptStringAES(idToken));
                _oCookie2.Expires = DateTime.Now.AddSeconds(60 * 60);

                creaCookieId(Request.Cookies["pIdNoAutenticada"].Value);
                borrarCookieIdNoAutenticada();

                Response.Cookies.Add(_oCookie2);
            }
            return Json(result);
        }
        public ActionResult descargarPDF(string idDocumento)
        {
            string[] datos = getDatos(Request.Cookies["pId"].Value);
            cerConfiguracionInstitucion oConfigConcentradora = new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(datos[1]);

            if (!verificaCookie())
            {
                return Redirect("/DescargaCertificado?id=" + Url.Encode(Request.Cookies["pId"].Value));
            }
            var archivo = new PlantillasCertificadosController().DescargarCertificado(idDocumento);
            //return null;
            //if (archivo == null)
            //{
            //    string rutaCertificadoNoDisponible = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaTituloNoDisponible"]);
            //    stream = new FileStream(rutaCertificadoNoDisponible, FileMode.Open);
            //}
            //new BitacoraBL().guardaBitacora(idDocumento, datos[0], "portalDescarga", true);

            return archivo;
        }

        public Boolean verificaCookie()
        {
            if (Request.Cookies.Count > 1)
            {
                if (Request.Cookies["pId"] != null && Request.Cookies["pToken"] != null)
                {
                    return true;
                }
            }
            return false;
        }
        public ActionResult formCorreo()
        {
            CertificadoViewModel certificado = new CertificadoViewModel();
            string[] datos = getDatos(Request.Cookies["pIdNoAutenticada"].Value);
            certificado = obtenerListado(datos[0], datos[1]);

            List<cerDocumento> auxListado = new List<cerDocumento>();
            foreach (var item in certificado.cerDocumentos)
            {
                if ((from c in auxListado where c.docCorreo == item.docCorreo select c).Any())
                    continue;
                string[] correo = item.docCorreo.Split('@');
                if (!String.IsNullOrWhiteSpace(correo[0]))
                {
                    var aux = correo[0].Length > 10 ? correo[0].Substring(0, 3) + "*****" + correo[0].Substring(correo[0].Length - 2) :
                        correo[0].Substring(0, 3) + "*******";
                    var aux2 = correo[1].Length > 10 ? "@" + correo[1].Substring(0, 3) + "*****" + correo[1].Substring(correo[1].Length - 2) :
                        '@' + correo[1].Substring(0, 3) + "*******";
                    item.docCorreo = aux + aux2;
                    if (!auxListado.Where(c => c.docCorreo == item.docCorreo).Any())
                    {
                        auxListado.Add(item);
                    }
                }
            }
            certificado.cerDocumentos = auxListado;

            return PartialView("modalEnvioCorreo", certificado);
        }
        public ActionResult enviarCorreo(string idCorreo)
        {
            String[] result = new String[2];
            if (idCorreo != null)
            {
                string[] datos = getDatos(Request.Cookies["pIdNoAutenticada"].Value);

                var idDocumento = idCorreo;
                CertificadoML documentoML = new CerDocumentoDAL().getDatosDocumentoPortalDescarga(idDocumento);
                CertificadoML.criteriosBusquedaCertificadosML criterios = new CertificadoML.criteriosBusquedaCertificadosML();
                criterios.listPlanAcceso = new List<string>();
                result = new CertificadosBL().enviaCorreoProfesionista(documentoML, criterios, datos[0]);
                if (result[0] == "True")
                {
                    borrarCookieId();
                        result[1] = "/DescargaCertificado?id=" + Url.Encode(Request.Cookies["pIdNoAutenticada"].Value);

                }
                
            }
            else
            {
                result[0] = "Error";
                result[1] = "Seleccione una cuenta de correo.";
            }
            return Json(result);
        }
        public String[] getDatos(string id)
        {


            return EncriptarAES.DecryptStringAES(id).Split('|');

        }
        public void creaCookieId(string id)
        {
            HttpCookie _oCookie = new HttpCookie("pId", id);
            Response.Cookies.Add(_oCookie);
        }

        public void creaCookieNoAutenticada(string id)
        {
            HttpCookie _oCookie = new HttpCookie("pIdNoAutenticada", id);
            Response.Cookies.Add(_oCookie);
        }

        public void borrarCookieIdNoAutenticada()
        {
            var c = new HttpCookie("pIdNoAutenticada");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);

        }
        public void borrarCookieId()
        {
            var c = new HttpCookie("pId");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);

        }

        public CertificadoViewModel obtenerListado(string curp, string conId)
        {
            CertificadoViewModel certificado = new CertificadoViewModel();
            var lista = new CertificadosBL().GetListadoCertificadosDescripcionesByCURP(curp, conId);
            certificado = AutoMapper.Mapper.Map(lista, certificado);

            return certificado;
        }
        public ActionResult cargaBotones()
        {
            var botones = "";
            string[] datos = getDatos(Request.Cookies["pId"].Value);
            cerConfiguracionInstitucion oConfigInstitucion = new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(datos[1]);
            if (!String.IsNullOrEmpty(oConfigInstitucion.insBotonMenu))
            {
                botones += " <a title='menuPrincipal' href='" + oConfigInstitucion.insBotonMenu + "' onclick='eliminaCookie()'>"
                    + "<img alt='menuPrincipal' src='/Images/Compartidas/Iconos/icono_menu.png' style='border:0px; vertical-align:text-bottom'/>Menú general&nbsp;&nbsp;"
                    + "</a><img src='/Images/Compartidas/Iconos/img_barra_gris.png' style='border:0px; vertical-align:text-bottom'/>&nbsp;&nbsp;";
            }
            if (!String.IsNullOrEmpty(oConfigInstitucion.insBotonSalir))
            {
                botones += "<a title='Salir' href='" + oConfigInstitucion.insBotonSalir + "' onclick='eliminaCookie()' >" +
                    "<img alt='Salir' src='/Images/Compartidas/Iconos/icono_salir.png' style='border:0px; vertical-align:text-bottom'/>Salir&nbsp;&nbsp;</a>";
            }

            return Json(botones);
        }
        public ActionResult modalAutenticacion()
        {
            return PartialView("modalDescargarCertificado");
        }

        public FileResult CertificadoPDF(string id)
        {
            try
            {
                string[] parametros = EncriptarAES.DecryptStringAES(id).Split('|');

                if (DateTime.Parse(parametros[1]).AddHours(1) > DateTime.Now)
                {


                    //  Stream stream = new BusinessModel.Business.CertificadosBL().GenerarPDF(parametros[0]);
                    //if (stream != null)
                    //  return File(stream, "application/pdf", "Certificado_electrónico.pdf");
                    //  }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public ActionResult PDF(string id)
        {
            FileResult archivo = null;
            try
            {
                archivo = new PlantillasCertificadosController().DescargarCertificado(id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
            return archivo;
        }

    }
}