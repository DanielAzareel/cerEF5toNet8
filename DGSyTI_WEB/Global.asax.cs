using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BusinessModel.MapperProfile;

using Helpers;
namespace DGSyTI_WEB
{
    /// <summary>
    /// Clase generada por el template del Visual Studio 2013
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Metodo sobreestrito de la clase base System.Web.HttpApplication para iniciar otros elementos al 
        /// momento de iniciar la applciación desde el IIS
        /// </summary>
        protected void Application_Start() 
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //INICIA MODIFICACION: Eduardo Jaramillo Fecha: 17 Septiembre 2014
            DisplayModeConfig.RegisterDisplayModes();
            //TERMINA MODIFICACIÓN Fecha: 17 Septiembre 2014
            //INICIA MODIFICACION: Eduardo Jaramillo FECHA: 02 Septiemnre 2014
            //Se agrega la llamada al AppConfig para cargar en la memoria las variables mas 
            //comunes para agilizar la consulta y distribución de las mismas, recomendación
            //por parte de Jorge Tapia
            AppConfig.Initialize();
            //TERMINA MODIFICACION-FECHA: 02 Septiembre 2014


            //Mapper.Initialize({< InstitucionEducativaViewModel, InstitucionesEducativasML > ()}).ReverseMap();

            Mapper.Initialize(cfg =>
            {

                cfg.AddProfile<AutoMapperProfile>();
                cfg.AddProfile<BussinessMapperProfile>();

            });

            DefaultModelBinder.ResourceClassKey = "MensajesValidación";

        }
    }
}
