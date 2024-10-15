using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BusinessModel.Models
{
    public class MonitoreoSEPML
    {
        public List<ViewSolicitud> listViewSolicitud { get; set; } = new List<ViewSolicitud>();
        public int iTotalRegistrosSolicitudes { get; set; }
        public List<ViewCerDocumento> listCerDocumento { get; set; } = new List<ViewCerDocumento>();
        public int iTotalRegistrosCertificados { get; set; }

        public List<cerCatPlantilla> listCerCatPlantillas { get; set; } = new List<cerCatPlantilla>();
        public int iTotalRegistrosPlantillas { get; set; }
    }
}
