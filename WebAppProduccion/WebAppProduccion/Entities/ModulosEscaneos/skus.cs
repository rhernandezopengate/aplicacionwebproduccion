//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppProduccion.Entities.ModulosEscaneos
{
    using System;
    using System.Collections.Generic;
    
    public partial class skus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public skus()
        {
            this.li_detcajasskus = new HashSet<li_detcajasskus>();
            this.skusbenavides = new HashSet<skusbenavides>();
            this.skusinventarios = new HashSet<skusinventarios>();
            this.wl_detcajassku = new HashSet<wl_detcajassku>();
            this.kitskus = new HashSet<kitskus>();
        }
    
        public int id { get; set; }
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public string codigobarras { get; set; }
        public int uom_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<li_detcajasskus> li_detcajasskus { get; set; }
        public virtual uom uom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<skusbenavides> skusbenavides { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<skusinventarios> skusinventarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wl_detcajassku> wl_detcajassku { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kitskus> kitskus { get; set; }
    }
}
