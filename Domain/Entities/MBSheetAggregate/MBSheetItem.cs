using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Entities.MBSheetAggregate;
public class MBSheetItem :AuditableEntity
{
    public int Id { get; private set; }    
    public int MBSheetId { get; set; }
    public int WorkOrderItemId { get; set; }

    private readonly List<MBItemMeasurement> _measurements = new List<MBItemMeasurement>();
    public IReadOnlyList<MBItemMeasurement> Measurements => _measurements.AsReadOnly();

    private readonly List<ItemAttachment> _attachments = new List<ItemAttachment>();
    public IReadOnlyList<ItemAttachment> Attachments => _attachments.AsReadOnly();

    [NotMapped]
    public decimal MeasuredQuantity
    {
        get
        {
            return _measurements.Aggregate((decimal)0, (acc, curr) => acc + curr.Total);
        }
    }
    public MBSheetItem(int workOrderItemId)
    {
        WorkOrderItemId = workOrderItemId;     
    }

    public MBSheetItem()
    {
    }   
  
    public void AddMeasurement(MBItemMeasurement measurement)
    {
        _measurements.Add(measurement);
    }

    public void ClearMeasurements()
    {
        _measurements.Clear();
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
