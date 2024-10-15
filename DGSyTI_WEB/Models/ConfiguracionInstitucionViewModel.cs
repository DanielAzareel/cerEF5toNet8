using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGSyTI_WEB.Models
{
    public class ConfiguracionInstitucionViewModel : InstitucionML
    {
        public string insId { get; set; }
        public string InsNombre { get; set; }
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La captura debe ser entre 3 y 100 caracteres.")]
        public string insUsuarioWS { get; set; }

        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La captura debe ser entre 3 y 100 caracteres.")]
        public string insContrasenaWS { get; set; }
        [Required(ErrorMessage = "El campo 'token de seguirdad' es requerido. Presione el botón 'Generar nuevo token' para generarlo.")]
        public string insTokenSeguridad { get; set; }
        public bool? insNotificacionProfesionista { get; set; }
        public string insTokenSeguridadDescarga { get; set; }
        public bool? insAutenticarDescarga { get; set; }
        public bool? insCertificadosPublicos { get; set; }

        [StringLength(100, MinimumLength = 10, ErrorMessage = "La captura debe ser entre 10 y 100 caracteres.")]
        [Url(ErrorMessage = "Ingresar una url válida.")]
        public string insBotonMenu { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = "La captura debe ser entre 10 y 100 caracteres.")]
        [Url(ErrorMessage = "Ingresar una url válida.")]
        public string insBotonSalir { get; set; }


        public List<cerParametroValor> ListCerParametroValor { get; set; } = new List<cerParametroValor>();
        public int iTotalRegistrosEtiquetas { get; set; }

        public List<SelectListItem> listSLTipoDocumento { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> listSLPlan { get; set; } = new List<SelectListItem>();
        public List<ViewPlantillas> listPlantillas { get; set; } = new List<ViewPlantillas>();
        public int iTotalRegistrosPlantillas { get; set; }

        public string sIdTipoDocumento { get; set; }
        public string sIdPlan { get; set; }
        public string sIdInstitucion { get; set; }

        public PlantillaCertificadoML oPlantilla = new PlantillaCertificadoML();

        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "La captura debe ser entre 1 y 255 caracteres.")]
        [Required(ErrorMessage = "El campo 'Valor' es requerido.")]
        public string parValor { get; set; }

        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "La captura debe ser entre 1 y 255 caracteres.")]
        public string parValorNoRequerido { get; set; }

        public List<cerCatFirmante> listCatFirmantes { get; set; } = new List<cerCatFirmante>();
        public FirmanteCertificadoML oCatFirmante { get; set; } = new FirmanteCertificadoML();
        public int iTotalRegistrosFirmantes { get; set; }
    }
}