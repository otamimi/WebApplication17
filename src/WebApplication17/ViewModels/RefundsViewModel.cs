using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication17.Models;

namespace WebApplication17.ViewModels
{
    public class RefundsViewModel : RequestsViewModel
    {

        [Display(Name = "رقم الآيبان")]
        public string ToAccountIban { get; set; }

        public string ToAccountHolderFullName { get; set; }



    }
    public class AddRefundViewModel
    {
        [Required]
        [Range(1, 10000)]
        public decimal Amount { get; set; }

        //  [Required]
        // [DataType(DataType.DateTime)]
        public DateTime TransactionTime { get; set; }


        //  [Required]
        [Display(Name = "نوع الطلب")]
        public RequestType Type { get; set; }




        // [Required]
        [Display(Name = "حالة الطلب")]
        public RequestStatus Status { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationUser Employee { get; set; }

        [Required]
        [StringLength(34, ErrorMessage = "not valid IBAN")]
        public string IBAN { get; set; }

        [Required]
        [Display(Name = "البنك")]
        public int BankId { get; set; }
        public SelectList BankList { get; set; }

        [Required]
        [Display(Name = "الدولة")]
        public int CountryId { get; set; }
        public SelectList CountryList { get; set; }





    }
    public class EditRefundViewModel : RequestsViewModel
    {
        [Display(Name = "رقم الهوية الوطنية/الاقامة"),ReadOnly(true)]
        public new string NationalIdNumber { get; set; }
        [Display(Name = "حالة الطلب"),ReadOnly(true)]
        public new RequestStatus Status { get; set; }
        [Display(Name = "رقم الآيبان")]
        public string ToAccountIban { get; set; }

        public string ToAccountHolderFullName { get; set; }

        [Required]
        [Display(Name = "البنك")]
        public int BankId { get; set; }
        public SelectList BankList { get; set; }

        [Required]
        [Display(Name = "الدولة")]
        public int CountryId { get; set; }
        public SelectList CountryList { get; set; }
    }
}