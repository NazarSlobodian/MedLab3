using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Model.MedLabTypes;

namespace MedLab.Model.Utils
{
    public class MockDatabaseGenerator
    {
        public void Generate()
        {
            RandomDataGenerator randomDataGenerator = new RandomDataGenerator();
            Random random = new Random();
            int amountOfLabs = random.Next(1, 11);
            List<Laboratory> laboratories = new List<Laboratory>();
            for (int labsGenerated = 0; labsGenerated < amountOfLabs;)
            {
                string email = randomDataGenerator.GenerateEmail();
                string contactNumber = randomDataGenerator.GeneratePhoneNumber();
                string address = randomDataGenerator.GenerateAddress();
                bool validLabValues = true;
                for (int i = 0; i < laboratories.Count; i++)
                {
                    if (email == laboratories[i].Email)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (contactNumber == laboratories[i].ContactNumber)
                    {
                        validLabValues = false;
                        break;
                    }
                    if (address == laboratories[i].Address)
                    {
                        validLabValues = false;
                        break;
                    }
                }
                if (!validLabValues)
                {
                    continue;
                }
                laboratories.Add(new Laboratory()
                {
                    Email = email,
                    ContactNumber = contactNumber,
                    Address = address,

                });
                labsGenerated++;
            }


            return;
        }
    }
}
