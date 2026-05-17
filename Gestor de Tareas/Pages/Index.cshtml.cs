using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskItem = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public IList<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int HighPriority { get; set; }
        public int MediumPriority { get; set; }
        public int LowPriority { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/Login");

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return RedirectToPage("/Login");

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            Tasks = await _context.Set<TaskItem>()
                .AsNoTracking()
                .Where(t => t.userId == user.id)
                .ToListAsync();

            TotalTasks = Tasks.Count;
            CompletedTasks = Tasks.Count(t => t.IsCompleted);
            PendingTasks = TotalTasks - CompletedTasks;

            HighPriority = Tasks.Count(t => t.Priority == 1);
            MediumPriority = Tasks.Count(t => t.Priority == 2);
            LowPriority = Tasks.Count(t => t.Priority == 3);

            return Page();
        }
    }
}
