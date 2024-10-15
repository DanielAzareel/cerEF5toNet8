using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BusinessModel.Models
{
    public class ObjetosCargaCertificadosML
    {
        public List<CerDocumento_TemML> listadoDocumentosAcargar = new List<CerDocumento_TemML>();
        public List<MateriasArchivoCarga> listadoMateriasCargar = new List<MateriasArchivoCarga>();
    }
    public class CargaCertificadosML
    {
        public string tipoDocumento { get; set; }
        public List<SelectListItem> comboTipoDocumento { set; get; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Es necesario ingresar un archivo.")]
        [RegularExpression("(.*?)\\.(xlsx)$", ErrorMessage = "Solo se aceptan archivos con extensión .xlsx.")]
        public HttpPostedFileBase certificadoIntegracion { get; set; }
    }
    public class FiltrosConsultaCarga
    {
        public bool registrosConError { get; set; }
        public bool registrosConObservavion { get; set; }
        public string idDeCarga { get; set; }
        public bool registrosCorrectos { get; set; }
        public string tipoDocumento { get; set; }
    }
}
