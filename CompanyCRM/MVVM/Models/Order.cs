using System;

namespace CompanyCRM.MVVM.Models
{
    public class Order
    {
        public virtual int Id { get; set; }
        
        public virtual DateTime Date { get; set; }
        
        public virtual decimal Amount { get; set; }
        
        public virtual Employee Employee { get; set; }
        
        public virtual Contractor Contractor { get; set; }
    }
}