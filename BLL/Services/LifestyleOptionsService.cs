using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LifestyleOptionsService
    {
        InsuranceDBEntities1 db;
        public LifestyleOptionsService()
        {
            db = new InsuranceDBEntities1();
        }
        public List<LifestyleOptions> GetAllLifestyleOptions()
        {
            return db.LifestyleOptions.ToList();
        }
    }
}
