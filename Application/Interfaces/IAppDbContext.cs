using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RABillAggregate;
using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Uom> Uoms { get; set; }
        DbSet<WorkOrder> WorkOrders { get; set; }
        DbSet<MeasurementBook> MeasurementBooks { get; set; }
        DbSet<MBSheet> MBSheets { get; set; }
        DbSet<RABill> RABills { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}