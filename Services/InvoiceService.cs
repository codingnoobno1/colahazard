using PackTrack.Data.Repositories;
using PackTrack.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PackTrack.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _repo;

        public InvoiceService(InvoiceRepository repo)
        {
            _repo = repo;
            // License key for QuestPDF (Community)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public void CreateInvoice(Invoice invoice)
        {
            invoice.Date = DateTime.Now;
            _repo.Upsert(invoice);
        }

        public IEnumerable<Invoice> GetInvoices()
        {
            return _repo.GetAll();
        }

        public byte[] GeneratePdf(Invoice invoice)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("TAX INVOICE").SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);
                                c.Item().Text("PackSight Retailer Intelligence").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text("123, Industrial Area, New Delhi - 110020").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text("GSTIN: 07AABCU9603R1Z2").FontSize(10).FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(150).AlignRight().Column(c =>
                            {
                                c.Item().Text($"Invoice #: {invoice.InvoiceNumber}").SemiBold();
                                c.Item().Text($"Date: {invoice.Date:dd-MMM-yyyy}");
                            });
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Text($"Bill To:").SemiBold();
                            x.Item().Text($"{invoice.CustomerName}").FontSize(14);
                            x.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                            x.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); // Description
                                    columns.RelativeColumn();  // Rate
                                    columns.RelativeColumn();  // Qty
                                    columns.RelativeColumn();  // Tax (18%)
                                    columns.RelativeColumn();  // Total
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Description");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Rate");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                                    header.Cell().Element(CellStyle).AlignRight().Text("GST (18%)");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var item in invoice.Items)
                                {
                                    var gstAmount = item.Total * 0.18m;
                                    var totalWithTax = item.Total + gstAmount;

                                    table.Cell().Element(CellStyle).Text(item.Description);
                                    table.Cell().Element(CellStyle).AlignRight().Text($"₹{item.UnitPrice:N2}");
                                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                                    table.Cell().Element(CellStyle).AlignRight().Text($"₹{gstAmount:N2}");
                                    table.Cell().Element(CellStyle).AlignRight().Text($"₹{totalWithTax:N2}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingVertical(5);
                                    }
                                }
                            });

                            var totalTax = invoice.Items.Sum(i => i.Total * 0.18m);
                            var grandTotal = invoice.TotalAmount + totalTax;

                            x.Item().PaddingTop(20).Column(c => 
                            {
                                c.Item().AlignRight().Text($"Subtotal: ₹{invoice.TotalAmount:N2}");
                                c.Item().AlignRight().Text($"Total GST (18%): ₹{totalTax:N2}");
                                c.Item().AlignRight().Text($"Grand Total: ₹{grandTotal:N2}").SemiBold().FontSize(16).FontColor(Colors.Blue.Darken2);
                            });
                            
                            x.Item().PaddingTop(30).Text("This is a computer-generated invoice.").FontSize(10).FontColor(Colors.Grey.Medium).AlignCenter();
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
