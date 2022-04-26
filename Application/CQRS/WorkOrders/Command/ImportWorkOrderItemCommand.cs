using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public class ImportWorkOrderItemCommand : IRequest<IResult>
    {
        public int WorkOrderId { get; set; }
        public byte[] Data { get; }

        public ImportWorkOrderItemCommand(int workOrderId, byte[] data)
        {
            WorkOrderId = workOrderId;
            Data = data;
        }
    }

    public record CreateWorkOrderItemTemplateCommand : IRequest<byte[]>
    {
    }

    public class ImportWorkOrderItemCommandHandler :
        IRequestHandler<CreateWorkOrderItemTemplateCommand, byte[]>,
        IRequestHandler<ImportWorkOrderItemCommand, IResult>
    {
        private readonly IAppDbContext _context;
        private readonly IExcelService _excelService;

        public ImportWorkOrderItemCommandHandler(IAppDbContext context, IMapper mapper, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<IResult> Handle(ImportWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.WorkOrderId);

            var uoms = await _context.Uoms.ToListAsync();

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(WorkOrder), request.WorkOrderId);
            }

            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, WorkOrderItemRequest, object>>
            {
              { "Description", (row,item) => item.Description = row["Description"]?.ToString() },
              { "Uom", (row,item) => item.Uom = row["Uom"]?.ToString() },
              { "UnitRate", (row,item) => item.UnitRate = row.IsNull("UnitRate")? 0m:Convert.ToDecimal(row["PoQuantity"]) },
              { "PoQuantity", (row,item) => item.PoQuantity = row.IsNull("PoQuantity") ? 0: (float)Convert.ToDouble(row["PoQuantity"]) }
            }, "Sheet1");

            if (result.Succeeded)
            {
                foreach (var dto in result.Data)
                {
                    var uom = uoms.Find(p => p.Name.ToLower().Contains(dto.Uom.ToLower()));

                    if(uom == null)
                    {
                        throw new NotFoundException($"No UOM with name '{dto.Uom}' found in the database");
                    }

                    workOrder.AddUpdateLineItem(dto.Description, uom.Id, dto.UnitRate, dto.PoQuantity);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Uploaded successfully");
            }
            else
            {
                return await Result<IEnumerable<WorkOrderItem>>.FailureAsync(result.Errors);
            }
        }

        public async Task<byte[]> Handle(CreateWorkOrderItemTemplateCommand request, CancellationToken cancellationToken)
        {
            var fields = new string[] {
                "Description",
                "Uom",
                "UnitRate",
                "PoQuantity"
            };

            var result = await _excelService.CreateTemplateAsync(fields, "Work Order Items");
            return result;
        }
    }
}
