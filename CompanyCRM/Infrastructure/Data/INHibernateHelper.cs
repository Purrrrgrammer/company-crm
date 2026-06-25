using NHibernate;

namespace CompanyCRM.Data
{
    public interface INHibernateHelper
    {
        ISession OpenSession();
    }
}