using System.Windows;
using CompanyCRM.MVVM.ViewModels;

namespace CompanyCRM.MVVM.Views
{
    public partial class OrderEditWindow : Window
    {
        public OrderEditWindow(IEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}