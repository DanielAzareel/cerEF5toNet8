using System;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;

namespace BusinessModel
{
    /// <summary>
    /// Autor: Secretaría de Eduación de Guanajuato
    /// Fecha de Creación: 05 Septiembre 2014
    /// Descripción: La clase LocalConfiguration tiene como finalidad en obtener las propiedades de su propio archivo de configuración, de está forma
    ///         se puede pa´ramétrizar dichos valores.
    /// </summary>
    /// <remarks>Esta clase fue creada por Eduardo Jaramillo, con el fin de encapsular toda la parte de negocio relacionado</remarks>
    static class LocalConfiguration
    {
        /// <summary>
        /// Atributo privado para almacenar la ruta donde se encuentta ejecutando el Assembly
        /// </summary>
        private static readonly string _AssemblyLocation;
        /// <summary>
        /// Atributo privado que devolvera la colección de datos relacionados a la sección del ConnectionStrings del App.Config
        /// </summary>
        private static readonly ConnectionStringSettingsCollection _ConnectionStrings;
        /// <summary>
        /// Atributo privado que devolverá la colección de datos relacionados a la sección del AppSettings del App.Config
        /// </summary>
        private static readonly KeyValueConfigurationCollection _AppSettings;
        /// <summary>
        /// Autor: Secretaría de Eduación de Guanajuato
        /// Fecha de Creación: 05 Septiembre 2014
        /// Descripción: Propiedad que devolvera la colección de valores de la sección de ConnectionStrings .
        /// </summary>
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _ConnectionStrings;  }
            private set { }
        }
        /// <summary>
        /// Autor: Secretaría de Eduación de Guanajuato
        /// Fecha de Creación: 05 Septiembre 2014
        /// Descripción: Propiedad que devolvera la colección de valores de la sección de AppSettings .
        /// </summary>
        public static KeyValueConfigurationCollection AppSettings
        {
            get { return _AppSettings; }
            private set { }
        }
        /// <summary>
        /// Constructor estático que se ejecutará solo una vez cuando sea llamada la librería por parte de cualquier método, propiedad u atributo.
        /// </summary>
        static LocalConfiguration()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            _AssemblyLocation = Uri.UnescapeDataString(uri.Path);
            ExeConfigurationFileMap fileConfig = new ExeConfigurationFileMap {
                ExeConfigFilename = _AssemblyLocation + ".config"
            };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileConfig, ConfigurationUserLevel.None);
            _ConnectionStrings = config.ConnectionStrings.ConnectionStrings;
            _AppSettings = config.AppSettings.Settings;
            
        }

    }
}
