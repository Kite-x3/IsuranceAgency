using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ContractFilterDTO
    {
        public int ContractID { get; set; }
        public int Number { get; set; }
        public string ClientName { get; set; }
        public string ObjectAddress { get; set; }
        public decimal Cost { get; set; }
        public string Comment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProgramName { get; set; }
        public bool? signed { get; set; }
        public bool? ready { get; set; }
        public string InsuranceAgent {  get; set; }
    }
}
