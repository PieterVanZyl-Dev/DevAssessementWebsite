using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevAssessementWebsite.Models;

namespace DevAssessementWebsite.Controllers
{
    public class UserInformationsController : Controller
    {
        private readonly DevAssessmentContext _context;

        public UserInformationsController(DevAssessmentContext context)
        {
            _context = context;
        }

        // GET: UserInformations
        public async Task<IActionResult> Index()
        {
            var devAssessmentContext = _context.UserInformations.Include(u => u.Person);
            return View(await devAssessmentContext.ToListAsync());
        }

        // GET: UserInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations
                .Include(u => u.Person)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // GET: UserInformations/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,TellNo,CellNo,AddressLine1,AddressLine2,AddressLine3,AddressCode,PostalAddress1,PostalAddress2,PostalCode")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", userInformation.PersonId);
            return View(userInformation);
        }

        // GET: UserInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations.FindAsync(id);
            if (userInformation == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", userInformation.PersonId);
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,TellNo,CellNo,AddressLine1,AddressLine2,AddressLine3,AddressCode,PostalAddress1,PostalAddress2,PostalCode")] UserInformation userInformation)
        {
            if (id != userInformation.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(userInformation.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", userInformation.PersonId);
            return View(userInformation);
        }

        // GET: UserInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations
                .Include(u => u.Person)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userInformation = await _context.UserInformations.FindAsync(id);
            _context.UserInformations.Remove(userInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInformationExists(int id)
        {
            return _context.UserInformations.Any(e => e.PersonId == id);
        }
    }
}
