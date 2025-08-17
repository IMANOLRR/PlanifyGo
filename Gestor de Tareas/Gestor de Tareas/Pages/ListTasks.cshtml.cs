using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;
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

        public List<Task> Tasks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return RedirectToPage("/Login");

            Tasks = await _context.Tasks
                .Where(t => t.userId == user.id)
                .OrderBy(t => t.priority)
                .ToListAsync();

            return Page();
        }

        public IActionResult OnPostToggleComplete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.id == id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                _context.SaveChanges();
            }

            return RedirectToPage("/ListTasks");
        }
    }
}
