using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication17.Models
{
    public class MisFund : Request
    {
        // the name of the SourceAccountNumber owner
        [Required]
        public string DepositorName { get; set; }
        //where the money came from
        [Required]
        public string SourceAccountNumber { get; set; }
        [Required]
        
        public string FromStudentNumber { get; set; }

        public string ToStudentNumber { get; set; }

    }

    class Refund : Request
    {
        public Payroll Payroll { get; set; }
        [Required]
        public string IBAN { get; set; }
    }
}
