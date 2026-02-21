using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PackTrack.Models;
using PackTrack.Data;

namespace PackTrack.Services
{
    public class ManifestGenerator
    {
        private readonly LiteDbContext _db;

        public ManifestGenerator(LiteDbContext db)
        {
            _db = db;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateManifest(int shipmentId)
        {
            var shipment = _db.Shipments.FindById(shipmentId);
            if (shipment == null) return Array.Empty<byte>();

            var batch = _db.ProductionBatches.FindById(shipment.BatchId);
            var pallets = _db.Pallets.Find(x => x.BatchId == shipment.BatchId).ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("ECOCOLA INDUSTRIAL LOGISTICS").FontSize(22).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Official Shipment Manifest").FontSize(14).Italic();
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text($"Manifest Date: {DateTime.Now:yyyy-MM-dd HH:mm}");
                            col.Item().Text($"Shipment ID: {shipment.Id}");
                            col.Item().Text($"Route: {shipment.RouteCode}");
                        });
                    });

                    page.Content().PaddingVertical(10).Column(x =>
                    {
                        x.Spacing(15);

                        // Logistics Details
                        x.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).Padding(8).Column(c =>
                            {
                                c.Item().Text("TRUCK & DRIVER").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"Truck ID: {shipment.TruckId}").SemiBold();
                                c.Item().Text($"Driver: {shipment.DriverId}").SemiBold();
                            });
                            row.Spacing(10);
                            row.RelativeItem().Border(1).Padding(8).Column(c =>
                            {
                                c.Item().Text("LOAD SUMMARY").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"{pallets.Count} Pallets").SemiBold();
                                c.Item().Text($"{pallets.Sum(p => p.TotalUnits)} Units Total").SemiBold();
                            });
                        });

                        // IoT Section
                        x.Item().Background(Colors.Grey.Lighten4).Padding(8).Column(c =>
                        {
                            c.Item().Text("INDUSTRIAL TELEMETRY ACTIVE").FontSize(9).SemiBold().FontColor(Colors.Blue.Medium);
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text($"Temp Log: {shipment.TemperatureLogId}");
                                r.RelativeItem().Text($"Hum Log: {shipment.HumidityLogId}");
                                r.RelativeItem().Text($"Shock Log: {shipment.ShockLogId}");
                            });
                        });

                        x.Item().Text("HIERARCHICAL LOAD BREAKDOWN").FontSize(12).SemiBold().Underline();

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Pallet Code");
                                header.Cell().Element(CellStyle).Text("Units");
                                header.Cell().Element(CellStyle).Text("Cartons");
                                header.Cell().Element(CellStyle).Text("Weight (Kg)");
                                header.Cell().Element(CellStyle).Text("Status");

                                static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            foreach (var p in pallets)
                            {
                                table.Cell().Element(ContentStyle).Text(p.PalletCode);
                                table.Cell().Element(ContentStyle).Text(p.TotalUnits.ToString());
                                table.Cell().Element(ContentStyle).Text(p.CartonCount.ToString());
                                table.Cell().Element(ContentStyle).Text(p.GrossWeight.ToString("F1"));
                                table.Cell().Element(ContentStyle).Text(p.Status);

                                static IContainer ContentStyle(IContainer container) => container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            }
                        });

                        // Legal / Verification
                        x.Item().AlignCenter().PaddingTop(20).Column(c =>
                        {
                            c.Spacing(5);
                            c.Item().Text("INDUSTRIAL VALIDATION TOKEN").FontSize(8).FontColor(Colors.Grey.Medium);
                            c.Item().Text(batch?.BatchCode ?? "N/A").FontSize(12).SemiBold();
                            c.Item().Text("Certified by PackSight Industrial Mesh").FontSize(8).Italic();
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Page ");
                        t.CurrentPageNumber();
                        t.Span(" / ");
                        t.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
