using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.MedLabTypes
{
    public class TestCollection
    {
        public int TestCollectionID { get; set; }
        public string TestCollectionName { get; set; }
        public List<TestType> TestTypes {  get; set; }  
    }
}
