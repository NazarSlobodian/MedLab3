using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using MedLab.Model.MedLabTypes;
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
        private List<string> names = new List<string>()
        {
            "Saul",
            "Petro",
            "Pedro",
            "Pavlo",
            "Roman",
            "Vlad",
            "Anastasia",
            "Katherine",
            "Mike"
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
        public char GenerateGender()
        {
            if (random.Next(0, 2) == 1)
            {
                return 'm';
            }
            return 'f';
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
        public double RandomTestResult()
        {
            return random.NextDouble() * 1000.0;
        }
        public char GenerateBatchStatus()
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return 'q';
                case 1:
                    return 'p';
                case 2:
                    return 's';
                default:
                    return '4';
            }
        }
        public string GenerateMeasurementUnit()
        {
            return measurementUnits[random.Next(0, measurementUnits.Count)];
        }
        public string GenerateFullname()
        {
            string name = names[random.Next(0, names.Count)];
            string surname = surnames[random.Next(0, surnames.Count)];
            return $"{name} {surname}";
        }
        public (List<TestType>, List<TestCollection>) GetTestTypes()
        {
            List<TestType> testTypes = new List<TestType>()
            {
                new TestType()
                {
                    TestTypeID = 1,
                    TestName = "Hemoglobin",
                    Cost = 100.0m,
                    DaysTillOverdue = 7,
                    MeasurementUnit = "g/dL",
                    TestNormalValues = new List<TestNormalValues>()
                    {
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 1,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'm',
                            MinResValue = 13.8,
                            MaxResValue = 17.2
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 2,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'f',
                            MinResValue = 12.1,
                            MaxResValue = 15.1
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 3,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'm',
                            MinResValue = 12.9,
                            MaxResValue = 16.1
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 4,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'f',
                            MinResValue = 11.7,
                            MaxResValue = 14.7
                        }
                    }
                },
                new TestType()
                {
                    TestTypeID = 2,
                    TestName = "Leukocytes",
                    Cost = 120.0m,
                    DaysTillOverdue = 5,
                    MeasurementUnit = "cells/mcL",
                    TestNormalValues = new List<TestNormalValues>()
                    {
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 5,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'm',
                            MinResValue = 4.5,
                            MaxResValue = 11.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 6,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'f',
                            MinResValue = 4.5,
                            MaxResValue = 11.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 7,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'm',
                            MinResValue = 4.5,
                            MaxResValue = 11.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 8,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'f',
                            MinResValue = 4.5,
                            MaxResValue = 11.0
                        }
                    }
                },
                new TestType()
                {
                    TestTypeID = 3,
                    TestName = "Glucose",
                    Cost = 110.0m,
                    DaysTillOverdue = 5,
                    MeasurementUnit = "mg/dL",
                    TestNormalValues = new List<TestNormalValues>()
                    {
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 9,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'm',
                            MinResValue = 70.0,
                            MaxResValue = 100.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 10,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'f',
                            MinResValue = 70.0,
                            MaxResValue = 100.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 11,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'm',
                            MinResValue = 70.0,
                            MaxResValue = 100.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 12,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'f',
                            MinResValue = 70.0,
                            MaxResValue = 100.0
                        }
                    }
                },
                new TestType()
                {
                    TestTypeID = 4,
                    TestName = "Createnine",
                    Cost = 130.0m,
                    DaysTillOverdue = 6,
                    MeasurementUnit = "mg/dL",
                    TestNormalValues = new List<TestNormalValues>()
                    {
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 13,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'm',
                            MinResValue = 0.74,
                            MaxResValue = 1.35
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 14,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'f',
                            MinResValue = 0.59,
                            MaxResValue = 1.04
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 15,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'm',
                            MinResValue = 0.7,
                            MaxResValue = 1.3
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 16,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'f',
                            MinResValue = 0.55,
                            MaxResValue = 1.0
                        }
                    }
                },
                new TestType()
                {
                    TestTypeID = 5,
                    TestName = "Alanine Aminotransferase",
                    Cost = 125.0m,
                    DaysTillOverdue = 10,
                    MeasurementUnit = "U/L",
                    TestNormalValues = new List<TestNormalValues>()
                    {
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 17,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'm',
                            MinResValue = 10.0,
                            MaxResValue = 40.0
                        },
                        new TestNormalValues()
                        {
                        TestNormalValuesID = 18,
                            MinAge = 18,
                            MaxAge = 45,
                            Gender = 'f',
                            MinResValue = 7.0,
                            MaxResValue = 35.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 19,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'm',
                            MinResValue = 10.0,
                            MaxResValue = 50.0
                        },
                        new TestNormalValues()
                        {
                            TestNormalValuesID = 20,
                            MinAge = 46,
                            MaxAge = 100,
                            Gender = 'f',
                            MinResValue = 7.0,
                            MaxResValue = 45.0
                        }
                    }
                }
            };
            List<TestCollection> testCollections = new List<TestCollection>()
            {
                new TestCollection()
                {
                    TestCollectionID = 1,
                    TestCollectionName = "Complete Blood Count Panel",
                    TestTypes = new List<TestType>()
                    {
                        testTypes.Single( (x) => x.TestName == "Hemoglobin"),
                        testTypes.Single( (x) => x.TestName == "Leukocytes")
                    }
                },
                new TestCollection()
                {
                    TestCollectionID = 2,
                    TestCollectionName = "Basic Metabolic Panel",
                    TestTypes = new List<TestType>()
                    {
                        testTypes.Single( (x) => x.TestName == "Glucose"),
                        testTypes.Single( (x) => x.TestName == "Createnine")
                    }
                }
            };
            return (testTypes, testCollections);
        }
    }
}
