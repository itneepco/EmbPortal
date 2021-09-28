using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domian;

namespace WebAdmin.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly Persistence.AppDbContext _context;

        public IndexModel(Persistence.AppDbContext context)
        {
            _context = context;
        }

        public IList<Project> Project { get;set; }

        public async Task OnGetAsync()
        {
            Project = await _context.Projects.ToListAsync();
        }
    }
}
