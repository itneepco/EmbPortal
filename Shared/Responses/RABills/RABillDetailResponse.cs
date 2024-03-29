﻿using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses
{
    public class RABillDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string WorkOrderNo { get; set; }
        public RABillStatus Status { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string LastBillDetail { get; set; } = string.Empty;
        public DateTime ApprovalDate { get; set; }
        public string EicEmpCode { get; set; }
        public int MeasurementBookId { get; set; }
        public List<RABillItemResponse> Items { get; set; }
        public List<RADeductionResponse> Deductions { get; set; }

        public decimal TotalAmount => Items.Aggregate((decimal)0, (curr, item) => curr + item.CurrentRAAmount);

        public decimal TotalDeduction => Deductions.Aggregate((decimal)0, (curr, item) => curr + item.Amount);

        public decimal NetAmount => TotalAmount - TotalDeduction;
    }
}
