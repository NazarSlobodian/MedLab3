using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class TestResult
    {
        public int TestResultID { get; set; }
        public double Result { get; set; }
        public DateTime DateOfTest { get; set; }
    }
}
