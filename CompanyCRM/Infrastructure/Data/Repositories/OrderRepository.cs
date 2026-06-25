using System.Collections.Generic;
using System.Linq;
using CompanyCRM.MVVM.Models;
using NHibernate;

namespace CompanyCRM.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly INHibernateHelper _helper;

        public OrderRepository(INHibernateHelper helper)
        {
            _helper = helper;
        }

        public Order GetById(int id)
        {
            using (var session = _helper.OpenSession())
            {
                return session.Get<Order>(id);
            }
        }

        public IList<Order> GetAll()
        {
            using (var session = _helper.OpenSession())
            {
                return session.QueryOver<Order>()
                    .Fetch(SelectMode.Fetch, x => x.Employee)
                    .Fetch(SelectMode.Fetch, x => x.Contractor)
                    .List();
            }
        }

        public void Save(Order order)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(order);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var order = session.Get<Order>(id);
                if (order != null)
                {
                    session.Delete(order);
                    transaction.Commit();
                }
            }
        }
    }
}