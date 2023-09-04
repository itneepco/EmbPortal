using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Constants;
using EmbPortal.Shared.Responses.RABills;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace Api.Reports;

public class RABillReport : IDocument
{
    Bill01_data_source o = new();
    private readonly IWebHostEnvironment env;
    private readonly RABillReportResponse raBill;

    public RABillReport(IWebHostEnvironment env, RABillReportResponse raBill)
    {
        this.env = env;
        this.raBill = raBill;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());

            page.MarginLeft(10, Unit.Millimetre);
            page.MarginTop(10, Unit.Millimetre);
            page.MarginRight(10, Unit.Millimetre);
            page.MarginBottom(10, Unit.Millimetre);

            page.Header().Element(Header);
            page.Content().Element(Content);
            page.Footer().Element(Footer);
        });
    }

    public void Header(IContainer container)
    {
    }

    public void Content(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().PaddingTop(4, Unit.Millimetre).Row(row =>
            {
                row.ConstantItem(16, Unit.Millimetre).Height(16, Unit.Millimetre).Border(1, Unit.Point).Column(column =>
                {
                    var image = Path.Combine(env.ContentRootPath, FileConstant.FolderName, "neepcologo.png");
                    column.Item().Padding(0, Unit.Millimetre).Image(image);
                });

                row.RelativeItem().Border(1, Unit.Point).Column(column =>
                {
                    column.Item().AlignCenter().PaddingTop(4, Unit.Millimetre).Text(text =>
                    {
                        text.Span("NEEPCO RA BILL FORMAT").FontSize(18).Bold();
                    });
                });
            }
            );

            // Header first row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Bill No.:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span($"{raBill.RABillTitle}").FontSize(9);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(10, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Date:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.BillDate.ToString("dd-MMM-yyyy")).FontSize(9);
                    });
                });
            });

            // Header second row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("PO No.:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.PoNo).FontSize(9);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(10, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("EIC:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.EIC).FontSize(9);
                    });
                });
            });

            // Header third row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("MB No.:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.MBTitle).FontSize(9);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(35, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Measurement Officer:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.MeaurementOfficer).FontSize(9);
                    });
                });
            });

            // Header fourth row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(30, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Period of the Bill:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("[Period of the Bill manual entry]").FontSize(9);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(30, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Contractor Name:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.Contractor).FontSize(9);
                    });
                });
            });

            // Header fifth row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(55, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Date of Actual Completion of Work:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Manul Entry....").FontSize(9);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(60, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("No.and date of the last Bill for this work:").FontSize(9).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Manual entry............").FontSize(9);
                    });
                });
            });

            // Header sixth row
            column.Item().BorderBottom(1).PaddingTop(2, Unit.Millimetre).PaddingBottom(2, Unit.Millimetre).Row(row =>
            {
                row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                {
                    text.Span("Remarks:").FontSize(9).Bold();
                });

                row.RelativeItem(1).AlignLeft().AlignTop().Text(text =>
                {
                    text.Span(raBill.Remarks).FontSize(9);
                });
            });

            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Bill details").FontSize(12).Bold();
                });
            });

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25, Unit.Millimetre);
                    columns.ConstantColumn(65, Unit.Millimetre);
                    columns.ConstantColumn(15, Unit.Millimetre);
                    columns.ConstantColumn(18, Unit.Millimetre);
                    columns.ConstantColumn(25, Unit.Millimetre);
                    columns.ConstantColumn(30, Unit.Millimetre);
                    columns.ConstantColumn(30, Unit.Millimetre);
                    columns.ConstantColumn(20, Unit.Millimetre);
                    columns.ConstantColumn(20, Unit.Millimetre);
                    columns.ConstantColumn(25, Unit.Millimetre);
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("PO Item no.").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Work Description").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Unit").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("PO Quantity").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Rate (INR)").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Total Measured Quantity(approved)").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Total Released Quantity till Last RA").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Balance Quantity").FontSize(8).Bold();
                    });
                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Current RA Quantity").FontSize(8).Bold();
                    });

                    header.Cell().Border(1).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Current RA Amount (INR)").FontSize(8).Bold();
                    });
                });

                //List<List<string>> bill_details = this.o.get_bill_details_data();
                foreach (var raBillItem in raBill.RABillItems)
                //foreach (List<string> line_item in bill_details)
                {
                   
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.ServiceNo.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.ShortServiceDesc.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.UoM).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.PoQuantity.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.UnitRate.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.AcceptedMeasuredQty.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.TillLastRAQty.ToString()).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span((raBillItem.AcceptedMeasuredQty - raBillItem.TillLastRAQty).ToString("0.00")).FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span((raBillItem.CurrentRAQty).ToString()).FontSize(8);
                        });
                         table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.CurrentRAAmount.ToString("0.00")).FontSize(8);
                        });
                }
                table.Cell().ColumnSpan(9).Border(1).Padding(2, Unit.Millimetre).AlignRight().AlignMiddle().Text(text =>
                {
                    text.Span(("Total : ").ToString()).FontSize(10).Bold();
                });
                table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                {
                    text.Span(raBill.TotalAmount.ToString("0.00")).FontSize(10).Bold();
                });
            });

            // Deductions in the bill            
            if (raBill.Deductions.Count > 0)
            {
                column.Item().PaddingTop(4, Unit.Millimetre).Row(row =>
                {
                    row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                    {
                        text.Span("Deductions in this Bill:").FontSize(12).Bold();
                    });
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem(1).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(15, Unit.Millimetre);
                            columns.ConstantColumn(75, Unit.Millimetre);
                            columns.ConstantColumn(45, Unit.Millimetre);                           
                        });

                        table.Header(header =>
                        {
                            header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span("Sl. No.").FontSize(8).Bold();
                            });
                            header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span("Description").FontSize(8).Bold();
                            });
                            header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span("Amount (INR)").FontSize(8).Bold();
                            });
                           
                        });

                        foreach (var deduction in raBill.Deductions)
                        {
                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Id.ToString()).FontSize(8).Bold();
                            });

                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Description).FontSize(8).Bold();
                            });

                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Amount.ToString("0.00")).FontSize(8).Bold();
                            });
                           
                        }
                        table.Cell().ColumnSpan(2).Border(1).Padding(2, Unit.Millimetre).AlignRight().AlignMiddle().Text(text =>
                        {
                            text.Span("Total ").FontSize(10).Bold();
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBill.TotalDeduction.ToString("0.00")).FontSize(10).Bold();
                        });
                    });
                });
            }

            column.Item().PaddingTop(12, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span($"Net Bill Amount = {raBill.NetAmount.ToString("0.00")}").FontSize(10).Bold();
                });
            });

            column.Item().PaddingTop(10, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal NEEPCO:………………………").FontSize(10).Bold();
                });

                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal Contractor:……………………").FontSize(10).Bold();
                });
            });
        });
    }

    public void Footer(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem(4).AlignLeft().AlignMiddle().Text(text =>
            {
                //text.Span("Bill generated on ").FontSize(9);
                //text.Span(this.o.GetReportGenerationDate()).FontSize(9);
            });
            row.RelativeItem(6);
            row.RelativeItem(2).AlignRight().AlignMiddle().Text(text =>
            {
                text.Span("Page").FontSize(9);
                text.CurrentPageNumber().FontSize(9);
                text.Span("/").FontSize(9);
                text.TotalPages().FontSize(9);
            });
        });
    }
}

class Bill01_data_source
{
    public List<List<string>> get_bill_details_data()
    {
        List<List<string>> bill_details = new();

        for (int i = 1; i <= 20; i++)
        {
            List<string> temp = new();
            temp.Add((i * 100).ToString());
            temp.Add(Placeholders.Paragraph());
            temp.Add(Placeholders.Label());
            temp.Add((i + 100).ToString());
            temp.Add((i * 1000000).ToString());
            temp.Add(Placeholders.Label());
            temp.Add(Placeholders.Label());
            temp.Add(Placeholders.Label());
            temp.Add((i * 1000000).ToString());
            bill_details.Add(temp);
        }

        return bill_details;
    }

    public List<List<string>> get_deduction_details_data()
    {
        List<List<string>> deduction_details = new();

        for (int i = 1; i <= 10; i++)
        {
            List<string> temp = new();
            temp.Add((i * 100).ToString());
            temp.Add(Placeholders.Paragraph());
            temp.Add(Placeholders.Label());
            temp.Add(Placeholders.Paragraph());
            deduction_details.Add(temp);
        }

        return deduction_details;
    }

    public string GetReportGenerationDate()
    {
        return DateAndTime.Now.ToString();
    }
}

