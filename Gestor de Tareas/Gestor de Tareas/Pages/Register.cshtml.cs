using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;

namespace Gestor_de_Tareas.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterModel(ApplicationDBContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string name { get; set; }

            [Required]
            public string lastName { get; set; }

            [Required]
            [EmailAddress]
            public string email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            public string password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("password", ErrorMessage = "Passwords do not match")] // <-- corregido para usar minúscula
            public string confirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var exists = await _context.Users.AnyAsync(u => u.email == Input.email);
            if (exists)
            {
                ErrorMessage = "Email is already registered.";
                return Page();
            }

            var user = new User
            {
                name = Input.name,
                lastName = Input.lastName,
                email = Input.email,
                password = _passwordHasher.HashPassword(null, Input.password) // o en texto si no quieres hash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }
    }

}

