using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MBSheetResponse
    {
        public int Id { get; set; }
        public int MeasurementBookId { get; set; }
        public string Title { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string MeasurementOfficer { get; set; }
        public UserResponse Measurer { get; set; }

        public DateTime ValidationDate { get; set; }
        public string ValidationOfficer { get; set; }
        public UserResponse Validator { get; set; }

        public DateTime AcceptingDate { get; set; }
        public string AcceptingOfficer { get; set; }
        public UserResponse Acceptor { get; set; }

        public string Status { get; set; }

        public List<MBSheetItemResponse> Items { get; set; }
    }
}
