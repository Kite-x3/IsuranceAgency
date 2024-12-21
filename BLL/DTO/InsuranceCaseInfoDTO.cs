using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class InsuranceCaseInfoDTO
    {
        public int CaseID { get; set; }
        public int ContractNumber { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public string CaseTypeName { get; set; }
        public string Description { get; set; }
        public string Comment {  get; set; }
        public decimal Cost {  get; set; }
        public bool? signed { get; set; }
    }
}
