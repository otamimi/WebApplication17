using System.ComponentModel.DataAnnotations;

namespace WebApplication17.Models
{
    public class  Note   
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}