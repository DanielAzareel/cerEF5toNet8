using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGSyTI_WEB.Models
{
    public class CriteriosBusquedaMonitoreoViewModel
    {
        [RegularExpression("^([a-zA-Z0-9\\/_.-]{1,40})([,]{1}[a-zA-Z0-9\\/_.-]{1,40})*$",
        ErrorMessage = "La longitud del número de control debe ser de 6 a 40 caracteres, separados por comas en caso de ser más de uno y sin caracteres especiales.")]
        public string sFolioControl { get; set; }

        [RegularExpression("^([a-zA-Z0-9]{18})([,]{1}[a-zA-Z0-9]{18})*$",
                ErrorMessage = "La longitud de la CURP debe ser de 18 caracteres, separadas por comas en caso de ser más de una y sin caracteres especiales.")]
        public string sCURP { get; set; }

        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public string sNombre { get; set; }
        public string sSolId { get; set; }
        public string sIdEstatus { get; set; }
        public string sIdPlan { get; set; }
        public string sIdTipoCertificado { get; set; }
        public string sIdDocumento { get; set; }
        public List<SelectListItem> listSLEstatus { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> listSLPlan { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> listSLTipoCertificado { get; set; } = new List<SelectListItem>();

        public List<string> lstFolio = new List<string>();
        public List<string> lstCURP = new List<string>();
        public List<string> lstPlan = new List<string>();
        public List<string> lstTipoCertificado = new List<string>();
        public List<string> lstEstatus = new List<string>();
    }
}