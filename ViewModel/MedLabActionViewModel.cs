using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MedLab.Model.Utils;

namespace MedLab.ViewModel
{
    public class MedLabActionViewModel : INotifyPropertyChanged
    {
        private readonly MockDatabaseGenerator _databaseGenerator;

        public MedLabActionViewModel()
        {
            _databaseGenerator = new MockDatabaseGenerator();
            GenerateCommand = new RelayCommand(GenerateData);
        }

        public ICommand GenerateCommand { get; }

        private void GenerateData()
        {
            _databaseGenerator.Generate();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
