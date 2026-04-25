using System.ComponentModel.DataAnnotations;

namespace Gestor_de_Tareas.Models
{
    public class User
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
