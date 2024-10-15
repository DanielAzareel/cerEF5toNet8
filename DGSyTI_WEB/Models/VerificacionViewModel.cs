using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Models
{
    public class VerificacionViewModel
    {
        public Dec datosCertificado { get; set; } = new Dec();
        public int? idEstatusDocumento {get;set;}
        public String estatusDocumento {get;set;}
        public cerDocumento CerDocumento { set; get; } = new cerDocumento();
        public List<cerUACDocumento> listUACDocumento { set; get; } = new List<cerUACDocumento>();
    }
}