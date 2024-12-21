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
