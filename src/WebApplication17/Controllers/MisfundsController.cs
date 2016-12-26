using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication17.Data;
using WebApplication17.Models;

namespace WebApplication17.Controllers
{
    public class MisfundsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MisfundsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Misfunds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Misfunds.ToListAsync());
        }

        // GET: Misfunds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misfund = await _context.Misfunds.SingleOrDefaultAsync(m => m.Id == id);
            if (misfund == null)
            {
                return NotFound();
            }

            return View(misfund);
        }

        // GET: Misfunds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Misfunds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Status,TransactionTime,Type,DepositorName,FromStudentNumber,SourceAccountNumber,ToStudentNumber")] Misfund misfund)
        {
            if (ModelState.IsValid)
            {
                _context.Add(misfund);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(misfund);
        }

        // GET: Misfunds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misfund = await _context.Misfunds.SingleOrDefaultAsync(m => m.Id == id);
            if (misfund == null)
            {
                return NotFound();
            }
            return View(misfund);
        }

        // POST: Misfunds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Status,TransactionTime,Type,DepositorName,FromStudentNumber,SourceAccountNumber,ToStudentNumber")] Misfund misfund)
        {
            if (id != misfund.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(misfund);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MisfundExists(misfund.Id))
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
            return View(misfund);
        }

        // GET: Misfunds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misfund = await _context.Misfunds.SingleOrDefaultAsync(m => m.Id == id);
            if (misfund == null)
            {
                return NotFound();
            }

            return View(misfund);
        }

        // POST: Misfunds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var misfund = await _context.Misfunds.SingleOrDefaultAsync(m => m.Id == id);
            _context.Misfunds.Remove(misfund);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MisfundExists(int id)
        {
            return _context.Misfunds.Any(e => e.Id == id);
        }
    }
}
