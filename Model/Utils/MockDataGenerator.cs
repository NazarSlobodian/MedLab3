﻿using System;
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
                    string fullName = random.NextDouble() < 0.5 ? randomDataGenerator.GenerateFullname('m') : randomDataGenerator.GenerateFullname('f');
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
                    string fullName = random.NextDouble() < 0.5 ? randomDataGenerator.GenerateFullname('m') : randomDataGenerator.GenerateFullname('f');
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
                string gender = randomDataGenerator.GenerateGender();
                string fullName = randomDataGenerator.GenerateFullname(gender[0]);
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
                int amountOfBatches = random.Next(1, generatedAmount.batchesPerPatient + 1);
                for (int i = 0; i < amountOfBatches; i++)
                {
                    string status = randomDataGenerator.GenerateBatchStatus();
                    DateTime openingDate = new DateTime(2023, 1, 1);
                    DateTime start = patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM")) < openingDate ? openingDate : patient.DateOfBirth.ToDateTime(TimeOnly.Parse("00:00 AM")).AddMonths(1);
                    //DateTime timeOfCreation = randomDataGenerator.GenerateDateTime(new DateTime(2024, 9, 2), new DateTime(2024, 10, 1));
                    DateTime timeOfCreation = randomDataGenerator.GenerateDateTime(start, DateTime.UtcNow).AddDays(-7);
                    if (timeOfCreation < DateTime.Now.AddDays(-14))
                        status = "done";
                    patient.TestBatches.Add(new TestBatch()
                    {
                        TestBatchId = batchID,
                        BatchStatus = status,
                        DateOfCreation = timeOfCreation,
                        TestOrders = new List<TestOrder>(),
                        Cost = 0.0m
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
                    int panelsAmount = random.Next(0, generatedAmount.panelsPerBatch + 1);
                    for (int i = 0; i < panelsAmount; i++)
                    {
                        int testPanelIndex = random.Next(0, testCollection.Count);
                        while (batch.TestOrders.Select(x => x.TestPanelId).Contains(testCollection[testPanelIndex].TestPanelId))
                            testPanelIndex = random.Next(0, testCollection.Count);
                        batch.Cost += testCollection[testPanelIndex].Cost;
                        foreach (TestType testType in testCollection[testPanelIndex].TestTypes)
                        {
                            Laboratory lab = labs[random.Next(0, labs.Count)];
                            TestResult result = null;
                            if (batch.BatchStatus == "processing" && batch.TestOrders.Count != 0 && random.NextDouble() < 0.5)
                                continue;
                            else if (batch.BatchStatus == "done")
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
                                DateTime dateOfTest = randomDataGenerator.GenerateDateTime(batch.DateOfCreation.AddDays(1), batch.DateOfCreation.AddDays(5));
                                result = new TestResult()
                                {
                                    TestOrderId = orderID,
                                    Result = (decimal)testResult,
                                    DateOfTest = dateOfTest,
                                    LabWorker = lab.LabWorkers.ElementAt(random.Next(0, lab.LabWorkers.Count))
                                };
                            }
                            TestOrder order = new TestOrder()
                            {
                                TestOrderId = orderID,
                                TestType = testType,
                                TestResult = result,
                                Laboratory = lab,
                                TestPanelId = testCollection[testPanelIndex].TestPanelId
                            };
                            batch.TestOrders.Add(order);
                            orderID++;
                        }
                    }


                    int amountOfTests = panelsAmount == 0 ? random.Next(1, generatedAmount.ordersPerBatch+1) : random.Next(0, generatedAmount.ordersPerBatch + 1);
                    for (int testsAdded = 0; testsAdded < amountOfTests;)
                    {
                        int testTypeIndex = random.Next(0, testTypes.Count);
                        TestType testType = testTypes[testTypeIndex];
                        Laboratory lab = labs[random.Next(0, labs.Count)];
                        bool validLabValues = true;

                        TestResult result = null;
                        if (batch.BatchStatus == "processing" && batch.TestOrders.Count != 0 && random.NextDouble() < 0.5)
                            continue;
                        else if (batch.BatchStatus == "done")
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
                                DateOfTest = dateOfTest,
                                LabWorker = lab.LabWorkers.ElementAt(random.Next(0, lab.LabWorkers.Count))
                            };
                        }
                        TestOrder order = new TestOrder()
                        {
                            TestOrderId = orderID,
                            TestType = testType,
                            TestResult = result,
                            Laboratory = lab
                        };

                        batch.TestOrders.Add(order);
                        batch.Cost += testType.Cost;
                        orderID++;
                        testsAdded++;
                    }
                }
            }

            //accounts
            PasswordHasher passwordHasher = new PasswordHasher();
            List<User> users = new List<User>();
            foreach (Patient patient in patients)
            {
                if (patient.Email != null && random.NextDouble() < 0.5)
                {
                    users.Add(new User
                    {
                        UserId = userID,
                        Role = "patient",
                        ReferencedId = patient.PatientId,
                        Login = patient.Email,
                        Hash = PasswordHasher.HashPassword(patient.Email.Substring(0, 2)),
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
                        Login = receptionist.Email,
                        Hash = PasswordHasher.HashPassword(receptionist.Email.Substring(0, 2)),
                    });
                    userID++;

                }
            }
            foreach (Laboratory lab in labs)
            {
                for (int i = 0;i < lab.LabWorkers.Count; i++)
                {
                    if (i != 0)
                    {
                        users.Add(new User
                        {
                            UserId = userID,
                            Role = "lab_worker",
                            ReferencedId = lab.LabWorkers.ElementAt(i).LabWorkerId,
                            Login = lab.LabWorkers.ElementAt(i).Email,
                            Hash = PasswordHasher.HashPassword(lab.LabWorkers.ElementAt(i).Email.Substring(0, 2)),
                        });
                    }
                    else
                    {
                        
                        users.Add(new User
                        {
                            UserId = userID,
                            Role = "lab_admin",
                            ReferencedId = lab.LabWorkers.ElementAt(i).LabWorkerId,
                            Login = lab.LabWorkers.ElementAt(i).Email,
                            Hash = PasswordHasher.HashPassword(lab.LabWorkers.ElementAt(i).Email.Substring(0, 2)),
                        });
                    }
                    userID++;

                }
            }
            bool adminExists = context.Users.Any(x => x.Role == "admin");
            if (!adminExists)
            {
                users.Add(new User
                {
                    UserId = userID,
                    Role = "admin",
                    ReferencedId = userID,
                    Login = "admin",
                    Hash = PasswordHasher.HashPassword("admin"),
                });
            }
            return new MedLabData(patients, collectionPoints, testTypes, testCollection, labs, users, newTypes);
        }
    }
}
