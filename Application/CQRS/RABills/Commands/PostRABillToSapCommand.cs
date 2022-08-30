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

namespace Application.CQRS.RABills.Commands
{
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
            var raBill = await _context.RABills
                .Include(i => i.Items)
                .Include(i => i.Deductions)
                .Include(p => p.MeasurementBook)
                .ThenInclude(m => m.WorkOrder)
                .FirstOrDefaultAsync(p => p.Id == request.RABillId && p.Status == RABillStatus.APPROVED);

            if (raBill == null)
            {
                throw new NotFoundException("RA Bill Not found for posting");
            }

            if (raBill.Items.Count == 0)
            {
                throw new NotFoundException("This RA Bill has no line items");
            }

            // Create remarks based on the deduction amounts
            var remarks = "";
            foreach (var item in raBill.Deductions)
            {
                remarks += $"Amount: {item.Amount}, Description: {item.Description}\n";
            }

            var sapSEHeader = new SapSEHeader
            {
                OrderNo = raBill.MeasurementBook.WorkOrder.OrderNo,
                Items = new() // only one line item
                {
                    new SapSEItem()
                    {
                        ItemNo = raBill.Items.First().ItemNo,
                        PackageNo = raBill.Items.First().PackageNo,
                        Remarks = remarks,
                        Details = new()
                    }
                }
            };

            // select only those items whose quantity is non zero - for type safety
            var items = raBill.Items.Where(i => i.CurrentRAQty > 0);
            foreach (var item in items)
            {
                var sapSEItem = new SapSESubItem
                {
                    ServiceNo = item.ServiceNo,
                    SubItemNo = item.SubItemNo,
                    SubItemPackageNo = item.SubItemPackageNo,
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
                raBill.MarkAsPosted();
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Successfully posted the RA Bill to SAP");
            }
            else
            {
                var result = JsonSerializer.Deserialize<SapResponse>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                foreach (var item in result.Response)
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
}

