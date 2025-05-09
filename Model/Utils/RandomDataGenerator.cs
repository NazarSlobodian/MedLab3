﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using MedLab.Model.DbModels;
using MedLab.Model.OtherModeld;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient.Authentication;

namespace MedLab.Model.Utils
{
    public class RandomDataGenerator
    {
        private static Random random = new Random();

        private List<string> phoneOperatorCodes = new List<string>()
        {
            "50", "66", "95", "99", "75",
            "67", "68", "96", "97", "98",
            "63", "73", "93"
        };
        private List<string> emailDomains = new List<string>()
        {
            "lpnu.ua",
            "gmail.com",
            "yahoo.com",
            "msn.com",
            "outlook.com"
        };
        private string emailCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        private List<string> cities = new List<string>()
        {
            "Lviv",
            "Kyiv",
            "Odesa",
            "Kharkiv",
            "Dnipro",
            "Ternopil",
            "Zhytomyr",
            "Mykolaiv"
        };
        private List<string> streets = new List<string>()
        {
            "Lychakivska",
            "Bandery",
            "Lysenka",
            "Vitovskoho",
            "Luganska",
            "Sadova",
            "Naukova",
            "Radisna"
        };
        private List<string> measurementUnits = new List<string>()
        {
            "mg/dL",
            "mmol/L",
            "g/dL",
            "IU/L",
            "ng/mL",
            "pg/mL",
            "μmol/L",
            "mEq/L",
            "U/L",
            "cells/μL",
            "fL",
            "g/L",
            "mm/h",
            "μg/dL",
            "mL/min",
            "kPa",
            "mmHg"
        };
        private List<string> namesM = new List<string>()
        {
            "Saul",
            "Petro",
            "Pedro",
            "Pavlo",
            "Roman",
            "Vlad",
            "Mike",
            "Paul",
            "Bob",
            "Ted",
        };
        private List<string> namesF = new List<string>()
        {
            "Anastasia",
            "Katherine",
            "Olivia",
            "Maria",
            "Alice",
            "Rose"
        };
        private List<string> surnames = new List<string>()
        {
            "Smith",
            "Goodman",
            "White",
            "McGill",
            "Hamlin",
            "Ehrmantraut",
            "Schrader",
            "Cruz",
            "Nguyen",
            "Adams",
            "Bailes",
            "Danson",
            "Carlson"
        };

        public string GeneratePhoneNumber()
        {
            StringBuilder phoneNumber = new StringBuilder("380");
            phoneNumber.Append(phoneOperatorCodes[random.Next(0, phoneOperatorCodes.Count)]);
            for (int i = 0; i < 7; i++)
            {
                phoneNumber.Append(random.Next(0, 10));
            }
            return phoneNumber.ToString();
        }
        public string GenerateEmail()
        {
            int usernameLength = random.Next(8, 50);
            StringBuilder username = new StringBuilder();
            for (int i = 0; i < usernameLength; i++)
            {
                username.Append(emailCharacters[random.Next(0, emailCharacters.Length)]);
            }
            return username + "@" + emailDomains[random.Next(0, emailDomains.Count)];
        }
        public string GenerateAddress()
        {
            string city = cities[random.Next(0, cities.Count)];
            string street = streets[random.Next(0, streets.Count)];
            int building = random.Next(1, 101);
            return $"{city}, {street} {building}";
        }
        public DateTime GenerateDateTime(DateTime start, DateTime end)
        {
            int range = (end - start).Days + 1;
            TimeOnly time = GenerateTime();
            DateTime randomDate = GenerateDate(start, end).AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(random.Next(0, 60));
            return randomDate;
        }
        public DateTime GenerateDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days + 1;
            DateTime randomDate = start.AddDays(random.Next(range));
            return randomDate.Date;
        }
        public TimeOnly GenerateTime()
        {
            TimeOnly time = new TimeOnly();
            TimeOnly randomTime = time.AddMinutes(random.Next(480, 1080));
            return randomTime;
        }
        public string GenerateGender()
        {
            if (random.Next(0, 2) == 1)
            {
                return "m";
            }
            return "f";
        }
        public string GeneratePassword()
        {
            int passwordLength = random.Next(8, 30);
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < passwordLength; i++)
            {
                password.Append(emailCharacters[random.Next(0, emailCharacters.Length)]);
            }
            return password.ToString();
        }
        public double RandomTestResult(double low, double high)
        {
            return (random.NextDouble() * (high - low)) + low;
        }
        public string GenerateBatchStatus()
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return "queued";
                case 1:
                    return "processing";
                case 2:
                    return "done";
                default:
                    return "";
            }
        }
        public string GenerateFullname(char sex)
        {
            string name = sex == 'm' ? namesM[random.Next(0, namesM.Count)] : namesF[random.Next(0, namesF.Count)];
            string surname = surnames[random.Next(0, surnames.Count)];
            return $"{name} {surname}";
        }
        public static async Task<(List<TestType>, List<TestPanel>)> GetDbTestTypes()
        {
            Wv1Context context = new Wv1Context();
            List<TestType> testTypes = await context.TestTypes.Include(x => x.TestNormalValues).ToListAsync();
            List<TestPanel> testPanel = await context.TestPanels.Include(x => x.TestTypes).ToListAsync();
            return (testTypes, testPanel);
        }
        public (List<TestType>, List<TestPanel>) GetDefaultTestTypes()
        {
            List<TestType> testTypes = Reader.GetTestTypes();
            List<TestPanel> testPanels = Reader.GetTestPanels();
            List<TestNormalValue> testNormalValues = Reader.GetTestNormalValues();
            List<TestPanelContentItem> panelContents = Reader.GetContentItems();
            for (int i = 0; i < panelContents.Count; i++)
            {
                for (int j = 0; j < testPanels.Count; j++)
                {
                    if (testPanels[j].TestPanelId == panelContents[i].PanelId)
                    {
                        for (int k = 0; k < testTypes.Count; k++)
                        {
                            if (panelContents[i].TestTypeId == testTypes[k].TestTypeId)
                            {
                                testPanels[j].TestTypes.Add(testTypes[k]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < testTypes.Count; i++)
            {
                for (int j = 0; j < testNormalValues.Count; j++)
                {
                    if (testTypes[i].TestTypeId == testNormalValues[j].TestTypeId)
                    {

                        testTypes[i].TestNormalValues.Add(testNormalValues[j]);

                    }
                }
            }
            return (testTypes, testPanels);
        }
    }
}
