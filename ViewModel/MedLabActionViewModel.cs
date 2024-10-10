using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MedLab.Model;
using MedLab.Model.Utils;

namespace MedLab.ViewModel
{
    public class MedLabActionViewModel : TheViewModel
    {
        private readonly MedLabActions medLabDatabase;

        public MedLabActionViewModel()
        {
            medLabDatabase = new MedLabActions();

            TruncateAllCommand = new RelayCommand(TruncateAll); 
            GenerateAndSqlizeExecuteCommand = new RelayCommand(GenerateAndSqlizeExecute);
            GenerateAndSqlizeInFileCommand = new RelayCommand(GenerateAndSqlizeInFile);
        }

        public ICommand TruncateAllCommand { get; }
        public ICommand GenerateAndSqlizeExecuteCommand { get; }
        public ICommand GenerateAndSqlizeInFileCommand { get; }

        private void TruncateAll()
        {
            medLabDatabase.TruncateAll();
        }
        private void GenerateAndSqlizeExecute()
        {
            try
            {
                medLabDatabase.GenerateAndSqlize(false);
            }
            catch (InvalidOperationException exception)
            {
                HandleInvalidDbExceptionExecute(exception);
            }
        }
        private void GenerateAndSqlizeInFile()
        {
            try
            {
                medLabDatabase.GenerateAndSqlizeInFile(false);
            }
            catch (InvalidOperationException exception)
            {
                HandleInvalidDbExceptionInFile(exception);
            }
        }
        private void HandleInvalidDbExceptionExecute(InvalidOperationException exception)
        {
            InvalidDbState popup = new InvalidDbState();
            popup.Owner = Application.Current.MainWindow;
            ErrorPopupViewModel errorViewModel = new ErrorPopupViewModel(
                proceedAction: () =>
                {
                    medLabDatabase.GenerateAndSqlize(true);
                    popup.Close();
                },
                abortAction: () =>
                {
                    popup.Close();
                },
                exception.Message
                );
            popup.DataContext = errorViewModel;
            popup.ShowDialog();
        }
        private void HandleInvalidDbExceptionInFile(InvalidOperationException exception)
        {
            InvalidDbState popup = new InvalidDbState();
            popup.Owner = Application.Current.MainWindow;
            ErrorPopupViewModel errorViewModel = new ErrorPopupViewModel(
                proceedAction: () =>
                {
                    medLabDatabase.GenerateAndSqlizeInFile(true);
                    popup.Close();
                },
                abortAction: () =>
                {
                    popup.Close();
                },
                exception.Message
                );
            popup.DataContext = errorViewModel;
            popup.ShowDialog();
        }
    }
}