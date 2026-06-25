using System.Collections.Generic;
using System.Linq;
using CompanyCRM.MVVM.Models;
using NHibernate;

namespace CompanyCRM.Data.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly INHibernateHelper _helper;

        public ContractorRepository(INHibernateHelper helper)
        {
            _helper = helper;
        }

        public Contractor GetById(int id)
        {
            using (var session = _helper.OpenSession())
            {
                return session.Get<Contractor>(id);
            }
        }
        
        public IList<Contractor> GetAll()
        {
            using (var session = _helper.OpenSession())
            {
                return session.QueryOver<Contractor>()
                    .Fetch(SelectMode.Fetch, x => x.Curator)
                    .List();
            }
        }

        public void Save(Contractor contractor)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(contractor);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var session = _helper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var contractor = session.Get<Contractor>(id);
                if (contractor != null)
                {
                    session.Delete(contractor);
                    transaction.Commit();
                }
            }
        }
    }
}