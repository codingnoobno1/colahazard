namespace PackTrack.Models.Dtos
{
    // ── Dashboard Overview ──────────────────────────────────────
    public record DashboardOverviewDto(
        int TotalStock,
        int TotalRacks,
        int TotalInvoices,
        decimal TotalRevenue,
        string TopDemand,
        int AlertCount,
        DateTime LastUpdated
    );

    public record DashboardStatsDto(
        int ThickBottles,
        int ThinBottles,
        int RPetBottles,
        int TotalStock,
        decimal StockChangePercent,
        string TrendDirection  // "up" | "down" | "stable"
    );

    // ── Inventory ───────────────────────────────────────────────
    public record InventorySnapshotDto(
        string BottleType,
        int Quantity,
        string RackId,
        DateTime LastUpdated,
        string Status  // "critical" | "low" | "sufficient"
    );

    public record StockAdjustRequest(
        string BottleType,
        int Change
    );

    public record StockAssignRackRequest(
        string BottleType,
        string RackId
    );

    public record InventoryBreakdownDto(
        Dictionary<string, int> ByType,
        Dictionary<string, int> ByRack,
        int TotalItems
    );

    // ── Rack ────────────────────────────────────────────────────
    public record RackDto(
        int Id,
        string RackName,
        int MaxCapacity,
        int CurrentCount,
        string BottleType,
        double UtilizationPercent,
        DateTime LastUpdated
    );

    public record CreateRackRequest(
        string RackName,
        int MaxCapacity,
        string BottleType
    );

    public record RackCapacityDto(
        int TotalCapacity,
        int TotalUsed,
        int TotalFree,
        double OverallUtilizationPercent,
        List<RackDto> Racks
    );

    // ── Invoices ────────────────────────────────────────────────
    public record InvoiceSummaryDto(
        int Id,
        string InvoiceNumber,
        string CustomerName,
        DateTime Date,
        decimal TotalAmount,
        int ItemCount
    );

    public record CreateInvoiceRequest(
        string CustomerName,
        List<InvoiceItemRequest> Items
    );

    public record InvoiceItemRequest(
        string Description,
        int Quantity,
        decimal UnitPrice
    );

    public record InvoiceDetailDto(
        int Id,
        string InvoiceNumber,
        string CustomerName,
        DateTime Date,
        decimal SubTotal,
        decimal GstAmount,
        decimal GrandTotal,
        List<InvoiceLineDto> Items
    );

    public record InvoiceLineDto(
        string Description,
        int Quantity,
        decimal UnitPrice,
        decimal LineTotal,
        decimal GstAmount
    );

    public record RevenueDto(
        decimal TotalRevenue,
        decimal TotalGst,
        decimal GrandTotal,
        int InvoiceCount,
        decimal AverageInvoiceValue
    );

    // ── Predictions ─────────────────────────────────────────────
    public record PredictionDto(
        string BottleType,
        string PredictedDemand,
        int SuggestedReorder,
        string Reason,
        string Urgency  // "immediate" | "soon" | "none"
    );

    public record ReorderSummaryDto(
        int TotalReorderItems,
        int TotalReorderQuantity,
        List<PredictionDto> Predictions
    );

    // ── OCR ─────────────────────────────────────────────────────
    public record OcrScanRequest(
        string Base64Image
    );

    public record OcrScanResultDto(
        string SupplierName,
        DateTime Date,
        decimal TotalAmount,
        List<OcrDetectedItemDto> Items
    );

    public record OcrDetectedItemDto(
        string Description,
        int Quantity,
        decimal UnitPrice,
        decimal LineTotal
    );

    // ── Shelf Calculator ────────────────────────────────────────
    public record ShelfCalcRequest(
        double ShelfWidthCm,
        double ShelfHeightCm,
        double ShelfDepthCm,
        string BottleType  // "Thick" | "Thin" | "rPET"
    );

    public record ShelfCalcResultDto(
        int MaxBottles,
        int Rows,
        int ColumnsPerRow,
        double BottleWidthCm,
        double BottleHeightCm,
        double WastedSpacePct,
        string BottleType
    );

    // ── Activity / Timeline ─────────────────────────────────────
    public record ActivityDto(
        string Type,  // "invoice" | "stock_update" | "rack_created" | "scan"
        string Title,
        string Description,
        DateTime Timestamp,
        string Icon
    );

    // ── Health / Status ─────────────────────────────────────────
    public record ApiHealthDto(
        string Status,
        string Version,
        DateTime ServerTime,
        long UptimeSeconds,
        DatabaseStatusDto Database
    );

    public record DatabaseStatusDto(
        bool Connected,
        long FileSizeBytes,
        int InventoryCount,
        int RackCount,
        int InvoiceCount
    );
}
