using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessModel.Models
{
    public class CriteriosBusquedaML
    {
        public List<SelectListItem> comboPlanEstudios { set; get; } = new List<SelectListItem>();
        public List<string> listaPlanEstudios { set; get; } = new List<string>();
        public List<SelectListItem> comboTipoDocumento { set; get; } = new List<SelectListItem>();

        public List<string> listaTipoDocumento { set; get; } = new List<string>();
        public List<string> listaAccesoPlanEstudios { set; get; } = new List<string>();

        public List<string> listaCurp { set; get; } = new List<string>();
        public List<string> listaFolios { set; get; } = new List<string>();
         public List<int> listaEstatus { set; get; } = new List<int>();
         
        [RegularExpression("^([a-zA-Z0-9\\/_.-]{1,40})([,]{1}[a-zA-Z0-9\\/_.-]{1,40})*$",
           ErrorMessage = "La longitud del número de control debe ser de 1 a 40 caracteres, para ingresar más de un número de control separar con una coma y sin caracteres especiales.")]
        public String folioControl { get; set; }

        [RegularExpression("^([a-zA-Z0-9]{18})([,]{1}[a-zA-Z0-9]{18})*$",
            ErrorMessage = "La longitud de la CURP debe ser de 18 caracteres, separadas por comas en caso de ser más de una y sin caracteres especiales.")]
        public String curp { get; set; }
        public String estatus { get; set; }

      
        [RegularExpression("^([^|\\\\~<>&']+)$", ErrorMessage = "Estos caracteres |\\~&<>' no son permitidos.")]
        public String nombreFiltro { get; set; }
        public String insId { get; set; }
    } 
}   
