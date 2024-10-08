using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
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
            int usernameLength = random.Next(8, 100);
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
        public string GenerateDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days + 1;
            DateTime randomDate = start.AddDays(random.Next(range));
            return randomDate.ToString("yyyy-MM-dd");
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
    }
}
