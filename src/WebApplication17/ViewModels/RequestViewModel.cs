using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication17.Models;

namespace WebApplication17.ViewModels
{
    public class AddRequestViewModel
    {
       [Required]
       [Range(1,10000)]
       public decimal Amount { get; set; }

      //  [Required]
       // [DataType(DataType.DateTime)]
        public DateTime TransactionTime { get; set; }
     

        //  [Required]
        [Display(Name = "نوع الطلب")]
        public RequestType Type { get; set; }

     //   [Required]
        [Display(Name = " حساب المصدر")]
        public BankAccount Account { get; set; }

       // [Required]
        [Display(Name = "حالة الطلب")]
        public RequestStatus Status { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationUser Employee { get; set; }

        [Required]
        [StringLength(34,ErrorMessage = "not valid IBAN")]
        public string IBAN { get; set; }

        [Required]
        [Display(Name = "البنك")]
        public  int BankId  { get; set; }
        public SelectList BankList { get; set; }

        [Required]
        [Display(Name = "الدولة")]
        public int CountryId { get; set; }
        public SelectList CountryList { get; set; }

      
       

        
    }
    public class ViewRequestsViewModel
    {
        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ العملية")]

        public DateTime TransactionTime { get; set; }


        
        [Display(Name = "نوع الطلب")]
        public RequestType Type { get; set; }

        [Display(Name = " حساب المصدر")]
        public BankAccount Account { get; set; }

        [Display(Name = "حالة الطلب")]
        public RequestStatus Status { get; set; }

        [Display(Name = "اسم مقدم الطلب")]
        public ApplicationUser Applicant { get; set; }
        [Display(Name = "اسم الموظف")]
        public ApplicationUser Employee { get; set; }


        [Display(Name = "رقم الآيبان")]
        public string IBAN { get; set; }

        [Display(Name = "البنك")]
        public Bank Bank { get; set; }
       

        [Display(Name = "الدولة")]
        public Country Country { get; set; }
      





    }

    public class BankViewModel
    {


        [Required]
        [Display(Name = "اسم البنك")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "بنك محلي؟")]
        public bool Local { get; set; }


    }

    public class BankAccountViewModel
    {
        [Required]
        public int BankId { get; set; }
        [Required]
        public string IBAN { get; set; }
    }
}
