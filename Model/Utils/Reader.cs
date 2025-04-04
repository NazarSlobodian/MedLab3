using MedLab.Model.DbModels;
using MedLab.Model.OtherModeld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                        TestTypeId = int.Parse(parts[0]),
                        Name = parts[1],
                        Cost = decimal.Parse(parts[2].Replace('.', ',')),
                        MeasurementsUnit = parts[3]
                    };
                    testTypes.Add(testType);
                }
            }
            List<TestNormalValue> normalValues = GetTestNormalValues();

            return testTypes;
        }
        public static List<TestNormalValue> GetTestNormalValues()
        {
            string filePath = "testNormalValues.txt";
            List<TestNormalValue> testNormalValues = new List<TestNormalValue>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                int id = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    line = line.Replace('.', ',');
                    string[] parts = line.Split(" ");

                    if (parts.Length != 6)
                        throw new Exception("Wrong format");
                    TestNormalValue testNormalValue = new TestNormalValue
                    {
                        TestTypeId = int.Parse(parts[0]),
                        MinAge =int.Parse(parts[1]),
                        MaxAge = int.Parse(parts[2]),
                        Gender = parts[3],
                        MinResValue = decimal.Parse(parts[4]),
                        MaxResValue = decimal.Parse(parts[5]),
                        TestNormalValueId = id,
                    };
                    testNormalValues.Add(testNormalValue);
                    id++;
                }
            }
            return testNormalValues;
        }
        public static List<TestPanel> GetTestPanels()
        {
            string filePath = "panels.txt";
            List<TestPanel> testPanels = new List<TestPanel>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(", ");

                    if (parts.Length != 3)
                        throw new Exception("Wrong format");
                    TestPanel testPanel = new TestPanel
                    {
                        TestPanelId = int.Parse(parts[0]),
                        Name = parts[1],
                        Cost = decimal.Parse(parts[2].Replace('.',','))
                    };
                    testPanels.Add(testPanel);
                }
            }
            return testPanels;
        }
        public static List<TestPanelContentItem> GetContentItems()
        {
            string filePath = "panelContents.txt";
            List<TestPanelContentItem> testPanelContents = new List<TestPanelContentItem>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(" ");

                    if (parts.Length != 2)
                        throw new Exception("Wrong format");
                    TestPanelContentItem content = new TestPanelContentItem
                    {
                        PanelId = int.Parse(parts[0]),
                        TestTypeId = int.Parse(parts[1])
                    };
                    testPanelContents.Add(content);
                }
            }
            return testPanelContents;
        }
    }
}
