using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace BusinessModel.Models
{
    public class FirmanteML : cerCatFirmante
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio.")]
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string contrasenia { set; get; }
        [Required(ErrorMessage = "El campo '{0}' es obligatorio.")]
        public HttpPostedFileBase archivoKey { set; get; }
    }
}
