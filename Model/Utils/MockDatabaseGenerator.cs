using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.MedLabTypes;
using MySql.Data.MySqlClient;

namespace MedLab.Model.Utils
{
    public class MockDatabaseGenerator
    {
        public void Generate()
        {
            string str = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            MySqlConnection connection = new MySqlConnection(str);

            MySqlCommand cmdLabs = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"laboratories\"", connection);
            MySqlCommand cmdTechs = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"technicians\"", connection);
            MySqlCommand cmdPatients = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"patients\"", connection);
            MySqlCommand cmdBatches = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_batches\"", connection);
            MySqlCommand cmdOrders = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_orders\"", connection);
            MySqlCommand cmdResults = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_results\"", connection);
            MySqlCommand cmdType = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_types\"", connection);
            MySqlCommand cmdNormal = new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_normal_values\"", connection);
            MySqlCommand cmdTestCollection= new MySqlCommand(
                "SELECT AUTO_INCREMENT" +
                " FROM information_schema.TABLES" +
                " WHERE TABLE_SCHEMA = \"medlab\"" +
                " AND TABLE_NAME = \"test_collections\"", connection);
            connection.Open();
            int labID = Convert.ToInt32(cmdLabs.ExecuteScalar());
            int techID = Convert.ToInt32(cmdTechs.ExecuteScalar());
            int patientID = Convert.ToInt32(cmdPatients.ExecuteScalar());
            int batchID = Convert.ToInt32(cmdBatches.ExecuteScalar());
            int orderID = Convert.ToInt32(cmdOrders.ExecuteScalar());
            //int resultID = Convert.ToInt32(cmdResults.ExecuteScalar());
            int typeID = Convert.ToInt32(cmdType.ExecuteScalar());
            //int normalID = Convert.ToInt32(cmdNormal.ExecuteScalar());
            int testCollectionID = Convert.ToInt32(cmdTestCollection.ExecuteScalar());
            connection.Close();
            

            RandomDataGenerator randomDataGenerator = new RandomDataGenerator();

            (List<TestType> testTypes, List<TestCollection> testCollection) = randomDataGenerator.GetTestTypes();

            Random random = new Random();
            //labs
            int amountOfLabs = random.Next(1, 11);
            List<Laboratory> laboratories = new List<Laboratory>();
            for (int labsGenerated = 0; labsGenerated < amountOfLabs;)
            {
                string email = randomDataGenerator.GenerateEmail();
                string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                string address = randomDataGenerator.GenerateAddress();
                bool validLabValues = true;
                for (int i = 0; i < laboratories.Count; i++)
                {
                    if (email == laboratories[i].Email)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (contactNumber == laboratories[i].ContactNumber)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (address == laboratories[i].Address)
                    {
                        validLabValues = false;
                        break;
                    }
                }
                if (!validLabValues)
                {
                    continue;
                }
                laboratories.Add(new Laboratory()
                {
                    LaboratoryID = labID,
                    Email = email,
                    ContactNumber = contactNumber,
                    Address = address,
                    Technicians = new List<Technician>()
                });
                labID++;
                labsGenerated++;
            }
            // techs for labs
            for (int i = 0; i < laboratories.Count; i++)
            {
                int amountOfTechnicians = random.Next(1, 5);
                for (int techniciansGenerated = 0; techniciansGenerated < amountOfTechnicians;)
                {
                    string fullName = randomDataGenerator.GenerateFullname();
                    string email = randomDataGenerator.GenerateEmail();
                    string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                    bool validLabValues = true;
                    for (int j = 0; j < laboratories[i].Technicians.Count; j++)
                    {
                        if (email == laboratories[i].Technicians[j].Email)
                        {
                            validLabValues = false;
                            break;
                        }
                        if (contactNumber == laboratories[i].Technicians[j].ContactNumber)
                        {
                            validLabValues = false;
                            break;
                        }
                    }
                    if (!validLabValues)
                    {
                        continue;
                    }
                    laboratories[i].Technicians.Add(new Technician()
                    {
                        TechnicianID = techID,
                        FullName = fullName,
                        Email = email,
                        ContactNumber = contactNumber
                    });
                    techID++;
                    techniciansGenerated++;
                }
            }
            //patients
            int patientAmount = random.Next(10, 20);
            List<Patient> patients = new List<Patient>();
            for (int patientsGenerated = 0; patientsGenerated < patientAmount;)
            {
                string fullName = randomDataGenerator.GenerateFullname();
                char gender = randomDataGenerator.GenerateGender();
                DateTime dateOfBirth = randomDataGenerator.GenerateDate(new DateTime(1940, 1, 1), new DateTime(2024, 9, 1)).Date;
                string email = null;
                string contactNumber = null;
                string password = null;
                if (random.Next(0, 2) == 1)
                {
                    if (random.Next(0, 2) == 1)
                        email = randomDataGenerator.GenerateEmail();
                    else
                        contactNumber = randomDataGenerator.GeneratePhoneNumber();
                    password = randomDataGenerator.GeneratePassword();
                }

                patients.Add(new Patient()
                {
                    PatientID = patientID,
                    FullName = fullName,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    Email = email,
                    ContactNumber = contactNumber,
                    PatientPassword = password,
                    TestBatches = new List<TestBatch>()
                });
                patientID++;
                patientsGenerated++;
            }
            // batches
            foreach (Patient patient in patients)
            {
                int amountOfBatches = random.Next(1, 3);
                for (int i = 0; i<amountOfBatches; i++)
                {
                    char status = randomDataGenerator.GenerateBatchStatus();
                    DateTime timeOfCreation = randomDataGenerator.GenerateDateTime(new DateTime(2024, 9, 2), DateTime.Now);
                    patient.TestBatches.Add(new TestBatch()
                    {
                        TestBatchID = batchID,
                        Status = status,
                        DateOfCreation = timeOfCreation,
                        TestOrders = new List<TestOrder>()
                    });
                    batchID++;
                }
            }
            // testTypes

            // testCollection

            //order


            return;
        }
    }
}
