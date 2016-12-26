using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication17.Data;
using WebApplication17.Models;
using WebApplication17.ViewModels;

namespace WebApplication17.Controllers
{
    public class RefundController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContextcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        public RefundController( UserManager<ApplicationUser> userManager, IHostingEnvironment environment, ApplicationDbContext applicationDbContextcontext )
        {
            
            _userManager = userManager;
            _environment = environment;
            _applicationDbContextcontext = applicationDbContextcontext;
        }

        [Authorize(Roles = "Applicant, Role1, Role2, Role3")]
        // GET: Refunds
        public async Task<IActionResult> Index(string sortOrder)
        {
            var model = new List<RefundsViewModel>();
          
            var refunds = await _applicationDbContextcontext.Requests.OfType<Refund>()
                .Include(x => x.Applicant)
                .Include(x => x.Bank)
                .Include(x => x.Country).ToListAsync();
            if (User.IsInRole("Applicant"))
                refunds =  refunds.Where(x => x.Applicant.UserName == User.Identity.Name).ToList();

            
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["SatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";

            //todo figure out sorting.
            #region sorting

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        requests = (requests.OrderByDescending(s => s.Applicant.FullName);
            //        break;
            //    case "Date":
            //        model = requests.OrderBy(s => s.TransactionTime);
            //        break;
            //    case "status_desc":
            //        model = requests.OrderByDescending(s => s.Status);
            //        break;
            //    case "type_desc":
            //        model = requests.OrderByDescending(s => s.Type);
            //        break;
            //    default:
            //        model = requests.OrderBy(s => s.TransactionTime);
            //        break;
            //}

            #endregion
            var  refundRequests = refunds.OrderBy(x => x.Status);
            
            model.AddRange(refundRequests.Select(refundRequest => new RefundsViewModel()
            {
                Id = refundRequest.Id,
                NationalIdNumber = refundRequest.Applicant.NationalId,
                Type = refundRequest.Type,
                IBAN = refundRequest.IBAN,
                Status = refundRequest.Status,
                Amount = refundRequest.Amount,
                TransactionTime = refundRequest.TransactionTime,
                ApplicantName = refundRequest.Applicant.FullName,
                BankName = refundRequest.Bank.ArabicName,
                CountryName = refundRequest.Country.Name,
                EmployeeName = refundRequest.Employee?.FullName
            }));

            return View(model);

          
        }
        [Authorize(Roles = "Applicant, Role1, Role2, Role3")]
        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refund = await _applicationDbContextcontext.Requests.OfType<Refund>().Include(x => x.Applicant)
                .Include(x => x.Bank)
                .Include(x => x.Country).SingleOrDefaultAsync(m => m.Id == id);
            if (refund == null) return NotFound();
            var model = new RefundsViewModel
            {
                Id = refund.Id,
                NationalIdNumber = refund.Applicant.NationalId,
                Type = refund.Type,
                IBAN = refund.IBAN,
                Status = refund.Status,
                Amount = refund.Amount,
                TransactionTime = refund.TransactionTime,
                ApplicantName = refund.Applicant.FullName,
                BankName = refund.Bank.ArabicName,
                CountryName = refund.Country.Name,
                EmployeeName = refund.Employee?.FullName
            };


            return View(model);
        }

        public JsonResult CanCreate()
        {

            return Json(_applicationDbContextcontext.Requests.Any(r =>
                      r.Applicant.UserName == User.Identity.Name && (
                      r.Status == RequestStatus.Accepted ||
                      r.Status == RequestStatus.Approved ||
                      r.Status == RequestStatus.Recieved)));
        }
        [Authorize(Roles = "Applicant")]
        // GET: Requests/Create
        public IActionResult Create()
        {
            var hasActiveRequest =
                _applicationDbContextcontext.Requests.Any(r =>
                       r.Applicant.UserName == User.Identity.Name && (
                       r.Status == RequestStatus.Accepted ||
                       r.Status == RequestStatus.Approved ||
                       r.Status == RequestStatus.Recieved));
            if (hasActiveRequest) return Ok(false);

            var countries = _applicationDbContextcontext.Countries.OrderBy(c => c.Name).Select(x => new {Id = x.Id, Value = x.Name});
            var banks = _applicationDbContextcontext.Banks.OrderBy(c => c.ArabicName).Select(x => new {Id = x.Id, Value = x.ArabicName});
            var model = new AddRefundViewModel()
            {
                BankList = new SelectList(banks, "Id", "Value"),
                CountryList = new SelectList(countries, "Id", "Value")
            };
            return View(model);
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Applicant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,TransactionTime,CountryId,Type,IBAN,BankId")] AddRefundViewModel refund, ICollection<IFormFile> files)

        {
            if (!ModelState.IsValid) return View(refund);
         
            //user is only allowed to create one refund at a time. when a refund status is "Paid" "Canceled" or "Rejected" it is considered finished, if the status is  " Recieved","Accepted" or "Approved" it is considered Open. so only allow if finished.

            var newRefund = new Refund()
            {
                Applicant = await GetCurrentUserAsync(),
                
                Status = refund.Status,
                Amount = refund.Amount,
                TransactionTime = refund.TransactionTime,
                Type = refund.Type,
                IBAN = refund.IBAN,
                Bank = _applicationDbContextcontext.Banks.FirstOrDefault(b=>b.Id== refund.BankId),
                Country = _applicationDbContextcontext.Countries.FirstOrDefault(c=>c.Id== refund.CountryId)
               
                
            };
            _applicationDbContextcontext.Add(newRefund);
            await _applicationDbContextcontext.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _applicationDbContextcontext.Requests.SingleOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Status,TransactionTime,Type")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _applicationDbContextcontext.Update(request);
                    await _applicationDbContextcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _applicationDbContextcontext.Requests.SingleOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _applicationDbContextcontext.Requests.SingleOrDefaultAsync(m => m.Id == id);
            _applicationDbContextcontext.Requests.Remove(request);
            await _applicationDbContextcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RequestExists(int id)
        {
            return _applicationDbContextcontext.Requests.Any(e => e.Id == id);
        }
    }
}
