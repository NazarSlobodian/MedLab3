using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class Technician
    {
        public int TechnicianID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public List<TestOrder> TestOrders { get; set; }
    }
}
