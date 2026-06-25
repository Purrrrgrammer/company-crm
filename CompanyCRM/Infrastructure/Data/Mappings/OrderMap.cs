using CompanyCRM.MVVM.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace CompanyCRM.Data.Mappings
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Table("orders");

            Id(x => x.Id, m =>
            {
                m.Column("id");
                m.Generator(Generators.Identity);
            });

            Property(x => x.Date, m =>
            {
                m.Column("date");
                m.NotNullable(true);
            });

            Property(x => x.Amount, m =>
            {
                m.Column("amount");
                m.NotNullable(true);
            });

            ManyToOne(x => x.Employee, m =>
            {
                m.Column("employee_id");
                m.NotNullable(true);
                m.ForeignKey("FK_Orders_Employees");
            });

            ManyToOne(x => x.Contractor, m =>
            {
                m.Column("contractor_id");
                m.NotNullable(true);
                m.ForeignKey("FK_Orders_Contractors");
            });
        }
    }
}