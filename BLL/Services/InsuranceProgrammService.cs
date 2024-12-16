using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InsuranceProgrammService
    {
        InsuranceDBEntities1 db;
        public InsuranceProgrammService()
        {
            db = new InsuranceDBEntities1();
        }
        public List<string> GetProgramTypeOptions()
        {
            return db.InsuranceProgram
                .Select(p => p.Name)
                .Distinct()
                .ToList();
        }

    }
}
