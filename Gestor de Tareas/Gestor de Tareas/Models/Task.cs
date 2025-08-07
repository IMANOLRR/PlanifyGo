using Microsoft.AspNetCore.Mvc;

namespace Gestor_de_Tareas.Models
{
    public class Task
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public int priority { get; set; }
        public int userId { get; set; }

        
    }
}
