using System;
using System.Collections.Generic;
using System.Linq;
using CompanyCRM.Common;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IContractorRepository _contractorRepository;
        
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IOrderRepository orderRepository,
            IContractorRepository contractorRepository)
        {
            _employeeRepository = employeeRepository;
            _orderRepository = orderRepository;
            _contractorRepository = contractorRepository;
        }
        
        public Employee GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        public IList<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public Result TrySave(Employee employee)
        {
            if (employee == null)
                return Result.CreateFailed("Сотрудник не может быть null");

            if (string.IsNullOrWhiteSpace(employee.FullName))
                return Result.CreateFailed("ФИО обязательно для заполнения");

            if (employee.BirthDate > DateTime.Today)
                return Result.CreateFailed("Дата рождения не может быть в будущем");

            _employeeRepository.Save(employee);
            return Result.CreateSuccessed();
        }

        public Result TryDelete(int id)
        {
            var employee = _employeeRepository.GetById(id);
            
            if (employee == null)
                return Result.CreateFailed("Сотрудник не найден");

            
            var hasOrders = _orderRepository.GetAll().Any(o => o.Employee.Id == id);
            var hasContractors = _contractorRepository.GetAll().Any(c => c.Curator.Id == id);
            
            if(hasOrders || hasContractors)
            {
                string message = "Нельзя удалить сотрудника, так как ";
                
                if (hasOrders && hasContractors)
                    message += "у него есть заказы и он является куратором контрагентов.";
                else if (hasOrders)
                    message += "у него есть заказы.";
                else 
                    message += "он является куратором контрагентов.";
                
                return Result.CreateFailed(message);
            }
            
            _employeeRepository.Delete(id);

            return Result.CreateSuccessed();
        }
        
    }
}