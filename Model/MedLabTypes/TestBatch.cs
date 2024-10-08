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
        public char Status { get; set; }
        public MySql.Data.Types.MySqlDateTime DateOfCreation { get; set; }
        public List<TestOrder> TestOrders { get; set; }
    }
}
