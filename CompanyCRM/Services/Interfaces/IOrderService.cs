using System.Collections.Generic;
using CompanyCRM.Common;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public interface IOrderService
    {
        Order GetById(int id);
        IList<Order> GetAll();
        Result TrySave(Order order);
        Result TryDelete(int id);
    }
}