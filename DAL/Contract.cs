//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contract
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contract()
        {
            this.InsuranceCase = new HashSet<InsuranceCase>();
        }
    
        public int ContractID { get; set; }
        public int ClientID { get; set; }
        public Nullable<int> InsuranceAgentID { get; set; }
        public decimal Cost { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public int ProgramID { get; set; }
        public Nullable<bool> signed { get; set; }
        public Nullable<bool> ready { get; set; }
        public string Comment { get; set; }
        public int Number { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual InsuranceAgent InsuranceAgent { get; set; }
        public virtual InsuranceProgram InsuranceProgram { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InsuranceCase> InsuranceCase { get; set; }
        public virtual Property Property { get; set; }
    }
}
