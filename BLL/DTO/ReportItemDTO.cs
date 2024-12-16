using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ReportItemDTO
    {
        public int TotalContracts { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalPayouts { get; set; }
        public decimal NetProfit { get; set; }
        public string MostProfitableProgram { get; set; }
    }
}
