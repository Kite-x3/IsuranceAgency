using BLL.DTO;
using DAL;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;



namespace BLL.Services
{
    public class ReportService
    {
        InsuranceDBEntities1 db;
        public ReportService()
        {
            db = new InsuranceDBEntities1();

        }
        public void GeneratePdfReport(List<ReportItemDTO> reportData, DateTime startDate, DateTime endDate, string savePath = null)
        {
            // If no save path is provided, use the default path
            if (string.IsNullOrEmpty(savePath))
            {
                savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Отчёт.pdf");
            }

            // Create the PDF document
            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Отчёт по договорам";

                // Create the first page
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Set fonts
                XFont boldFont = new XFont("Arial", 20);
                XFont regularFont = new XFont("Arial", 12);

                // Header
                gfx.DrawString("Отчёт по договорам", boldFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.Center);

                // Space
                double yPosition = 60;

                // Add report period
                gfx.DrawString($"Период отчёта: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += 20;

                // Fill in the data
                foreach (var item in reportData)
                {
                    gfx.DrawString($"Всего договоров: {item.TotalContracts}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 20;
                    gfx.DrawString($"Общая стоимость: {item.TotalCost}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 20;
                    gfx.DrawString($"Всего выплат: {item.TotalPayouts}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 20;
                    gfx.DrawString($"Прибыль: {item.NetProfit}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 20;
                    gfx.DrawString($"Самая прибыльная программа: {item.MostProfitableProgram}", regularFont, XBrushes.Black, new XRect(50, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 40; // Add space between elements
                }

                // Save the PDF document
                document.Save(savePath);
            }

            // Notify user about the completion
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = savePath,
                UseShellExecute = true
            });
        }



        public ReportItemDTO GetReportData(DateTime startDate, DateTime endDate)
        {
            var contracts = from c in db.Contract
                            join cl in db.Client on c.ClientID equals cl.ClientID into clientGroup
                            from client in clientGroup.DefaultIfEmpty()
                            join p in db.Property on c.ObjectID equals p.PropertyID into propertyGroup
                            from property in propertyGroup.DefaultIfEmpty()
                            where c.StartDate >= startDate && c.EndDate <= endDate
                            select new
                            {
                                c.ContractID,
                                c.Number,
                                c.ProgramID,
                                ClientName = client != null ? client.FullName : "Неизвестный клиент",
                                ObjectAddress = c.ObjectID.HasValue && property != null
                                    ? property.Address
                                    : "Страхование жизни",
                                c.Cost,
                                c.StartDate,
                                c.EndDate,
                                Payouts = db.InsuranceCase
                                    .Where(ic => ic.ContractID == c.ContractID)
                                    .Sum(ic => (decimal?)ic.PayoutAmount) ?? 0
                            };

            var contractList = contracts.ToList();

            // Подсчет общих значений
            var totalContracts = contractList.Count;
            var totalCost = contractList.Sum(c => c.Cost);
            var totalPayouts = contractList.Sum(c => c.Payouts);
            var netProfit = totalCost - totalPayouts;

            // Определение наиболее прибыльной программы
            var programProfits = contractList
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
                mostProfitableProgram = (from ip in db.InsuranceProgram
                                         where ip.ProgramID == programProfits.ProgramID
                                         select ip.Name).FirstOrDefault();
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
