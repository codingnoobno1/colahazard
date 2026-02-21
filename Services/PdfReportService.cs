using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PackTrack.Models;

namespace PackTrack.Services
{
    public class PdfReportService
    {
        private readonly FactoryService _factoryService;

        public PdfReportService(FactoryService factoryService)
        {
            _factoryService = factoryService;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateProductionReport()
        {
            var batches = _factoryService.GetAllBatches().ToList();
            var stats = _factoryService.GetProductionStats();
            var profit = _factoryService.GetTotalProfit();

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
                            col.Item().Text("EcoCola Enterprise").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Factory Production Summary").FontSize(14).Italic();
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text($"Date: {DateTime.Now:yyyy-MM-dd HH:mm}");
                            col.Item().Text("Report ID: ERP-EXP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper());
                        });
                    });

                    page.Content().PaddingVertical(10).Column(x =>
                    {
                        x.Spacing(10);

                        // KPI Section
                        x.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).Padding(5).Column(c =>
                            {
                                c.Item().Text("Total Units").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(batches.Sum(b => b.TotalProducedUnits).ToString()).FontSize(14).SemiBold();
                            });
                            row.Spacing(10);
                            row.RelativeItem().Border(1).Padding(5).Column(c =>
                            {
                                c.Item().Text("Rejected Units").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(batches.Sum(b => b.TotalRejectedUnits).ToString()).FontSize(14).SemiBold().FontColor(Colors.Red.Medium);
                            });
                            row.Spacing(10);
                            row.RelativeItem().Border(1).Padding(5).Column(c =>
                            {
                                c.Item().Text("Estimated Profit").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"₹{profit:N0}").FontSize(14).SemiBold().FontColor(Colors.Green.Medium);
                            });
                        });

                        x.Item().Text("Detailed Batch Log").FontSize(12).SemiBold().Underline();

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
                                header.Cell().Element(CellStyle).Text("Batch Code");
                                header.Cell().Element(CellStyle).Text("Type");
                                header.Cell().Element(CellStyle).Text("Produced");
                                header.Cell().Element(CellStyle).Text("Rejected");
                                header.Cell().Element(CellStyle).Text("Status");

                                static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            foreach (var b in batches)
                            {
                                table.Cell().Element(ContentStyle).Text(b.BatchCode);
                                table.Cell().Element(ContentStyle).Text(b.BottleType);
                                table.Cell().Element(ContentStyle).Text(b.TotalProducedUnits.ToString());
                                table.Cell().Element(ContentStyle).Text(b.TotalRejectedUnits.ToString());
                                table.Cell().Element(ContentStyle).Text(b.Status);

                                static IContainer ContentStyle(IContainer container) => container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateQrLabels(string entityType, List<string> qrCodes)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        foreach (var code in qrCodes)
                        {
                            table.Cell().Padding(5).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(c =>
                            {
                                c.Item().AlignCenter().Text(entityType.ToUpper()).FontSize(8).SemiBold();
                                c.Item().AlignCenter().PaddingVertical(5).Height(80).AlignMiddle().Text("[QR CODE PLACEHOLDER]").FontSize(8).Italic().FontColor(Colors.Grey.Medium);
                                c.Item().AlignCenter().Text(code).FontSize(10).SemiBold();
                                c.Item().AlignCenter().Text("Certified by PackSight").FontSize(6);
                            });
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }

    }
}

