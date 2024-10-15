using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DGSyTI_WEB.Models
{
    public class MonitoreoSEPViewModel
    {
        public List<ViewSolicitud> listCerSolicitud { get; set; } = new List<ViewSolicitud>();
        public int totalRegistrosSolicitudes { get; set; }

        public List<ViewCerDocumento> listCerDocumento { get; set; } = new List<ViewCerDocumento>();
        public int totalRegistrosCertificados { get; set; }

        public List<cerCatPlantilla> listCerCatPlantillas { get; set; } = new List<cerCatPlantilla>();
        public int iTotalRegistrosPlantillas { get; set; }

        public string sIdDocumento { get; set; }
        public string sIdPlantilla { get; set; }
        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico no válido.")]
        public string sDocCorreo { get; set; }

        [Required(ErrorMessage = "El campo es requerido.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La captura debe ser entre {2} y {1} caracteres.")]
        [RegularExpression("^([a-zA-ZZáéíóúÁÉÍÓÚÑñüÜ0-9 \\.,_-]+)$", ErrorMessage = "Se ingresaron caracteres no permitidos.")]
        public string sObservaciones { get; set; }
    }
}