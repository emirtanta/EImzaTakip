using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EImzaTakip.Controllers
{
    [SessionAuthorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DepartmentList()
        {
            var values = await _context.Departments
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> DepartmentDetails(int id)
        {
            var value = await _context.Departments
                    .Include(x => x.Persons)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (value==null)
            {
                return NotFound("Birim bulunamadı!");
            }

            return View(value);
        }

        [HttpGet]
        public IActionResult DepartmentCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DepartmentCreate(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Birim başarıyla eklendi";

            return RedirectToAction(nameof(DepartmentList));
        }

        [HttpGet]
        public async Task<IActionResult> DepartmentUpdate(int id)
        {
            var value = await _context.Departments
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (value==null)
            {
                return NotFound("Birim bulunamadı!");
            }

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentUpdate(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var value=await _context.Departments
                    .FirstOrDefaultAsync(x=>x.Id== department.Id);

            if (value==null)
            {
                return NotFound("Birim bulunamadı!");
            }

            value.Name = department.Name;
            value.Status = department.Status;

            _context.Departments.Update(value);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Birim başarıyla güncellendi!";

            return RedirectToAction(nameof(DepartmentList));
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            var value = await _context.Departments
                    .Include(x => x.Persons)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (value==null)
            {
                TempData["Error"] = "Birim bulunamadı!";
            }

            //birim içeriisndeki kişi kontrolü
            if (value.Persons.Any())
            {
                TempData["Error"] = "Bu birim içerisinde kişiler bulunmaktadır!";

                return RedirectToAction(nameof(DepartmentList));
            }

            value.Status = !value.Status;

            _context.Departments.Update(value);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Birim durumu başarıyla değiştirildi!";

            return RedirectToAction(nameof(DepartmentList));
        }
    }
}
