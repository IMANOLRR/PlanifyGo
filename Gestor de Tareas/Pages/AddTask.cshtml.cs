using Gestor_de_Tareas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gestor_de_Tareas.Pages
{
    public class AddTaskModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public AddTaskModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string title { get; set; } = string.Empty;

            public string? description { get; set; }

            public string? tags { get; set; }

            [Range(1, 3)]
            public int priority { get; set; }

            public DateTime? DueDate { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
                return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToPage("/Login");

            var task = new Gestor_de_Tareas.Models.Task
            {
                Title = Input.title,
                Description = Input.description,
                Tags = Input.tags,
                Priority = Input.priority,
                DueDate = Input.DueDate,
                userId = user.id,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
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
