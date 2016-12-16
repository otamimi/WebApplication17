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
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var model = new List<Request>();
            foreach (var contextRequest in _context.Requests)
            {
                model.Add(contextRequest);
            }
            var vvv = await _context.Database.ExecuteSqlCommandAsync("Select * from Requests");
            var v =  _context.Requests.ToList();
           return View(v);
            return User.Identity.IsAuthenticated
                ? (User.IsInRole("Role1, Role2, Role3")
                    ? View(await _context.Requests.ToListAsync())
                    : View(await _context.Requests.Where(x => x.Applicant.UserName == User.Identity.Name).ToListAsync()))
                : View();
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }
        [Authorize(Roles = "Applicant")]
        // GET: Requests/Create
        public IActionResult Create()
        {
            var hasActiveRequest =
                _context.Requests.Any(r =>
                       r.Applicant.UserName == User.Identity.Name && (
                       r.Status == RequestStatus.Accepted ||
                       r.Status == RequestStatus.Approved ||
                       r.Status == RequestStatus.Recieved));
            if (hasActiveRequest) return RedirectToAction("Index");

            var countries = _context.Countries.OrderBy(c => c.Name).Select(x => new {Id = x.Id, Value = x.Name});
            var banks = _context.Banks.OrderBy(c => c.ArabicName).Select(x => new {Id = x.Id, Value = x.ArabicName});
            var model = new AddRequestViewModel()
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
        public async Task<IActionResult> Create([Bind("Id,Amount,TransactionTime,CountryId,Type,IBAN,BankId")] AddRequestViewModel request, ICollection<IFormFile> files)

        {
            if (!ModelState.IsValid) return View(request);
         
            //user is only allowed to create one request at a time. when a request status is "Paid" "Canceled" or "Rejected" it is considered finished, if the status is  " Recieved","Accepted" or "Approved" it is considered Open. so only allow if finished.

            var req = new Request()
            {
                Applicant = await GetCurrentUserAsync(),
                //Country = request.Countries,
                Status = request.Status,
                Amount = request.Amount,
                TransactionTime = request.TransactionTime,
                Type = request.Type,
                IBAN = request.IBAN,
                Bank = _context.Banks.FirstOrDefault(b=>b.Id==request.BankId),
                Country = _context.Countries.FirstOrDefault(c=>c.Id==request.CountryId)
               // Banks = request.BanksList.
                
            };
            _context.Add(req);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);
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
                    _context.Update(request);
                    await _context.SaveChangesAsync();
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

            var request = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);
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
            var request = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
