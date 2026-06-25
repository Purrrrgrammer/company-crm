using System;
using System.Collections.Generic;
using System.Windows.Input;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.Commands;
using CompanyCRM.MVVM.Models;
using CompanyCRM.Services;

namespace CompanyCRM.MVVM.ViewModels
{
    public class EmployeeEditViewModel : ViewModelBase, IEditViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly Employee _employee;
        private string _fullName;
        private Position _position;
        private DateTime _birthDate;

        public string FullName
        {
            get => _fullName;
            set => SetField(ref _fullName, value);
        }

        public Position Position
        {
            get => _position;
            set => SetField(ref _position, value);
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetField(ref _birthDate, value);
        }
        
        public Dictionary<Position, string> Positions { get; } = new Dictionary<Position, string>
        {
            { Position.Director, "Руководитель" },
            { Position.Employee, "Работник" }
        };

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler Saved;
        public event EventHandler Canceled;
        public event EventHandler<string> ErrorOccurred;

        public EmployeeEditViewModel(IEmployeeService employeeService, Employee employee = null)
        {
            _employeeService = employeeService;

            if (employee == null)
            {
                _employee = new Employee();
                BirthDate = DateTime.Today;
            }
            else
            {
                _employee = employee;
                FullName = employee.FullName;
                Position = employee.Position;
                BirthDate = employee.BirthDate;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            _employee.FullName = FullName;
            _employee.Position = Position;
            _employee.BirthDate = BirthDate;

            var result = _employeeService.TrySave(_employee);
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