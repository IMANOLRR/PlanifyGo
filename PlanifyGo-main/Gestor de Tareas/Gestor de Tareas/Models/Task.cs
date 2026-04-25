using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gestor_de_Tareas.Models
{
    public class Task
    {
        public int id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Tags { get; set; }

        public int Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int userId { get; set; }
        public User? User { get; set; }
    }
}
