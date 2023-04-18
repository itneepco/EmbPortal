using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Application.Interfaces;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using System.Linq;
using System;

namespace Application.CQRS.RABills.Commands;

public record GeneratePdfCommand(int RABillId) : IRequest<byte[]>
{
}

public class GeneratePdfCommandHandler : IRequestHandler<GeneratePdfCommand, byte[]>
{
    private readonly IAppDbContext _context;

    public GeneratePdfCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(GeneratePdfCommand request, CancellationToken cancellationToken)
    {
        //RABill raBill = await _context.RABills
        //    .Include(p => p.Items)
        //    .FirstOrDefaultAsync(p => p.Id == request.RABillId);

        //if (raBill == null)
        //{
        //    throw new NotFoundException(nameof(RABill), request.RABillId);
        //}

        //byte[] pdfBytes;
        //using (var stream = new MemoryStream())
        //using (var writer = new PdfWriter(stream))
        //using (var pdf = new PdfDocument(writer))
        //using (var doc = new Document(pdf, PageSize.A4.Rotate()))
        //{
        //    doc.SetMargins(20, 20, 20, 20);

        //    doc.Add(new Paragraph("RA Bill Details"));

        //    Table table = new Table(UnitValue.CreatePercentArray(100)).UseAllAvailableWidth().SetFixedLayout();

        //    Cell desc = new Cell(1, 35).Add(new Paragraph("Item Description"));
        //    table.AddCell(desc);

        //    Cell rate = new Cell(1, 10).Add(new Paragraph("Rate"));
        //    table.AddCell(rate);

        //    Cell measuredQty = new Cell(1, 10).Add(new Paragraph("Measured Qty"));
        //    table.AddCell(measuredQty);

        //    Cell lastRaQty = new Cell(1, 10).Add(new Paragraph("Till Last RA Qty"));
        //    table.AddCell(lastRaQty);

        //    Cell currRaQty = new Cell(1, 10).Add(new Paragraph("Current RA Qty"));
        //    table.AddCell(currRaQty);

        //    Cell currRaAmt = new Cell(1, 12).Add(new Paragraph("Current Amount"));
        //    table.AddCell(currRaAmt);

        //    Cell remarks = new Cell(1, 13).Add(new Paragraph("Remarks"));
        //    table.AddCell(remarks);

        //    foreach (var item in raBill.Items.Where(p => p.CurrentRAQty > 0))
        //    {
        //        desc = new Cell(1, 35).Add(new Paragraph(item.ItemDescription));
        //        table.AddCell(desc);

        //        rate = new Cell(1, 10).Add(new Paragraph(item.UnitRate.ToString("0.00")));
        //        table.AddCell(rate);

        //        measuredQty = new Cell(1, 10).Add(new Paragraph(item.AcceptedMeasuredQty.ToString("0.00")));
        //        table.AddCell(measuredQty);

        //        lastRaQty = new Cell(1, 10).Add(new Paragraph(item.TillLastRAQty.ToString("0.00")));
        //        table.AddCell(lastRaQty);

        //        currRaQty = new Cell(1, 10).Add(new Paragraph(item.CurrentRAQty.ToString("0.00")));
        //        table.AddCell(currRaQty);

        //        decimal amount = (decimal)item.CurrentRAQty * item.UnitRate;
        //        currRaAmt = new Cell(1, 12).Add(new Paragraph(amount.ToString("0.00")));
        //        table.AddCell(currRaAmt);

        //        var remarkStr = string.IsNullOrEmpty(item.Remarks) ? " " : item.Remarks;
        //        remarks = new Cell(1, 13).Add(new Paragraph(remarkStr));
        //        table.AddCell(remarks);
        //    }

        //    doc.Add(table);

        //    doc.Close();
        //    pdfBytes = stream.ToArray();
        //}

        //return pdfBytes;

        return new byte[0];
    }
}
