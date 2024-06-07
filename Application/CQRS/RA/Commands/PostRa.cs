using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using EmbPortal.Shared.Responses.RA;

namespace Application.CQRS.RA.Commands;

public record PostRaCommand(int RaId, string url, byte[] authToken) : IRequest
{
}

public class PostRaCommandHandler : IRequestHandler<PostRaCommand>
{
    private readonly IAppDbContext _db;
    private readonly ILogger<PostRaCommand> _logger;
    public PostRaCommandHandler(IAppDbContext db, ILogger<PostRaCommand> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task Handle(PostRaCommand request, CancellationToken cancellationToken)
    {
        var wOrderQuery = _db.WorkOrders.Include(p => p.Items).AsQueryable();

        var raQuery = _db.RAHeaders                               
                                .Include(i => i.Items)
                                .Include(i => i.Deductions)
                                .AsQueryable();

        var query = from order in wOrderQuery
                    join ra in raQuery  
                    on order.Id equals ra.WorkOrderId
                    select new { order, ra };

        var result = await query.FirstOrDefaultAsync(p => p.ra.Id == request.RaId);
        if (result == null)
        {
            throw new NotFoundException("RA Bill Not found for posting");
        }
        var sapSEHeader = new SapSEHeader
        {
            OrderNo = result.order.OrderNo,
            Items = result.ra.Items.GroupBy(p => new { p.ItemNo, p.PackageNo })
            .Select(item => new SapSEItem
            {
                ItemNo = item.Key.ItemNo,
                Description = result.ra.Title,
                PackageNo = item.Key.PackageNo,
                Remarks = result.ra.Remarks,
                Details = item.Select(subItem => new SapSESubItem
                {                   
                    SubItemNo = subItem.SubItemNo,
                    SubItemPackageNo = subItem.SubItemPackageNo,
                    ServiceNo = subItem.ServiceNo,
                    ShortDesc =subItem.ShortServiceDesc,
                    Quantity = subItem.CurrentRAQty
                }).ToList()
            }).ToList()            
        };
       
        //--- post to RA Bill to sap ---
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(request.authToken));

        var response = await httpClient.PostAsJsonAsync(request.url, sapSEHeader);
        var responseAsString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                result.ra.MarkAsPosted();
                await _db.SaveChangesAsync(cancellationToken);

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
    public string Description { get; set; }
    public string PackageNo { get; set; }
    public string Remarks { get; set; }
    public List<SapSESubItem> Details { get; set; }
}
class SapSESubItem
{
    public long SubItemNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortDesc { get; set; }
    public string SubItemPackageNo { get; set; }
    public decimal Quantity { get; set; }
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