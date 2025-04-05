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
        }

        public ICommand TruncateAllCommand { get; }
        public ICommand GenerateAndSqlizeExecuteCommand { get; }
        public ICommand GenerateAndSqlizeInFileCommand { get; }
        private int collectionPointAmount = 1;
        public int CollectionPointAmount
        {
            get { return collectionPointAmount; }
            set
            {
                collectionPointAmount = value;
                OnPropertyChanged(nameof(CollectionPointAmount));
            }
        }
        private int receptionistsAmount = 1;
        public int ReceptionistsAmount
        {
            get { return receptionistsAmount; }
            set
            {
                receptionistsAmount = value;
                OnPropertyChanged(nameof(ReceptionistsAmount));
            }
        }
        private int patientAmount = 1;
        public int PatientAmount
        {
            get { return patientAmount; }
            set
            {
                patientAmount = value;
                OnPropertyChanged(nameof(PatientAmount));
            }
        }
        private int batchesPerPatient = 1;
        public int BatchesPerPatient
        {
            get { return batchesPerPatient; }
            set
            {
                batchesPerPatient = value;
                OnPropertyChanged(nameof(BatchesPerPatient));
            }
        }
        private int ordersPerBatch = 1;
        public int OrdersPerBatch
        {
            get { return ordersPerBatch; }
            set
            {
                ordersPerBatch = value;
                OnPropertyChanged(nameof(OrdersPerBatch));
            }
        }

        private int labsAmount = 1;

        public int LabsAmount
        {
            get { return labsAmount; }
            set
            {
                labsAmount = value;
                OnPropertyChanged(nameof(LabsAmount));
            }
        }

        private int workersPerLab = 1;

        public int WorkersPerLab
        {
            get { return workersPerLab; }
            set
            {
                workersPerLab = value;
                OnPropertyChanged(nameof(WorkersPerLab));
            }
        }


        private void TruncateAll()
        {
            medLabDatabase.TruncateAll();
        }
        private void GenerateAndSqlizeExecute()
        {
            try
            {
                medLabDatabase.GenerateAndInsert(
                    new GenerationAmounts(CollectionPointAmount, ReceptionistsAmount, PatientAmount,
                    BatchesPerPatient, OrdersPerBatch, LabsAmount, WorkersPerLab));
            }
            catch (InvalidOperationException exception)
            {
                HandleInvalidDbExceptionExecute(exception);
            }
        }
        private void HandleInvalidDbExceptionExecute(InvalidOperationException exception)
        {
            InvalidDbState popup = new InvalidDbState();
            popup.Owner = Application.Current.MainWindow;
            ErrorPopupViewModel errorViewModel = new ErrorPopupViewModel(
                proceedAction: () =>
                {
                    medLabDatabase.GenerateAndInsert(
                        new GenerationAmounts(CollectionPointAmount, ReceptionistsAmount, PatientAmount,
                        BatchesPerPatient,OrdersPerBatch, LabsAmount, WorkersPerLab));
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