using Domain.Common;
using Domain.Entities.Identity;
using Domain.Entities.MeasurementBookAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.WorkOrderAggregate;

public class WorkOrder : AuditableEntity, IAggregateRoot
{
    public int Id { get; private set; }
    public long OrderNo { get;  set; }
    public DateTime OrderDate { get;  set; }      
    
    public string Project { get;  set; }
    public string Contractor { get;  set; }
    public string EngineerInCharge { get;  set; }
   // public AppUser Engineer { get;  set; }

    private readonly List<WorkOrderItem> _items = new List<WorkOrderItem>();
    public IReadOnlyList<WorkOrderItem> Items => _items.AsReadOnly();

    private readonly List<MeasurementBook> _measurementBooks = new List<MeasurementBook>();
    public IReadOnlyList<MeasurementBook> MeasurementBooks => _measurementBooks.AsReadOnly();

      
    public void AddUpdateLineItem(
        int itemNo,
        string pacakageNo,
        string itemDesc,
        int subItemNo,
        string subItemPacakageNo,
        long serviceNo,
        string shortServiceDesc,
        string longServiceDesc,
        string uom, 
        decimal unitRate, 
        float poQuantity, 
        int id=0)
    {
        // for item update
        if (id != 0)
        {
            var item = _items.FirstOrDefault(p => p.Id == id);
            item.ItemNo = itemNo;
            item.PackageNo =pacakageNo;
            item.ItemDescription = itemDesc;
            item.SubItemNo = subItemNo;
            item.SubItemPackageNo = subItemPacakageNo;
            item.ServiceNo = serviceNo;
            item.ShortServiceDesc = shortServiceDesc;
            item.LongServiceDesc = longServiceDesc;
            item.Uom = uom; 
            item.UnitRate = unitRate;
            item.PoQuantity = poQuantity;
            
        }
        else // new item
        {
            _items.Add(new WorkOrderItem(
                itemNo: itemNo,
                packageNo: pacakageNo,
                itemDesc: itemDesc,
                subItemNo: subItemNo,
                subItemPackageNo: subItemPacakageNo,
                serviceNo: serviceNo,
                shortServiceDesc: shortServiceDesc,
                longServiceDesc: longServiceDesc,
                uom: uom,
                unitRate: unitRate,
                poQuantity: poQuantity
            ));
        }
    }

    public void RemoveLineItem(int id)
    {
        var item = _items.SingleOrDefault(p => p.Id == id);

        if(item != null) // if item exists in the list
        {                
            _items.Remove(item);
        }
    }

   
    public void SetOrderNo(long orderNo)
    {
        OrderNo = orderNo;
    }

    public void SetEngineerInCharge(string engineerInCharge)
    {
        EngineerInCharge = engineerInCharge;
    }

    public void SetOrderDate(DateTime orderDate)
    {
        OrderDate = orderDate;
    }
    public void SetProject(string project)
    {
        Project = project;
    }

    public void SetContractor(string contractor)
    {
        Contractor = contractor;
    }
}