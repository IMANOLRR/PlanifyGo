using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public Task Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.id == id && t.userId == user.id);
            if (task == null)
                return RedirectToPage("/ListTasks");

            Input = task;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.id == Input.id && t.userId == user.id);
            if (task == null)
                return RedirectToPage("/ListTasks");

            task.Title = Input.Title;
            task.Description = Input.Description;
            task.Tags = Input.Tags;
            task.Priority = Input.Priority;
            task.DueDate = Input.DueDate;

            await _context.SaveChangesAsync();

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
