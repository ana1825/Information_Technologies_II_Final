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
using Microsoft.Extensions.Logging;

namespace OrganizationsDirectory.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly OrganizationsDirectoryDbContext _context;

        private ILogger<OrganizationsController> _logger;

        public OrganizationsController(OrganizationsDirectoryDbContext context, ILogger<OrganizationsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // get -> Organizations
        public IActionResult Index(string searchString, string currentFilter, int? page)
        {
            _logger.LogInformation("Entered index");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var newOrganizations = _context.Organization.ToListAsync().Result;

            if (!String.IsNullOrEmpty(searchString))
            {

                newOrganizations = newOrganizations.Where(p => p.Name.ToLower().Contains(searchString.ToLower()) || p.Address.ToLower().Contains(searchString.ToLower())).ToList();
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(newOrganizations.ToPagedList(pageNumber, pageSize));
        }

        // get -> Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // get -> Create
        public IActionResult Create()
        {
            return View();
        }

        // set -> Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,Name,Address,Activities")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // get -> Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // set -> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationId,Name,Address,Activities")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
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
            return View(organization);
        }

        // get -> Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // set -> Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _context.Organization.FindAsync(id);
            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organization.Any(e => e.OrganizationId == id);
        }
    }
}
