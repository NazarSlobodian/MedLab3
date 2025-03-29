using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class TestType
    {
        public int TestTypeID { get; set; }
        public string TestName { get; set; }
        public decimal Cost { get; set; }
        public string MeasurementUnit { get; set; }
        public List<TestNormalValues> TestNormalValues { get; set; }
    }
}
