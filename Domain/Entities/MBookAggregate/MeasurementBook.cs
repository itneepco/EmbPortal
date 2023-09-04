using Domain.Common;
using EmbPortal.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.MeasurementBookAggregate;
public class MeasurementBook : AuditableEntity, IAggregateRoot
{
    public int Id { get; private set; }
    public int WorkOrderId { get; private set; }
    public string Title { get; private set; }    
    public string MeasurerEmpCode { get; private set; }  
    public string ValidatorEmpCode { get; private set; }
    public string EicEmpCode { get; private set; }
    public MBookStatus Status { get; private set; }   

    private readonly List<MBookItem> _items = new List<MBookItem>();
    public IReadOnlyList<MBookItem> Items => _items.AsReadOnly();

    public MeasurementBook()
    {
    }

    public MeasurementBook(int workOrderId, string title, string measurerEmpCode, string validatorEmpCode, string eicEmpCode)
    {
        Title = title;
        WorkOrderId = workOrderId;
        MeasurerEmpCode = measurerEmpCode;
        ValidatorEmpCode = validatorEmpCode;
        EicEmpCode = eicEmpCode;
        Status = MBookStatus.CREATED;
    }

    public void AddUpdateLineItem(int wOrderItemId, int id=0)
    {
        if (Status == MBookStatus.PUBLISHED || Status == MBookStatus.COMPLETED) return;

        if (id != 0)  // for item update
        {
            var item = _items.FirstOrDefault(p => p.Id == id);
           if (item != null)
            {
                item.WorkOrderItemId = wOrderItemId;
            }

        }
        else  // new item
        {
            _items.Add(new MBookItem(wOrderItemId));         
          
        }
    }

    public void RemoveLineItem(int id)
    {
        if (Status == MBookStatus.PUBLISHED || Status == MBookStatus.COMPLETED) return;

        var item = _items.SingleOrDefault(p => p.Id == id);

        if (item != null) // if item exists in the list
        {
            _items.Remove(item);
        }
    }

    public void MarkPublished()
    {
        Status = MBookStatus.PUBLISHED;
    }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetWorkOrderId(int workOrderId)
    {
        WorkOrderId = workOrderId;
    }

    public void SetMeasurementOfficer(string measurerEmpCode)
    {
        MeasurerEmpCode = measurerEmpCode;
    }

    public void SetValidatingOfficer(string validatorEmpCode)
    {
        ValidatorEmpCode = validatorEmpCode;
    }
    public void RemoveByOrderItemId(int orderItemid)
    {
        if (Status == MBookStatus.PUBLISHED || Status == MBookStatus.COMPLETED) return;

        var item = _items.SingleOrDefault(p => p.WorkOrderItemId == orderItemid);

        if (item != null) // if item exists in the list
        {
            _items.Remove(item);
        }
    }
}
