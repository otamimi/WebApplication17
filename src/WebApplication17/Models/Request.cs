using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication17.Models
{
    public class Request
    {
       
        [Key]
        public int Id { get; set; }
        [Required]
        public ApplicationUser Applicant{ get; set; }
       // [Required]
        public ApplicationUser Employee { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
       [Required]
        public RequestType Type { get; set; }
       [Required]
        public decimal Amount { get; set; }
        public Payroll Payroll { get; set; }
        public List<Note> Notes { get; set; }
        [Required]
        public Country Country { get; set; }
        
        public DateTime TransactionTime { get; set; }
        [Required]
        public Bank Bank { get; set; }

       // public file Type1 { get; set; }

        [Required]
        public string IBAN { get; set; }
    }
}
