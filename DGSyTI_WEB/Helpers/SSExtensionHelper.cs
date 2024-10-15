using System;
using System.Collections.Generic;
using DGSyTI_WEB;
using ServiciosWeb.SesionWS;
using System.Web;
using BusinessModel.Models;
using System.Linq;

using BusinessModel.DataAccess;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using BusinessModel.Business;

namespace Helpers
{
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 06Junio204
    /// Descripción: SSExtension es en relación al Security Session Extension, su fin es proveer una serie de métodos que permitan 
    ///         encapsular el manejo de las variables de sesión del aplicativo Web, así como también proveer de los objetos
    ///         necesarios en la sesión para su uso posterior.
    /// </summary>
    /// <remarks>Clase construída por Eduardo Jaramillo, su fin es para representar el manejos de seguridad que estaba en una librería de 
    /// Modelo de Negocio Contextual creada por Julio Rangel y Jorge Tapia </remarks>
    [Serializable()]
    public static class SSExtensionHelper
    {
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción:Método estático público que encapsula el manejo de los elementos de seguridad de la sesión.
        /// </summary>
        /// <remarks>Creado por Eduardo Jaramillo en base al esquema original porpuesto por Julio Rangel y Jorge Tapia</remarks>
        /// <param name="session">Parámetro que permitira extender el objeto Session al momento de ser llamado desde la vista</param>
        /// <returns>Devuelve la cadena de valores de seguridad correspondiente al usuario de la sesión</returns>
        public static string GetWscs(this HttpSessionStateBase session)
        {
            return session["wscs"] != null ? session["wscs"].ToString() : null;
        }



        public static PerfilML ObtenerPerfilActivo( )
        {
            try
            {
                List<PerfilML> listPerfiles = ((Usuario)System.Web.HttpContext.Current.Session["appUser"]).Perfiles;
                PerfilML perfilActivo = (from perfil in listPerfiles where perfil.seleccionado == true select perfil).FirstOrDefault();
                return perfilActivo;
            }
            catch (Exception)
            {
                return new PerfilML();

            }

        }

        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método público estático cuya finalidad es validar la sesión correspondiente del usuario autenticado, en caso
        ///         de que la validación fuera exitosa, creará la sesión correspondiente con la información del usuario a través de un 
        ///         objeto de tipo Usuario.
        /// </summary>
        /// <param name="session">Parámetro que permitira extender el objeto Session al momento de ser llamado desde la vista</param>
        /// <returns>Devuelve un valor lógico en base a los resultados de la validación de la sesión, en caso de existir alguna
        /// inconsistencia devolverá el valor false</returns>
        public static bool ValidateSession(this HttpSessionStateBase session)
        {
            //suponemos que es no es valida la autenticación de la sesión
            bool result = false;
            if (session["wscs"] != null)
            {
                SesionWSService sessionWS = new SesionWSService();
                sessionWS.Url = AppConfig.AppEnviroment.urlSesionWs;
                sesionMensajesImpl sesionimpl = new sesionMensajesImpl();
                sesionimpl = sessionWS.getSesionDinamica(session["wscs"].ToString(), AppConfig.AppEnviroment.idAplicacion);
                if (sesionimpl.usuarioSesion != null)
                {
                    Usuario user = new Usuario();

                    List<filtroRolSesion> filtros = new List<filtroRolSesion>(); //verificar para que sirve
                    if (sesionimpl.arbolAcciones != null && sesionimpl.arbolAcciones.Length > 0)
                    {
                        if (user.ListaAcciones == null) user.ListaAcciones = new List<Acciones>();
                        foreach (accionSesion accion in sesionimpl.arbolAcciones)
                        {
                            user.ListaAcciones.Add(new Acciones
                            {
                                AccNombre = accion.accNombre.Trim(),
                                AccOrden = accion.accOrden,
                                AccId = (long)accion.accId,
                                AccURL = accion.accURL,
                                AccIdAccionGrupo = (long)accion.accIdAccionGrupo,
                                AccDescripcion = accion.accDescripcion,
                                Nivel = accion.nivel
                            });
                        }
                    }
                    user.EmployeeId = sesionimpl.usuarioSesion.usrEmployeeId;
                    user.Nombre = sesionimpl.usuarioSesion.usrNombre;
                    user.Apellido1 = sesionimpl.usuarioSesion.usrApellido1;
                    user.Apellido2 = sesionimpl.usuarioSesion.usrApellido2;
                    user.Login = sesionimpl.usuarioSesion.usrLogin;
                    user.Email = sesionimpl.usuarioSesion.usrEmail;
                    user.Menu = sesionimpl.sesionMensajes.menu;
                    user.AccesoLiveedu = sesionimpl.usuarioSesion.usrURLLE;
                    user.FiltrosRoles = filtros;

                    PerfilML oPerfilML = new PerfilML();
                    List<PerfilML> listPerfilML = new List<PerfilML>();
                    List<string> listRolId = new List<string>();
                    if (user.ListaAcciones == null || user.ListaAcciones.Count == 0)
                    {
                        if (user.ListaAcciones == null) user.ListaAcciones = new List<Acciones>();
                        user.ListaAcciones.Add(new Acciones
                        {
                            AccNombre = "Acceso limitado, no tiene permisos asignados. Para mayor información consulte al administrador del sistema.",
                            Nivel = 2,
                            AccURL = ""
                        });
                    }
                    else
                    {
                        List<rolSesion> lstRol = new List<rolSesion>(sesionimpl.roles);

                        if (listPerfilML.Count > 0)
                        {
                            listPerfilML[0].seleccionado = true;
                        }

                        user.Roles = string.Join("<br>", lstRol.ConvertAll(x => x.rolNombre).ToArray()) + "<br>";
                        user.ClaveRoles = string.Join("|", lstRol.ConvertAll(x => x.rolClave).ToArray());

                        try
                        {
                            List<rolSesion> lstRol2 = new List<rolSesion>(sesionimpl.roles);
                            List<cerConfiguracionInstitucion> listConfiguracionInstituciones = new ConfiguracionInstitucionBL().GetConfiguracionInstituciones();
                            //iteramos la lista de roles y les sacamos los filtro asociados al rol 
                            foreach (var rol in lstRol)
                            {
                                if (rol.filtroRolSesion != null)
                                {
                                    PerfilML perfil = new PerfilML();

                                    perfil.rolId = rol.rolClave;
                                    perfil.rolDescripcion = rol.rolDescripcion;
                                    perfil.seleccionado = false;

                                    foreach (var filtroItem in rol.filtroRolSesion)
                                    {
                                        filtros.Add(filtroItem);

                                        var accesos = filtroItem.fltNombre.Split(',').ToList();
                                        //var vPerfil = user.Perfiles.Where(x => x.rolId == rol.rolClave).FirstOrDefault();

                                        if (listConfiguracionInstituciones.Where(x => x.insId == accesos[0]).Count() == 0)
                                        {
                                            listRolId.Add(rol.rolClave);

                                        }
                                        else
                                        {
                                            var institucion = listConfiguracionInstituciones.Where(x => x.insId == accesos[0]).FirstOrDefault();
                                            perfil.nivelAcceso.Add((institucion.insId, (accesos.Count >= 2 ? accesos[1] : "")));
                                        }
 
                                    }

                                    perfil.insId = perfil.nivelAcceso.FirstOrDefault().institucion;
                                    perfil.insDescripción = listConfiguracionInstituciones.Where(x => x.insId == perfil.insId).FirstOrDefault().InsNombre;
                                    if (!String.IsNullOrEmpty(perfil.insId))
                                    {
                                        perfil.insId = listConfiguracionInstituciones.Where(x => x.insId == perfil.insId).FirstOrDefault().insId;
                                    }

                                    if (user.Perfiles.Where(x => x.rolId == perfil.rolId && x.insId == perfil.insId).Count() == 0)
                                    {
                                        user.Perfiles.Add(perfil);
                                    }
                                    else
                                    {
                                        user.Perfiles.Where(x => x.rolId == perfil.rolId && x.insId == perfil.insId).FirstOrDefault().nivelAcceso.Add(perfil.nivelAcceso.FirstOrDefault());
                                    }
                                    for (int i = 0; i < listRolId.Count; i++)
                                    {
                                        user.Perfiles.Remove(user.Perfiles.Where(x => x.rolId == listRolId[i]).FirstOrDefault());
                                    }

                                    try
                                    {
                                        if (filtros != null && filtros.Count > 0)
                                        {
                                            user.nombresAmbito = rol.filtroRolSesion[0].fltNombre;
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }

                            if (user.Perfiles.Count > 0)
                            {
                                user.Perfiles.FirstOrDefault().seleccionado = true;
                                user.ListaAcciones = ValidaAccesoAttribute.CargarAcciones(user.Perfiles.FirstOrDefault().rolId);
                            }
                            else
                            {
                                user.ListaAcciones = new List<Acciones>();

                                if (listRolId.Count > 0)
                                {
                                    user.ListaAcciones.Add(new Acciones
                                    {
                                        AccNombre = "La configuración de roles no es correcta.",
                                        Nivel = 2,
                                        AccURL = ""
                                    });
                                }
                                else
                                {

                                    user.ListaAcciones.Add(new Acciones
                                    {
                                        AccNombre = "Acceso limitado, no tiene permisos asignados. Para mayor información consulte al administrador del sistema.",
                                        Nivel = 2,
                                        AccURL = ""
                                    });
                                }
                            }

                            user.FiltrosRoles = filtros;
                            user.Roles = string.Join("<br>", lstRol.ConvertAll(x => x.rolNombre).ToArray()) + "<br>";
                            user.ClaveRoles = string.Join("|", lstRol.ConvertAll(x => x.rolClave).ToArray());
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }

                    }

                    session.Add("appUser", user);
                    session.Add("appUsuario", user.Login);



                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método público estático cuya finalidad es obtener el objeto almacenado en la sesión correspondiente al usuario 
        ///         autenticado.
        /// </summary>
        /// <param name="session">Parámetro que permitira extender el objeto Session al momento de ser llamado desde la vista</param>
        /// <returns>Devuelve un objeto de tipo Usuario, en caso de no existir en la sesión devolverá un objeto nulo.</returns>
        public static Usuario GetAppUser()
        {
            return System.Web.HttpContext.Current.Session["appUser"] != null ? (Usuario)System.Web.HttpContext.Current.Session["appUser"] : null;
        }
       
        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: 06Junio204
        /// Descripción: Método público estático cuya finalidad es limpiar la sesión concurrente.
        /// </summary>
        /// <param name="session">Parámetro que permitira extender el objeto Session al momento de ser llamado desde la vista</param>
        public static void CloseSession(this HttpSessionStateBase session)
        {
            session.Remove("appUser");
           
        }
    }
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 06Junio204
    /// Descripción: Clase que representa el contexto de las Acciones asignadas a un usuario, en ella se almacena la información correspondiente
    ///             a la acción que fue definida en el Sistema de Control de Seguridad para el usuario autenticado.
    /// </summary>
    /// <remarks>Clase construída por Eduardo Jaramillo, su fin es para representar el esquema de acciones de un usuario autenticado en base 
    /// al código original de Julio Rangel y Jorge Tapia </remarks>
    [Serializable]
    public class Acciones
    {
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de Orden de una acción
        /// </summary>
        public int? AccOrden { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del Nivel donde se encuentra la acción. 
        /// </summary>
        public int Nivel { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor que identifica la acción 
        /// </summary>
        public long AccId { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del identificador correspondiente al grupo que pertenece la acción
        /// </summary>
        public long AccIdAccionGrupo { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del nombre de la acción, esta propiedad es la que se desppliega en el menú
        /// </summary>
        public string AccNombre { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de la acción en terminos de URL. Para estos casos se utiliza la notación 
        /// {Controller/Acción}
        /// </summary>
        public string AccURL { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de la descipción propia de la acción
        /// </summary>
        public string AccDescripcion { get; set; }
    }
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 06Junio204
    /// Descripción: Clase que encapsula los elementos importantes que representan al usuario, de esta forma se evita que sus valores esten 
    ///             distribuidos de manera separada en la sesión.
    /// </summary>
    /// <remarks>Clase construída por Eduardo Jaramillo, su fin es encapsular la información del usuario autenticado en base 
    /// al código original de Julio Rangel y Jorge Tapia </remarks>
    [Serializable]
    public class Usuario
    {
        /// <summary>
        /// Propiedad pública que proveerá solo el valor del nombre completo del usuario.
        /// </summary>
        public string NombreCompleto
        {
            get
            {
                return string.Format("{0} {1} {2}", (!string.IsNullOrEmpty(Nombre)) ? Nombre.Trim() : "", (!string.IsNullOrEmpty(Apellido1)) ? Apellido1.Trim() : "", (!string.IsNullOrEmpty(Apellido2)) ? Apellido2.Trim() : "");
            }
            private set { }
        }

        public static Usuario GetUser()
        {
            return (Usuario)System.Web.HttpContext.Current.Session["appUser"];
        }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del identificador de empleado correspondiente al usuario.
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del primero apellido del usuario.
        /// </summary>
        public string Apellido1 { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del segundo apellido del usuario.
        /// </summary>
        public string Apellido2 { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del nombre o nombres de pila del usuario.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del login del usuario. En este caso correspondiete al email de la institución de la SEG.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de las caráteristicas del menu inferior del aplicativo conforme a la información de los 
        /// sistemas que fueron asignados al usuario en el Sistema de Control de Seguridad. 
        /// </summary>
        public string Menu { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor del correo electrónico del LiveEdu o bien Office365
        /// </summary>
        public string AccesoLiveedu { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de los roles asigandos al usuario separados por medio del tag <BR>
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de las claves identificadoras de los roles separados por el caracter |
        /// </summary>
        public string ClaveRoles { get; set; }

        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de las nombres de los ambitos separados por el caracter |
        /// </summary>
        public string nombresAmbito { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de la colección de Acciones del menú izquierdo que le fueron asociadas al usuario.
        /// </summary>
        public List<Acciones> ListaAcciones { get; set; }
        /// <summary>
        /// Propiedad pública que almacenará/proveerá el valor de la colección de los filtros a aplicar asociados al usuario.
        /// </summary>
        public List<filtroRolSesion> FiltrosRoles { get; set; }

        public List<PerfilML> Perfiles { set; get; } = new List<PerfilML>();
    }
}