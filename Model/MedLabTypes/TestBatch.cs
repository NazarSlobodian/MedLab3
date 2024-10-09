using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class TestBatch
    {
        public int TestBatchID { get; set; }
        public char Status { get; set; }
        public DateTime DateOfCreation { get; set; }
        public List<TestOrder> TestOrders { get; set; }
    }
}
