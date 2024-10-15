using BusinessModel.Models.Personalizados;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class SelladoML
    {
        public FirmanteML firmanteML { set; get; }
        public List<VWGridSellado> documentos { set; get; }
        public CriteriosBusquedaML CriteriosBusquedaML { set; get; }
        public List<TabML> tabs { set; get; } = new List<TabML>();

        public cerDocumento CerDocumento { set; get; } = new cerDocumento();
        public int totalRegistros { set; get; } = 0;
     
        
              
        
        
        public SelladoML()
        {

        }
      public   SelladoML(string rol)
        {

            tabs.Add(new TabML
            {
                activa = true,
                controller="Sellado",
                metodo="IniciarModulo",
                label ="Sellado de Certificados",
                parametro="",
                subTabs=null,
                
            }) ;

            tabs.Add(new TabML
            {
                activa = true,
                controller = "Sellado",
                metodo = "IniciarModulo",
                label = "Monitoreo",
                parametro = "",
                subTabs = null,

            });

        }
    }
}
