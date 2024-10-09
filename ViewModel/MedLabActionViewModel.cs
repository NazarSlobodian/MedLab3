using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MedLab.Model;
using MedLab.Model.Utils;

namespace MedLab.ViewModel
{
    public class MedLabActionViewModel : INotifyPropertyChanged
    {
        private readonly MockDatabaseGenerator databaseGenerator;

        public MedLabActionViewModel()
        {
            databaseGenerator = new MockDatabaseGenerator();
            GenerateCommand = new RelayCommand(GenerateData);
            GenerateAndSqlizeCommand = new RelayCommand(GenerateAndSqlize);
        }

        public ICommand GenerateCommand { get; }
        public ICommand GenerateAndSqlizeCommand { get; }

        private void GenerateData()
        {
            databaseGenerator.GenerateData();
        }
        private void GenerateAndSqlize()
        {
            MedLabData data = databaseGenerator.GenerateData();
            string statement = data.Sqlize();
            return;
            //save in file
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
