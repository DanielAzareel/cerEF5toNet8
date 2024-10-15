using BusinessModel.Models.Personalizados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Business
{
   public  class AccesosBL
    {
        public List<TabML> DefinirTabs(List<string>acciones){

            List<TabML> tabs = new List<TabML>();

            var accion = (from c in acciones where c=="CargaInformacion" select c);

            if(accion!=null && accion.Any())
            { 
            tabs.Add(new TabML
            {
                activa = true,
                controller = "CargaCertificadosTerminacion",
                metodo = "Index",
                label = "Carga de información",
                parametro = "",
                subTabs = null,

            });
            }

            accion = (from c in acciones where c=="Sellado" select c);

            if (accion != null && accion.Any())
            {
                tabs.Add(new TabML
                {
                    activa = true,
                    controller = "Sellado",
                    metodo = "IniciarModulo",
                    label = "Sellado de Certificados",
                    parametro = "",
                    subTabs = null,

                });
            }

            accion = (from c in acciones where c== "EnvioSEP" select c);

            if (accion != null && accion.Any())
            {
                tabs.Add(new TabML
                {
                    activa = true,
                    controller = "Certificado",
                    metodo = "IniciarModulo",
                    label = "Monitoreo SEP",
                    parametro = "",
                    subTabs = null,

                });
            }

            return tabs; 
        }
    }
}
