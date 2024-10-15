using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class cerUACDocumentoML: cerUACDocumento
    {
        public string idPlan { get; set; }
        public int orden { get; set; }
        public int idPeriodo { get; set; }
    }
}
