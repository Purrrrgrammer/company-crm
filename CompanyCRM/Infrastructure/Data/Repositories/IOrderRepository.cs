using System.Collections.Generic;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Data.Repositories
{
    public interface IOrderRepository
    {
        Order GetById(int id);
        IList<Order> GetAll();
        void Save(Order order);
        void Delete(int id);
    }
}