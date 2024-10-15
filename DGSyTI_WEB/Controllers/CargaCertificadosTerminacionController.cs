using BusinessModel.Business;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using ClosedXML.Excel;
using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGSyTI_WEB.Controllers
{
    [ValidarSesion]
    [ValidaAcceso(accion = "CargaInformacion")]
    public class CargaCertificadosTerminacionController : Controller
    {
        // GET: CargaCertificadosTerminacion

        public ActionResult Index(string parametro = "")
        {
            PerfilML perfilActivo = ObtenerPerfilActivo();
            //List<string> inst = perfilActivo.accesos.Select(t => t.institucion).Distinct().Where(x => x != "").ToList();
            var user = ((Usuario)Session["appUser"]);
            var criterios = new CargaCertificadosBL().criteriosBusqueda();
            return View(criterios);
        }

        public FileResult ExportLayout(string tipoDocumento = "", string docDescripcion = "")
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            

                return File(new CargaCertificadosBL().GenerarPlantillaCarga(tipoDocumento, perfilActivo.insId), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "cargaCertificados" + docDescripcion + ".xlsx");

            
        }

        public ActionResult ValidarLayout(CargaCertificadosML model)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();

            var user = ((Usuario)Session["appUser"]).Login;
            var result = new CargaCertificadosBL().validarArchivo(model, perfilActivo.insId, user);
            return Json(result);

        }

        public ActionResult criterios()
        {
            FiltrosConsultaCarga cristerios = new FiltrosConsultaCarga();

            return PartialView("CriteriosValidacion", cristerios);
        }

        public ActionResult ListadoResultadosValidacionCarga(string filtro = "", int pagina = 1, int bloque = 10, string carId = "")
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();

            FiltrosConsultaCarga filtroBusqueda = filtrosValidacionArchivo(filtro);
            int totalR = 0;
            int totalRconErrores = 0, totalRconObservaciones = 0, RegistrosTotales = 0, RegistrosSinErrores = 0;
            ObjetosCargaCertificadosML ListDocumentos = new ObjetosCargaCertificadosML();
            ListDocumentos.listadoDocumentosAcargar = new CargaCertificadosBL().GetLstDocumentosValidacion(filtroBusqueda, out totalR, out totalRconErrores, out totalRconObservaciones, out RegistrosTotales, out RegistrosSinErrores, pagina, bloque);
            ListDocumentos.listadoMateriasCargar = new CargaCertificadosBL().LstMateriasCarga(perfilActivo.insId, filtroBusqueda.tipoDocumento);
            ViewBag.totalRegistros = totalR;
            ViewBag.totalRconErrores = totalRconErrores;
            ViewBag.totalRconObservaciones = totalRconObservaciones;
            ViewBag.RegistrosTotales = RegistrosTotales;
            ViewBag.RegistrosSinErrores = RegistrosSinErrores;
            ViewBag.tipoDocumento = filtroBusqueda.tipoDocumento;
            return PartialView("SeccionListado", ListDocumentos);
        }


        private FiltrosConsultaCarga filtrosValidacionArchivo(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            FiltrosConsultaCarga result = js.Deserialize<FiltrosConsultaCarga>((json));
            if (result == null)
            {
                result = new FiltrosConsultaCarga();
            }

            return result;
        }



        public ActionResult cargarArchivo(string idcarga, bool cargaConObservaciones)
        {
            var user = ((Usuario)Session["appUser"]).Login;
            var result = new CargaCertificadosBL().CargarArchivoBL(idcarga, cargaConObservaciones, user);
            return Json(result);
        }

        public PerfilML ObtenerPerfilActivo()
        {
            List<PerfilML> listPerfiles = ((Usuario)Session["appUser"]).Perfiles;
            PerfilML perfilActivo = (from perfil in listPerfiles where perfil.seleccionado == true select perfil).FirstOrDefault();

            return perfilActivo;
        }


    }
}