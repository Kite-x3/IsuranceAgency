using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LoginService
    {
        InsuranceDBEntities1 db;
        public LoginService()
        {
            db = new InsuranceDBEntities1();

        }
        public string CheckLoginAndPassword(string Username, string Password)
        {
            if (Username == null || Password == null)
            {
                return "";
            }
            else if(Username.StartsWith("Ag_"))
            {
                var agent = db.InsuranceAgent
                .FirstOrDefault(a => a.FullName == Username && a.Password == Password);

                if (agent != null)
                {
                    return $"agent:{agent.InsuranceAgentID}";
                }
            }
            else
            {
                var client = db.Client
                .FirstOrDefault(a => a.FullName == Username && a.Password == Password);

                if (client != null)
                {
                    return $"client:{client.ClientID}";
                }
            }

            return "";
        }
    }
}
