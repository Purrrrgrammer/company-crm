using System;

namespace CompanyCRM.MVVM.ViewModels
{
    public interface IEditViewModel
    {
        event EventHandler Saved;
        event EventHandler Canceled;
        event EventHandler<string> ErrorOccurred;
    }
}