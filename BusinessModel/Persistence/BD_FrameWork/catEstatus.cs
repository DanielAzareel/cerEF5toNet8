//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessModel.Persistence.BD_FrameWork
{
    using System;
    using System.Collections.Generic;
    
    public partial class catEstatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public catEstatus()
        {
            this.catTipoIncorp = new HashSet<catTipoIncorp>();
        }
    
        public int idEstatus { get; set; }
        public string estNombre { get; set; }
        public string estModulo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<catTipoIncorp> catTipoIncorp { get; set; }
    }
}
