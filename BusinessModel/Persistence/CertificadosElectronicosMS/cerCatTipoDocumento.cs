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
    
    public partial class cerCatTipoDocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cerCatTipoDocumento()
        {
            this.cerDocumento = new HashSet<cerDocumento>();
            this.cerParametroValor = new HashSet<cerParametroValor>();
            this.cerRelPlanPeriodoAreaConocimientoMateria = new HashSet<cerRelPlanPeriodoAreaConocimientoMateria>();
        }
    
        public string docTipoId { get; set; }
        public string docDescripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cerDocumento> cerDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cerParametroValor> cerParametroValor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cerRelPlanPeriodoAreaConocimientoMateria> cerRelPlanPeriodoAreaConocimientoMateria { get; set; }
    }
}
