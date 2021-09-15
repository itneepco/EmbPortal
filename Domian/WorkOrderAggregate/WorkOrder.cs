using System;
using System.Collections.Generic;
using Domian.Common;

namespace Domian.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string Title { get; private set; }
        public string AggrementNo { get; private set; }
        public int ProjectId { get; private set; }
        public Project Project { get; private set; }
        public int ContractorId { get; private set; }

        public Contractor Contractor { get; private set; }
        private readonly List<WorkOrderItem> _items = new List<WorkOrderItem>();
        public IReadOnlyList<WorkOrderItem> Items => _items.AsReadOnly();

        private WorkOrder()
        {
        }

        public WorkOrder(string orderNo, DateTime orderDate, string title, string aggrementNo, int projectId, int contractorId)
        {

            OrderNo = orderNo;
            OrderDate = orderDate;
            Title = title;
            AggrementNo = aggrementNo;
            ProjectId = projectId;
            ContractorId = contractorId;

        }

        public void AddLineItem(string name, int no, int uomId, int poQuantity)
        {
           _items.Add(new WorkOrderItem(name,no,uomId,poQuantity));
        }
        

    }
}