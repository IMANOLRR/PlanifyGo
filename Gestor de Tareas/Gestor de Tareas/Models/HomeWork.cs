using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Etiqueta { get; set; }

        [Required]
        public string Prioridad { get; set; }

        public int usersId { get; set; }
    }
}
