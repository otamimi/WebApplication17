using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication17.Models
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        public Bank Bank { get; set; }
        public string IBAN { get; set; }

    }
}
