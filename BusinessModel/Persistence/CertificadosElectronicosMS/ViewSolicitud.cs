//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessModel.Persistence.CertificadosElectronicosMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewSolicitud
    {
        public string solId { get; set; }
        public string solFolioLoteSEP { get; set; }
        public Nullable<System.DateTime> solFechaSellado { get; set; }
        public Nullable<int> numerocertificados { get; set; }
        public Nullable<int> estSolicitudId { get; set; }
        public string EstatusDescripcion { get; set; }
        public Nullable<System.DateTime> solFechaEnvio { get; set; }
        public Nullable<System.DateTime> solFechaResultado { get; set; }
        public string insId { get; set; }
        public string solMensajeEnvio { get; set; }
        public string solMensajeResultado { get; set; }
        public string solMensajeRetorno { get; set; }
        public Nullable<bool> solEnProceso { get; set; }
    }
}
