using PackTrack.Models;

namespace PackTrack.Services
{
    public class PredictionService
    {
        private readonly InventoryService _inventoryService;

        public PredictionService(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public List<PredictionModel> GetPredictions()
        {
            // Mock logic: In a real app, this would analyze past sales history.
            // Here we just look at current stock and suggest reorders if low.
            
            var predictions = new List<PredictionModel>();
            var stock = _inventoryService.GetAllStock();

            foreach (var item in stock)
            {
                var type = item.Key;
                var qty = item.Value;
                
                // Simple threshold logic
                if (qty < 100)
                {
                    predictions.Add(new PredictionModel
                    {
                        BottleType = type,
                        PredictedDemand = "High",
                        SuggestedReorder = 500,
                        Reason = "Stock critical (< 100)"
                    });
                }
                else if (qty < 300)
                {
                     predictions.Add(new PredictionModel
                    {
                        BottleType = type,
                        PredictedDemand = "Moderate",
                        SuggestedReorder = 200,
                        Reason = "Stock nearing reorder point"
                    });
                }
                else
                {
                     predictions.Add(new PredictionModel
                    {
                        BottleType = type,
                        PredictedDemand = "Low",
                        SuggestedReorder = 0,
                        Reason = "Stock sufficient"
                    });
                }
            }

            return predictions;
        }

        public List<PredictionModel> GetRackAdvisory(IEnumerable<RetailRack> racks)
        {
            var advisories = new List<PredictionModel>();
            foreach (var rack in racks)
            {
                double percentage = (double)rack.CurrentUnits / rack.CapacityUnits * 100;
                
                if (percentage < 20)
                {
                    advisories.Add(new PredictionModel
                    {
                        BottleType = $"{rack.RackCode} ({rack.BottleTypeAssigned})",
                        PredictedDemand = "CRITICAL",
                        SuggestedReorder = rack.CapacityUnits - rack.CurrentUnits,
                        Reason = "Stock depleted below 20%"
                    });
                }
                else if (percentage < 50)
                {
                    advisories.Add(new PredictionModel
                    {
                        BottleType = $"{rack.RackCode} ({rack.BottleTypeAssigned})",
                        PredictedDemand = "REFILL",
                        SuggestedReorder = (int)((rack.CapacityUnits - rack.CurrentUnits) * 0.8),
                        Reason = "Optimizing shelf space"
                    });
                }
            }
            return advisories;
        }
    }


    public class PredictionModel
    {
        public string BottleType { get; set; } = "";
        public string PredictedDemand { get; set; } = "";
        public int SuggestedReorder { get; set; }
        public string Reason { get; set; } = "";
    }
}
