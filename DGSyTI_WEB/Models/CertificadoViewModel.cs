using BusinessModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Models
{
    public class CertificadoViewModel : CertificadoML
    {
        public bool AccionesSellado { get; set; }
        public bool AccionesConsultaSolicitudes { get; set; }
        public bool AccionesMonitoreo { get; set; }
        public string accionController { get; set; }

    }
}