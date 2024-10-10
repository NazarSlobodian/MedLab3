using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.Utils;
using MySql.Data.MySqlClient;

namespace MedLab.Model
{
    public class MedLabActions
    {
        MockDataGenerator databaseGenerator = new MockDataGenerator();
        public void GenerateAndSqlizeInFile(GenerationAmounts generatedAmount, bool validTestTypes)
        {
            string statement = GenerateAndSqlize(generatedAmount, validTestTypes);
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(exeDirectory, "InsertSample.txt");
            File.WriteAllText(filePath, statement);
        }
        public string GenerateAndSqlize(GenerationAmounts generatedAmount, bool validTestTypes)
        {
            MedLabData data = databaseGenerator.GenerateData(generatedAmount, validTestTypes);
            string statement = data.Sqlize(validTestTypes);
            return statement;
        }
        public void TruncateAll()
        {
            string str = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            MySqlConnection connection = new MySqlConnection(str);

            StringBuilder sb = new StringBuilder();
            sb.Append("SET FOREIGN_KEY_CHECKS = 0;\r\n");

            sb.Append("TRUNCATE TABLE laboratories;\r\n");
            sb.Append("TRUNCATE TABLE technicians;\r\n");
            sb.Append("TRUNCATE TABLE test_orders;\r\n");
            sb.Append("TRUNCATE TABLE test_results;\r\n");
            sb.Append("TRUNCATE TABLE test_batches;\r\n");
            sb.Append("TRUNCATE TABLE patients;\r\n");
            sb.Append("TRUNCATE TABLE test_types;\r\n");
            sb.Append("TRUNCATE TABLE test_normal_values;\r\n");
            sb.Append("TRUNCATE TABLE test_collections;\r\n");
            sb.Append("TRUNCATE TABLE included_tests_for_collection;\r\n");

            sb.Append("SET FOREIGN_KEY_CHECKS = 1;\r\n");

            MySqlCommand cmd = new MySqlCommand(sb.ToString(), connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
