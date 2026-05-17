using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Pages
{
    public class ListTasksModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public ListTasksModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Task> Tasks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            Tasks = await _context.Tasks
                .AsNoTracking()
                .Where(t => t.userId == user.id)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostToggleCompleteAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == id && t.userId == user.id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
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
