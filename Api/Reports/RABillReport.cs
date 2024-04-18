
using EmbPortal.Shared.Constants;
using EmbPortal.Shared.Responses.RABills;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic;
using QuestPDF.Drawing;
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
        FontManager.RegisterFont(File.OpenRead("Metropolis-Regular.ttf"));
    }

    public void Compose(IDocumentContainer container)
    {
       
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());            
            page.MarginLeft(8, Unit.Millimetre);
            page.MarginTop(8, Unit.Millimetre);
            page.MarginRight(8, Unit.Millimetre);
            page.MarginBottom(8, Unit.Millimetre);

            page.Header().Element(Header);
            page.Content().Element(Content);
            page.Footer().Element(Footer);
        });
    }

    public void Header(IContainer container)
    {
        
        container.Column(column =>
        {
            column.Item().BorderBottom(1).PaddingTop(4, Unit.Millimetre).Row(row =>
            {
                row.ConstantItem(16, Unit.Millimetre).Height(16, Unit.Millimetre).Column(column =>
                {
                    var image = Path.Combine(env.ContentRootPath, FileConstant.FolderName, "neepcologo.png");
                    column.Item().Padding(0, Unit.Millimetre).Image(image);
                });

                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignCenter().PaddingTop(4, Unit.Millimetre).Text(text =>
                    {
                        text.Span("North Eastern Electric Power Corpporation Ltd.").FontFamily("Metropolis").FontSize(16);
                    });
                    column.Item().AlignCenter().PaddingTop(2, Unit.Millimetre).Text(text =>
                    {
                        text.Span("Running Account Bill").FontFamily("Metropolis").FontSize(16).Bold();
                    });
                });
            }
            );
        });
        }

    public void Content(IContainer container)
    {
        container.Column(column =>
        {
          
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Bill No.:").FontSize(8).FontFamily("Metropolis");
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span($"{raBill.RABillTitle}").FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(8, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Date:").FontFamily("Metropolis").FontSize(8).Bold();
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.BillDate.ToString("dd-MMM-yyyy")).FontFamily("Metropolis").FontSize(8);
                    });
                });
            });

            // Header second row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("PO No.:").FontSize(8).FontFamily("Metropolis");
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.PoNo).FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(8, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("EIC:").FontFamily("Metropolis").FontSize(8).FontFamily("Metropolis");
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.EIC).FontFamily("Metropolis").FontSize(8);
                    });
                });
            });

            // Header third row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("MB No.:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.MBTitle).FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(35, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Measurement Officer:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.MeaurementOfficer).FontFamily("Metropolis").FontSize(8);
                    });
                });
            });

            // Header fourth row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(30, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Period of the Bill:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.FromDate.ToString("dd-MMM-yyyy")).FontFamily("Metropolis").FontSize(8);
                        text.Span(" TO ").FontFamily("Metropolis").FontSize(8);
                        text.Span(raBill.ToDate.ToString("dd-MMM-yyyy")).FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(30, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Contractor Name:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.Contractor).FontFamily("Metropolis").FontSize(8);
                    });
                });
            });

            // Header fifth row
            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(55, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("Date of Actual Completion of Work:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.ActualCompletionDate?.ToString("dd-MMM-yyyy")).FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(60, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("No.and date of the last Bill for this work:").FontFamily("Metropolis").FontSize(8);
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.LastBill).FontFamily("Metropolis").FontSize(8);
                    });
                });
            });

            // Header sixth row
            column.Item().BorderBottom(1).PaddingTop(2, Unit.Millimetre).PaddingBottom(2, Unit.Millimetre).Row(row =>
            {
                row.ConstantItem(15, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                {
                    text.Span("Remarks:").FontFamily("Metropolis").FontSize(8);
                });

                row.RelativeItem(1).AlignLeft().AlignTop().Text(text =>
                {
                    text.Span(raBill.Remarks).FontFamily("Metropolis").FontSize(8);
                });
            });

            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("BILL DETAILS").FontFamily("Metropolis").FontSize(8);
                });
            });

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25, Unit.Millimetre);
                    columns.ConstantColumn(65, Unit.Millimetre);
                    columns.ConstantColumn(15, Unit.Millimetre);
                    columns.ConstantColumn(22, Unit.Millimetre);
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
                        text.Span("PO ITEM NO.").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("WORK DESCRIPTION").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("UNIT").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("PO QUANTITY").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("RATE (INR)").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("TOTAL MEASURED QTY.(Approved)").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Total RELEASED QTY Till Last RA").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("BLALANCE QUANTITY").FontFamily("Metropolis").FontSize(8);
                    });
                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("CURRENT RA QUANTITY").FontFamily("Metropolis").FontSize(8);
                    });

                    header.Cell().Border(1).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("CURRENT RA AMT. (INR)").FontFamily("Metropolis").FontSize(8);
                    });
                });
               
                foreach (var raBillItem in raBill.RABillItems)
             
                {
                   
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.ServiceNo.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.ShortServiceDesc.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.UoM).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.PoQuantity.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.UnitRate.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.AcceptedMeasuredQty.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.TillLastRAQty.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span((raBillItem.AcceptedMeasuredQty - raBillItem.TillLastRAQty).ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span((raBillItem.CurrentRAQty).ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                         table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBillItem.CurrentRAAmount.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                        });
                }
                table.Cell().ColumnSpan(9).Border(1).Padding(2, Unit.Millimetre).AlignRight().AlignMiddle().Text(text =>
                {
                    text.Span(("Total : ").ToString()).FontFamily("Metropolis").FontSize(8); 
                });
                table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                {
                    text.Span(raBill.TotalAmount.ToString("0.00")).FontFamily("Metropolis").FontSize(8); 
                });
            });

            // Deductions in the bill            
            if (raBill.Deductions.Count > 0)
            {
                column.Item().PaddingTop(4, Unit.Millimetre).Row(row =>
                {
                    row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                    {
                        text.Span("DEDUCTIONS").FontFamily("Metropolis").FontSize(8); 
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
                                text.Span("Sl. No.").FontFamily("Metropolis").FontSize(8);
                            });
                            header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span("DESCRIPTION").FontFamily("Metropolis").FontSize(8);
                            });
                            header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span("AMOUNT(INR)").FontFamily("Metropolis").FontSize(8);
                            });
                           
                        });

                        foreach (var deduction in raBill.Deductions)
                        {
                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Id.ToString()).FontFamily("Metropolis").FontSize(8);
                            });

                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Description).FontFamily("Metropolis").FontSize(8);
                            });

                            table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                            {
                                text.Span(deduction.Amount.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                            });
                           
                        }
                        table.Cell().ColumnSpan(2).Border(1).Padding(2, Unit.Millimetre).AlignRight().AlignMiddle().Text(text =>
                        {
                            text.Span("TOTAL ").FontFamily("Metropolis").FontSize(8); ;
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(raBill.TotalDeduction.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                        });
                    });
                });
            }

            column.Item().PaddingTop(8, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span($"Net Bill Amount = {raBill.NetAmount.ToString("0.00")}").FontFamily("Metropolis").FontSize(8); 
                });
            });

            column.Item().PaddingTop(8, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal NEEPCO:………………………").FontFamily("Metropolis").FontSize(8); 
                });

                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal Contractor:……………………").FontFamily("Metropolis").FontSize(8);
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
                //text.Span("Bill generated on ").FontSize(8);
                //text.Span(this.o.GetReportGenerationDate()).FontSize(8);
            });
            row.RelativeItem(6);
            row.RelativeItem(2).AlignRight().AlignMiddle().Text(text =>
            {
                text.Span("Page").FontSize(8);
                text.CurrentPageNumber().FontSize(8);
                text.Span("/").FontSize(8);
                text.TotalPages().FontSize(8);
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
            temp.Add((i * 80).ToString());
            temp.Add(Placeholders.Paragraph());
            temp.Add(Placeholders.Label());
            temp.Add((i + 80).ToString());
            temp.Add((i * 800000).ToString());
            temp.Add(Placeholders.Label());
            temp.Add(Placeholders.Label());
            temp.Add(Placeholders.Label());
            temp.Add((i * 800000).ToString());
            bill_details.Add(temp);
        }

        return bill_details;
    }

    public List<List<string>> get_deduction_details_data()
    {
        List<List<string>> deduction_details = new();

        for (int i = 1; i <= 8; i++)
        {
            List<string> temp = new();
            temp.Add((i * 80).ToString());
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

