using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.DbModels;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI;

namespace MedLab.Model.Utils
{
    public class MockDataGenerator
    {
        public async Task<MedLabData> GenerateData(GenerationAmounts generatedAmount)
        {
            Wv1Context context = new Wv1Context();

            int collectionPointID = (await context.CollectionPoints.MaxAsync(e => (int?)e.CollectionPointId) ?? 0) + 1;
            int receptionistsID = (await context.Receptionists.MaxAsync(e => (int?)e.ReceptionistId) ?? 0) + 1;
            int laboratoryID = (await context.Laboratories.MaxAsync(e => (int?)e.LaboratoryId) ?? 0) + 1;
            int workerID = (await context.LabWorkers.MaxAsync(e => (int?)e.LabWorkerId) ?? 0) + 1;
            int patientID = (await context.Patients.MaxAsync(e => (int?)e.PatientId) ?? 0) + 1;
            int batchID = (await context.TestBatches.MaxAsync(e => (int?)e.TestBatchId) ?? 0) + 1;
            int orderID = (await context.TestOrders.MaxAsync(e => (int?)e.TestOrderId) ?? 0) + 1;

            int typeID = (await context.TestTypes.MaxAsync(e => (int?)e.TestTypeId) ?? 0) + 1;

            int userID = (await context.Users.MaxAsync(e => (int?)e.UserId) ?? 0) + 1;

            RandomDataGenerator randomDataGenerator = new RandomDataGenerator();

            List<TestType> testTypes = new List<TestType>();
            List<TestPanel> testCollection = new List<TestPanel>();
            bool newTypes = false;
            if (typeID == 1)
            {
                newTypes = true;
                (testTypes, testCollection) = randomDataGenerator.GetDefaultTestTypes();
            }
            else
            {
                (testTypes, testCollection) = await RandomDataGenerator.GetDbTestTypes();
            }
            Random random = new Random();
            //collectionPoints
            int amountOfCollectionPoints = generatedAmount.collectionPointAmount;
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();
            for (int collectionPointsGenerated = 0; collectionPointsGenerated < amountOfCollectionPoints;)
            {
                string email = randomDataGenerator.GenerateEmail();
                string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                string address = randomDataGenerator.GenerateAddress();
                bool validCollectionPointValues = true;
                for (int i = 0; i < collectionPoints.Count; i++)
                {
                    if (email == collectionPoints[i].Email)
                    {
                        validCollectionPointValues = false;
                        break;
                    }
                    if (contactNumber == collectionPoints[i].ContactNumber)
                    {
                        validCollectionPointValues = false;
                        break;
                    }
                    if (address == collectionPoints[i].Address)
                    {
                        validCollectionPointValues = false;
                        break;
                    }
                }
                if (!validCollectionPointValues)
                {
                    continue;
                }
                collectionPoints.Add(new CollectionPoint()
                {
                    CollectionPointId = collectionPointID,
                    Address = address,
                    Email = email,
                    ContactNumber = contactNumber,
                    Receptionists = new List<Receptionist>()
                });
                collectionPointID++;
                collectionPointsGenerated++;
            }
            // receptionists for collection points
            for (int i = 0; i < collectionPoints.Count; i++)
            {
                int amountOfReceptionists = generatedAmount.receptionistsAmount;
                for (int receptionistsGenerated = 0; receptionistsGenerated < amountOfReceptionists;)
                {
                    string fullName = randomDataGenerator.GenerateFullname();
                    string email = randomDataGenerator.GenerateEmail();
                    string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                    bool validReceptionistValues = true;
                    for (int j = 0; j < collectionPoints[i].Receptionists.Count; j++)
                    {
                        if (email == collectionPoints[i].Receptionists.ElementAt(j).Email)
                        {
                            validReceptionistValues = false;
                            break;
                        }
                        if (contactNumber == collectionPoints[i].Receptionists.ElementAt(j).ContactNumber)
                        {
                            validReceptionistValues = false;
                            break;
                        }
                    }
                    if (!validReceptionistValues)
                    {
                        continue;
                    }
                    collectionPoints[i].Receptionists.Add(new Receptionist()
                    {
                        ReceptionistId = receptionistsID,
                        FullName = fullName,
                        Email = email,
                        ContactNumber = contactNumber,
                        TestBatches = new List<TestBatch>()
                    });
                    receptionistsID++;
                    receptionistsGenerated++;
                }
            }
            //labs
            int amountOfLabs = generatedAmount.labsAmount;
            List<Laboratory> labs = new List<Laboratory>();
            for (int labsGenerated = 0; labsGenerated < amountOfLabs;)
            {
                string email = randomDataGenerator.GenerateEmail();
                string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                string address = randomDataGenerator.GenerateAddress();
                bool validLabValues = true;
                for (int i = 0; i < labs.Count; i++)
                {
                    if (email == labs[i].Email)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (contactNumber == labs[i].ContactNumber)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (address == labs[i].Address)
                    {
                        validLabValues = false;
                        break;
                    }
                }
                if (!validLabValues)
                {
                    continue;
                }
                labs.Add(new Laboratory()
                {
                    LaboratoryId = laboratoryID,
                    Address = address,
                    Email = email,
                    ContactNumber = contactNumber,
                    LabWorkers = new List<LabWorker>()
                });
                laboratoryID++;
                labsGenerated++;
            }
            //performers
            foreach (Laboratory lab in labs)
            {
                foreach (TestType testType in testTypes)
                {
                    lab.TestTypes.Add(testType);
                }
            }
            // workers for labs
            for (int i = 0; i < labs.Count; i++)
            {
                int amountOfWorkers = generatedAmount.workersAmount;
                for (int workersGenerated = 0; workersGenerated < amountOfWorkers;)
                {
                    string fullName = randomDataGenerator.GenerateFullname();
                    string email = randomDataGenerator.GenerateEmail();
                    string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                    bool validWorkerValues = true;
                    for (int j = 0; j < labs[i].LabWorkers.Count; j++)
                    {
                        if (email == labs[i].LabWorkers.ElementAt(j).Email)
                        {
                            validWorkerValues = false;
                            break;
                        }
                        if (contactNumber == labs[i].LabWorkers.ElementAt(j).ContactNumber)
                        {
                            validWorkerValues = false;
                            break;
                        }
                    }
                    if (!validWorkerValues)
                    {
                        continue;
                    }
                    labs[i].LabWorkers.Add(new LabWorker()
                    {
                        LabWorkerId = workerID,
                        FullName = fullName,
                        Email = email,
                        ContactNumber = contactNumber,
                    });
                    workerID++;
                    workersGenerated++;
                }
            }
            //patients
            int patientAmount = generatedAmount.patientAmount;
            List<Patient> patients = new List<Patient>();
            for (int patientsGenerated = 0; patientsGenerated < patientAmount;)
            {
                string fullName = randomDataGenerator.GenerateFullname();
                string gender = randomDataGenerator.GenerateGender();
                DateTime dateOfBirth = randomDataGenerator.GenerateDate(new DateTime(1940, 1, 1), new DateTime(2024, 9, 1)).Date;
                string email = null;
                string contactNumber = null;
                if (random.Next(0, 2) == 1)
                {
                    if (random.Next(0, 2) == 1)
                        email = randomDataGenerator.GenerateEmail();
                    else
                        contactNumber = randomDataGenerator.GeneratePhoneNumber();
                }

                patients.Add(new Patient()
                {
                    PatientId = patientID,
                    FullName = fullName,
                    Gender = gender,
                    DateOfBirth = DateOnly.FromDateTime(dateOfBirth),
                    Email = email,
                    ContactNumber = contactNumber,
                    TestBatches = new List<TestBatch>()
                });
                patientID++;
                patientsGenerated++;
            }
            // batches
            foreach (Patient patient in patients)
            {
                int amountOfBatches = generatedAmount.batchesPerPatient;
                for (int i = 0; i < amountOfBatches; i++)
                {
                    string status = randomDataGenerator.GenerateBatchStatus();
                    DateTime openingDate = new DateTime(2024, 1, 1);
                    DateTime start = patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM")) < openingDate ? openingDate : patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM")).AddMonths(1);

                    //DateTime timeOfCreation = randomDataGenerator.GenerateDateTime(new DateTime(2024, 9, 2), new DateTime(2024, 10, 1));
                    DateTime timeOfCreation = randomDataGenerator.GenerateDateTime(start, DateTime.UtcNow);
                    patient.TestBatches.Add(new TestBatch()
                    {
                        TestBatchId = batchID,
                        BatchStatus = status,
                        DateOfCreation = timeOfCreation,
                        TestOrders = new List<TestOrder>()
                    });
                    batchID++;
                }
            }
            // test order
            foreach (Patient patient in patients)
            {
                foreach (TestBatch batch in patient.TestBatches)
                {
                    List<Receptionist> receptionists = collectionPoints[random.Next(0, collectionPoints.Count)].Receptionists.ToList();
                    receptionists[random.Next(0, receptionists.Count)].TestBatches.Add(batch);
                    double chance = random.NextDouble();
                    if (chance < 0.5)
                    {
                        int amountOfTests = generatedAmount.ordersPerBatch;
                        int testTypeIndex = random.Next(0, testTypes.Count);
                        for (int testsAdded = 0; testsAdded < amountOfTests;)
                        {
                            TestType testType = testTypes[testTypeIndex];
                            bool validLabValues = true;
                            for (int i = 0; i < batch.TestOrders.Count; i++)
                            {
                                if (batch.TestOrders.ElementAt(i).TestType == testType)
                                {
                                    validLabValues = false;
                                    break;
                                }
                            }
                            if (!validLabValues)
                            {
                                testTypeIndex++;
                                if (testTypeIndex >= testTypes.Count)
                                    testTypeIndex = 0;
                                continue;
                            }
                            TestResult result = null;
                            if (batch.BatchStatus != "queued")
                            {
                                int patientAge = (int)(DateTime.Now - patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM"))).TotalDays / 365;
                                double testResult = 0.0;
                                TestNormalValue resultNormalValues = testTypes
                                    .Find((x) => x == testType).TestNormalValues.ToList()
                                    .Find((x) =>
                                    (x.Gender == patient.Gender) && patientAge >= x.MinAge && patientAge <= x.MaxAge);
                                if (resultNormalValues != null)
                                {
                                    testResult = randomDataGenerator.RandomTestResult((double)resultNormalValues.MinResValue, (double)resultNormalValues.MaxResValue);
                                }
                                else
                                {
                                    testResult = randomDataGenerator.RandomTestResult(0.0, 100.0);
                                }
                                DateTime dateOfTest = randomDataGenerator.GenerateDate(batch.DateOfCreation, batch.DateOfCreation.AddDays(5));
                                result = new TestResult()
                                {
                                    TestOrderId = orderID,
                                    Result = (decimal)testResult,
                                    DateOfTest = DateOnly.FromDateTime(dateOfTest)
                                };
                            }
                            TestOrder order = new TestOrder()
                            {
                                TestOrderId = orderID,
                                TestType = testType,
                                TestResult = result,
                                Laboratory = labs[random.Next(0, labs.Count)]
                            };

                            batch.TestOrders.Add(order);
                            orderID++;
                            testsAdded++;
                        }

                    }
                    else
                    {
                        int testPanelIndex = random.Next(0, testCollection.Count);
                        foreach (TestType testType in testCollection[testPanelIndex].TestTypes)
                        {
                            TestResult result = null;
                            if (batch.BatchStatus != "queued")
                            {
                                int patientAge = (int)(DateTime.Now - patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM"))).TotalDays / 365;
                                double testResult = 0.0;
                                TestNormalValue resultNormalValues = testTypes
                                    .Find((x) => x == testType).TestNormalValues.ToList()
                                    .Find((x) =>
                                    (x.Gender == patient.Gender) && patientAge >= x.MinAge && patientAge <= x.MaxAge);
                                if (resultNormalValues != null)
                                {
                                    testResult = randomDataGenerator.RandomTestResult((double)resultNormalValues.MinResValue, (double)resultNormalValues.MaxResValue);
                                }
                                else
                                {
                                    testResult = randomDataGenerator.RandomTestResult(0.0, 100.0);
                                }
                                DateTime dateOfTest = randomDataGenerator.GenerateDate(batch.DateOfCreation, batch.DateOfCreation.AddDays(5));
                                result = new TestResult()
                                {
                                    TestOrderId = orderID,
                                    Result = (decimal)testResult,
                                    DateOfTest = DateOnly.FromDateTime(dateOfTest)
                                };
                            }
                            TestOrder order = new TestOrder()
                            {
                                TestOrderId = orderID,
                                TestType = testType,
                                TestResult = result,
                                Laboratory = labs[random.Next(0, labs.Count)],
                                TestPanelId = testCollection[testPanelIndex].TestPanelId
                            };
                            batch.TestOrders.Add(order);
                            orderID++;
                        }
                    }
                }
            }

            //accounts
            PasswordHasher passwordHasher = new PasswordHasher();
            List<User> users = new List<User>();
            foreach (Patient patient in patients)
            {
                if (patient.Email != null)
                {
                    users.Add(new User
                    {
                        UserId = userID,
                        Role = "patient",
                        ReferencedId = patient.PatientId,
                        Login = patient.FullName.Substring(0, 1),
                        Hash = PasswordHasher.HashPassword(patient.FullName.Substring(0, 1) + patient.Email.Substring(0, 1)),
                    });
                    userID++;
                }
            }
            foreach (CollectionPoint collectionPoint in collectionPoints)
            {
                foreach (Receptionist receptionist in collectionPoint.Receptionists)
                {
                    users.Add(new User
                    {
                        UserId = userID,
                        Role = "receptionist",
                        ReferencedId = receptionist.ReceptionistId,
                        Login = receptionist.FullName.Substring(0, 1),
                        Hash = PasswordHasher.HashPassword(receptionist.FullName.Substring(0, 1) + receptionist.Email.Substring(0, 1)),
                    });
                    userID++;

                }
            }
            foreach (Laboratory lab in labs)
            {
                foreach (LabWorker labWorker in lab.LabWorkers)
                {
                    users.Add(new User
                    {
                        UserId = userID,
                        Role = "lab_worker",
                        ReferencedId = labWorker.LabWorkerId,
                        Login = labWorker.FullName.Substring(0, 1),
                        Hash = PasswordHasher.HashPassword(labWorker.FullName.Substring(0, 1) + labWorker.Email.Substring(0, 1)),
                    });
                    userID++;

                }
            }

            return new MedLabData(patients, collectionPoints, testTypes, testCollection, labs, users, newTypes);
        }
    }
}
