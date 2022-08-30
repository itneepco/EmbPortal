using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Entities.Identity;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public long OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }        
        public WorkOrderStatus Status { get; private set; }
        public string Project { get; private set; }
        public string Contractor { get; private set; }

        public string EngineerInCharge { get; private set; }
        public AppUser Engineer { get; private set; }

        private readonly List<WorkOrderItem> _items = new List<WorkOrderItem>();
        public IReadOnlyList<WorkOrderItem> Items => _items.AsReadOnly();

        private readonly List<MeasurementBook> _measurementBooks = new List<MeasurementBook>();
        public IReadOnlyList<MeasurementBook> MeasurementBooks => _measurementBooks.AsReadOnly();

        private WorkOrder()
        {
        }

        public WorkOrder(
            long orderNo, 
            DateTime orderDate, 
            string project, 
            string contractor, 
            string engineerInCharge
        )
        {
            OrderNo = orderNo;
            OrderDate = orderDate;
            Project = project;
            Contractor = contractor;
            Status = WorkOrderStatus.CREATED;
            EngineerInCharge = engineerInCharge;
        }

        public void AddUpdateLineItem(
            int itemNo,
            string pacakageNo,
            string itemDesc,
            int subItemNo,
            string subItemPacakageNo,
            long serviceNo,
            string shortServiceDesc,
            string longServiceDesc,
            int uomId, 
            decimal unitRate, 
            float poQuantity, 
            int id=0)
        {
            // for item update
            if (id != 0)
            {
                var item = _items.FirstOrDefault(p => p.Id == id);
                item.SetItemNo(itemNo);
                item.SetPackageNo(pacakageNo);
                item.SetItemDescription(itemDesc);
                item.SetSubItemNo(subItemNo);
                item.SetSubItemPackageNo(subItemPacakageNo);
                item.SetServiceNo(serviceNo);
                item.SetShortServiceDesc(shortServiceDesc);
                item.SetLongServiceDesc(longServiceDesc);
                item.SetUomId(uomId);
                item.SetUnitRate(unitRate);
                item.SetPoQuantity(poQuantity);
                item.MarkPublished();
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
                    uomId: uomId,
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

        public void MarkPublished()
        {
            Status = WorkOrderStatus.PUBLISHED;
            foreach (var item in _items)
            {
                item.MarkPublished();
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
}