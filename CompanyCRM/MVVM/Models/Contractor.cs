namespace CompanyCRM.MVVM.Models
{
    public class Contractor
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Inn { get; set; }
        public virtual Employee Curator { get; set; }
    }
}