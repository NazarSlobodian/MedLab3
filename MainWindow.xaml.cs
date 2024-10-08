using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace MedLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.MedLabActionViewModel();
            //string str = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            //MySqlConnection connection = new MySqlConnection(str);

            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM patients", connection);

            //connection.Open();
            //DataTable dataTable = new DataTable();
            //dataTable.Load(cmd.ExecuteReader());
            //connection.Close();
            //dbTable.ItemsSource = dataTable.DefaultView;
        }
    }
}