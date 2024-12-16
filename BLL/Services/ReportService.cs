using BLL.DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ReportService
    {
        InsuranceDBEntities1 db;
        public ReportService()
        {
            db = new InsuranceDBEntities1();

        }
        public ReportItemDTO GetReportData(DateTime startDate, DateTime endDate)
        {
           var contracts = db.Contract
           .Where(c => c.StartDate >= startDate && c.EndDate <= endDate)
           .Select(c => new
           {
               c.ContractID,
               c.Number,
               c.ProgramID,
               ClientName = c.Client.FullName,
               ObjectAddress = c.ObjectID.HasValue ? c.Property.Address : "Страхование жизни",
               c.Cost,
               c.StartDate,
               c.EndDate,
               Payouts = db.InsuranceCase
                .Where(ic => ic.ContractID == c.ContractID)
                .Sum(ic => ic.PayoutAmount)
           })
           .ToList();

            var totalContracts = contracts.Count;
            var totalCost = contracts.Sum(c => c.Cost);
            var totalPayouts = contracts.Sum(c => c.Payouts);
            var netProfit = totalCost - totalPayouts;

            var programProfits = contracts
            .GroupBy(c => c.ProgramID)
            .Select(group => new
            {
                ProgramID = group.Key,
                Profit = group.Sum(c => c.Cost) - group.Sum(c => c.Payouts)
            })
            .OrderByDescending(p => p.Profit)
            .FirstOrDefault();

            string mostProfitableProgram = null;
            if (programProfits != null)
            {
                mostProfitableProgram = db.InsuranceProgram
                    .Where(ip => ip.ProgramID == programProfits.ProgramID)
                    .Select(ip => ip.Name)
                    .FirstOrDefault();
            }
            return new ReportItemDTO
            {
                TotalContracts = totalContracts,
                TotalCost = totalCost,
                TotalPayouts = totalPayouts,
                NetProfit = netProfit,
                MostProfitableProgram = mostProfitableProgram
            };
        }
    }
}
