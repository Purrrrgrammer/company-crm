using CompanyCRM.MVVM.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace CompanyCRM.Data.Mappings
{
    public class ContractorMap : ClassMapping<Contractor>
    {
        public ContractorMap()
        {
            Table("contractors");

            Id(x => x.Id, m =>
            {
                m.Column("id");
                m.Generator(Generators.Identity);
            });

            Property(x => x.Name, m =>
            {
                m.Column("name");
                m.Length(200);
                m.NotNullable(true);
            });

            Property(x => x.Inn, m =>
            {
                m.Column("inn");
                m.Length(12);
                m.NotNullable(true);
            });

            ManyToOne(x => x.Curator, m =>
            {
                m.Column("curator_id");
                m.NotNullable(true);
                m.ForeignKey("FK_Contractors_Employees");
            });
        }
    }
}