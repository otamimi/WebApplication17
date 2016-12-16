using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication17.Models
{
    public class UploadedDocument
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public int RequiredDocumentId { get; set; }
        public bool Verified { get; set; }
        [Required]
        public int RequestId { get; set; }
    }
}
