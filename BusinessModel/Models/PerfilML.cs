using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class PerfilML
    {
        public string rolId { get; set; }
        public string rolDescripcion { get; set; }
        public bool seleccionado { get; set; }
        public string sConFolio { get; set; }

        public string insId { get; set; }
        public string insDescripción { get; set; }
        public string sConId { get; set; }
        public List<(string concentradora, string institucion, string carrera)> accesos { get; set; } = new List<(string concentradora, string institucion, string carrera)>();

        public List<(string institucion, string planes)> nivelAcceso { get; set; } = new List<(string institucion, string planes)>();

    }
}
