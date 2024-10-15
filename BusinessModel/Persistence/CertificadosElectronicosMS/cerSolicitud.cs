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
    
    public partial class cerSolicitud
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cerSolicitud()
        {
            this.cerDocumento = new HashSet<cerDocumento>();
        }
    
        public string solId { get; set; }
        public Nullable<System.DateTime> solFechaSellado { get; set; }
        public Nullable<System.DateTime> solFechaEnvio { get; set; }
        public Nullable<System.DateTime> solFechaResultado { get; set; }
        public Nullable<System.DateTime> solFechaRetorno { get; set; }
        public byte[] solArchivoEnvio { get; set; }
        public byte[] solArchivoResultado { get; set; }
        public byte[] solArchivoRetorno { get; set; }
        public string solFolioLoteSEP { get; set; }
        public string solMensajeEnvio { get; set; }
        public string solMensajeResultado { get; set; }
        public string solMensajeRetorno { get; set; }
        public Nullable<int> estSolicitudId { get; set; }
        public string insId { get; set; }
        public Nullable<bool> solEnProceso { get; set; }
    
        public virtual cerCatEstatusSolicitud cerCatEstatusSolicitud { get; set; }
        public virtual cerConfiguracionInstitucion cerConfiguracionInstitucion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cerDocumento> cerDocumento { get; set; }
    }
}
