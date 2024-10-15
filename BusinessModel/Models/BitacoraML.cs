using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class BitacoraML
    {
        public string bitId { get; set; }
        public System.DateTime bitFecha { get; set; }
        public string bitUsuario { get; set; }
        public string bitDescripcion { get; set; }
        public bool bitExitoso { get; set; }
        public string accId { get; set; }
    }
}
