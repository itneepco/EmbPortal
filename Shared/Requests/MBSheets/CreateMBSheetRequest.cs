using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Requests
{
    public class CreateMBSheetRequest
    {
        public string MeasurementOfficer { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string ValidationOfficer { get; set; }
        public DateTime ValidationDate { get; set; }
        public string AcceptingOfficer { get; set; }
        public DateTime AcceptingDate { get; set; }
        public int MeasurementBookId { get; set; }
        public List<MBSheetItemRequest> Items { get; set; }
    }
}
