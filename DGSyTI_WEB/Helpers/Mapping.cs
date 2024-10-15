using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DGSyTI_WEB.Helpers
{
    /// <summary>
    /// Clase con un metodo que se usa para mapear las propiedades de un objeto a otro objeto y copiar los valores de estas propiedades
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// Metodo estatico que mapea las propiedades de un objeto a otro objeto y copia los valores de estas propiedades
        /// </summary>
        /// <param name="source">objeto recurso</param>
        /// <param name="destination">objeto destino</param>
        /// <returns>objeto destino</returns>
        public static object mapeoManual(object source, object destination)
        {
            Type tSource = source.GetType();
            Type tDestination = destination.GetType();
            PropertyInfo[] properties = tSource.GetProperties();

            foreach (var p in properties)
            {
                if (p.GetIndexParameters().Length == 0)
                {
                    PropertyInfo dPInstance = tDestination.GetProperty(p.Name);
                    if (dPInstance != null)
                    {
                        dPInstance.SetValue(destination, p.GetValue(source));
                    }

                }
            }

            return destination;
        }

    }
}