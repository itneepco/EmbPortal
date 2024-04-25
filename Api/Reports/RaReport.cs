using EmbPortal.Shared.Constants;
using EmbPortal.Shared.Responses.RA;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace Api.Reports;

public class RaReport : IDocument
{
    private readonly IWebHostEnvironment env;
    private readonly RAReportResponse raBill;

    public RaReport(IWebHostEnvironment env, RAReportResponse raBill)
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
                        text.Span($"{raBill.RaTitle}").FontFamily("Metropolis").FontSize(8);
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
                        text.Span(raBill.PoNo.ToString()).FontFamily("Metropolis").FontSize(8);
                    });
                });

                row.RelativeItem(1).Row(row => {
                    row.ConstantItem(8, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span("EIC:").FontFamily("Metropolis").FontSize(8).FontFamily("Metropolis");
                    });
                    row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                    {
                        text.Span(raBill.Eic).FontFamily("Metropolis").FontSize(8);
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
                    text.Span("Line Items").FontFamily("Metropolis").FontSize(10).Bold();
                });
            });
            foreach (var item in raBill.Items)
            {
                column.Item().BorderBottom(1).PaddingTop(2, Unit.Millimetre).PaddingBottom(2, Unit.Millimetre).Row(row =>
                {
                    row.RelativeItem(1).Row(row => {
                        row.ConstantItem(20, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                        {
                            text.Span("Item No:").FontFamily("Metropolis").FontSize(8);
                        });
                        row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                        {
                            text.Span(item.No.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                    });
                    row.RelativeItem(1).Row(row => {
                        row.ConstantItem(30, Unit.Millimetre).AlignLeft().AlignTop().Text(text =>
                        {
                            text.Span("Description:").FontFamily("Metropolis").FontSize(8);
                        });
                        row.RelativeItem().AlignLeft().AlignTop().Text(text =>
                        {
                            text.Span(item.Description).FontFamily("Metropolis").FontSize(8);
                        });
                    });
                });
                column.Item().Table(table => {
                    table.ColumnsDefinition(columns => {
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(70, Unit.Millimetre);
                        columns.ConstantColumn(15, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(20, Unit.Millimetre);
                        columns.ConstantColumn(55, Unit.Millimetre);
                    });
                    table.Header(header => {
                       header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                       {
                           text.Span("Sub Item No.").FontFamily("Metropolis").FontSize(8);
                       });
                       header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Description").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("UoM").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Unit Rate").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("PO Quantity").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Measured Quantity").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Qty Till Last RA").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Qty Current RA").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Amount").FontFamily("Metropolis").FontSize(8);
                        });
                        header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span("Remarks").FontFamily("Metropolis").FontSize(8);
                        });
                    });
                    foreach(var subItem in item.SubItems)
                    {
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                         {
                           text.Span(subItem.No.ToString()).FontFamily("Metropolis").FontSize(8);
                         });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.ShortServiceDesc).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.Uom).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.UnitRate.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.PoQuantity.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.MeasuredQty.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.TillLastRaQty.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.CurrentRaQty.ToString()).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.RaAmount.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                        });
                        table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                        {
                            text.Span(subItem.Remarks).FontFamily("Metropolis").FontSize(8);
                        });
                    }                   

                });
            }

            column.Item().PaddingTop(2, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Deductions").FontFamily("Metropolis").FontSize(10).Bold();
                });
            });
            column.Item().Table(table => {
                table.ColumnsDefinition(columns => {
                    columns.ConstantColumn(60, Unit.Millimetre);
                    columns.ConstantColumn(30, Unit.Millimetre);                    
                });
                table.Header(header => {
                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Description.").FontFamily("Metropolis").FontSize(8);
                    });
                    header.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("Amount").FontFamily("Metropolis").FontSize(8);
                    });
                });
                foreach (var deduction in raBill.Deductions)
                {
                    table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span(deduction.Description).FontFamily("Metropolis").FontSize(8);
                    });
                    table.Cell().Border(1).Padding(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span(deduction.Amount.ToString("0.00")).FontFamily("Metropolis").FontSize(8);
                    });
                }
            });

            column.Item().PaddingTop(8, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span($"RA Bill Amount = {raBill.TotalRaAmount.ToString("0.00")}").FontFamily("Metropolis").FontSize(10).Bold();
                });
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span($"Total Deduction = {raBill.TotalDeduction.ToString("0.00")}").FontFamily("Metropolis").FontSize(10);
                });
                row.RelativeItem(1).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span($"Net Bill Amount = {raBill.NetRaAmount.ToString("0.00")}").FontFamily("Metropolis").FontSize(10);
                });
            });

            column.Item().PaddingTop(12, Unit.Millimetre).Row(row =>
            {
                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal NEEPCO:………………………").FontFamily("Metropolis").FontSize(10);
                });

                row.RelativeItem(1).PaddingTop(2, Unit.Millimetre).AlignLeft().AlignMiddle().Text(text =>
                {
                    text.Span("Signature with Seal Contractor:……………………").FontFamily("Metropolis").FontSize(10);
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


