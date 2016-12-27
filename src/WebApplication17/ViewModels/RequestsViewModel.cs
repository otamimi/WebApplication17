using System;
using System.ComponentModel.DataAnnotations;
using WebApplication17.Models;

namespace WebApplication17.ViewModels
{
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
        public BankType LocalBank { get; set; }

        [Display(Name = "البنك")]
        public string BankName { get; set; }
       

        [Display(Name = "الدولة")]
        public string CountryName { get; set; }
      





    }
}