using System;

namespace CompanyCRM.MVVM.Models
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual Position Position { get; set; }
        public virtual DateTime BirthDate { get; set; }
    }
}