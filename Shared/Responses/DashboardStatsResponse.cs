namespace EmbPortal.Shared.Responses
{
    public class DashboardStatsResponse
    {
        public int MBSheetValidation { get; set; }
        public int MBSheetApproval { get; set; }
        public int RAPending { get; set; }
        public int RAPosted { get; set; }
        public int WorkOrderCount { get; set; }
        public int MBookCount { get; set; }
    }
}
