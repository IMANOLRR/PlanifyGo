using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;
using Microsoft.AspNetCore.Http;
using Task = Gestor_de_Tareas.Models.Task;

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
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            public string title { get; set; }
            public string description { get; set; }
            public string tags { get; set; }
            public int priority { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
                return RedirectToPage("/Login");

            var user = _context.Users.FirstOrDefault(u => u.email == userEmail);
            if (user == null)
            {
                ErrorMessage = "Usuario no encontrado.";
                return Page();
            }

            var task = new Task
            {
                title = Input.title,
                description = Input.description,
                tags = Input.tags,
                priority = Input.priority,
                userId = user.id
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}

