using MedLab.Model.MedLabTypes;
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
                        TestTypeID = int.Parse(parts[0]),
                        TestName = parts[1],
                        Cost = decimal.Parse(parts[2]),
                        MeasurementUnit = parts[3]
                    };
                    testTypes.Add(testType);
                }
            }
            List<TestNormalValues> normalValues = GetTestNormalValues();

            return testTypes;
        }
        public static List<TestNormalValues> GetTestNormalValues()
        {
            string filePath = "testNormalValues.txt";
            List<TestNormalValues> testNormalValues = new List<TestNormalValues>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    line = line.Replace('.', ',');
                    string[] parts = line.Split(" ");

                    if (parts.Length != 6)
                        throw new Exception("Wrong format");
                    TestNormalValues testNormalValue = new TestNormalValues
                    {
                        TestNormalValuesID = int.Parse(parts[0]),
                        MinAge =int.Parse(parts[1]),
                        MaxAge = int.Parse(parts[2]),
                        Gender = char.Parse(parts[3]),
                        MinResValue = double.Parse(parts[4]),
                        MaxResValue = double.Parse(parts[5]),
                    };
                    testNormalValues.Add(testNormalValue);
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

                    if (parts.Length != 2)
                        throw new Exception("Wrong format");
                    TestPanel testPanel = new TestPanel
                    {
                        TestPanelID = int.Parse(parts[0]),
                        Name = parts[1]
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
                        PanelID = int.Parse(parts[0]),
                        TestTypeID = int.Parse(parts[1])
                    };
                    testPanelContents.Add(content);
                }
            }
            return testPanelContents;
        }
    }
}
