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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace OrganizationsDirectory.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly OrganizationsDirectoryDbContext _context;
        private readonly IWebHostEnvironment _IWebHostEnvironment;


        public PeopleController(OrganizationsDirectoryDbContext context, IWebHostEnvironment IWebHostEnvironment)
        {
            _context = context;
            _IWebHostEnvironment = IWebHostEnvironment;
        }

        // get -> People
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

            var newPersons = _context.Person.ToListAsync().Result;

            if (!String.IsNullOrEmpty(searchString))
            {

                newPersons = newPersons.Where(p => p.FullName.ToLower().Contains(searchString.ToLower()) || p.City.ToLower().Contains(searchString.ToLower())).ToList();
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(newPersons.ToPagedList(pageNumber, pageSize));
        }

        // get -> Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // get -> Create
        public IActionResult Create()
        {
            return View();
        }

        // set -> Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Picture,FirstName,LastName,PersonalId,Sex,BirthDate,City,PhoneNumber,FullName,ImageFile")] Person person)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _IWebHostEnvironment.WebRootPath;
                string FileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                string extension = Path.GetExtension(person.ImageFile.FileName);
                person.Picture = FileName = FileName + DateTime.Now.ToString("yymmssff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await person.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // get -> Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // set -> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Picture,FirstName,LastName,PersonalId,Sex,BirthDate,City,PhoneNumber,FullName,ImageFile")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _IWebHostEnvironment.WebRootPath;
                    string FileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                    string extension = Path.GetExtension(person.ImageFile.FileName);
                    person.Picture = FileName = FileName + DateTime.Now.ToString("yymmssff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/", FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await person.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            return View(person);
        }

        // get -> Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // set -> Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}
