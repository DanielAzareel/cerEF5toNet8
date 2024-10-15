using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class FirmaResponsableSelladoML
    {
        public string docId { get; set; }
        public string firCurp { get; set; }
        public string firSello { get; set; }
        public string firNoCertificadoResponsable { get; set; }
        public string firCadenaOriginal { get; set; }
        public bool firConcentradora { get; set; }
    }
}
