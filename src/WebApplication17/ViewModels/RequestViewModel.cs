using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication17.Models;

namespace WebApplication17.ViewModels
{
    public class AddRefundViewModel
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

    public class RefundsViewModel : RequestsViewModel
    {
       
        [Display(Name = "رقم الآيبان")]
        public string IBAN { get; set; }
        public Payroll Payroll { get; set; }
    }
    public class  RequestsViewModel
    {

        public int Id { get; set; }
        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }


        [Display(Name = "رقم الهوية الوطنية/الاقامة")]
        public string NationalIdNumber { get; set; }
        [Display(Name = "تاريخ العملية")]

        public DateTime TransactionTime { get; set; }


        
        [Display(Name = "نوع الطلب")]
        public RequestType Type { get; set; }

        

        [Display(Name = "حالة الطلب")]
        public RequestStatus Status { get; set; }

        [Display(Name = "اسم مقدم الطلب")]
        public string ApplicantName { get; set; }
        [Display(Name = "اسم الموظف")]
        public string EmployeeName { get; set; }

        [Display(Name = "نوع البنك")]
        public bool LocalBank { get; set; }

        [Display(Name = "البنك")]
        public string BankName { get; set; }
       

        [Display(Name = "الدولة")]
        public string CountryName { get; set; }
      





    }

    public class BankViewModel
    {


        [Required]
        [Display(Name = "اسم البنك")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "بنك محلي؟")]
        public BankType Type { get; set; }


    }

    public class BankAccountViewModel
    {
        [Required]
        public int BankId { get; set; }
        [Required]
        public string IBAN { get; set; }
    }
}
