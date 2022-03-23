using System;

namespace EmbPortal.Shared.Responses
{
    public class MBookHeaderResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Contractor { get; set; }
    }
}
