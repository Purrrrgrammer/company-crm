using System.Collections.Generic;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(int id);
        IList<Employee> GetAll();
        void Save(Employee employee);
        void Delete(int id);
    }
}