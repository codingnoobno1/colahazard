using System.Text;
using PackTrack.Models;
using System.Linq;

namespace PackTrack.Services
{
    public class ReportingService
    {
        private readonly FactoryService _factoryService;

        public ReportingService(FactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        public byte[] GenerateProductionCsv()
        {
            var batches = _factoryService.GetAllBatches();
            var csv = new StringBuilder();
            csv.AppendLine("BatchCode,BottleType,Planned,Produced,Rejected,Status,ProductionDate,WholesaleRate,TotalRevenue,EstimatedProfit");

            foreach (var b in batches)
            {
                var revenue = (b.TotalProducedUnits - b.TotalRejectedUnits) * b.WholesaleRate;
                var profit = revenue * 0.45m; 
                csv.AppendLine($"{b.BatchCode},{b.BottleType},{b.TotalPlannedUnits},{b.TotalProducedUnits},{b.TotalRejectedUnits},{b.Status},{b.ManufactureDate:yyyy-MM-dd},{b.WholesaleRate},{revenue},{profit}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        public byte[] GenerateIndustrialLedgerCsv()
        {
            var batches = _factoryService.GetAllBatches();
            var csv = new StringBuilder();
            csv.AppendLine("Timestamp,EntityId,Layer,Movement,From,To,RevenueImpact,TaxCode");

            foreach (var b in batches)
            {
                var deliveredUnits = b.TotalProducedUnits - b.TotalRejectedUnits;
                var revenue = deliveredUnits * b.WholesaleRate;
                
                csv.AppendLine($"{b.ManufactureDate:yyyy-MM-dd HH:mm},{b.BatchCode},Batch,Produced,Plant,{b.PlantId},0,{b.TaxCode}");
                
                if (b.Status == "Shipped" || b.Status == "Delivered")
                {
                    csv.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm},{b.BatchCode},Shipment,InTransit,Plant,Logistics,0,{b.TaxCode}");
                }
                
                if (b.Status == "Delivered")
                {
                    csv.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm},{b.BatchCode},Retail,Sold,Logistics,Retailer,{revenue},{b.TaxCode}");
                }
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        public FinancialSummary GetFinancialSummary()
        {
            var batches = _factoryService.GetAllBatches();
            var totalRevenue = batches.Sum(b => (b.TotalProducedUnits - b.TotalRejectedUnits) * b.WholesaleRate);
            var totalProfit = totalRevenue * 0.45m;

            return new FinancialSummary
            {
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
                ProfitMargin = 45.0,
                BatchCount = batches.Count(),
                TotalUnits = batches.Sum(b => b.TotalProducedUnits)
            };
        }
    }

    public class FinancialSummary
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public double ProfitMargin { get; set; }
        public int BatchCount { get; set; }
        public int TotalUnits { get; set; }
    }
}
