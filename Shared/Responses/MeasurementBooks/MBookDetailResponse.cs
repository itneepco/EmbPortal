using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MBookDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string MeasurerEmpCode { get; set; }
        public string MeasurerName { get; set; }
        
        public string ValidatorEmpCode { get; set; }
        public string ValidatorName { get; set; }

        public string EicEmpCode { get; set; }
        public string EicName { get; set; }

        public string Status { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Contractor { get; set; }
        public List<MBookItemResponse> Items { get; set; }
    }
}
