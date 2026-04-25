using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskItem = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Pages
{
    public class IndexModel(ApplicationDBContext context) : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;
        private readonly ApplicationDBContext _context = context;

        public IList<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int HighPriority { get; set; }
        public int MediumPriority { get; set; }
        public int LowPriority { get; set; }

        public void OnGet()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                Response.Redirect("/Login");
            }

            Tasks = _context.Set<TaskItem>().AsNoTracking().ToList();

            TotalTasks = Tasks.Count;
            CompletedTasks = Tasks.Count(t => t.IsCompleted);
            PendingTasks = TotalTasks - CompletedTasks;

            HighPriority = Tasks.Count(t => t.Priority == 3);
            MediumPriority = Tasks.Count(t => t.Priority == 2);
            LowPriority = Tasks.Count(t => t.Priority == 1);
        }
    }
}
