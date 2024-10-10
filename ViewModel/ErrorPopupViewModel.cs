using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedLab.ViewModel
{
    class ErrorPopupViewModel : TheViewModel
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ErrorPopupViewModel(Action proceedAction, Action abortAction, string errorMessage)
        {
            ErrorMessage = errorMessage;

            ProceedCommand = new RelayCommand(proceedAction);
            AbortCommand = new RelayCommand(abortAction);
        }
        public ICommand ProceedCommand { get; }
        public ICommand AbortCommand { get; }
    }
}
