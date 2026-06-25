using System.Collections.Generic;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Data.Repositories
{
    public interface IContractorRepository
    {
        Contractor GetById(int id);
        IList<Contractor> GetAll();
        void Save(Contractor contractor);
        void Delete(int id);
    }
}