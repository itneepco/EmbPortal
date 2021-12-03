using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrder : AuditableEntity, IAggregateRoot
    {

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }        
        public bool IsCompleted { get; private set; }
        public string Title { get; private set; }
        public string AgreementNo { get; private set; }
        public DateTime AgreementDate { get; private set; }
        public int ProjectId { get; private set; }
        public int ContractorId { get; private set; }
        public string EngineerInCharge { get; private set; }

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
            IsCompleted = false;
            EngineerInCharge = engineerInCharge;
        }

        public void AddUpdateLineItem(string description, int itemNo, int uomId, decimal rate, float poQuantity, int id = 0)
        {
            if(IsCompleted) return;

            // for item update
            if (id != 0)
            {
                var item = _items.FirstOrDefault(p => p.Id == id);
                item.SetDescription(description);
                item.SetItemNo(itemNo);
                item.SetUomId(uomId);
                item.SetPoQuantity(poQuantity);
                item.SetUnitRate(rate);
            }
            else // new item
            {
                _items.Add(new WorkOrderItem(description, itemNo, uomId, rate, poQuantity));
            }
        }

        public void RemoveLineItem(int id)
        {
            if (IsCompleted) return;

            var item = _items.SingleOrDefault(p => p.Id == id);

            if(item != null) // if item exists in the list
            {
                _items.Remove(item);
            }
        }
        
        public void MarkComplete() {
            IsCompleted = true;
        }

        public void SetOrderNo(string orderNo)
        {
            OrderNo = orderNo;
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