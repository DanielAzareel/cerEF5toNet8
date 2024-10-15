using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class InstitucionML
    {
        public cerConfiguracionInstitucion oCerConfiguracionInstitucion { get; set; } = new cerConfiguracionInstitucion();

        public List<cerParametroValor> listCerParametroValor { get; set; } = new List<cerParametroValor>();
    }
}
