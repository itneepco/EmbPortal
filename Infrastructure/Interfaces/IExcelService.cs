﻿using EmbPortal.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> CreateTemplateAsync(IEnumerable<string> fields, string sheetName = "Sheet1");
        Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, 
            Func<TData, object>> mappers, string sheetName = "Sheet1");

        Task<IResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(byte[] data, Dictionary<string, 
            Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1");
    }
}
