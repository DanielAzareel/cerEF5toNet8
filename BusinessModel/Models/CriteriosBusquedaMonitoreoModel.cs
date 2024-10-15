using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class CriteriosBusquedaMonitoreoModel
    {
        public string sFolioControl { get; set; }
        public string sCURP { get; set; }
        public string sNombre { get; set; }
        public string sSolId { get; set; }

        public string sIdEstatus { get; set; }

        public string sIdPlan { get; set; }
        public string sIdDocumento { get; set; }
        public string sIdInstitucion { get; set; }

        public string sIdTipoCertificado { get; set; }

        public List<string> lstFolio = new List<string>();
        public List<string> lstCURP = new List<string>();
        public List<string> lstPlan = new List<string>();
        public List<string> lstTipoCertificado = new List<string>();
        public List<string> lstEstatus = new List<string>();
    }
}
