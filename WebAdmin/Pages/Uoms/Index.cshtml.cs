using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domian;
using Persistence;

namespace WebAdmin.Pages.Uoms
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Uom> Uoms { get;set; }

        public async Task OnGetAsync()
        {
            Uoms = await _context.Uoms.ToListAsync();
        }
    }
}
