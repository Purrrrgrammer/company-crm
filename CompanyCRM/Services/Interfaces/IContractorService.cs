using System.Collections.Generic;
using CompanyCRM.Common;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public interface IContractorService
    {
        Contractor GetById(int id);
        IList<Contractor> GetAll();
        Result TrySave(Contractor contractor);
        Result TryDelete(int id);
    }
}