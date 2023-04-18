using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities.WorkOrderAggregate;

namespace Application.CQRS.RABills.Commands;

public record PostRABillToSapCommand(int RABillId, string url, byte[] authToken) : IRequest
{
}

public class PostRABillToSapCommandHandler : IRequestHandler<PostRABillToSapCommand>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<PostRABillToSapCommand> _logger;

    public PostRABillToSapCommandHandler(IAppDbContext context, ILogger<PostRABillToSapCommand> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(PostRABillToSapCommand request, CancellationToken cancellationToken)
    {
        var wOrderQuery = _context.WorkOrders
                                .Include(p => p.Items)
                                .AsQueryable();

        var raBillQuery = _context.RABills
                                .Include(i => i.Items)
                                .Include(i => i.Deductions)
                                .AsQueryable();

        var query = from order in wOrderQuery
                    join rabill in raBillQuery
                    on order.Id equals rabill.WorkOrderId
                    select new { order, rabill };

        var result = await query.FirstOrDefaultAsync(p => p.order.Id == request.RABillId && 
                                                          p.rabill.Status == RABillStatus.APPROVED);

        if (result == null)
        {
            throw new NotFoundException("RA Bill Not found for posting");
        }

        if (result.rabill.Items.Count == 0)
        {
            throw new NotFoundException("This RA Bill has no line items");
        }

        // Create remarks based on the deduction amounts
        var remarks = "Remarks --> ";
        foreach (var item in result.rabill.Deductions)
        {
            remarks += $"Amount: {item.Amount}, Description: {item.Description}\n";
        }

        var sapSEHeader = new SapSEHeader
        {
            OrderNo = result.order.OrderNo,
            Items = new() // only one line item
            {
                new SapSEItem()
                {
                    ItemNo = result.order.Items.First().ItemNo,
                    PackageNo = result.order.Items.First().PackageNo,
                    Remarks = remarks,
                    Details = new()
                }
            }
        };

        // select only those items whose quantity is non zero - for type safety
        var items = result.rabill.Items.Where(i => i.CurrentRAQty > 0);
        foreach (var item in items)
        {
            var workOrderItem = result.order.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);
            if(workOrderItem == null)
            {
                throw new NotFoundException(nameof(WorkOrderItem), item.WorkOrderItemId);
            }

            var sapSEItem = new SapSESubItem
            {
                ServiceNo = workOrderItem.ServiceNo,
                SubItemNo = workOrderItem.SubItemNo,
                SubItemPackageNo = workOrderItem.SubItemPackageNo,
                Quantity = item.CurrentRAQty,
            };
            sapSEHeader.Items.First().Details.Add(sapSEItem);
        }

        //--- post to RA Bill to sap ---

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(request.authToken));

        var response = await httpClient.PostAsJsonAsync(request.url, sapSEHeader);

        var responseAsString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            result.rabill.MarkAsPosted();
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully posted the RA Bill to SAP");
        }
        else
        {
            var json = JsonSerializer.Deserialize<SapResponse>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var item in json.Response)
            {
                _logger.LogError(responseAsString);
                _logger.LogError($"Code: {item.Code}, Message: {item.Message}");
            }

            throw new BadRequestException("Not able to post RA Bill to SAP");
        }


        return Unit.Value;

    }
}
class SapSEHeader
{
    public long OrderNo { get; set; }
    public List<SapSEItem> Items { get; set; }
}

class SapSEItem
{
    public long ItemNo { get; set; }
    public string PackageNo { get; set; }
    public string Remarks { get; set; }
    public List<SapSESubItem> Details { get; set; }
}

class SapSESubItem
{
    public long SubItemNo { get; set; }
    public long ServiceNo { get; set; }
    public string SubItemPackageNo { get; set; }
    public float Quantity { get; set; }
}

class SapResponse
{
    public List<SapResponseItem> Response { get; set; }
}

class SapResponseItem
{
    public string Code { get; set; }
    public string Message { get; set; }
}

