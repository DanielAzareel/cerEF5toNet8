using System;
using System.Web.Mvc;
using Helpers;
using BusinessModel.Business;
using BusinessModel.Models;
using DGSyTI_WEB.Models;
using BusinessModel.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using BusinessModel.Models.Personalizados;
using ServiciosWeb.SesionWS;
using CertificadosEletronicosMS.Models;

namespace DGSyTI_WEB.Controllers
{
    
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var user = SSExtensionHelper.GetAppUser();
            if (user == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("IndexTabs");
            }
            
        }

        public ActionResult IndexTabs()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

            var user = SSExtensionHelper.GetAppUser();
            if (user != null)
            {
                user.ListaAcciones.Select(cm => cm.AccDescripcion).ToList();

                homeViewModel.tabs = new AccesosBL().DefinirTabs(user.ListaAcciones.Select(cm => cm.AccNombre).ToList());
            }


            return View(homeViewModel);

        }



        public String prepararInformacionUsuario()
        {
            String result = "";

            if (SSExtensionHelper.GetAppUser() == null || (Session["wscs"] != null))
            {


                if (Session["wscs"] != null)
                {
                    //  Session["wscs"] = Request["wscs"];

                    if (!Session.ValidateSession())
                    {
                        result = AppConfig.AppEnviroment.urlSieg;


                    }
                    else
                    {
                        result = "/Home";
                    }
                }
                else if (SSExtensionHelper.GetAppUser() == null)
                {
                    result = "/Home";

                }
            }
            else
            {
                result = "/Home";
            }
            return result;

        }
        [ValidarSesion]
        public PerfilML ObtenerPerfilActivo()
        {
            List<PerfilML> listPerfiles = ((Usuario)Session["appUser"]).Perfiles;
            PerfilML perfilActivo = (from perfil in listPerfiles where perfil.seleccionado == true select perfil).FirstOrDefault();

            return perfilActivo;
        }
        [ValidarSesion]
        public ActionResult DashboardGraficas()
        {
            ViewBag.Graficas = "True";
            return PartialView("Dashboard");
        }
        [ValidarSesion]
        public ActionResult DashboardTablas()
        {
            ViewBag.Graficas = "False";
            return PartialView("Dashboard");
        }
        
        
       
        [ValidarSesion]
        public PartialViewResult datosInstitucion(string conId, string docInstitucionClave, string docInstitucionNombre)
        {
            PerfilML perfilActivo = ObtenerPerfilActivo();
            ViewBag.conId = conId;
            ViewBag.docInstitucionClave = docInstitucionClave;
            ViewBag.docInstitucionNombre = docInstitucionNombre;
            return PartialView("DatosConcentradora");
        }
       
        /// <summary>
        /// Método cuya función es atender la petición de la acción Close emitida por el Request. Su principal
        /// funcionamiento es limpiar la sessión concurrente a través de llamar el método extendido CloseSession() que
        /// se encuentra en la clase helper SSExtensionHelper.
        /// </summary>
        /// <returns>Devuelve el contenido de la página que se encuentra definida en la variable global AppConfig.AppEnviroment.urlSieg 
        /// y que esta localizada en el Web.Config en la sección de AppSettings.</returns>
        [ValidarSesion]
        public ActionResult Close()
        {
            if (Session["wscs"] != null)
            {
                SesionWSService sessionWS = new SesionWSService();
                sessionWS.Url = AppConfig.AppEnviroment.urlSesionWs;
                sessionWS.getCerrarSesion(Session["wscs"].ToString());
                Session.CloseSession();
            }
            try
            {
                return Content("<script>window.location.href='" + AppConfig.AppEnviroment.urlSieg + "'</script>");
            }
            catch (Exception ex)
            {
                return Content("<script>window.location.href='" + AppConfig.AppEnviroment.urlSieg + "'</script>");
            }
        }

        [ValidarSesion]
        public ActionResult CambiarPerfil(string sValor)
        {
            bool bBandera = false;
            try
            {
                string[] arr = sValor.Split('|');
                List<Acciones> lstAcciones = ValidaAccesoAttribute.CargarAcciones(arr[0]);


                Usuario usuario = Usuario.GetUser();
                usuario.ListaAcciones = lstAcciones;

                usuario.Perfiles.Where(x => x.insId == arr[1] && x.rolId == arr[0]);

                foreach (var user in usuario.Perfiles)
                {
                    user.seleccionado = false;
                }

                foreach (var user in usuario.Perfiles.Where(x => x.insId == arr[1] && x.rolId == arr[0]))
                {

                    user.seleccionado = true;
                }

                bBandera = true;
            }
            catch (Exception ex)
            {
                bBandera = false;
                Console.WriteLine(ex.Message);
            }
            return Json(bBandera);
        }

        public string ChecarSesionAjax()
        {
            var user=SSExtensionHelper.GetAppUser();
            string result = "";
            if (user == null)
            {

                string urlSieg = ConfigurationManager.AppSettings["urlSieg"].ToString();
                result = "<script>window.location.replace('" + urlSieg + "');</script>";

            }
            return result;

        }

       
    }
}