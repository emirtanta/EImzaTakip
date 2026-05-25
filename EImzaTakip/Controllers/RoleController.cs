using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EImzaTakip.Controllers
{
    [SessionAuthorize]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> RoleList()
        {
            var result = await _context.Roles.OrderByDescending(x => x.Id).ToListAsync();

            return View(result);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleCreate(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            await _context.Roles.AddAsync(role);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(RoleList));
        }

        [HttpGet]
        public async Task<IActionResult> RoleUpdate(int id)
        {
            var result = await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result==null)
            {
                return NotFound("Rol bulunamdı!");
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleUpdate(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var value=await _context.Roles
                .FirstOrDefaultAsync(x=>x.Id==role.Id);

            if (value==null)
            {
                return NotFound("Rol bulunamadı!");
            }

            value.Name=role.Name;
            value.Status = role.Status;

            _context.Roles.Update(value);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(RoleList));
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            var value = await _context.Roles
        .Include(x => x.Users)
        .FirstOrDefaultAsync(x => x.Id == id);

            if (value == null)
            {
                TempData["Error"] = "Rol bulunamadı!";

                return RedirectToAction(nameof(RoleList));
            }

            // Rol içerisinde kullanıcı kontrolü
            if (value.Users.Any())
            {
                TempData["Error"] =
                    "Bu rolde kullanıcılar mevcut. İlk öncesinde kullanıcıları rolden çıkarıp daha sonrasında bu rolün durumunu değiştirebilirsiniz.";

                return RedirectToAction(nameof(RoleList));
            }

            value.Status = !value.Status;

            _context.Roles.Update(value);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Rol durumu başarıyla değiştirildi.";

            return RedirectToAction(nameof(RoleList));
        }
    }
}
