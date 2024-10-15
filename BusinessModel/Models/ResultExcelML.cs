using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class ResultExcelML
    {
        public string archivo { set; get; }
        public int estatus { set; get; }
        public string mensaje { set; get; }
        public string folioControl { set; get; }
        public string docId { get; set; }
        public int? estatusdocumento { get; set; }
        public string observacionId { get; set; }
        public DateTime dtFecha { get; set; }
        public string obsUsuario { get; set; }
        public string accId { get; set; }

    }

    public class ResultExcelEstatusML
    {
        public int estatus { set; get; }
        public string mensaje { set; get; }
        public string docId { get; set; }
    }

    public class ResultXMLSelladoML
    {
        public string docId { get; set; }
        public byte[] bXML { set; get; }
        public string sFolioDigital { set; get; }
        public string SNoCertificadoSep { set; get; }
        public string sSelloDec { set; get; }
        public string sSelloSep { set; get; }
        public string sVersion { set; get; }
        public DateTime dFechaSsep { get; set; }
    }
}
