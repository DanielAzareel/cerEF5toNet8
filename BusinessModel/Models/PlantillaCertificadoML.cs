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
    public class PlantillaCertificadoML
    {
        [Required(ErrorMessage = "El campo 'Archivo' es obligatorio.")]
        [RegularExpression("(.*?)\\.(docx|zip)$", ErrorMessage = "Solo se aceptan archivos con extensión .docx y .zip.")]
        public HttpPostedFileBase archivoPlantilla { get; set; }

        [RegularExpression("(.*?)\\.(docx|zip)$", ErrorMessage = "Solo se aceptan archivos con extensión .docx y .zip.")]
        public HttpPostedFileBase archivoPlantillaEditar { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 100 caracteres.")]
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        [Remote("VerficarNombrePlantilla", "ConfiguracionInstitucion", AdditionalFields = "plaNombre,planId", ErrorMessage = "El nombre de la plantilla ya existe, ingresar uno diferente.")]
        public string plaNombre { get; set; }

        [RegularExpression(@"\d{0,10}", ErrorMessage = "Es necesario capturar un número válido.")]
        [Range(1, 99, ErrorMessage = "Debe ser un valor entre 1 y 99.")]
        [Required(ErrorMessage = "El campo 'Número de archivos' es obligatorio.")]
        public int? plaNumHojas { get; set; }
        [Required(ErrorMessage = "El campo 'Tipo documento' es obligatorio.")]
        public string docTipoId { get; set; }
        [Required(ErrorMessage = "El campo 'Plan' es obligatorio.")]
        public string idPlan { get; set; }
        public string insId { get; set; }
        public string planId { get; set; }
        public string sIdPlantillaAnterior { get; set; }
        public string sFileNombrePlantilla { get; set; }
    }

    public class FirmanteCertificadoML
    {

        [RegularExpression("(.*?)\\.(cer|key)$", ErrorMessage = "Solo se aceptan archivos con extensión .cer y key.")]
        [Required(ErrorMessage = "El campo 'Archivo' es obligatorio.")]
        public HttpPostedFileBase firArchivoCertificado { get; set; }
        [RegularExpression("(.*?)\\.(cer|key)$", ErrorMessage = "Solo se aceptan archivos con extensión .cer y key.")]
        public HttpPostedFileBase firArchivoCertificadoEditar { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 100 caracteres.")]
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string firNombre { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 100 caracteres.")]
        [Required(ErrorMessage = "El campo 'Primer apellido' es obligatorio.")]
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string firPrimerApellido { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 100 caracteres.")]
       
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string firSegundoApellido { get; set; }

        [StringLength(14, ErrorMessage = "La captura debe ser de 14 caracteres.")]
        [Required(ErrorMessage = "El campo 'CURP' es obligatorio.")]
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string firCurp { get; set; }

        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico no válido.")]
        public string firCorreo { get; set; }

        [StringLength(4, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 9999.")]
        [Required(ErrorMessage = "El campo 'Identificador de cargo' es obligatorio.")]
        [RegularExpression(@"\d{0,10}", ErrorMessage = "Es necesario capturar un número válido.")]
        public string firIdCargo { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 100 caracteres.")]       
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string firCargo { get; set; }

        public bool firPredeterminado { get; set; }

        public string insId { get; set; }

        public string firId { get; set; }

        public DateTime firVigenciaCertificado { get; set; }
    }
}
