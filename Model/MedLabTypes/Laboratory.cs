using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class Laboratory
    {
        public int LaboratoryID { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public List<Technician> Technicians { get; set; }
    }
}
