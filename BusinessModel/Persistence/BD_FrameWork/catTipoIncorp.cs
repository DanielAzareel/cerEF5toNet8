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
    
    public partial class catTipoIncorp
    {
        public int idTipoIncorporacion { get; set; }
        public string tincNombre { get; set; }
        public string tIncAbreviatura { get; set; }
        public bool tincFechaImpresion { get; set; }
        public Nullable<int> idEstatus { get; set; }
    
        public virtual catEstatus catEstatus { get; set; }
    }
}
