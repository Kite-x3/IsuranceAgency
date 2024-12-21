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
        public List<InsuranceProgram> GetInsuranceProgramList()
        {
            return db.InsuranceProgram.ToList();
        }
        public List<string> GetProgramTypeOptions()
        {
            var programTypes = db.InsuranceProgram
                                  .Select(p => p.Name)
                                  .Distinct()
                                  .ToList();

            // вариант "Без фильтра" в начало списка
            programTypes.Insert(0, "Без фильтра");

            return programTypes;
        }


    }
}
