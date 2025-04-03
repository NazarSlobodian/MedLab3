using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class TestPanel
    {
        public int TestPanelID { get; set; }
        public string Name { get; set; }
        public List<TestType> TestTypes {  get; set; }  
    }
}
