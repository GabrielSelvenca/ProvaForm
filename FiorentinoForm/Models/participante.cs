//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiorentinoForm.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class participante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public participante()
        {
            this.participantes_evento = new HashSet<participantes_evento>();
            this.vendas = new HashSet<vendas>();
        }
    
        public int id { get; set; }
        public string nome { get; set; }
        public Nullable<int> idade { get; set; }
        public Nullable<int> cidadeId { get; set; }
        public string genero { get; set; }
    
        public virtual cidade cidade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participantes_evento> participantes_evento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vendas> vendas { get; set; }
    }
}
