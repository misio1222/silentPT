//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCIdentityConfirm
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.companyEmail = new HashSet<companyEmail>();
        }
    
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Wojewodztwo { get; set; }
        public byte[] Logo { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        public Nullable<int> NIP { get; set; }
        public Nullable<int> Regon { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<companyEmail> companyEmail { get; set; }
    }
}