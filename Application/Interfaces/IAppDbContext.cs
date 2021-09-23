using System.Threading;
using System.Threading.Tasks;
using Domian;
using Domian.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Uom> Uoms { get; set; }
        DbSet<Contractor> Contractors {get; set;}
        DbSet<WorkOrder> WorkOrders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}