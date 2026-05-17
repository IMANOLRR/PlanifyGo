using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Pages
{
    public class DeleteTaskModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public DeleteTaskModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Task Task { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.id == id && t.userId == user.id);
            if (task == null)
                return RedirectToPage("/ListTasks");

            Task = task;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == Task.id && t.userId == user.id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/ListTasks");
        }

        private async Task<Gestor_de_Tareas.Models.User?> GetCurrentUserAsync()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }
    }
}
