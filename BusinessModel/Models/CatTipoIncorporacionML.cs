using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Persistence.BD_FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessModel.Models
{
    public class CatTipoIncorporacionML:catTipoIncorp
    {
        public List<catTipoIncorp> listaTipoIncorporacion = new List<catTipoIncorp>();
        public catTipoIncorp tipoIncorporacion = new catTipoIncorp();

        public catTipoIncorp filtros = new catTipoIncorp();
        public int totalRegistros { get; set; }

        //Acciones
        public bool permiteEditar = false;
        public bool permiteActivar = false;
        public bool permiteAgregar = false;
      

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo '{0}' es requerido."), StringLength(50, ErrorMessage = "La longitud máxima del campo '{0}' es de 50 caracteres.")]
        public new string tincNombre { get; set; }

        [DisplayName("Clave")]
        [Required(ErrorMessage = "El campo '{0}' es requerido."), StringLength(50, ErrorMessage = "La longitud máxima del campo '{0}' es de 50 caracteres.")]
        public new string tIncAbreviatura { get; set; }

        [DisplayName("¿Incluye fecha en certificado?")]        
        public new bool tincFechaImpresion { get; set; }

    }
}
