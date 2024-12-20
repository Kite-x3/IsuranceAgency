﻿using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InsuranceTypeService
    {
        InsuranceDBEntities1 db;
        public InsuranceTypeService() {
            db = new InsuranceDBEntities1();
        }
        public List<string> GetAllInsuranceTypes()
        {
            var insuranceTypes = db.CaseType.Select(c => c.Situation).ToList();

            insuranceTypes.Insert(0, "Без фильтра");

            return insuranceTypes;
        }

    }
}
