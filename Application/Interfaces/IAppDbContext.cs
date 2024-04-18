using System.Threading;
using System.Threading.Tasks;

using Domain.Entities.Identity;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RAAggregate;
using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;
public interface IAppDbContext
{
    DbSet<AppUser> AppUsers { get; set; }
    DbSet<WorkOrder> WorkOrders { get; set; }
    DbSet<MeasurementBook> MeasurementBooks { get; set; }
    DbSet<MBSheet> MBSheets { get; set; }
    DbSet<RAHeader> RAHeaders { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}