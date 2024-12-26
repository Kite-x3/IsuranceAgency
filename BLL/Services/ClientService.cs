using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ClientService
    {
        InsuranceDBEntities1 db;
        public ClientService()
        {
            db = new InsuranceDBEntities1();

        }

        public void AddClient(string name, string passport, DateTime birthDate, string password, string email)
        {
            var existingClient = db.Client.FirstOrDefault(c => c.Passport == passport);
            if (existingClient != null)
            {
                throw new ArgumentException("Клиент с таким паспортом уже существует.");
            }

            int newClientId = db.Client.Any() ? db.Client.Max(c => c.ClientID) + 1 : 1;

            var newClient = new Client
            {
                ClientID = newClientId,
                FullName = name,
                Passport = passport,
                BirthDate = birthDate,
                Password = password,
                Email = email
            };

            db.Client.Add(newClient);
            db.SaveChanges();
        }


        public int GetAgeOfUser(int userId)
        {
            var client = db.Client.FirstOrDefault(c => c.ClientID == userId);
            if (client != null)
            {
                var birthDate = client.BirthDate;
                var today = DateTime.Today;
                int age = today.Year - birthDate.Year;

                if (birthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }

            return 0;
        }
        
    }
}
