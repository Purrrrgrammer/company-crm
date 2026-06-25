using System.Collections.Generic;
using CompanyCRM.Common;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public interface IEmployeeService
    {
        Employee GetById(int id);
        IList<Employee> GetAll();
        Result TrySave(Employee employee);
        Result TryDelete(int id);
    }
}