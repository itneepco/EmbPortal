using System;
using System.Collections.Generic;
using Domian.Common;
using Domian.MeasurementBookAggregate;

namespace Domian.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }        
        public bool IsCompleted { get; private set; }
        public string Title { get; private set; }
        public string AggrementNo { get; private set; }
        public DateTime AggrementDate { get; private set; }
        public int ProjectId { get; private set; }
        public Project Project { get; private set; }
        public int ContractorId { get; private set; }
        public Contractor Contractor { get; private set; }
        public string EngineerInCharge { get; private set; }
        private readonly List<WorkOrderItem> _items = new List<WorkOrderItem>();
        public IReadOnlyList<WorkOrderItem> Items => _items.AsReadOnly();
        private readonly List<MeasurementBook> _measurementBooks = new List<MeasurementBook>();
        public IReadOnlyList<MeasurementBook> MeasurementBooks => _measurementBooks.AsReadOnly();

        private WorkOrder()
        {
        }

        public WorkOrder(string orderNo, DateTime orderDate, string title, string aggrementNo, DateTime aggrementDate, int projectId, int contractorId,string engineerInCharge)
        {

            OrderNo = orderNo;
            OrderDate = orderDate;
            Title = title;
            AggrementNo = aggrementNo;
            AggrementDate = aggrementDate;
            ProjectId = projectId;
            ContractorId = contractorId;
            IsCompleted = false;
            EngineerInCharge = engineerInCharge;
        }

        public void AddLineItem(string name, int no, int uomId, decimal rate, int poQuantity)
        {
            if(IsCompleted) return;
           _items.Add(new WorkOrderItem(name,no,uomId,rate,poQuantity));
        }
        
        public void MarkComplete() {
            IsCompleted = true;
        }
    }
}