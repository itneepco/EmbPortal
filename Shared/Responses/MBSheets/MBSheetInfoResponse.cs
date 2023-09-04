using System;

namespace EmbPortal.Shared.Responses
{
    public class MBSheetInfoResponse
    {
        public int Id { get; set; }
        public int MeasurementBookId { get; set; }

        public string Title { get; set; }
        public DateTime MeasurementDate { get; set; }

        public string MeasurerEmpCode { get; set; }
        public string MeasurerName { get; set; }

        public DateTime ValidationDate { get; set; }
        public string ValidatorEmpCode { get; set; }
        public string ValidatorName { get; set; }

        public DateTime AcceptingDate { get; set; }
        public string EicEmpcode { get; set; }
        public string EicName { get; set; }

        public string Status { get; set; }
    }
}
