using LiteDB;
using System;
using System.Collections.Generic;

namespace PackTrack.Models
{
    // --- CORE HIERARCHY ---

    // 1. PLANTS
    public class Plant
    {
        [BsonId]
        public int PlantId { get; set; }
        public string PlantCode { get; set; } = string.Empty; // QR-able
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Country { get; set; } = "India";
        public string Timezone { get; set; } = "IST";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    // 2. PRODUCTION BATCH
    public class ProductionBatch
    {
        [BsonId]
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string BatchCode { get; set; } = string.Empty; // B-20260221-3130
        
        // Bottle/Container Specs
        public string BottleType { get; set; } = string.Empty; // Thick / Thin / rPET
        public string BottleMaterialGrade { get; set; } = string.Empty;
        public int BottleThicknessMicron { get; set; }
        public int CapacityML { get; set; } // 250/500/1000
        public string ContainerType { get; set; } = "Bottle"; // Bottle / Can

        // Liquid Specs
        public string LiquidType { get; set; } = string.Empty; // Cola / Zero / etc
        public string LiquidBatchCode { get; set; } = string.Empty;
        public double BrixLevel { get; set; }
        public double AcidityPH { get; set; }
        public double CO2Volumes { get; set; }
        public string IngredientsList { get; set; } = string.Empty;

        // Production Metadata
        public string ProductionLineId { get; set; } = string.Empty;
        public string MachineId { get; set; } = string.Empty;
        public string ShiftCode { get; set; } = string.Empty;
        public string OperatorId { get; set; } = string.Empty;
        public string SupervisorId { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        public int TotalPlannedUnits { get; set; }
        public int TotalProducedUnits { get; set; }
        public int TotalRejectedUnits { get; set; }
        
        public string QualityStatus { get; set; } = "Pending"; // Pending/Approved/Rejected
        public string Status { get; set; } = "Produced"; // Produced / Packed / Shipped
        
        // Commercial
        public string TargetMarket { get; set; } = string.Empty;
        public string DistributorId { get; set; } = string.Empty;
        public decimal WholesaleRate { get; set; }
        public decimal MRP { get; set; }
        public string TaxCode { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ApprovedAt { get; set; }

        [BsonIgnore]
        public int DaysToExpiry => (ExpiryDate - DateTime.Now).Days;
    }

    // 3. PALLETS
    public class Pallet
    {
        [BsonId]
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string PalletCode { get; set; } = string.Empty;
        public int CartonCount { get; set; }
        public int TotalUnits { get; set; }
        public double NetWeight { get; set; }
        public double GrossWeight { get; set; }
        public string Status { get; set; } = "Packed"; // Packed/Shipped
        public DateTime? DispatchReadyAt { get; set; }
    }

    // 4. CARTONS
    public class Carton
    {
        [BsonId]
        public int Id { get; set; }
        public int PalletId { get; set; }
        public int BatchId { get; set; }
        public string CartonCode { get; set; } = string.Empty;
        public int UnitsPerCarton { get; set; }
        public int CurrentUnits { get; set; }
        public double Weight { get; set; }
        public string Status { get; set; } = "Packed";
    }

    // 5. BOTTLE UNITS
    public class BottleUnit
    {
        [BsonId]
        public string BottleId { get; set; } = string.Empty; // Unique QR ID
        public int BatchId { get; set; }
        public int? PalletId { get; set; }
        public int? CartonId { get; set; }
        
        // Snapshot Specs
        public string BottleType { get; set; } = string.Empty;
        public string ContainerType { get; set; } = "Bottle";
        public int ThicknessMicron { get; set; }
        public int CapacityML { get; set; }
        public string MaterialGrade { get; set; } = string.Empty;
        public string LiquidType { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        // Telemetry
        public double FillTemperature { get; set; }
        public double PressureAtSeal { get; set; }
        public double CarbonationLevel { get; set; }
        public double pHValue { get; set; }
        public double BrixLevel { get; set; }
        public double CO2Volumes { get; set; }
        
        public string CapType { get; set; } = "Standard";
        public string LabelVersion { get; set; } = "v1.0";
        
        public string CurrentStatus { get; set; } = "Produced"; // Produced/InTransit/Retail/Sold/Returned/Recycled
        public string CurrentLocationType { get; set; } = "Plant"; // Plant/Truck/Retail/Consumer/Recovery
        public string CurrentLocationId { get; set; } = string.Empty;
        public int RecycleCycleCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonIgnore]
        public bool IsExpired => DateTime.Now > ExpiryDate;
    }

    // 6. SHIPMENTS (LOGISTICS)
    public class Shipment
    {
        [BsonId]
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string TruckId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public DateTime PickupDate { get; set; }
        public DateTime? DispatchTime { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public string RouteCode { get; set; } = string.Empty;
        public string Status { get; set; } = "InTransit"; // InTransit/Delivered
        
        // IoT Logs
        public string TemperatureLogId { get; set; } = string.Empty;
        public string HumidityLogId { get; set; } = string.Empty;
        public string ShockLogId { get; set; } = string.Empty;
    }

    // 7. TRUCKS
    public class Truck
    {
        [BsonId]
        public string TruckId { get; set; } = string.Empty;
        public string TruckNumber { get; set; } = string.Empty;
        public string GPSDeviceId { get; set; } = string.Empty;
        public int CapacityUnits { get; set; }
        public string Status { get; set; } = "Available";
    }

    // 8. RETAIL LOCATIONS
    public class RetailLocation
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Country { get; set; } = "India";
        public string ContactPerson { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }

    // 9. RACKS (Retail shelf tracking)
    public class RetailRack
    {
        [BsonId]
        public int RackId { get; set; }
        public int RetailId { get; set; }
        public string RackCode { get; set; } = string.Empty;
        public int CapacityUnits { get; set; }
        public int CurrentUnits { get; set; }
        public string BottleTypeAssigned { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    // 10. RECOVERY CENTERS
    public class RecoveryCenter
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CapacityPerDay { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // 11. RECYCLING EVENTS
    public class RecyclingEvent
    {
        [BsonId]
        public int Id { get; set; }
        public string BottleId { get; set; } = string.Empty;
        public int CenterId { get; set; }
        public DateTime ReturnedAt { get; set; } = DateTime.Now;
        public DateTime? VerifiedAt { get; set; }
        public string MaterialGradeAfterSort { get; set; } = string.Empty;
        public string RecyclePlantId { get; set; } = string.Empty;
        public double CarbonSavedGrams { get; set; }
        public string Status { get; set; } = "Returned";
    }

    // 12. MOVEMENT LOG (CRITICAL)
    public class BottleMovement
    {
        [BsonId]
        public int Id { get; set; }
        public string BottleId { get; set; } = string.Empty;
        public string FromLocationType { get; set; } = string.Empty;
        public string FromLocationId { get; set; } = string.Empty;
        public string ToLocationType { get; set; } = string.Empty;
        public string ToLocationId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty; // Produced/Shipped/Retail/Sold/Returned/Recycled
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;
    }

    public class QrRegistry
    {
        [BsonId]
        public string QrId { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
