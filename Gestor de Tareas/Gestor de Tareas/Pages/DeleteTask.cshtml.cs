using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;
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
        public Task Task { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return RedirectToPage("/Login");

            Task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == id && t.userId == user.id);

            if (Task == null)
                return RedirectToPage("/ListTasks");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == Task.id && t.userId == user.id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/ListTasks");
        }
    }
}
