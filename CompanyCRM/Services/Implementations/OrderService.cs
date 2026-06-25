using System;
using System.Collections.Generic;
using System.Linq;
using CompanyCRM.Common;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public IList<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Result TrySave(Order order)
        {
            if (order == null)
                return Result.CreateFailed("Заказ не может быть null");

            if (order.Amount <= 0)
                return Result.CreateFailed("Сумма заказа должна быть больше 0");

            if (order.Employee == null)
                return Result.CreateFailed("Не выбран сотрудник");

            if (order.Contractor == null)
                return Result.CreateFailed("Не выбран контрагент");

            _orderRepository.Save(order);
            return Result.CreateSuccessed();
        }

        public Result TryDelete(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
                return Result.CreateFailed("Заказ не найден");

            _orderRepository.Delete(id);
            return Result.CreateSuccessed();
        }
    }
}