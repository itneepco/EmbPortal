using Api.Models;
using Api.Reports;
using Application.CQRS.RABills.Commands;
using Application.CQRS.RABills.Queries;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class RABillController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public RABillController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        [HttpGet("MBook/{mBookId}")]
        public async Task<ActionResult<List<RABillResponse>>> GetRABillsByMBookId(int mBookId)
        {
            var query = new GetRABillByMBookIdQuery(mBookId);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("Pending")]
        public async Task<ActionResult<IList<RABillInfoResponse>>> GetPendingRABills()
        {
            var query = new GetApprovalPendingRABillQuery();

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{raBillId}")]
        [ProducesResponseType(typeof(RABillDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RABillDetailResponse>> GetRABillById(int raBillId)
        {
            var query = new GetRABillByIdQuery(raBillId);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateRABill(RABillRequest data)
        {
            var command = new CreateRABillCommand(data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditRABill(RABillRequest data, int id)
        {
            var command = new EditRABillCommand(id, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ApproveRABill(int id)
        {
            var command = new ApproveRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RevokeRABill(int id)
        {
            var command = new RevokeRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/PostToSAP")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostRABillToSAP(int id)
        {
            var url = $"{_config["SESUrl"]}";
            var authToken = Encoding.ASCII.GetBytes($"{_config["UserId"]}:{_config["Password"]}");

            var command = new PostRABillToSapCommand(id, url, authToken);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRABill(int id)
        {
            var command = new DeleteRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet("{id}/Download")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GenerateRABill(int id)
        {
            try
            {
                var result = await Mediator.Send(new RABillReportQuery(id));
                var report = new RABillReport(_env, result).GeneratePdf();

                return Ok(Convert.ToBase64String(report));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception();
            }
        }

        [HttpPost("{id}/Deduction")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateRADeduction(RADeductionRequest data, int id)
        {
            var command = new CreateRADeductionCommand(data, id);

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}/Deduction/{deductionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRADeduction(int id, int deductionId)
        {
            var command = new DeleteRADeductionCommand(id, deductionId);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
