using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;
using Task = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Pages
{
    public class EditTaskModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public EditTaskModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Task Input { get; set; }
        public Task Task { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == id && t.userId == user.id);
            if (task == null)
            {
                ErrorMessage = "Tarea no encontrada o no tienes permisos.";
                return RedirectToPage("/ListTasks");
            }

            Input = task;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var email = HttpContext.Session.GetString("UserEmail");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == Input.id && t.userId == user.id);
            if (task == null)
            {
                ErrorMessage = "Tarea no encontrada.";
                return RedirectToPage("/ListTasks");
            }

            task.title = Input.title;
            task.description = Input.description;
            task.tags = Input.tags;
            task.priority = Input.priority;
            task.DueDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToPage("/ListTasks");
        }
    }
}
