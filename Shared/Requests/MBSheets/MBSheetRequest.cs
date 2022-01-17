using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Requests
{
    public class MBSheetRequest
    {
        public DateTime? MeasurementDate { get; set; }
        public int MeasurementBookId { get; set; }
        public List<MBSheetItemRequest> Items { get; set; }
    }
}
