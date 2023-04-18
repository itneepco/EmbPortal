using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace Domain.Entities.MBSheetAggregate;
public class MBSheetItem :AuditableEntity
{
    public int Id { get; private set; }    
    public int MBSheetId { get; set; }
    public int WorkOrderItemId { get; set; }

    private readonly List<MBItemMeasurement> _measurments = new List<MBItemMeasurement>();
    public IReadOnlyList<MBItemMeasurement> Measurements => _measurments.AsReadOnly();

    private readonly List<ItemAttachment> _attachments = new List<ItemAttachment>();
    public IReadOnlyList<ItemAttachment> Attachments => _attachments.AsReadOnly();

    [NotMapped]
    public float MeasuredQuantity
    {
        get
        {
            return _measurments.Aggregate((float)0, (acc, curr) => acc + curr.Total);
        }
    }
    public MBSheetItem(int workOrderItemId)
    {
        WorkOrderItemId = workOrderItemId;     
     
    }

    public MBSheetItem()
    {
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
