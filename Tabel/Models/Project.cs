using System.ComponentModel.DataAnnotations;

namespace Tabel.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}