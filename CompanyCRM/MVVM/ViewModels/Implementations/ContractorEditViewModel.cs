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
    public class ContractorEditViewModel : ViewModelBase, IEditViewModel
    {
        private readonly IContractorService _contractorService;
        private readonly Contractor _contractor;
        private string _name;
        private string _inn;
        private int _curatorId;
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string Inn
        {
            get => _inn;
            set => SetField(ref _inn, value);
        }

        public int CuratorId
        {
            get => _curatorId;
            set => SetField(ref _curatorId, value);
        }

        public List<Employee> Employees { get; }

        public event EventHandler Saved;
        public event EventHandler Canceled;
        public event EventHandler<string> ErrorOccurred;

        public ContractorEditViewModel(
            IContractorService contractorService,
            IEmployeeService employeeService,
            Contractor contractor = null)
        {
            _contractorService = contractorService;

            Employees = employeeService.GetAll().ToList();

            if (contractor == null)
            {
                _contractor = new Contractor();
                CuratorId = Employees.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                _contractor = contractor;
                Name = contractor.Name;
                Inn = contractor.Inn;
                CuratorId = contractor.Curator?.Id ?? 0;
            }
            
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            _contractor.Name = Name;
            _contractor.Inn = Inn;
            _contractor.Curator = Employees.FirstOrDefault(e => e.Id == CuratorId);

            var result = _contractorService.TrySave(_contractor);
            
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