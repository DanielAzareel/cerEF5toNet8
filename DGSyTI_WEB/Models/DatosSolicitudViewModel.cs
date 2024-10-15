using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Models
{
    public class DatosSolicitudViewModel
    {
        public string sEstatusSolicitud { get; set; }

        public string sFolio { get; set; }

        public string sTotalDocumentos { get; set; }
        public string FechaSellado { get; set; }
        public string sLoteSEP { get; set; }
        public string FechaEnvioSEP { get; set; }
        public string FechaRespuestaSEP { get; set; }
    }
}