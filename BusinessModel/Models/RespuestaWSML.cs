using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class RespuestaWSML
    {
        public int idEstatus { get; set; }
        public string estatus { get; set; }
        public List<datos> datos { get; set; }
        public string sConcentradora { get; set; }

    }

    public class datos
    {
        public double ID_PROCESS { get; set; }
        public string ACC_NOMBRE { get; set; }
        public double? ACC_ID_ACCION_GRUPO { get; set; }
        public string ACC_DESCRIPCION { get; set; }
        public string ACC_URL { get; set; }
        public string ACC_ORDEN { get; set; }

    }
}
