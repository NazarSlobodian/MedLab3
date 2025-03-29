using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.MedLabTypes;
using Mysqlx.Crud;

namespace MedLab.Model
{
    public class MedLabData
    {
        private List<Patient> patients;
        private List<Laboratory> laboratories;
        private List<TestType> testTypes;
        private List<TestCollection> testCollections;
        public MedLabData(List<Patient> patients, List<Laboratory> laboratory, List<TestType> testTypes, List<TestCollection> testCollections)
        {
            this.patients = patients;
            this.laboratories = laboratory;
            this.testTypes = testTypes;
            this.testCollections = testCollections;
        }
        public string Sqlize(bool skipTestTypeData)
        {
            string dbName = "wv1";
            StringBuilder testCollectionsInsert = new StringBuilder($"INSERT INTO `{dbName}`.`test_collections` (`testCollectionID`, `collectionName`)\r\nVALUES\r\n");
            StringBuilder testTypesInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`test_types` (`testTypeID`, `testName`, `cost`, `daysTillOverdue`, `measurementsUnit`)\r\nVALUES\r\n");

            StringBuilder includedTestsInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`included_tests_for_collection` (`testCollectionID`, `testTypeID`)\r\nVALUES\r\n");

            StringBuilder testNormalValuesInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`test_normal_values` (`testTypeID`, `minAge`, `maxAge`, `gender`, `minResValue`, `maxResValue`)\r\nVALUES\r\n");

            StringBuilder labsInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`laboratories` (`laboratoryID`, `address`, `email`, `contactNumber`)\r\nVALUES\r\n");
            StringBuilder techsInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`technicians` (`technicianID`, `fullName`, `email`, `contactNumber`, `laboratoryID`)\r\nVALUES\r\n");

            StringBuilder patientsInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`patients`\r\n(`patientID`,\r\n`fullName`,\r\n`gender`,\r\n`dateOfBirth`,\r\n`email`,\r\n`contactNumber`)\r\nVALUES\r\n");
            StringBuilder testBatchesInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`test_batches`\r\n(`testBatchID`,\r\n`batchStatus`,\r\n`dateOfCreation`,\r\n`patientID`,\r\n`technicianID`)\r\nVALUES\r\n");
            StringBuilder testOrderInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`test_orders`\r\n(`testOrderID`,\r\n`testTypeID`,\r\n`testBatchID`)\r\nVALUES\r\n");
            StringBuilder testResultInsert = new StringBuilder($";\r\nINSERT INTO `{dbName}`.`test_results`\r\n(`testOrderID`,\r\n`result`,\r\n`dateOfTest`)\r\nVALUES\r\n");

            bool firstInsertInserted = false;

            bool firstTestCollection = true;
            bool firstTestType = true;

            bool firstIncludedTests = true;

            bool firstTestNormalValue = true;

            bool firstLab = true;
            bool firstTech = true;

            bool firstPatient = true;
            bool firstBatch = true;
            bool firstOrder = true;
            bool firstResult = true;

            if (!skipTestTypeData)
            {
                foreach (TestCollection testCollection in testCollections)
                {
                    if (!firstTestCollection)
                    {
                        testCollectionsInsert.Append(",\r\n");
                    }
                    testCollectionsInsert.Append($"({testCollection.TestCollectionID}, '{testCollection.TestCollectionName}')");
                    firstTestCollection = false;

                    foreach (TestType testType in testCollection.TestTypes)
                    {
                        if (!firstIncludedTests)
                        {
                            includedTestsInsert.Append(",\r\n");
                        }
                        includedTestsInsert.Append($"({testCollection.TestCollectionID}, {testType.TestTypeID})");
                        firstIncludedTests = false;
                    }
                }

                foreach (TestType type in testTypes)
                {
                    if (!firstTestType)
                    {
                        testTypesInsert.Append(",\r\n");
                    }
                    testTypesInsert.Append($"({type.TestTypeID}, '{type.TestName}', {Math.Round(type.Cost, 2).ToString("F2", CultureInfo.InvariantCulture)}, '{type.MeasurementUnit}')");
                    firstTestType = false;

                    foreach (TestNormalValues normal in type.TestNormalValues)
                    {
                        if (!firstTestNormalValue)
                        {
                            testNormalValuesInsert.Append(",\r\n");
                        }
                        testNormalValuesInsert.Append($"({type.TestTypeID}, {normal.MinAge}, {normal.MaxAge}, '{normal.Gender}', {Math.Round(normal.MinResValue, 2).ToString("F2", CultureInfo.InvariantCulture)}, {Math.Round(normal.MaxResValue, 2).ToString("F2", CultureInfo.InvariantCulture)})");
                        firstTestNormalValue = false;
                    }
                }
            }
            foreach (Laboratory lab in laboratories)
            {
                if (!firstLab)
                {
                    labsInsert.Append(",\r\n");
                }
                labsInsert.Append($"({lab.LaboratoryID}, '{lab.Address}', '{lab.Email}', '{lab.ContactNumber}')");
                firstLab = false;

                foreach (Technician tech in lab.Technicians)
                {
                    if (!firstTech)
                    {
                        techsInsert.Append(",\r\n");
                    }
                    techsInsert.Append($"({tech.TechnicianID}, '{tech.FullName}', '{tech.Email}', '{tech.ContactNumber}', {lab.LaboratoryID})");
                    firstTech = false;
                }
            }

            foreach (Patient patient in patients)
            {
                if (!firstPatient)
                {
                    patientsInsert.Append(",\r\n");
                }
                patientsInsert.Append($"({patient.PatientID}, '{patient.FullName}', '{patient.Gender}', '{patient.DateOfBirth.ToString("yyyy-MM-dd")}', '{patient.Email}', '{patient.ContactNumber}')");
                firstPatient = false;

                foreach (TestBatch batch in patient.TestBatches)
                {
                    // find who did test
                    int techId = -1;
                    bool batchMatched = false;
                    foreach (Laboratory lab in laboratories)
                    {
                        if (batchMatched)
                            break;
                        foreach (Technician tech in lab.Technicians)
                        {
                            if (batchMatched)
                                break;
                            foreach (TestBatch techBatch in tech.TestBatches)
                            {
                                if (techBatch.TestBatchID == batch.TestBatchID)
                                {
                                    techId = tech.TechnicianID;
                                    batchMatched = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!firstBatch)
                    {
                        testBatchesInsert.Append(",\r\n");
                    }
                    testBatchesInsert.Append($"({batch.TestBatchID}, '{batch.Status}', '{batch.DateOfCreation.ToString("yyyy-MM-dd HH:mm:ss")}', {patient.PatientID}, {techId})");
                    firstBatch = false;

                    foreach (TestOrder order in batch.TestOrders)
                    {
                    
                        if (!firstOrder)
                        {
                            testOrderInsert.Append(",\r\n");
                        }
                        testOrderInsert.Append($"({order.TestOrderID}, {order.TestType.TestTypeID}, {batch.TestBatchID})");
                        firstOrder = false;

                        if (order.TestResult == null)
                            continue;
                        if (!firstResult)
                        {
                            testResultInsert.Append(",\r\n");
                        }
                        testResultInsert.Append($"({order.TestOrderID}, {Math.Round(order.TestResult.Result, 2).ToString("F2", CultureInfo.InvariantCulture)}, '{order.TestResult.DateOfTest.ToString("yyyy-MM-dd HH:mm:ss")}')");
                        firstResult = false;
                    }
                }
            }

            if (firstLab)
                labsInsert.Clear();
            if (firstTech)
                techsInsert.Clear();
            if (firstPatient)
                patientsInsert.Clear();
            if (firstBatch)
                testBatchesInsert.Clear();
            if (firstOrder)
                testOrderInsert.Clear();
            if (firstResult)
                testResultInsert.Clear();

            if (!skipTestTypeData)
            {
                return (testCollectionsInsert.ToString() + testTypesInsert.ToString() + includedTestsInsert.ToString() + testNormalValuesInsert.ToString()
                    + labsInsert.ToString() + techsInsert.ToString()
                    + patientsInsert.ToString() + testBatchesInsert.ToString() + testOrderInsert.ToString() + testResultInsert.ToString());
            }
            return (labsInsert.Remove(0, 3).ToString() + techsInsert.ToString()
                    + patientsInsert.ToString() + testBatchesInsert.ToString() + testOrderInsert.ToString() + testResultInsert.ToString());
        }
    }
}
