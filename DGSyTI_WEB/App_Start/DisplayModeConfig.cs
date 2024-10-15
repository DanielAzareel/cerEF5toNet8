using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace DGSyTI_WEB
{
    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: 17 de Septiembre de 2014
    /// Descripción: Clase que permite extender los modos de despliegue que debe utilizar según el tipo de dispositivo
    ///         que este solicitando la petición de la página. Dichos modos se definen al momento de iniciar el aplicativo
    ///         por lo que es necesario inicializarlo en el Global.asax.cs
    /// </summary>
    /// <remarks>Clase obtenida del protitipo inicial provisto por Jorge Tapia</remarks>
    public class DisplayModeConfig
    {
        /// <summary>
        /// Método que definirá los dos tipo de modos de despliegue mas comunes
        /// </summary>
        public static void RegisterDisplayModes()
        {
            //La clase DisplayModeProvider fija la lista de objetos de despliegue que se deseen utilizar, en caso de no que no coincida
            //el modo de despliegue este utilizará el modo por defecto relacionado a las navegadores estándar de un equipo de computo, 
            //en este caso se aplciará la plantilla ~/Views/Shared/_Layout.cshtml
            //Se define el modo de despliegue para SmarthPhones para que utilice la plantilla ~/Views/Shared/_Layout.Phone.cshtml
            DisplayModeProvider.Instance.Modes.Insert(0,
               new DefaultDisplayMode("Phone")
               {
                   ContextCondition = (context => (
                     (context.GetOverriddenUserAgent() != null) &&
                     (
                       (context.GetOverriddenUserAgent().IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (context.GetOverriddenUserAgent().IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (context.GetOverriddenUserAgent().IndexOf("Droid", StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (context.GetOverriddenUserAgent().IndexOf("Blackberry", StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (context.GetOverriddenUserAgent().StartsWith("Blackberry", StringComparison.OrdinalIgnoreCase))
                     )
                   ))
               });
            //Se define el modo de despliegue para Tablets para que utilice la plantilla ~/Views/Shared/_Layout.Tablet.cshtml
            DisplayModeProvider.Instance.Modes.Insert(0,
              new DefaultDisplayMode("Tablet")
              {
                  ContextCondition = (context => (
                    (context.GetOverriddenUserAgent() != null) &&
                    (
                      (context.GetOverriddenUserAgent().IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0) ||
                      (context.GetOverriddenUserAgent().IndexOf("Playbook", StringComparison.OrdinalIgnoreCase) >= 0) ||
                      (context.GetOverriddenUserAgent().IndexOf("Transformer", StringComparison.OrdinalIgnoreCase) >= 0) ||
                      (context.GetOverriddenUserAgent().IndexOf("Kindle", StringComparison.OrdinalIgnoreCase) >= 0) ||
                      (context.GetOverriddenUserAgent().IndexOf("Xoom", StringComparison.OrdinalIgnoreCase) >= 0)
                    )
                  ))
              });
        }
    }
}