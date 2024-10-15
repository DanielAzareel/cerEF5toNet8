
namespace DGSyTI_WEB
{
    /// <summary>
    /// Clase creada por el template del Visual Studio 2013
    /// </summary>
    /// <remarks>Se modificó la clase, ya que el template sugería incorporar el metodo de autenticación OAuthritity, con
    /// el fin de que si el desarrollo requería utilizar el esquema de manejo usuarios, pudiera iniciar bajo este contexto. 
    /// La modificación fue relaizada por Eduardo Jaramillo</remarks>
    public partial class Startup
    {
        /// <summary>
        /// Constructor de la clase Startup, su fin es volver a llamar la clase estática AppConfig para la carga de variables globales.
        /// </summary>
        public Startup()
        {
            //Inicializa la clase estática
            AppConfig.Initialize();
        }


    }


}
