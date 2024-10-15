using BusinessModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Models;
using System.Web.Mvc;
using DGSyTI_WEB.Models;
using CertificadosEletronicosMS.Models;
using Helpers;

namespace DGSyTI_WEB.Controllers
{
    [ValidarSesion]
    [ValidaAcceso(accion ="Sellado")]
    public class SelladoController : Controller
    {
        Usuario user = SSExtensionHelper.GetAppUser();
        PerfilML perfilActivo = SSExtensionHelper.ObtenerPerfilActivo();
        // GET: Sellado
        public ActionResult Index()
        {
            SelladoViewModel selladoViewModel = new SelladoViewModel();

            SelladoML selladoML = new SelladoML("");
            selladoViewModel.tabs = selladoML.tabs;



            return View(selladoViewModel);
        }
        public ActionResult Sellar(string nameFiltros, HttpPostedFileBase archivoKey, String nameContrasenia)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            CriteriosBusquedaML criteriosBusquedaM = js.Deserialize<CriteriosBusquedaML>((nameFiltros));

            FirmanteML firmanteML = new FirmanteML();
            firmanteML.archivoKey = archivoKey;
            firmanteML.contrasenia = nameContrasenia;
            firmanteML.insId = perfilActivo.insId;

            var resultado = new SelladoBL().SellarRegistros(criteriosBusquedaM, firmanteML, perfilActivo,user.Login);

            return Json(resultado);
        }
        public ActionResult IniciarModulo(string parametro = "")
        {
            SelladoViewModel selladoViewModel = new SelladoViewModel();
            //new EnvioSEPBL().ProcesarSolicitud("113C78D5-962C-49E8-9D61-F104F6AE0E3D", "Memo");
            return PartialView("TabSellado");
        }
        public ActionResult Paginar(string filtro = "", int pagina = 1, int bloque = 10)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            CriteriosBusquedaML criteriosBusquedaM = js.Deserialize<CriteriosBusquedaML>((filtro));
            SelladoViewModel selladoViewModel = new SelladoViewModel();

            if (criteriosBusquedaM == null)
            {
                criteriosBusquedaM = new CriteriosBusquedaML();
            }

            criteriosBusquedaM.estatus = "1";
            SelladoML sellado = new SelladoBL().GetVWGridSelladosPagina(criteriosBusquedaM, perfilActivo, pagina, bloque);
            AutoMapper.Mapper.Map(sellado, selladoViewModel);

            return PartialView("ListadoDocumentos", selladoViewModel);
        }
        public ActionResult IniciarCriteriosConsulta(string parametro = "")
        {

            CriteriosBusquedaViewModel criteriosBusquedaViewModel = new CriteriosBusquedaViewModel();

            var criterios = new SelladoBL().criteriosBusqueda(perfilActivo);
            AutoMapper.Mapper.Map(criterios, criteriosBusquedaViewModel);
            return PartialView("CriteriosBusqueda", criteriosBusquedaViewModel);
        }


        [ValidaAcceso(accion = "SelladoSellar")]
        public ActionResult MostrarModalSellado()
        {
            SelladoViewModel selladoViewModel = new SelladoViewModel();
            try
            {
                SelladoML selladoML = new SelladoML();

                selladoML.firmanteML = new FirmanteBL().GetFirmanteActivo(perfilActivo.insId);
                AutoMapper.Mapper.Map(selladoML, selladoViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return PartialView("ModalSellado", selladoViewModel);
        }

        [ValidaAcceso(accion = "SelladoCancelar")]
        public ActionResult CancelarRegistro(string documentoId)
        {
            var resultado = new SelladoBL().CacelarRegistro(documentoId, perfilActivo.insId, user.Login);

            return Json(resultado);

        }


        [ValidaAcceso(accion = "SelladoConsultar")]
        public ActionResult MostrarModalVerDetalleRegistro(string documentoId)
        {
            SelladoViewModel selladoViewModel =  new SelladoViewModel();
            SelladoML selladoML = new SelladoML();
            selladoML.CerDocumento=      new CertificadosBL().GetCerDocumento(documentoId);
            AutoMapper.Mapper.Map(selladoML, selladoViewModel);


            return PartialView("ModalDetalleDocumento", selladoViewModel);

        }
    }
}