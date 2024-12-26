using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class InsuranceCaseDTO
    {
        public int CaseID { get; set; }
        public int ContractID { get; set; }
        public int ContractNumber { get; set; }

        public decimal Payout {  get; set; }
        public string CaseTypeName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string signed { get; set; }
    }
}
