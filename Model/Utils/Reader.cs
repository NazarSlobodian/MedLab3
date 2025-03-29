using MedLab.Model.MedLabTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.Utils
{
    public class Reader
    {
        public static List<TestType> GetTestTypes()
        {
            string filePath = "testTypes.txt";
            List<TestType> testTypes = new List<TestType>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(", ");

                    if (parts.Length != 4)
                        throw new Exception("Wrong format");
                    TestType testType = new TestType
                    {
                        TestTypeID = int.Parse(parts[0]),
                        TestName = parts[1],
                        Cost = decimal.Parse(parts[2]),
                        MeasurementUnit = parts[3]
                    };
                    testTypes.Add(testType);
                }
            }
            return testTypes;
        }
    }
}
