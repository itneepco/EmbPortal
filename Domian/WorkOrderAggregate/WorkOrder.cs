using System;
using Domian.Common;

namespace Domian.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {
        private WorkOrder()
        {
        }

        public WorkOrder( string orderNo, DateTime orderDate, string title, string aggrementNo, int projectId,  int contractorId)
        {
            
            OrderNo = orderNo;
            OrderDate = orderDate;
            Title = title;
            AggrementNo = aggrementNo;
            ProjectId = projectId;           
            ContractorId = contractorId;
           
        }

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string Title { get; private set; }
        public string AggrementNo { get; private set; }
        public int ProjectId { get; private set; }
        public Project Project { get; private set; }
        public int ContractorId { get; private set; }
        public Contractor Contractor { get; private set; }


    }
}