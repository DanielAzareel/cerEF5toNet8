using System;
using System.Dynamic;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DGSyTI_WEB
{
    public static class AppConfig
    {
        /// <summary>
        /// <b>_AppSettings</b> es un atributo privado estático, se encarga de almacenar la información relacionada a la sección
        /// <b>appSettings</b> del archivo <b>Web.Config</b> principal del aplicativo.
        /// </summary>
        private static readonly NameValueCollection _AppSettings;
        /// <summary>
        /// <b>_AppConnectionStrings</b> es un atributo privado estático, se encarga de almacenar la información relacionada a la sección
        /// <b>connectionStrings</b> del archivo <b>Web.Config</b> principal del aplicativo.
        /// </summary>
        private static readonly ConnectionStringSettingsCollection _AppConnectionStrings;
        /// <summary>
        /// <b>_AppEnviroment</b> es un atributo dinámico privado estático, se encarga de proveer otras propiedades relacionados a los 
        /// índices y valores contenidos en los atributos <b>_AppSettings</b> y <_AppConnectionStrings>, además de incluir las relacionadas
        /// al <b>HttpRuntime</b>.
        /// </summary>
        private static readonly dynamic _AppEnviroment;
        /// <summary>
        /// <b>AppSettings</b> es una propiedad pública estática de solo lectura. Su finalidad es proveer una vínculo con el atributo
        /// <b>_AppSettings</b>.
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get { return _AppSettings; }
            private set { }
        }
        /// <summary>
        /// <b>AppConnectionStrings</b> es una propiedad pública estática de solo lectura. Su finalidad es proveer una vínculo con el atributo
        /// <b>_AppConnectionStrings</b>.
        /// </summary>
        public static ConnectionStringSettingsCollection AppConnectionStrings
        {
            get { return _AppConnectionStrings; }
            private set { }
        }
        /// <summary>
        /// <b>AppEnviroment</b> es una propiedad pública estática de solo lectura. Su finalidad es proveer una vínculo con el atributo
        /// <b>_AppEnviroment</b>.
        /// </summary>
        public static dynamic AppEnviroment
        {
            get { return _AppEnviroment;  }
            private set { }
        }
        /// <summary>
        /// El método público estático <b>Initialize()</b> tiene como fin instanciar la propia clase cuando se necesite.
        /// </summary>
        /// <remarks>
        /// Este método público estático será llamado cuando se inicialice la aplicación, que es en el método <b>Application_Start()</b> que se encuentra en el la
        /// clase <b>Global.asax</b>.
        /// </remarks>
        public static void Initialize(){ }
        /// <summary>
        /// El constructor estático <b>AppConfig()</b> tiene como finalidad inicializar cualquier miembro estático de la propia clase a través de una acción una sola vez.
        /// </summary>
        /// <remarks>
        /// El constructor es llamado una sóla vez de manera automática cuando se crea la primera instancia o bien cuando se accede a cualquiera de sus miembros estáticos.
        /// </remarks>
        /// <see cref="http://msdn.microsoft.com/es-es/library/k9x6w0hc.aspx"/>
        static AppConfig()
        {
            // Se obtiene de HttpContext la información inicial del primer Request del aplicativo.
            //Uri url = HttpContext.Current.Request.Url;
            //Se construye la cadena URI conforme a la información del URL del Request
            //string urlDomain = string.Format("{0}{1}{2}{3}", url.Scheme, Uri.SchemeDelimiter, url.Host, (url.IsDefaultPort ? "" : ":" + url.Port));
            //Se obtiene el virtualpath del URL conforme al primer Request
            string urlVirtualPath = HttpRuntime.AppDomainAppVirtualPath;            
            //Posteriormente se inicializa el atributo privado estático dinámico para generarle propiedades dinámicas para transformar la información
            _AppEnviroment = new ExpandoObject();
            //Se agregan nuevas propiedades dinámicas del atributo _AppEnviroment
            ((IDictionary<string, object>)_AppEnviroment).Add("appDomainAppId", HttpRuntime.AppDomainAppId);
            ((IDictionary<string, object>)_AppEnviroment).Add("appDomainAppPath", HttpRuntime.AppDomainAppPath);
            ((IDictionary<string, object>)_AppEnviroment).Add("appDomainAppVirtualPath", urlVirtualPath);
            ((IDictionary<string, object>)_AppEnviroment).Add("appDomainId", HttpRuntime.AppDomainId);
            ((IDictionary<string, object>)_AppEnviroment).Add("aspInstallDirectory", HttpRuntime.AspInstallDirectory);
            ((IDictionary<string, object>)_AppEnviroment).Add("clrInstallDirectory", HttpRuntime.ClrInstallDirectory);
            ((IDictionary<string, object>)_AppEnviroment).Add("codegenDir", HttpRuntime.CodegenDir);
            ((IDictionary<string, object>)_AppEnviroment).Add("binDirectory", HttpRuntime.BinDirectory);
            ((IDictionary<string, object>)_AppEnviroment).Add("isOnUNCShare", HttpRuntime.IsOnUNCShare.ToString());
            ((IDictionary<string, object>)_AppEnviroment).Add("machineConfigurationDirectory", HttpRuntime.MachineConfigurationDirectory);
            ((IDictionary<string, object>)_AppEnviroment).Add("IISVersion", HttpRuntime.IISVersion.ToString());            
            //((IDictionary<string, object>)_AppEnviroment).Add("absoluteURLApplicacion", urlDomain);

            //Se obtiene la información del archivo Web.Config de las secciones appSettings y connectionStrings
            _AppSettings = WebConfigurationManager.AppSettings;
            _AppConnectionStrings = WebConfigurationManager.ConnectionStrings;
            //Se construyen las propiedades dínamicas del atributo _AppEnviroment a partir de la información del _AppSettings
            if( _AppSettings != null && _AppSettings.Count > 0 ) {
                //En caso de existir información en _AppSettings se itera para construir las propiedades dinámicas
                using (List<string>.Enumerator key = _AppSettings.AllKeys.ToList().GetEnumerator())
                {
                    while (key.MoveNext())
                    {
                        //Mientras se encuentre un valor en _AppSettings se procesa la información
                        if (((IDictionary<string, object>)_AppEnviroment).ContainsKey(key.Current))
                            ((IDictionary<string, object>)_AppEnviroment).Remove(key.Current);
                        //En caso de que el valor _AppSettings sea lógico
                        if (_AppSettings[key.Current].Trim().ToLower().Equals("false") || _AppSettings[key.Current].Trim().ToLower().Equals("true"))
                            ((IDictionary<string, object>)_AppEnviroment).Add(key.Current, Convert.ToBoolean(_AppSettings[key.Current].Trim().ToLower()));
                        else 
                            ((IDictionary<string, object>)_AppEnviroment).Add(key.Current, _AppSettings[key.Current]);
                    }
                }
            }
            //Se construyen las propiedades dínamicas del atributo _AppEnviroment a partir de la información del _AppConnectionStrings
            if (_AppConnectionStrings != null && _AppConnectionStrings.Count > 0)
            {
                //En caso de existir información en _AppConnectionStrings se itera para construir las propiedades dinámicas
                System.Collections.IEnumerator cs = _AppConnectionStrings.GetEnumerator();
                int indexCollection = 0;
                while (cs.MoveNext())
                {
                    //Mientras se encuentre un valor en _AppConnectionStrings se procesa la información
                    string name = _AppConnectionStrings[indexCollection].Name;
                    string conStr = _AppConnectionStrings[indexCollection].ConnectionString;
                    string proNam = _AppConnectionStrings[indexCollection].ProviderName;
                    if (((IDictionary<string, object>)_AppEnviroment).ContainsKey(name))
                        ((IDictionary<string, object>)_AppEnviroment).Remove(name);
                    ((IDictionary<string, object>)_AppEnviroment).Add(name, new ExpandoObject());
                    ((IDictionary<string, object>)((IDictionary<string, object>)_AppEnviroment)[name]).Add("ConnectionString", conStr);
                    ((IDictionary<string, object>)((IDictionary<string, object>)_AppEnviroment)[name]).Add("ProviderName", proNam);
                    indexCollection++;
                }
            }
        }
    }
}