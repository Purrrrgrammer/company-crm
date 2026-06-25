using System.Windows;
using CompanyCRM.MVVM.ViewModels;

namespace CompanyCRM.MVVM.Views
{
    public partial class ContractorEditWindow : Window
    {
        public ContractorEditWindow(IEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}