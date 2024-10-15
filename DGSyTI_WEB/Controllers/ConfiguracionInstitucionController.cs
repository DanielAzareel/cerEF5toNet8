using BusinessModel.Business;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using DGSyTI_WEB.Controllers;
using DGSyTI_WEB.Models;
using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CertificadosEletronicosMS.Controllers
{
    [ValidarSesion]
    public class ConfiguracionInstitucionController : Controller
    {

        // GET: ConfiguracionInstitucion
        public ActionResult Index()
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            cerConfiguracionInstitucion oCerConfiguracionInstitucion = new ConfiguracionInstitucionBL().GetConfiguracionInstitucionByIdIns(perfilActivo.insId);
            ConfiguracionInstitucionViewModel oConfiguracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            cerCatFirmante oCerCatFirmante = new ConfiguracionInstitucionBL().GetFirmanteActivoSinArchivo(perfilActivo.insId);
            oConfiguracionInstitucionViewModel.oCatFirmante = new FirmanteCertificadoML();
            if (oCerCatFirmante != null)
            {
                oConfiguracionInstitucionViewModel.oCatFirmante.insId = oCerCatFirmante.insId;
                oConfiguracionInstitucionViewModel.oCatFirmante.firCurp = oCerCatFirmante.firCurp;
            }

            oConfiguracionInstitucionViewModel.insId = oCerConfiguracionInstitucion.insId;
            oConfiguracionInstitucionViewModel.InsNombre = oCerConfiguracionInstitucion.InsNombre;
            oConfiguracionInstitucionViewModel.insNotificacionProfesionista = oCerConfiguracionInstitucion.insNotificacionProfesionista;

            oConfiguracionInstitucionViewModel.insTokenSeguridadDescarga = oCerConfiguracionInstitucion.insTokenSeguridadDescarga;
            oConfiguracionInstitucionViewModel.insUsuarioWS = oCerConfiguracionInstitucion.insUsuarioWS;
            oConfiguracionInstitucionViewModel.insContrasenaWS = oCerConfiguracionInstitucion.insContrasenaWS;
            oConfiguracionInstitucionViewModel.insBotonMenu = oCerConfiguracionInstitucion.insBotonMenu;
            oConfiguracionInstitucionViewModel.insBotonSalir = oCerConfiguracionInstitucion.insBotonSalir;
            oConfiguracionInstitucionViewModel.insCertificadosPublicos = oCerConfiguracionInstitucion.insCertificadosPublicos;

            List<cerCatTipoDocumento> listTipoDocumento = new ConfiguracionInstitucionBL().GetListCerCatTipoDocumento();
            List<cerCatPlan> listPlan = new ConfiguracionInstitucionBL().GetListCerCatPlan(perfilActivo.insId);

            oConfiguracionInstitucionViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = "", Text = "Seleccionar" });

            foreach (var tipodocumento in listTipoDocumento)
            {
                oConfiguracionInstitucionViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            oConfiguracionInstitucionViewModel.listSLPlan.Add(new SelectListItem() { Value = "", Text = "Seleccionar" });

            foreach (var plan in listPlan)
            {
                oConfiguracionInstitucionViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan, Text = plan.planDescripcion });
            }

            return View(oConfiguracionInstitucionViewModel);
        }

        public PartialViewResult Formulario(ConfiguracionInstitucionViewModel oConfiguracionInstitucionViewModel)
        {
            return PartialView(oConfiguracionInstitucionViewModel);
        }

        // Guardar datos de configuración
        public ActionResult GuardarConfigInstitucion(string insId, string InsNombre, string insUsuarioWS, string insContrasenaWS,
            bool? insNotificacionProfesionista, string insTokenSeguridadDescarga, bool? insCertificadosPublicos,
            string insBotonMenu, string insBotonSalir)
        {

            cerConfiguracionInstitucion oCerConfiguracionInstitucion = new cerConfiguracionInstitucion();
            oCerConfiguracionInstitucion.insId = insId;
            oCerConfiguracionInstitucion.InsNombre = InsNombre;
            oCerConfiguracionInstitucion.insUsuarioWS = insUsuarioWS;
            oCerConfiguracionInstitucion.insContrasenaWS = insContrasenaWS;
            oCerConfiguracionInstitucion.insNotificacionProfesionista = Convert.ToBoolean(insNotificacionProfesionista);
            oCerConfiguracionInstitucion.insTokenSeguridadDescarga = insTokenSeguridadDescarga;
            oCerConfiguracionInstitucion.insCertificadosPublicos = Convert.ToBoolean(insCertificadosPublicos);
            oCerConfiguracionInstitucion.insBotonMenu = insBotonMenu;
            oCerConfiguracionInstitucion.insBotonSalir = insBotonSalir;
            bool bResult = new ConfiguracionInstitucionBL().AddOrUpdateInstitucion(oCerConfiguracionInstitucion);
            string[] arrResult = new string[2];
            if (bResult)
            {
                arrResult[0] = "True";
                arrResult[1] = "Se ha guardado la información correctamente.";
            }
            else
            {
                arrResult[0] = "False";
                arrResult[1] = "No se ha guardado la información correctamente.";
            }

            return Json(arrResult);
        }

        public PartialViewResult ListadoEtiquetas(string filtro = "", int pagina = 1, int bloque = 10)
        {
            ConfiguracionInstitucionViewModel oCriteriosBusqueda = new ConfiguracionInstitucionViewModel();
            if (!String.IsNullOrEmpty(filtro))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                oCriteriosBusqueda = js.Deserialize<ConfiguracionInstitucionViewModel>((filtro));
            }
            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            List<cerParametroValor> cerParametroValors = new ConfiguracionInstitucionBL().GetListCerParametroValor(oCriteriosBusqueda.sIdInstitucion, oCriteriosBusqueda.sIdTipoDocumento, pagina, bloque);
            configuracionInstitucionViewModel.iTotalRegistrosEtiquetas = new ConfiguracionInstitucionBL().GetCountCerParametroValor(oCriteriosBusqueda.sIdInstitucion, oCriteriosBusqueda.sIdTipoDocumento);
            configuracionInstitucionViewModel.insId = oCriteriosBusqueda.sIdInstitucion;
            configuracionInstitucionViewModel.ListCerParametroValor = cerParametroValors;


            return PartialView(configuracionInstitucionViewModel);
        }

        public PartialViewResult CriteriosBusquedaEtiquetas()
        {
            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            List<cerCatTipoDocumento> listTipoDocumento = new ConfiguracionInstitucionBL().GetListCerCatTipoDocumento();

            configuracionInstitucionViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = "", Text = "Seleccionar" });

            foreach (var tipodocumento in listTipoDocumento)
            {
                configuracionInstitucionViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            return PartialView(configuracionInstitucionViewModel);
        }

        public PartialViewResult ListadoPlantillas(string filtro = "", int pagina = 1, int bloque = 10)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            ConfiguracionInstitucionViewModel oCriteriosBusqueda = new ConfiguracionInstitucionViewModel();
            if (!String.IsNullOrEmpty(filtro))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                oCriteriosBusqueda = js.Deserialize<ConfiguracionInstitucionViewModel>((filtro));
            }
            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            oCriteriosBusqueda.sIdInstitucion = perfilActivo.insId;
            configuracionInstitucionViewModel.listPlantillas = new ConfiguracionInstitucionBL().GetPlantillas(oCriteriosBusqueda.sIdInstitucion, oCriteriosBusqueda.sIdTipoDocumento, oCriteriosBusqueda.sIdPlan, pagina, bloque);
            configuracionInstitucionViewModel.iTotalRegistrosPlantillas = new ConfiguracionInstitucionBL().GetCountPlantillas(oCriteriosBusqueda.sIdInstitucion, oCriteriosBusqueda.sIdTipoDocumento, oCriteriosBusqueda.sIdPlan);

            return PartialView(configuracionInstitucionViewModel);
        }

        public PartialViewResult CriteriosBusquedaPlantilla()
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            cerConfiguracionInstitucion oCerConfiguracionInstitucion = new ConfiguracionInstitucionBL().GetConfiguracionInstitucionByIdIns(perfilActivo.insId);

            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            List<cerCatTipoDocumento> listTipoDocumento = new ConfiguracionInstitucionBL().GetListCerCatTipoDocumento();
            List<cerCatPlan> listPlan = new ConfiguracionInstitucionBL().GetListCerCatPlan(perfilActivo.insId);


            foreach (var tipodocumento in listTipoDocumento)
            {
                configuracionInstitucionViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }


            foreach (var plan in listPlan)
            {
                configuracionInstitucionViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan, Text = plan.planDescripcion });
            }

            return PartialView(configuracionInstitucionViewModel);
        }

        public JsonResult EditarEtiqueta(string parId, string docTipoId, string parValor, string parValorNoRequerido, bool parValorRequerido)
        {
            if (parValorNoRequerido == "") { parValorNoRequerido = null; }
            string sValor = parValorRequerido ? parValor : parValorNoRequerido;

            bool bResultado = new ConfiguracionInstitucionBL().EditarEtiqueta(parId, docTipoId, sValor);
            string[] arrResult = new string[2];
            if (bResultado)
            {
                arrResult[0] = "True";
                arrResult[1] = "Se ha guardado la información correctamente.";
            }
            else
            {
                arrResult[0] = "False";
                arrResult[1] = "No se ha guardado la información correctamente.";
            }
            return Json(arrResult);
        }

        public ActionResult AgregarPlantilla(PlantillaCertificadoML oPlantilla)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            string[] arrResult = new string[2];
            oPlantilla.insId = perfilActivo.insId;

            if (oPlantilla.plaNumHojas > 1)
            {
                if (oPlantilla.archivoPlantilla == null)
                {
                    if (oPlantilla.archivoPlantillaEditar != null)
                    {
                        if (Path.GetExtension(oPlantilla.archivoPlantillaEditar.FileName) != ".zip")
                        {
                            arrResult[0] = "False";
                            arrResult[1] = "Cuando el número de archivos es mayor a 1 el archivo a cargar debe ser .ZIP.";
                            return Json(arrResult);
                        }
                    }
                    else
                    {
                        if (oPlantilla.sFileNombrePlantilla != ".zip")
                        {
                            arrResult[0] = "False";
                            arrResult[1] = "Cuando el número de archivos es mayor a 1 el archivo a cargar debe ser .ZIP.";
                            return Json(arrResult);
                        }
                    }
                }
                else
                {
                    if (Path.GetExtension(oPlantilla.archivoPlantilla.FileName) != ".zip")
                    {
                        arrResult[0] = "False";
                        arrResult[1] = "Cuando el número de archivos es mayor a 1 el archivo a cargar debe ser .ZIP.";
                        return Json(arrResult);
                    }
                }
            }
            else
            {
                if (oPlantilla.archivoPlantilla == null)
                {
                    if (oPlantilla.archivoPlantillaEditar != null)
                    {
                        if (Path.GetExtension(oPlantilla.archivoPlantillaEditar.FileName) != ".docx")
                        {
                            arrResult[0] = "False";
                            arrResult[1] = "Cuando el número de archivos es 1 el archivo a cargar debe ser .docx.";
                            return Json(arrResult);
                        }
                    }
                    else
                    {
                        if (oPlantilla.sFileNombrePlantilla != ".docx")
                        {
                            arrResult[0] = "False";
                            arrResult[1] = "Cuando el número de archivos es  1 el archivo a cargar debe ser .docx.";
                            return Json(arrResult);
                        }
                    }
                }
                else
                {
                    if (Path.GetExtension(oPlantilla.archivoPlantilla.FileName) != ".docx")
                    {
                        arrResult[0] = "False";
                        arrResult[1] = "Cuando el número de archivos es 1 el archivo a cargar debe ser .docx.";
                        return Json(arrResult);
                    }
                }
            }


            if (oPlantilla.archivoPlantillaEditar != null)
            {
                oPlantilla.archivoPlantilla = oPlantilla.archivoPlantillaEditar;
            }


            bool bResultado = new ConfiguracionInstitucionBL().AgregarPlantilla(oPlantilla);

            if (bResultado)
            {
                arrResult[0] = "True";
                arrResult[1] = "Se ha guardado la información correctamente.";
            }
            else
            {
                arrResult[0] = "False";
                arrResult[1] = "No se ha guardado la información correctamente.";
            }
            return Json(arrResult);
        }

        public PartialViewResult EditarPlantilla(string sIdPlantilla)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            ConfiguracionInstitucionViewModel oViewModel = new ConfiguracionInstitucionViewModel();
            cerCatPlantilla cerCatPlantilla = new cerCatPlantilla();
            if (!String.IsNullOrEmpty(sIdPlantilla))
            {
                cerCatPlantilla = new ConfiguracionInstitucionBL().GetPlantillaById(sIdPlantilla);
            }
            List<cerCatTipoDocumento> listTipoDocumento = new ConfiguracionInstitucionBL().GetListCerCatTipoDocumento();
            List<cerCatPlan> listPlan = new ConfiguracionInstitucionBL().GetListCerCatPlan(perfilActivo.insId);

            oViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = "", Text = "Seleccionar" });

            foreach (var tipodocumento in listTipoDocumento)
            {
                oViewModel.listSLTipoDocumento.Add(new SelectListItem() { Value = tipodocumento.docTipoId, Text = tipodocumento.docDescripcion });
            }

            oViewModel.listSLPlan.Add(new SelectListItem() { Value = "", Text = "Seleccionar" });

            foreach (var plan in listPlan)
            {
                oViewModel.listSLPlan.Add(new SelectListItem() { Value = plan.idPlan, Text = plan.planDescripcion });
            }

            oViewModel.oPlantilla.docTipoId = cerCatPlantilla.docTipoId;
            oViewModel.oPlantilla.insId = cerCatPlantilla.insId;
            oViewModel.oPlantilla.plaNumHojas = cerCatPlantilla.planNumHojas;
            oViewModel.oPlantilla.plaNombre = cerCatPlantilla.planNombre;
            oViewModel.oPlantilla.planId = cerCatPlantilla.planId;
            oViewModel.oPlantilla.idPlan = cerCatPlantilla.idPlan;

            oViewModel.oPlantilla.sFileNombrePlantilla = cerCatPlantilla.planNumHojas > 1 ? ".zip" : ".docx";
            return PartialView(oViewModel);
        }

        public JsonResult EliminarPlantilla(string sIdPlantilla)
        {
            string[] arrResult = new ConfiguracionInstitucionBL().EliminarPlantilla(sIdPlantilla);

            return Json(arrResult);
        }

        public JsonResult AsignarPlantilla(string sIdPlantilla)
        {

            bool bResultado = new ConfiguracionInstitucionBL().AsignarPlantilla(sIdPlantilla);
            string[] arrResult = new string[2];
            if (bResultado)
            {
                arrResult[0] = "True";
                arrResult[1] = "Se ha asignado la plantilla correctamente.";
            }
            else
            {
                arrResult[0] = "False";
                arrResult[1] = "No se ha asignado la plantilla correctamente.";
            }
            return Json(arrResult);
        }

        public string GenerarGUID()
        {
            return Guid.NewGuid().ToString();
        }

        public JsonResult VerficarNombrePlantilla(PlantillaCertificadoML oPlantilla)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            var result = new ConfiguracionInstitucionBL().ExisteNombrePlantilla(oPlantilla.planId, oPlantilla.plaNombre, perfilActivo.insId);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult ListadoFirmantes(string filtro = "", int pagina = 1, int bloque = 10)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            ConfiguracionInstitucionViewModel oCriteriosBusqueda = new ConfiguracionInstitucionViewModel();
            if (!String.IsNullOrEmpty(filtro))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                oCriteriosBusqueda = js.Deserialize<ConfiguracionInstitucionViewModel>((filtro));
            }
            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();
            oCriteriosBusqueda.sIdInstitucion = perfilActivo.insId;
            configuracionInstitucionViewModel.listCatFirmantes = new ConfiguracionInstitucionBL().GetListFirmantes(oCriteriosBusqueda.sIdInstitucion, pagina, bloque);
            configuracionInstitucionViewModel.iTotalRegistrosFirmantes = new ConfiguracionInstitucionBL().GetCountFirmantes(oCriteriosBusqueda.sIdInstitucion);

            return PartialView(configuracionInstitucionViewModel);
        }

        public PartialViewResult EditarFirmante(string firId, string insId)
        {
            ConfiguracionInstitucionViewModel configuracionInstitucionViewModel = new ConfiguracionInstitucionViewModel();

            cerCatFirmante cerCatFirmante = new cerCatFirmante();
            if (!String.IsNullOrEmpty(firId))
            {
                cerCatFirmante = new ConfiguracionInstitucionBL().getFirmanteById(firId, insId);
            }
            configuracionInstitucionViewModel.oCatFirmante.firId = cerCatFirmante.firId;
            configuracionInstitucionViewModel.oCatFirmante.insId = cerCatFirmante.insId;
            configuracionInstitucionViewModel.oCatFirmante.firNombre = cerCatFirmante.firNombre;
            configuracionInstitucionViewModel.oCatFirmante.firPrimerApellido = cerCatFirmante.firPrimerApellido;
            configuracionInstitucionViewModel.oCatFirmante.firSegundoApellido = cerCatFirmante.firSegundoApellido;
            configuracionInstitucionViewModel.oCatFirmante.firIdCargo = cerCatFirmante.firIdCargo;
            configuracionInstitucionViewModel.oCatFirmante.firCargo = cerCatFirmante.firCargo;
            configuracionInstitucionViewModel.oCatFirmante.firCorreo = cerCatFirmante.firCorreo;
            configuracionInstitucionViewModel.oCatFirmante.firPredeterminado = cerCatFirmante.firPredeterminado;
            configuracionInstitucionViewModel.oCatFirmante.firCurp = cerCatFirmante.firCurp;
            configuracionInstitucionViewModel.oCatFirmante.firVigenciaCertificado = Convert.ToDateTime(cerCatFirmante.firVigenciaCertificado);
            return PartialView(configuracionInstitucionViewModel);
        }

        public JsonResult AgregarEditarFirmante(FirmanteCertificadoML oCatFirmante)
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            oCatFirmante.insId = perfilActivo.insId;
            string[] arrResult = new ConfiguracionInstitucionBL().AgregarEditarFirmante(oCatFirmante);
            return Json(arrResult);
        }

        public JsonResult AsignarFirmante(string firId, string insId)
        {
            bool bResultado = new ConfiguracionInstitucionBL().AsignarFirmante(firId, insId);
            string[] arrResult = new string[2];
            if (bResultado)
            {
                arrResult[0] = "True";
                arrResult[1] = "Se ha guardado la información correctamente.";
            }
            else
            {
                arrResult[0] = "False";
                arrResult[1] = "No se ha guardado la información correctamente.";
            }
            return Json(arrResult);
        }

        public JsonResult EliminarFirmante(string firId, string insId)
        {
            string[] arrResult = new ConfiguracionInstitucionBL().EliminarFirmanteById(firId, insId);
            return Json(arrResult);
        }

        public FileResult DescargarPlantilla(string sIdPlantilla)
        {
            cerCatPlantilla cerCatPlantilla = new ConfiguracionInstitucionBL().GetPlantillaFileById(sIdPlantilla);
            byte[] bArchivo = cerCatPlantilla.planArchivo;
            if (bArchivo == null)
            {
                return null;
            }
            //Cuando es más de una hoja es un archivo zip que contiene varios documentos word.
            if (cerCatPlantilla.planNumHojas > 1)

            {
                return File(bArchivo, "application/zip", cerCatPlantilla.planNombre + ".zip");
            }
            else
            {
                return File(bArchivo, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", cerCatPlantilla.planNombre + ".docx");
            }
        }

        public FileResult DescargarCertificado(string firId, string insId)
        {
            cerCatFirmante oCerCatFirmante  = new ConfiguracionInstitucionBL().GetCertificadoFileById(firId, insId);

            byte[] bArchivo = new byte[0];
            string sArchivoBase64 = oCerCatFirmante.firCertificadoPublico;

            if (!String.IsNullOrEmpty(sArchivoBase64))
            {
                bArchivo = Convert.FromBase64String(sArchivoBase64);
            }
            else
            {
                bArchivo = null;
            }

            if (bArchivo != null)
            {
                return File(bArchivo, "multipart/form-data", oCerCatFirmante.firCurp + " - " + insId + ".cer");
            }
            else
            {
                return null;
            }
        }

        public JsonResult ObtenerFirmantePredeterminado()
        {
            PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
            var firmantePredeterminado = new ConfiguracionInstitucionBL().GetRFCFirmantePredeterminado(perfilActivo.insId);
            string[] arrResult = new string[] { firmantePredeterminado.firCurp, perfilActivo.insId, firmantePredeterminado.firId  };

            return Json(arrResult);
        }
    }
}