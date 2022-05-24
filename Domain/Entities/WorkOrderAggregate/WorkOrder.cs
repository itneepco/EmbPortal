using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }        
        public WorkOrderStatus Status { get; private set; }
        public string Title { get; private set; }
        public string AgreementNo { get; private set; }
        public DateTime AgreementDate { get; private set; }
        public int ProjectId { get; private set; }
        public int ContractorId { get; private set; }
        public string EngineerInCharge { get; private set; }
        public DateTime CommencementDate { get; private set; }
        public DateTime CompletionDate { get; private set; }

        public Project Project { get; private set; }
        public Contractor Contractor { get; private set; }

        private readonly List<WorkOrderItem> _items = new List<WorkOrderItem>();
        public IReadOnlyList<WorkOrderItem> Items => _items.AsReadOnly();

        private readonly List<MeasurementBook> _measurementBooks = new List<MeasurementBook>();
        public IReadOnlyList<MeasurementBook> MeasurementBooks => _measurementBooks.AsReadOnly();

        private WorkOrder()
        {
        }

        public WorkOrder(string orderNo, DateTime orderDate, string title, string agreementNo, DateTime agreementDate, int projectId, int contractorId, string engineerInCharge)
        {
            OrderNo = orderNo;
            OrderDate = orderDate;
            Title = title;
            AgreementNo = agreementNo;
            AgreementDate = agreementDate;
            ProjectId = projectId;
            ContractorId = contractorId;
            Status = WorkOrderStatus.CREATED;
            EngineerInCharge = engineerInCharge;
        }

        public void AddUpdateLineItem(string description, int uomId, decimal unitRate, float poQuantity, int id=0)
        {
            if(Status == WorkOrderStatus.COMPLETED) return;

            // for item update
            if (id != 0)
            {
                var item = _items.FirstOrDefault(p => p.Id == id);
                item.SetDescription(description);
                item.SetUomId(uomId);
                item.SetUnitRate(unitRate);
                item.SetPoQuantity(poQuantity);
                item.MarkPublished();
            }
            else // new item
            {
                _items.Add(new WorkOrderItem(description, uomId, unitRate, poQuantity));
            }
        }

        public void RemoveLineItem(int id)
        {
            if (Status == WorkOrderStatus.COMPLETED) return;

            var item = _items.SingleOrDefault(p => p.Id == id);

            if(item != null) // if item exists in the list
            {                
                _items.Remove(item);
            }
        }
        
        public void MarkCompleted() {
            Status = WorkOrderStatus.COMPLETED;
        }

        public void MarkPublished()
        {
            Status = WorkOrderStatus.PUBLISHED;
            foreach (var item in _items)
            {
                item.MarkPublished();
            }
        }

        public void SetOrderNo(string orderNo)
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

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetAgreementNo(string agreementNo)
        {
            AgreementNo = agreementNo;
        }

        public void SetAgreementDate(DateTime agreementDate)
        {
            AgreementDate = agreementDate;
        }

        public void SetProjectId(int projectId)
        {
            ProjectId = projectId;
        }

        public void SetContractorId(int contractorId)
        {
            ContractorId = contractorId;
        }
    }
}