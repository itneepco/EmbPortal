namespace EmbPortal.Shared.Responses
{
    public class DashboardStatsResponse
    {
        public int MBSheetValidation { get; set; }
        public int MBSheetApproval { get; set; }
        public int RABillApproval { get; set; }
        public int WorkOrderCount { get; set; }
        public int MBookCount { get; set; }
    }
}
