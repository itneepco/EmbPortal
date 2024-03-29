﻿using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands;

public record CreateRABillCommand(RABillRequest Data) : IRequest<int>
{
}

public class CreateRABillCommandQueryHandler : IRequestHandler<CreateRABillCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly IRABillService _billService;
    private readonly IMeasurementBookService _mBookService;

    public CreateRABillCommandQueryHandler(IAppDbContext context, IRABillService billService, IMeasurementBookService mBookService)
    {
        _context = context;
        _billService = billService;
        _mBookService = mBookService;
    }

    public async Task<int> Handle(CreateRABillCommand request, CancellationToken cancellationToken)
    {
        bool anyActiveRaBill = await _context.RABills.AnyAsync(
            p => p.MeasurementBookId == request.Data.MeasurementBookId && 
                 (p.Status == RABillStatus.CREATED || p.Status == RABillStatus.REVOKED));

        if(anyActiveRaBill)
        {
            throw new BadRequestException("Cannot create new RA Bill when there are existing unapproved RA Bill");
        }

        MeasurementBook mBook = await _context.MeasurementBooks            
            .Include(p => p.Items)               
            .Where(p => p.Id == request.Data.MeasurementBookId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (mBook == null)
        {
            throw new NotFoundException($"MeasurementBook does not exist with Id: {request.Data.MeasurementBookId}");
        }

        List<MBookItemQtyStatus> mBItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);
        List<RAItemQtyStatus> raItemQtyStatuses = await _billService.GetRAItemQtyStatus(mBook.Id);

        var raBillCount = _context.RABills.Where(i => i.MeasurementBookId == mBook.Id).Count() + 1;
        var title = mBook.Title+"-RA-"+raBillCount;

        var raBill = new RABill(
            title: title,
            fromDate : (DateTime)request.Data.FromDate,
            toDate : (DateTime)request.Data.ToDate,
            completionDate : request.Data.CompletionDate,
            lastRADetail : request.Data.LastBillDetail,
            remarks : request.Data.Remarks,
            billDate: (DateTime)request.Data.BillDate,
            eicEmpCode: mBook.EicEmpCode,
            mBookId: mBook.Id,
            workOrderId: mBook.WorkOrderId
        );

        foreach (var item in request.Data.Items)
        {
            var mbItemQtyStatus = mBItemQtyStatuses.FirstOrDefault(p => p.WorkOrderItemId == item.WorkOrderItemId);
            var raItemQtyStatus = raItemQtyStatuses.FirstOrDefault(p => p.WorkOrderItemId == item.WorkOrderItemId);

            raBill.AddLineItem(new RABillItem(
                acceptedMeasuredQty: mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0,
                tillLastRAQty: raItemQtyStatus != null ? raItemQtyStatus.ApprovedRAQty : 0,
                currentRAQty: item.CurrentRAQty,
                remarks: item.Remarks,
                workOrderItemId: item.WorkOrderItemId
            ));
        }

        _context.RABills.Add(raBill);
        await _context.SaveChangesAsync(cancellationToken);

        return raBill.Id;
    }
}
