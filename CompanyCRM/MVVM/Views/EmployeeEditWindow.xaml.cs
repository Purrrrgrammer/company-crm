using System.Windows;
using CompanyCRM.MVVM.ViewModels;

namespace CompanyCRM.MVVM.Views
{
    public partial class EmployeeEditWindow : Window
    {
        public EmployeeEditWindow(IEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}