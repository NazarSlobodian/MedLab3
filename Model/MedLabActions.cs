using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.Utils;
using MySql.Data.MySqlClient;
using Mysqlx.Prepare;

namespace MedLab.Model
{
    public class MedLabActions
    {
        MockDataGenerator databaseGenerator = new MockDataGenerator();
        public async void GenerateAndInsert(GenerationAmounts generatedAmount)
        {
            MedLabData data = await databaseGenerator.GenerateData(generatedAmount);
            data.Insert();
        }
        public void TruncateAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SET FOREIGN_KEY_CHECKS = 0;\r\n");

            sb.Append("TRUNCATE TABLE collection_points;\r\n");
            sb.Append("TRUNCATE TABLE lab_workers;\r\n");
            sb.Append("TRUNCATE TABLE laboratories;\r\n");
            sb.Append("TRUNCATE TABLE patients;\r\n");
            sb.Append("TRUNCATE TABLE receptionists;\r\n");
            sb.Append("TRUNCATE TABLE test_batches;\r\n");
            sb.Append("TRUNCATE TABLE test_normal_values;\r\n");
            sb.Append("TRUNCATE TABLE test_orders;\r\n");
            sb.Append("TRUNCATE TABLE test_panels;\r\n");
            sb.Append("TRUNCATE TABLE test_panels_contents;\r\n");
            sb.Append("TRUNCATE TABLE test_performers;\r\n");
            sb.Append("TRUNCATE TABLE test_results;\r\n");
            sb.Append("TRUNCATE TABLE test_types;\r\n");
            sb.Append("TRUNCATE TABLE users;\r\n");
            sb.Append("TRUNCATE TABLE activity_logs;\r\n");
            sb.Append("TRUNCATE TABLE registration_codes;\r\n");

            sb.Append("SET FOREIGN_KEY_CHECKS = 1;\r\n");

            Execute(sb.ToString());
        }
        private void Execute(string statement)
        {
            string str = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            MySqlConnection connection = new MySqlConnection(str);
            MySqlCommand cmd = new MySqlCommand(statement, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
