using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using DAL.Entities;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace OrganizationsDirectory.Controllers
{
    [Authorize]
    public class PersonInOrganizationsController : Controller
    {
        private readonly OrganizationsDirectoryDbContext _context;

        public PersonInOrganizationsController(OrganizationsDirectoryDbContext context)
        {
            _context = context;
        }

        // get -> PersonInOrganizations
        public IActionResult Index(string searchString, string currentFilter, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var organizationsDirectoryDbContext = _context.PersonInOrganization.Include(p => p.Organization).Include(p => p.Person).Include(p => p.Position);

            var newList = organizationsDirectoryDbContext.ToListAsync().Result;

            if (!String.IsNullOrEmpty(searchString))
            {

                newList = newList.Where(p => p.Person.FullName.ToLower().Contains(searchString.ToLower()) || p.Organization.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(newList.ToPagedList(pageNumber, pageSize));
        }

        // get -> Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInOrganization = await _context.PersonInOrganization
                .Include(p => p.Organization)
                .Include(p => p.Person)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.PersonInOrganizationId == id);
            if (personInOrganization == null)
            {
                return NotFound();
            }

            return View(personInOrganization);
        }

        // get -> Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "Name");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "FullName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "Position");
            return View();
        }

        // set ->  Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonInOrganizationId,OrganizationId,PersonId,PositionId")] PersonInOrganization personInOrganization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personInOrganization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId", personInOrganization.OrganizationId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personInOrganization.PersonId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "Position", personInOrganization.PositionId);
            return View(personInOrganization);
        }

        // get -> Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInOrganization = await _context.PersonInOrganization.FindAsync(id);
            if (personInOrganization == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "Name", personInOrganization.OrganizationId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "FullName", personInOrganization.PersonId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "Position", personInOrganization.PositionId);
            return View(personInOrganization);
        }

        // set -> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonInOrganizationId,OrganizationId,PersonId,PositionId")] PersonInOrganization personInOrganization)
        {
            if (id != personInOrganization.PersonInOrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personInOrganization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonInOrganizationExists(personInOrganization.PersonInOrganizationId))
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
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationId", "OrganizationId", personInOrganization.OrganizationId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", personInOrganization.PersonId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "Position", personInOrganization.PositionId);
            return View(personInOrganization);
        }

        // get -> Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInOrganization = await _context.PersonInOrganization
                .Include(p => p.Organization)
                .Include(p => p.Person)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.PersonInOrganizationId == id);
            if (personInOrganization == null)
            {
                return NotFound();
            }

            return View(personInOrganization);
        }

        // set -> Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personInOrganization = await _context.PersonInOrganization.FindAsync(id);
            _context.PersonInOrganization.Remove(personInOrganization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonInOrganizationExists(int id)
        {
            return _context.PersonInOrganization.Any(e => e.PersonInOrganizationId == id);
        }
    }
}
