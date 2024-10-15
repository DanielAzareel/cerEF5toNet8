using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteIEMS.Models
{
    public class autenticacionType
    {
        public string password { get; set; }
        public string usuario { get; set; }
    }
    public class cargaRequest
    {
        public byte[] archivoBase64 { get; set; }
        public string nombreArchivo { get; set; }
        public autenticacionType autenticacion { get; set; }
    }
    public class cargaResponse
    {
        public string numeroLote { get; set; }
        public string mensaje { get; set; }
    }
    public class consultaRequest
    { 
        public string numeroLote { get; set; }
        public autenticacionType autenticacion { get; set; }
    }
    public class consultaResponse
    {
        public string numeroLote { get; set; }
        public string estatusLote { get; set; }
        public string mensaje { get; set;}
        public string excelBase64 { get; set; }
    }
    public class descargaRequest
    {
        public string numeroLote { get; set; }
        public autenticacionType autenticacion { get; set; }
    }
    public class descargaResponse
    {
        public string numeroLote { get; set; }
        public string certificadosBase64 { get; set; }
        public string mensaje { get; set;}
    }
    public class cancelarRequest
    {
        public string folioCertificado { get; set; }
        public autenticacionType autenticacion { get; set; }
    }
    public class cancelarResponse
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
    }
    internal class ClienteModel
    {
    }
}
