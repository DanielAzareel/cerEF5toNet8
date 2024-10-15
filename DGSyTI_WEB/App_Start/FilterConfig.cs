using System.Web;
using System.Web.Mvc;

namespace DGSyTI_WEB
{
    /// <summary>
    /// Clase creada por el propio template del proyecto de Visual Studio 2013
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
