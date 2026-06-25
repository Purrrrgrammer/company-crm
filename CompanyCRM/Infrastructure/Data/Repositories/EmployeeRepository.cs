using System.Collections.Generic;
using System.Linq;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Data.Repositories
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly INHibernateHelper _helper;

        public EmployeeRepository(INHibernateHelper helper)
        {
            _helper = helper;
        }
        
        public Employee GetById(int id)
        {
            using (var session = _helper.OpenSession())
            {
                return session.Get<Employee>(id);
            }
        }

        public IList<Employee> GetAll()
        {
            using (var session = _helper.OpenSession())
            {
                return session.Query<Employee>().ToList();
            }
        }

        public void Save(Employee employee)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(employee);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var employee = session.Get<Employee>(id);
                if (employee != null)
                {
                    session.Delete(employee);
                    transaction.Commit();
                }
            }
        }
    }
}