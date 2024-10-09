using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FullName { get; set; }
        public char Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string PatientPassword { get; set; }
        public List<TestBatch> TestBatches { get; set; }
    }
}
