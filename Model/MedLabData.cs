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
        private List<Laboratory> laboratories;
        private List<User> users;
        private bool newTypes;
        public MedLabData(List<Patient> patients, List<CollectionPoint> collectionPoints, List<TestType> testTypes, List<TestPanel> testPanels, List<Laboratory> laboratories, List<User> users, bool newTypes)
        {
            this.patients = patients;
            this.collectionPoints = collectionPoints;
            this.testTypes = testTypes;
            this.testPanels = testPanels;
            this.newTypes = newTypes;
            this.laboratories = laboratories;
            this.users = users;
        }
        public void Insert()
        {
            Wv1Context context = new Wv1Context();
            if (newTypes)
            {
                context.TestTypes.AddRange(testTypes);
                context.TestPanels.AddRange(testPanels);
            }
            else
            {
                context.TestTypes.AttachRange(testTypes);
                context.TestPanels.AttachRange(testPanels);
            }
            context.CollectionPoints.AddRange(collectionPoints);
            context.Laboratories.AddRange(laboratories);
            context.Patients.AddRange(patients);
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
