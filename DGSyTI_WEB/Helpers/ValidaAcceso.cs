using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using BusinessModel.Models;
using BusinessModel.Business;
using ServiciosWeb.ServicioPermisos;
using System.Web.Script.Serialization;
using DGSyTI_WEB;

namespace Helpers
{
    public class ValidaAccesoAttribute : AuthorizeAttribute
    {
        public string accion { get; set; }
       
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var user = (Usuario)HttpContext.Current.Session["appUser"];

            if (user != null)
            {
                if (user.ListaAcciones.Where(cm => cm.AccNombre.Equals(accion)).Count() == 0 && !accion.Equals("NoValidar"))
                {
                    //filterContext.HttpContext.Response.StatusCode = 401;
                    //string errorRoute = filterContext.HttpContext.Request.IsAjaxRequest() ? "/Errors/e401_ajax" : "/Errors/e401";
                    //filterContext.Result = new RedirectResult(errorRoute);
                    filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["urlSieg"]);

                }
 
            }
            else
            {
                filterContext.Result = new RedirectResult("http://" + HttpContext.Current.Request.Url.Host + "/Home/Close");
                //filterContext.Result = new RedirectResult("<script>window.location.href='" + AppConfig.AppEnviroment.urlSieg + "'</script>");
                //HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.Url.Host + "/Home/Close");
            }
        }
        public static Boolean validaAccion(string accion)
        {

            try
            {
                
                var user = (Usuario)HttpContext.Current.Session["appUser"];
                return user.ListaAcciones.Where(cm => cm.AccDescripcion== accion).Any();
            }
            catch (Exception ex) { return false; }
        }


        public static List<Acciones> CargarAcciones(string sClaveRol)
        {
            PerfilML oPerfilUsuario = new PerfilML();
            List<RespuestaWSML> ListRespuestaWSML = new List<RespuestaWSML>();
            RespuestaWSML oRespuestaWSML = new RespuestaWSML();
            string sQuery = "";
            string sWSRespuesta = "";


            GenericoConsultaSCSWSClient genericoConsultaSCSWS = new GenericoConsultaSCSWSClient();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string sClaveAplicacion = AppConfig.AppEnviroment.idAplicacion;
            var idConsulta =Convert.ToInt32(ConfigurationManager.AppSettings["idConsultaPermisos"]);
           


            sQuery = "@IDAPLICACION='" + sClaveAplicacion + "'|@CLAVEROL='" + sClaveRol + "'";
           
            sWSRespuesta = genericoConsultaSCSWS.obtieneDatosGenericoSCS(idConsulta, sQuery);

            oRespuestaWSML = new RespuestaWSML();
            oRespuestaWSML = json_serializer.Deserialize<RespuestaWSML>(sWSRespuesta);

            oPerfilUsuario = new PerfilML();
            oPerfilUsuario.rolId = sClaveRol;

            List<Acciones> lstAcciones = new List<Acciones>();
            int iOrden = 0;
            foreach (var oDatos in oRespuestaWSML.datos)
            {
                iOrden = 0;
                if (oDatos.ACC_ORDEN != null)
                {
                    bool bOrden = int.TryParse(oDatos.ACC_ORDEN.Split('.')[0], out iOrden);
                }

                lstAcciones.Add(new Acciones()
                {
                    AccId = (long)oDatos.ID_PROCESS,
                    AccIdAccionGrupo = oDatos.ACC_ID_ACCION_GRUPO != null ? (long)oDatos.ACC_ID_ACCION_GRUPO : 0,
                    AccNombre = oDatos.ACC_NOMBRE,
                    AccDescripcion = oDatos.ACC_DESCRIPCION,
                    AccURL = oDatos.ACC_URL,
                    AccOrden = iOrden
                });
            }
            List<Acciones> lstAccionesAux = new List<Acciones>();
            Acciones accionesAux = new Acciones();
            Acciones accionesAux2 = new Acciones();
            foreach (var oAcciones in lstAcciones)
            {
                accionesAux = new Acciones();
                if (oAcciones.AccIdAccionGrupo == 0)
                {
                    oAcciones.Nivel = 1;
                    accionesAux = oAcciones;
                    lstAccionesAux.Add(accionesAux);

                    List<Acciones> lst = (from l in lstAcciones
                                          where l.AccIdAccionGrupo == accionesAux.AccId
                                          select l).ToList();

                    foreach (var valor in lst)
                    {
                        accionesAux2 = new Acciones();
                        accionesAux2 = valor;
                        if (valor != null)
                        {
                            if (valor.AccIdAccionGrupo == 0)
                            {
                                valor.Nivel = 1;
                            }
                            else
                            {
                                valor.Nivel = 2;
                            }

                            lstAccionesAux.Add(accionesAux2);
                        }
                    }

                }
                else
                {
                    try
                    {
                        if (lstAccionesAux.Where(x => x.AccId == oAcciones.AccId).Count() == 0)
                        {

                            var vAccion = (from l in lstAcciones
                                           where l.AccId == oAcciones.AccIdAccionGrupo
                                           select l).FirstOrDefault();
                            if (vAccion != null)
                            {
                                if (vAccion.AccIdAccionGrupo == 0)
                                {
                                    vAccion.Nivel = 1;
                                }
                                else
                                {
                                    vAccion.Nivel = 2;
                                }
                           

                            oAcciones.Nivel = vAccion.Nivel + 1;
                            accionesAux = oAcciones;
                            lstAccionesAux.Add(accionesAux); 
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }


            }


            return lstAccionesAux;
        }
    }
}