namespace EmbPortal.Shared.Responses
{
    public class DashboardStatsResponse
    {
        public int MBSheetValidation { get; set; }
        public int MBSheetApproval { get; set; }
        public int RABillApproval { get; set; }
        public int WorkOrderPending { get; set; }
        public int WorkOrderPublished { get; set; }
    }
}
