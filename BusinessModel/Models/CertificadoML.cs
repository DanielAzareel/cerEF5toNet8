using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessModel.Persistence.CertificadosElectronicosMS;

namespace BusinessModel.Models
{
    public class CertificadoML:cerDocumento
    {
        public List<cerDocumento> cerDocumentos = new List<cerDocumento>();
        public cerDocumento documento = new cerDocumento();
        public List<cerCatFirmante> listadoFirmantes = new List<cerCatFirmante>();
        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico no válido.")]
        public new string docCorreo { get; set; }
        [Required(ErrorMessage = "El campo es requerido.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La captura debe ser entre {2} y {1} caracteres.")]
        [RegularExpression("^([a-zA-ZZáéíóúÁÉÍÓÚÑñüÜ0-9 \\.,_-]+)$", ErrorMessage = "Se ingresaron caracteres no permitidos.")]
        public string observaciones { get; set; }
        public IEnumerable<SelectListItem> comboMotivosCancelacion { get; set; }
        public IEnumerable<SelectListItem> comboPlantillas { get; set; }

        [Required]
        public string idMotivoCancelacion { get; set; }
        [Required]
        public string idPlantilla { get; set; }
        public int totalRegistros { get; set; }

        public class criteriosBusquedaCertificadosML
        {
            public List<String> listInstitucionBusqueda { get; set; }
            public List<String> listInstitucionAcceso { get; set; }
            public List<String> listCarrerasBusqueda { get; set; }
            public List<String> listPlanAcceso { get; set; }

            [RegularExpression("^([a-zA-Z0-9\\/_.-]{6,40})([,]{1}[a-zA-Z0-9\\/_.-]{6,40})*$",
                ErrorMessage = "La longitud del folio debe ser de 6 a 40 caracteres, separados por comas en caso de ser más de uno y sin caracteres especiales.")]
            public String folioControl { get; set; }

            [RegularExpression("^([a-zA-Z0-9]{18})([,]{1}[a-zA-Z0-9]{18})*$",
                ErrorMessage = "La longitud de la CURP debe ser de 18 caracteres, separadas por comas en caso de ser más de una y sin caracteres especiales.")]
            public String curp { get; set; }
            public String estatus { get; set; }
            public String idSolicitud { get; set; }
            public List<SelectListItem> comboEstatus { get; set; }
        }
        }
}
