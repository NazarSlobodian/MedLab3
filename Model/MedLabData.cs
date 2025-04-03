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
        private List<TestPanel> testCollections;
        public MedLabData(List<Patient> patients, List<CollectionPoint> laboratory, List<TestType> testTypes, List<TestPanel> testCollections)
        {
            this.patients = patients;
            this.collectionPoints = laboratory;
            this.testTypes = testTypes;
            this.testCollections = testCollections;
        }
        public void Insert()
        {

        }
    }
}
