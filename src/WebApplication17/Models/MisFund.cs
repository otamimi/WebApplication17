using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication17.Models
{
   
    public class Misfund : Request
    {

        // the name of the SourceAccountNumber owner
        [Required]
        public string DepositorName { get; set; }
        //where the money came from
        [Required]
        public string SourceAccountNumber { get; set; }

        [Required]
        public string FromStudentNumber { get; set; }
        [Required]
        public string ToStudentNumber { get; set; }

    }
}
