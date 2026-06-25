using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;
namespace CompanyCRM.Data
{
    public class NHibernateHelper : INHibernateHelper
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateHelper()
        {
            _sessionFactory = CreateSessionFactory();
        }
        
        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
        
        private static ISessionFactory CreateSessionFactory()
        {
            var connectionString = 
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.Driver<MySqlDataDriver>();
                db.Dialect<MySQL5Dialect>();
                db.LogSqlInConsole = true;
                db.Timeout = 30;
            });

            var mapper = new ModelMapper();
            var mappingTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(ClassMapping<>));
            foreach (var type in mappingTypes)
            {
                mapper.AddMapping(type);
            }
            
            cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            
            new SchemaUpdate(cfg).Execute(false, true);

            return cfg.BuildSessionFactory();
        }
    }
}