using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.DbModels;
using Mysqlx.Crud;

namespace MedLab.Model
{
    public class MedLabData
    {
        private List<Patient> patients;
        private List<CollectionPoint> collectionPoints;
        private List<TestType> testTypes;
        private List<TestPanel> testPanels;
        private bool newTypes;
        public MedLabData(List<Patient> patients, List<CollectionPoint> collectionPoints, List<TestType> testTypes, List<TestPanel> testPanels, bool newTypes)
        {
            this.patients = patients;
            this.collectionPoints = collectionPoints;
            this.testTypes = testTypes;
            this.testPanels = testPanels;
            this.newTypes = newTypes;
        }
        public void Insert()
        {
            Wv1Context context = new Wv1Context();
            if (newTypes)
            {
                context.TestTypes.AddRangeAsync(testTypes);
                //foreach (TestType testType in context.TestTypes)
                //{
                //    testType.TestNormalValues = null;
                //}
                //context.CollectionPoints.AddRangeAsync(collectionPoints);
                context.TestPanels.AddRangeAsync(testPanels);
            }
            context.SaveChanges();
        }
    }
}
