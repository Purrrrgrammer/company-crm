using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CompanyCRM.Data.Repositories;
using CompanyCRM.Extensions;
using CompanyCRM.Infrastructure;
using CompanyCRM.MVVM.Commands;
using CompanyCRM.Services;
using ContractorEditWindow = CompanyCRM.MVVM.Views.ContractorEditWindow;
using EmployeeEditWindow = CompanyCRM.MVVM.Views.EmployeeEditWindow;
using OrderEditWindow = CompanyCRM.MVVM.Views.OrderEditWindow;

namespace CompanyCRM.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IContractorService _contractorService;
        private readonly IOrderService _orderService;
        private readonly Dictionary<string, Action> _addActions;
        private readonly Dictionary<string, Action> _editActions;
        private readonly Dictionary<string, Action> _deleteActions;
        private readonly Dictionary<string, Func<bool>> _canEditDelete;
        
        private ObservableCollection<EmployeeDisplayItem> _employees;
        private ObservableCollection<ContractorDisplayItem> _contractors;
        private ObservableCollection<OrderDisplayItem> _orders;
        private EmployeeDisplayItem _selectedEmployee;
        private object _selectedTab;
        private ContractorDisplayItem _selectedContractor;
        private OrderDisplayItem _selectedOrder;
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        
        public EmployeeDisplayItem SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetField(ref _selectedEmployee, value);
        }
        
        public ContractorDisplayItem SelectedContractor
        {
            get => _selectedContractor;
            set => SetField(ref _selectedContractor, value);
        }

        public OrderDisplayItem SelectedOrder
        {
            get => _selectedOrder;
            set => SetField(ref _selectedOrder, value);
        }
        
        public object SelectedTab
        {
            get => _selectedTab;
            set => SetField(ref _selectedTab, value);
        }
        
        public ObservableCollection<EmployeeDisplayItem> Employees
        {
            get => _employees;
            private set => SetField(ref _employees, value);
        }
        
        public ObservableCollection<ContractorDisplayItem> Contractors
        {
            get => _contractors;
            private set => SetField(ref _contractors, value);
        }

        public ObservableCollection<OrderDisplayItem> Orders
        {
            get => _orders;
            private set => SetField(ref _orders, value);
        }

        public MainViewModel(
            IEmployeeService employeeService,
            IContractorService contractorService,
            IOrderService orderService)
        {
            _employeeService = employeeService;
            _contractorService = contractorService;
            _orderService = orderService;

            _employees = new ObservableCollection<EmployeeDisplayItem>();
            _contractors = new ObservableCollection<ContractorDisplayItem>();
            _orders = new ObservableCollection<OrderDisplayItem>();
            
            _addActions = new Dictionary<string, Action>
            {
                { "Employees", AddEmployee },
                { "Contractors", AddContractor },
                { "Orders", AddOrder }
            };

            _editActions = new Dictionary<string, Action>
            {
                { "Employees", EditEmployee },
                { "Contractors", EditContractor },
                { "Orders", EditOrder }
            };

            _deleteActions = new Dictionary<string, Action>
            {
                { "Employees", DeleteEmployee },
                { "Contractors", DeleteContractor },
                { "Orders", DeleteOrder }
            };

            _canEditDelete = new Dictionary<string, Func<bool>>
            {
                { "Employees", () => SelectedEmployee != null },
                { "Contractors", () => SelectedContractor != null },
                { "Orders", () => SelectedOrder != null }
            };

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanEditOrDelete);
            DeleteCommand = new RelayCommand(Delete, CanEditOrDelete);
            LoadEmployees();
            LoadContractors();
            LoadOrders();
        }
        
        private string GetCurrentTabTag()
        {
            return (SelectedTab as System.Windows.Controls.TabItem)?.Tag?.ToString();
        }
        
        private void Add()
        {
            var tag = GetCurrentTabTag();
            if (tag != null && _addActions.TryGetValue(tag, out var action))
                action();
        }

        private void Edit()
        {
            var tag = GetCurrentTabTag();
            if (tag != null && _editActions.TryGetValue(tag, out var action))
                action();
        }

        private void Delete()
        {
            var tag = GetCurrentTabTag();
            if (tag != null && _deleteActions.TryGetValue(tag, out var action))
                action();
        }

        private bool CanEditOrDelete()
        {
            var tag = GetCurrentTabTag();
            return tag != null && _canEditDelete.TryGetValue(tag, out var can) && can();
        }

        private void LoadEmployees()
        {
            var employees = _employeeService.GetAll();
            var displayItems = employees.Select(e 
                => new EmployeeDisplayItem
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    PositionName = e.Position.GetDisplayName(),
                    BirthDate = e.BirthDate.ToString("dd.MM.yyyy")
                });
            Employees = new ObservableCollection<EmployeeDisplayItem>(displayItems);
        }
        
        private void LoadContractors()
        {
            var contractors = _contractorService.GetAll();
            var displayItems = contractors.Select(c => new ContractorDisplayItem
            {
                Id = c.Id,
                Name = c.Name,
                Inn = c.Inn,
                CuratorName = c.Curator?.FullName ?? "-"
            });

            Contractors = new ObservableCollection<ContractorDisplayItem>(displayItems);
        }

        private void LoadOrders()
        {
            var orders = _orderService.GetAll();
            var displayItems = orders.Select(o => new OrderDisplayItem
            {
                Id = o.Id,
                Date = o.Date.ToString("dd.MM.yyyy"),
                Amount = o.Amount.ToString("N2"),
                EmployeeName = o.Employee?.FullName ?? "-",
                ContractorName = o.Contractor?.Name ?? "-"
            });

            Orders = new ObservableCollection<OrderDisplayItem>(displayItems);
        }
        
        private void AddEmployee()
        {
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("EmployeeEdit") as EmployeeEditViewModel;
            var window = Bootstrapper.Resolve<EmployeeEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadEmployees);
        }
                
        private void AddContractor()
        {
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("ContractorEdit") as ContractorEditViewModel;
            var window = Bootstrapper.Resolve<ContractorEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadContractors);
        }

        private void AddOrder()
        {
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("OrderEdit") as OrderEditViewModel;
            var window = Bootstrapper.Resolve<OrderEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadOrders);
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null)
                return;

            var employee = _employeeService.GetById(SelectedEmployee.Id);
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("EmployeeEdit", new { employee }) as EmployeeEditViewModel;
            var window = Bootstrapper.Resolve<EmployeeEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadEmployees);
        }
        
        private void EditContractor()
        {
            if (SelectedContractor == null)
                return;

            var contractor = _contractorService.GetById(SelectedContractor.Id);
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("ContractorEdit", new { contractor }) as ContractorEditViewModel;
            var window = Bootstrapper.Resolve<ContractorEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadContractors);
        }

        private void EditOrder()
        {
            if (SelectedOrder == null)
                return;

            var order = _orderService.GetById(SelectedOrder.Id);
            var viewModel = Bootstrapper.Resolve<IEditViewModel>("OrderEdit", new { order }) as OrderEditViewModel;
            var window = Bootstrapper.Resolve<OrderEditWindow>();
            ShowEditWindow(viewModel, window, onSaved: LoadOrders);
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;

            var result = MessageBox.Show(
                $"Удалить сотрудника '{SelectedEmployee.FullName}'?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            var deleteResult = _employeeService.TryDelete(SelectedEmployee.Id);
            if (deleteResult.IsSuccessed)
            {
                LoadEmployees();
            }
            else
            {
                MessageBox.Show(deleteResult.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void DeleteContractor()
        {
            if (SelectedContractor == null)
                return;

            var result = MessageBox.Show(
                $"Удалить контрагента '{SelectedContractor.Name}'?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            var deleteResult = _contractorService.TryDelete(SelectedContractor.Id);
            if (deleteResult.IsSuccessed)
            {
                LoadContractors();
            }
            else
            {
                MessageBox.Show(deleteResult.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteOrder()
        {
            if (SelectedOrder == null)
                return;

            var result = MessageBox.Show(
                $"Удалить заказ #{SelectedOrder.Id}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            var deleteResult = _orderService.TryDelete(SelectedOrder.Id);
            if (deleteResult.IsSuccessed)
            {
                LoadOrders();
            }
            else
            {
                MessageBox.Show(deleteResult.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void ShowEditWindow<TViewModel, TWindow>(
            TViewModel viewModel,
            TWindow window,
            Action onSaved = null,
            Action onCanceled = null)
            where TViewModel : IEditViewModel
            where TWindow : Window
        {
            window.DataContext = viewModel;

            void OnSaved(object s, EventArgs e)
            {
                window.DialogResult = true;
                window.Close();
                onSaved?.Invoke();
            }

            void OnCanceled(object s, EventArgs e)
            {
                window.DialogResult = false;
                window.Close();
                onCanceled?.Invoke();
            }

            void OnError(object s, string message)
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            viewModel.Saved += OnSaved;
            viewModel.Canceled += OnCanceled;
            viewModel.ErrorOccurred += OnError;
            
            window.Closed += (s, e) =>
            {
                viewModel.Saved -= OnSaved;
                viewModel.Canceled -= OnCanceled;
                viewModel.ErrorOccurred -= OnError;
            };
                
            window.ShowDialog();
        }
    }
}