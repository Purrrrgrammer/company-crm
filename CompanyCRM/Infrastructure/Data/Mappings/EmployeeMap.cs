using CompanyCRM.MVVM.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
namespace CompanyCRM.Data.Mappings
{
    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            Table("employees");
            Id(x => x.Id, m =>
            {
                m.Column("id");
                m.Generator(Generators.Identity);
            });
            
            Property(x => x.FullName, m =>
            {
                m.Column("full_name");
                m.Length(100); 
                m.NotNullable(true);
            }); 
            
            Property(x => x.Position, m =>
            {
                m.Column("position");
                m.NotNullable(true);
            }); 
            
            Property(x => x.BirthDate, m =>
            {
                m.Column("birth_date");
                m.NotNullable(true);
            }); 
        }
    }
}