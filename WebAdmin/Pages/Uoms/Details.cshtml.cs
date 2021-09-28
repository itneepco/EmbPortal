using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domian;
using Persistence;

namespace WebAdmin.Pages.Uoms
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Uom Uom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Uom = await _context.Uoms.FirstOrDefaultAsync(m => m.Id == id);

            if (Uom == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
