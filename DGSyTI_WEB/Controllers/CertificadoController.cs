using AutoMapper;
using BusinessModel.Business;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using CertificadosEletronicosMS.Controllers;
using CertificadosEletronicosMS.Models;
using DGSyTI_WEB.Models;
using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DGSyTI_WEB.Controllers
{
    [ValidarSesion]
    public class CertificadoController : Controller
    {

        Usuario user = SSExtensionHelper.GetAppUser();
        PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
        // GET: Certificado
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IniciarModulo(string parametro = "")
        {
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();

             return PartialView("TabMonitoreoSEP", oMonitoreoSEPViewModel);
        }
        public PartialViewResult ListadoSolicitudes(int iPagina, int iBloque, string filtro = "")
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = js.Deserialize<CriteriosBusquedaMonitoreoViewModel>((filtro));
            CriteriosBusquedaMonitoreoModel oCriterios = new CriteriosBusquedaMonitoreoModel();
            Mapper.Map(oCriteriosBusquedaMonitoreoViewModel, oCriterios);
            oCriterios.sIdInstitucion = perfilActivo.insId;
            EnvioSEPBL oEnvioSEPBL = new EnvioSEPBL();
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();
            MonitoreoSEPML oMonitoreoSEPML = new EnvioSEPBL().GetListadoSolicitudes(oCriterios, iPagina, iBloque);
            oMonitoreoSEPViewModel.listCerSolicitud = oMonitoreoSEPML.listViewSolicitud;
            oMonitoreoSEPViewModel.totalRegistrosSolicitudes = oMonitoreoSEPML.iTotalRegistrosSolicitudes;
            return PartialView(oMonitoreoSEPViewModel);
        }

        public PartialViewResult ListadoCertificados(string filtro = "", int pagina = 1, int bloque = 10)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = js.Deserialize<CriteriosBusquedaMonitoreoViewModel>((filtro));
            CriteriosBusquedaMonitoreoModel oCriterios = new CriteriosBusquedaMonitoreoModel();
            Mapper.Map(oCriteriosBusquedaMonitoreoViewModel, oCriterios);

            EnvioSEPBL oEnvioSEPBL = new EnvioSEPBL();
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();
            MonitoreoSEPML oMonitoreoSEPML = new EnvioSEPBL().GetListadoCertificados(oCriterios, pagina, bloque);
            oMonitoreoSEPViewModel.listCerDocumento = oMonitoreoSEPML.listCerDocumento;
            oMonitoreoSEPViewModel.totalRegistrosCertificados = oMonitoreoSEPML.iTotalRegistrosCertificados;
            return PartialView();
        }

        public PartialViewResult CriteriosBusquedaSolicitudes()
        {
            CriteriosBusquedaMonitoreoViewModel criteriosBusquedaMonitoreoViewModel = new CriteriosBusquedaMonitoreoViewModel();
            List<cerCatTipoDocumento> listTipoDocumento = new EnvioSEPBL().GetListCerCatTipoDocumentos();

            foreach (var tipodocumento in listTipoDocumento)
            {
                criteriosBusquedaMonitoreoViewModel.listSLTipoCertificado.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            List<cerCatEstatusSolicitud> listEstatusSolicitud = new EnvioSEPBL().GetListEstatusSolicitud();

            foreach (var estatussolicitud in listEstatusSolicitud)
            {
                criteriosBusquedaMonitoreoViewModel.listSLEstatus.Add(new SelectListItem() { Value = estatussolicitud.estSolicitudId.ToString(), Text = estatussolicitud.estSolicitudDescripcion });
            }

            List<cerCatPlan> listPlan = new EnvioSEPBL().GetListPlanByInstitucion(perfilActivo.insId);

            foreach (var plan in listPlan)
            {
                criteriosBusquedaMonitoreoViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan.ToString(), Text = plan.planDescripcion });
            }


            return PartialView();
        }

        public PartialViewResult CriteriosBusquedaCertificados()
        {
            CriteriosBusquedaMonitoreoViewModel criteriosBusquedaMonitoreoViewModel = new CriteriosBusquedaMonitoreoViewModel();

            List<cerCatTipoDocumento> listTipoDocumento = new EnvioSEPBL().GetListCerCatTipoDocumentos();

            foreach (var tipodocumento in listTipoDocumento)
            {
                criteriosBusquedaMonitoreoViewModel.listSLTipoCertificado.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            List<cerCatEstatusDocumento> listEstatusDocumento = new EnvioSEPBL().GetListEstatusDocumento();

            foreach (var estatusdocumento in listEstatusDocumento)
            {
                criteriosBusquedaMonitoreoViewModel.listSLEstatus.Add(new SelectListItem() { Value = estatusdocumento.estDocumentoId.ToString(), Text = estatusdocumento.estDocumentoDescripcion });
            }

            List<cerCatPlan> listPlan = new EnvioSEPBL().GetListPlanByInstitucion(perfilActivo.insId);

            foreach (var plan in listPlan)
            {
                criteriosBusquedaMonitoreoViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan.ToString(), Text = plan.planDescripcion });
            }

            return PartialView(criteriosBusquedaMonitoreoViewModel);
        }

        public PartialViewResult DetalleCertificado(string idDocumento)
        {
            DetalleCertificadoViewModel detalleCertificadoViewModel = new DetalleCertificadoViewModel();
            cerDocumento cerDocumento = new EnvioSEPBL().GetDocumentoById(idDocumento);
            Mapper.Map(cerDocumento, detalleCertificadoViewModel);
            detalleCertificadoViewModel.listUACDocumento = new CertificadosBL().GetListUACDocumentoByIdDocumento(idDocumento);
            cerCatTipoDocumento cerCatTipoDocumento = new EnvioSEPBL().GetTipoDocumentoById(cerDocumento.docTipoId);
            if (cerCatTipoDocumento != null)
            {
                detalleCertificadoViewModel.sDocTipoCertificado = cerCatTipoDocumento.docDescripcion;
            }
            return PartialView(detalleCertificadoViewModel);
        }

        public PartialViewResult TabMonitoreoSEP()
        {
            return PartialView();
        }

        public ActionResult Paginar(string filtro = "", int pagina = 1, int bloque = 10)
        {
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = js.Deserialize<CriteriosBusquedaMonitoreoViewModel>((filtro));
            CriteriosBusquedaMonitoreoModel oCriterios = new CriteriosBusquedaMonitoreoModel();



            Mapper.Map(oCriteriosBusquedaMonitoreoViewModel, oCriterios);

            EnvioSEPBL oEnvioSEPBL = new EnvioSEPBL();

            MonitoreoSEPML oMonitoreoSEPML = new EnvioSEPBL().GetListadoSolicitudes(oCriterios, pagina, bloque);
            oMonitoreoSEPViewModel.listCerSolicitud = oMonitoreoSEPML.listViewSolicitud;
            oMonitoreoSEPViewModel.totalRegistrosSolicitudes = oMonitoreoSEPML.iTotalRegistrosSolicitudes;

            return PartialView("ListadoSolicitudes", oMonitoreoSEPViewModel);
        }

        public ActionResult IniciarCriteriosConsulta(string parametro = "")
        {
            CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = new CriteriosBusquedaMonitoreoViewModel();
            CriteriosBusquedaViewModel criteriosBusquedaViewModel = new CriteriosBusquedaViewModel();

            List<cerCatTipoDocumento> listTipoDocumento = new EnvioSEPBL().GetListCerCatTipoDocumentos();

            foreach (var tipodocumento in listTipoDocumento)
            {
                oCriteriosBusquedaMonitoreoViewModel.listSLTipoCertificado.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            List<cerCatEstatusSolicitud> listEstatusSolicitud = new EnvioSEPBL().GetListEstatusSolicitud();

            foreach (var estatussolicitud in listEstatusSolicitud)
            {
                oCriteriosBusquedaMonitoreoViewModel.listSLEstatus.Add(new SelectListItem() { Value = estatussolicitud.estSolicitudId.ToString(), Text = estatussolicitud.estSolicitudDescripcion });
            }

            List<cerCatPlan> listPlan = new EnvioSEPBL().GetListPlanByInstitucion(perfilActivo.insId);


            foreach (var plan in listPlan)
            {
                oCriteriosBusquedaMonitoreoViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan.ToString(), Text = plan.planDescripcion });
            }


            return PartialView("CriteriosBusquedaSolicitudes", oCriteriosBusquedaMonitoreoViewModel);
        }

        public PartialViewResult gridCertificadosMonitoreo(string filtro = "", int pagina = 1, int bloque = 10, string sSolId = "")
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = js.Deserialize<CriteriosBusquedaMonitoreoViewModel>((filtro));
            CriteriosBusquedaMonitoreoModel oCriterios = new CriteriosBusquedaMonitoreoModel();
            Mapper.Map(oCriteriosBusquedaMonitoreoViewModel, oCriterios);
            oCriterios.sIdInstitucion = perfilActivo.insId;
            EnvioSEPBL oEnvioSEPBL = new EnvioSEPBL();
            if (String.IsNullOrEmpty(filtro)) { oCriterios.sSolId = sSolId; }
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();
            MonitoreoSEPML oMonitoreoSEPML = new EnvioSEPBL().GetListadoCertificados(oCriterios, pagina, bloque);
            oMonitoreoSEPViewModel.listCerDocumento = oMonitoreoSEPML.listCerDocumento;
            oMonitoreoSEPViewModel.totalRegistrosCertificados = oMonitoreoSEPML.iTotalRegistrosCertificados;


            return PartialView("ListadoCertificados", oMonitoreoSEPViewModel);
        }

        public PartialViewResult DatosSolicitud(string solId)
        {
            DatosSolicitudViewModel datosSolicitudViewModel = new DatosSolicitudViewModel();
            ViewSolicitud oViewSolicitud = new EnvioSEPBL().GetSolicitudById(solId);

            datosSolicitudViewModel.FechaEnvioSEP = oViewSolicitud.solFechaEnvio.ToString();
            datosSolicitudViewModel.FechaRespuestaSEP = oViewSolicitud.solFechaResultado.ToString();
            datosSolicitudViewModel.FechaSellado = oViewSolicitud.solFechaSellado.ToString();
            datosSolicitudViewModel.sEstatusSolicitud = oViewSolicitud.EstatusDescripcion;
            datosSolicitudViewModel.sLoteSEP = oViewSolicitud.solFolioLoteSEP;
            datosSolicitudViewModel.sFolio = oViewSolicitud.solId;
            datosSolicitudViewModel.sTotalDocumentos = oViewSolicitud.numerocertificados.ToString();

            return PartialView(datosSolicitudViewModel);
        }

        #region Monitorear solicitudes en proceso SEP
        public ActionResult ProcesarSolicitud(string solId)
        {
            var result = new EnvioSEPBL().ProcesarSolicitud(solId, user.Login);

            return Json(result);
        }

        public FileResult DescargarEnvioSEP(string sSolId)
        {
            byte[] bArchivo = new EnvioSEPBL().GetFileEnvioSEP(sSolId);
            return File(bArchivo, "application/zip", sSolId + "_envio.zip");
        }

        public FileResult DescargarResultadoSEP(string sSolId)
        {
            byte[] bArchivo = new EnvioSEPBL().GetFileResultadoSEP(sSolId);
            return File(bArchivo, "application/zip", sSolId + "_resultado.zip");
        }

        public FileResult DescargarRetornoSEP(string sSolId)
        {
            byte[] bArchivo = new EnvioSEPBL().GetFileRetornoSEP(sSolId);
            return File(bArchivo, "application/zip", sSolId + "_retorno.zip");
        }

        public FileResult DescargaXMLRetornoByIdDoc(string sDocId)
        {
            MemoryStream memoryStream = new EnvioSEPBL().GetXMLRetornoByDocId(sDocId);
            return File(memoryStream, "multipart/form-data", sDocId + "_retorno.xml");
        }

        public FileResult DescargaXMLEnvioByIdDoc(string sDocId)
        {
            MemoryStream memoryStream = new EnvioSEPBL().GetXMLEnvioByDocId(sDocId);
            return File(memoryStream, "multipart/form-data", sDocId + "_envio.xml");
        }


        #endregion

        public PartialViewResult ListadoPlantillas(string filtro, string sIdDocumento, string sIdPlan, string sIdTipoDocumento, int pagina = 1, int bloque = 10 )
        {

            if (!String.IsNullOrEmpty(filtro) && filtro != "\"\"")
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                CriteriosBusquedaMonitoreoViewModel oCriteriosBusquedaMonitoreoViewModel = js.Deserialize<CriteriosBusquedaMonitoreoViewModel>((filtro));
                sIdDocumento = oCriteriosBusquedaMonitoreoViewModel.sIdDocumento;
                sIdPlan = oCriteriosBusquedaMonitoreoViewModel.sIdPlan;
                sIdTipoDocumento = oCriteriosBusquedaMonitoreoViewModel.sIdTipoCertificado;
            }

            MonitoreoSEPML oMonitoreoSEPML = new EnvioSEPBL().GetPlantillasByInstitucion(perfilActivo.insId, sIdPlan, sIdTipoDocumento, pagina, bloque);
            MonitoreoSEPViewModel monitoreoSEPViewModel = new MonitoreoSEPViewModel();
            monitoreoSEPViewModel.listCerCatPlantillas = oMonitoreoSEPML.listCerCatPlantillas;
            monitoreoSEPViewModel.iTotalRegistrosPlantillas = oMonitoreoSEPML.iTotalRegistrosPlantillas;
            monitoreoSEPViewModel.sIdDocumento = sIdDocumento;
            monitoreoSEPViewModel.sIdPlantilla = new EnvioSEPBL().GetPlantillaByidDoc(sIdDocumento);
            return PartialView(monitoreoSEPViewModel);
        }

        public FileResult Previsualizar(string sDocId, string sIdPlantilla)
        {
            MemoryStream memoryStream = new EnvioSEPBL().GetXMLEnvioByDocId(sDocId);
            return File(memoryStream, "multipart/form-data", sDocId + ".xml");
        }

        public JsonResult AsignarPlantilla(string sDocId, string sIdPlantilla)
        {
            bool bResult = new EnvioSEPBL().AsignarPlantilla(sDocId, sIdPlantilla);

            return Json(bResult);
        }

        public PartialViewResult ModalEnviarCorreo(string sIdDocumento, string sCorreo)
        {
            MonitoreoSEPViewModel monitoreoSEPViewModel = new MonitoreoSEPViewModel();
            monitoreoSEPViewModel.sDocCorreo = sCorreo;
            monitoreoSEPViewModel.sIdDocumento = sIdDocumento;
            return PartialView(monitoreoSEPViewModel);
        }

        public JsonResult EnviarCorreo(MonitoreoSEPViewModel monitoreoSEPViewModel)
        {

            CertificadoML.criteriosBusquedaCertificadosML criterios = new CertificadoML.criteriosBusquedaCertificadosML();
            criterios.listPlanAcceso = new List<string>();
            string[] result = new string[2];
            bool bBandera = false;
            if (new EnvioSEPBL().ActualizarCorreoByIdDocumento(monitoreoSEPViewModel.sIdDocumento, monitoreoSEPViewModel.sDocCorreo))
            {
                CertificadoML documentoML = new EnvioSEPBL().getDatosDocumentoPortalDescarga(monitoreoSEPViewModel.sIdDocumento);
                result = new CertificadosBL().enviaCorreoProfesionista(documentoML, criterios, documentoML.docAlumnoCurp);
                if (result[0] == "True")
                {
                    bBandera = true;
                }
            }
            return Json(bBandera);


        }

        public PartialViewResult ModalCancelarCertificado(string idDocumento)
        {
            MonitoreoSEPViewModel oMonitoreoSEPViewModel = new MonitoreoSEPViewModel();
            oMonitoreoSEPViewModel.sIdDocumento = idDocumento;
            return PartialView(oMonitoreoSEPViewModel);
        }

        public ActionResult CancelarCertificadoSEP(MonitoreoSEPViewModel oMonitoreoSEPViewModel)
        {
            var user = ((Usuario)Session["appUser"]).Login;
            String[] result = new EnvioSEPBL().CancelarCertificadoSEP(oMonitoreoSEPViewModel.sIdDocumento, user, oMonitoreoSEPViewModel.sObservaciones); ;
            return Json(result);
        }

        public FileResult DescargaPDFCertificado(string sIdDocumento, string sIdPlantilla = "")
        {
            return new PlantillasCertificadosController().DescargarCertificadoWithSession(sIdDocumento, sIdPlantilla);
        }
    }
}