using Domain.Common;

namespace Domain.Entities.RABillAggregate;

public class RABillItem : AuditableEntity
{
    public int Id { get; private set; }       
    public float AcceptedMeasuredQty { get; private set; }
    public float TillLastRAQty { get; private set; }
    public float CurrentRAQty { get; private set; }
    public string Remarks { get; private set; } // character 100 max limit
    public int WorkOrderItemId { get; private set; }
    public int RABillId { get; set; }

    public RABillItem()
    {
    }

    public RABillItem(           
        float acceptedMeasuredQty,
        float tillLastRAQty,
        float currentRAQty,
        string remarks,
        int workOrderItemId)
    {           
        AcceptedMeasuredQty = acceptedMeasuredQty;
        TillLastRAQty = tillLastRAQty;
        CurrentRAQty = currentRAQty;
        Remarks = remarks;
        WorkOrderItemId = workOrderItemId;
    }

    public void SetAcceptedMeasuredQty(float val)
    {
        AcceptedMeasuredQty = val;
    }
    public void SetTillLastRAQty(float val)
    {
        TillLastRAQty = val;
    }
    public void SetCurrentRAQty(float val)
    {
        CurrentRAQty = val;
    }
    public void SetRemarks(string val)
    {
        Remarks = val;
    }
}
