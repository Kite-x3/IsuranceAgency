using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{

    public class InsurancePeriodService
    {
        InsuranceDBEntities1 db;
        public InsurancePeriodService()
        {
            db = new InsuranceDBEntities1();
        }
        public List<TimingOptions> GetInsurancePeriods()
        {
            return db.TimingOptions.ToList();
        }
    }
}
