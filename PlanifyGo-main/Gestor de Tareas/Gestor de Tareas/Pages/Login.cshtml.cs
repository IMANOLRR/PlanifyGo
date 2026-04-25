using Gestor_de_Tareas.Data;
using Gestor_de_Tareas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestor_de_Tareas.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;
        public LoginModel(ApplicationDBContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.email == Email);

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ErrorMessage = "Email o contraseña incorrectos.";
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.email);
            HttpContext.Session.SetString("UserName", user.name);

            return RedirectToPage("/Index");
        }

    }
}
