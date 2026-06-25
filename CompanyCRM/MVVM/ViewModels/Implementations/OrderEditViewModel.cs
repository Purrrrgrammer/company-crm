using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.Commands;
using CompanyCRM.MVVM.Models;
using CompanyCRM.Services;

namespace CompanyCRM.MVVM.ViewModels
{
    public class OrderEditViewModel : ViewModelBase, IEditViewModel
    {
        private readonly IOrderService _orderService;
        private readonly Order _order;
        private DateTime _date;
        private decimal _amount;
        private int _employeeId;
        private int _contractorId;
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public DateTime Date
        {
            get => _date;
            set => SetField(ref _date, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => SetField(ref _amount, value);
        }

        public int EmployeeId
        {
            get => _employeeId;
            set => SetField(ref _employeeId, value);
        }

        public int ContractorId
        {
            get => _contractorId;
            set => SetField(ref _contractorId, value);
        }

        public List<Employee> Employees { get; }
        public List<Contractor> Contractors { get; }

        public event EventHandler Saved;
        public event EventHandler Canceled;
        public event EventHandler<string> ErrorOccurred;

        public OrderEditViewModel(
            IOrderService orderService,
            IEmployeeService employeeService,
            IContractorService contractorService,
            Order order = null)
        {
            _orderService = orderService;

            Employees = employeeService.GetAll().ToList();
            Contractors = contractorService.GetAll().ToList();

            if (order == null)
            {
                _order = new Order();
                Date = DateTime.Today;
                EmployeeId = Employees.FirstOrDefault()?.Id ?? 0;
                ContractorId = Contractors.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                _order = order;
                Date = order.Date;
                Amount = order.Amount;
                EmployeeId = order.Employee?.Id ?? 0;
                ContractorId = order.Contractor?.Id ?? 0;
            }
            
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            _order.Date = Date;
            _order.Amount = Amount;
            _order.Employee = Employees.FirstOrDefault(e => e.Id == EmployeeId);
            _order.Contractor = Contractors.FirstOrDefault(c => c.Id == ContractorId);

            var result = _orderService.TrySave(_order);
            if (result.IsSuccessed)
            {
                Saved?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ErrorOccurred?.Invoke(this, result.Message);
            }
        }

        private void Cancel()
        {
            Canceled?.Invoke(this, EventArgs.Empty);
        }
    }
}