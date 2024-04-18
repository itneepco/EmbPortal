using Api.Models;
using Api.Reports;
using Application.CQRS.RA.Commands;
using Application.CQRS.RA.Queries;
using EmbPortal.Shared.Requests.RA;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.RA;
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

namespace Api.Controllers;
[Authorize]
public class RAController : ApiController
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public RAController(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }
    [HttpGet("{raId}")]
    [ProducesResponseType(typeof(RABillDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RADetailResponse>> GetRAById(int raId)
    {
        var query = new GetRAById(raId);
        return Ok(await Mediator.Send(query));
    }
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateRA(RARequest data)
    {
        var command = new CreateRACommand(data);
        return Ok(await Mediator.Send(command));
    }

    [HttpGet("WOrder/{wOrderId}")]
    public async Task<ActionResult<List<RAResponse>>> GetRABillsByWOrderId(int wOrderId)
    {
        var query = new GetRAByWorkOrder(wOrderId);
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> EditRa(RARequest data, int id)
    {
        var command = new EditRaCommand(data,id);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/Publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PublishRa(int id)
    {
        var command = new PublishRaCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{raId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRa(int raId)
    {
        var command = new DeleteRa(raId);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}/RaPdf")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GenerateRABill(int id)
    {
        try
        {
            var result = await Mediator.Send(new RaReportQuery(id));
            var report = new RaReport(_env, result).GeneratePdf();
            return Ok(Convert.ToBase64String(report));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw new Exception();
        }
    }
    [HttpPut("{id}/PostToSAP")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PostRABillToSAP(int id)
    {
        var url = $"{_config["SESUrl"]}";
        var authToken = Encoding.ASCII.GetBytes($"{_config["UserId"]}:{_config["Password"]}");

        var command = new PostRaCommand(id, url, authToken);
        await Mediator.Send(command);

        return NoContent();
    }

}
