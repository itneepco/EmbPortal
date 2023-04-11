using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities.MBSheetAggregate;
public class MBSheetItem :AuditableEntity
{
    public int Id { get; private set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; private set; }
    public string Description { get; private set; }
    public int Nos { get; private set; }
    public float MeasuredQuantity { get; private set; }   
    public string Uom { get; private set; }
    public decimal UnitRate { get; private set; }   
    public int MBookItemId { get; private set; }    
    public int MBSheetId { get; set; }
    public MBSheet MBSheet { get; set; }
    private readonly List<ItemAttachment> _attachments = new List<ItemAttachment>();    public IReadOnlyList<ItemAttachment> Attachments => _attachments.AsReadOnly();

    public MBSheetItem(
        long serviceNo, 
        string serviceDesc, 
        string description, 
        string uom, 
        decimal rate, 
        int mBookItemId, 
        int nos, 
        float measuredQuantity
    )
    {
        ServiceNo = serviceNo;
        ShortServiceDesc = serviceDesc;
        Description = description;
        Uom = uom;
        UnitRate = rate;
        MBookItemId = mBookItemId;
        Nos = nos;
        MeasuredQuantity = measuredQuantity;       
    }

    public MBSheetItem()
    {
    }

    public void SetDescription(string desc)
    {
        Description = desc;
    }

    public void SetNos(int no)
    {
        Nos = no;
    }

    public void SetMeasuredQuantity(float measuredQuantity)
    {
        MeasuredQuantity = measuredQuantity;
    }
    public void AddAttachment(ItemAttachment attachment)
    {
        _attachments.Add(attachment);
    }

    public void RemoveAttachment(ItemAttachment attachment)
    {
        _attachments.Remove(attachment);
    }
}
