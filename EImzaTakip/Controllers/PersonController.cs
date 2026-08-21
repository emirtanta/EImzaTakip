using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Dtos.PersonDtos;
using EImzaTakip.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EImzaTakip.Controllers
{
    [SessionAuthorize]
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PersonList()
        {
            var values = await _context.Persons
                .Include(x => x.Department)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> PersonDetails(int id)
        {
            var value = await _context.Persons
                .Include(x => x.Department)
                .Include(x => x.Certificates)
                .Include(x => x.PersonNotes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (value==null)
            {
                return NotFound("Kişi bulunamadı!");
            }

            return View(value);
        }

        [HttpGet]
        public async Task<IActionResult> PersonCreate()
        {
            ViewBag.Departments = await _context.Departments
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var today = DateTime.Today;

            var model = new PersonCreateUpdateDto
            {
                Birthdate = today,
                Status = true
            };

            model.Certificates.Add(new Certificate
            {
                StartDate = today,
                ExpirationDate = today
            });

            model.PersonNotes.Add(new PersonNote());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonCreate(PersonCreateUpdateDto dto)
        {
            ViewBag.Departments = await _context.Departments
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            // TC KİMLİK NO KONTROLÜ
            var identityNumberExists = await _context.Persons
                .AnyAsync(x => x.IdentityNumber == dto.IdentityNumber);

            if (identityNumberExists)
            {
                ModelState.AddModelError(
                    nameof(dto.IdentityNumber),
                    "Bu TC Kimlik Numarasına ait kişi sistemde kayıtlıdır!"
                );
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Person person = new Person
            {
                IdentityNumber = dto.IdentityNumber,
                Name = dto.Name,
                Surname = dto.Surname,
                Birthdate = dto.Birthdate,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,
                RecourseType = dto.RecourseType,
                YedekMi = dto.YedekMi,
                SmartCardReaderType = dto.SmartCardReaderType,
                Description = dto.Description,
                VIP = dto.VIP,
                Status = dto.Status
            };

            await _context.Persons.AddAsync(person);

            await _context.SaveChangesAsync();

            // CERTIFICATES
            foreach (var certificate in dto.Certificates?
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CertificateName))
                ?? Enumerable.Empty<Certificate>())
            {
                certificate.PersonId = person.Id;

                await _context.Certificates
                    .AddAsync(certificate);
            }

            // NOTES
            foreach (var note in dto.PersonNotes?
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Description))
                ?? Enumerable.Empty<PersonNote>())
            {
                note.PersonId = person.Id;

                await _context.PersonNotes
                    .AddAsync(note);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Kişi başarıyla eklendi!";

            return RedirectToAction(nameof(PersonList));
        }

        [HttpGet]
        public async Task<IActionResult> PersonUpdate(int id)
        {
            ViewBag.Departments = await _context.Departments
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var person = await _context.Persons
        .Include(x => x.Certificates)
        .Include(x => x.PersonNotes)
        .FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            PersonCreateUpdateDto dto =
                new PersonCreateUpdateDto
                {
                    Id = person.Id,
                    IdentityNumber = person.IdentityNumber,
                    Name = person.Name,
                    Surname = person.Surname,
                    Birthdate = person.Birthdate,
                    Email = person.Email,
                    DepartmentId = person.DepartmentId,
                    RecourseType = person.RecourseType,
                    YedekMi = person.YedekMi,
                    SmartCardReaderType = person.SmartCardReaderType,
                    Description = person.Description,
                    VIP = person.VIP,
                    Status = person.Status,

                    Certificates = person.Certificates.ToList(),

                    PersonNotes = person.PersonNotes.ToList()
                };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonUpdate(PersonCreateUpdateDto dto)
        {
            ViewBag.Departments = await _context.Departments
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            // TC KİMLİK NO KONTROLÜ
            var identityNumberExists = await _context.Persons
                .AnyAsync(x =>
                    x.IdentityNumber == dto.IdentityNumber &&
                    x.Id != dto.Id);

            if (identityNumberExists)
            {
                ModelState.AddModelError(
                    nameof(dto.IdentityNumber),
                    "Bu TC Kimlik Numarasına ait başka bir kişi sistemde kayıtlıdır!"
                );
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var person = await _context.Persons
                .Include(x => x.Certificates)
                .Include(x => x.PersonNotes)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (person == null)
            {
                return NotFound("Kişi bulunamadı!");
            }

            // PERSON UPDATE
            person.IdentityNumber = dto.IdentityNumber;
            person.Name = dto.Name;
            person.Surname = dto.Surname;
            person.Email = dto.Email;
            person.Birthdate = dto.Birthdate;
            person.DepartmentId = dto.DepartmentId;
            person.RecourseType = dto.RecourseType;
            person.YedekMi = dto.YedekMi;
            person.SmartCardReaderType = dto.SmartCardReaderType;
            person.Description = dto.Description;
            person.VIP = dto.VIP;
            person.Status = dto.Status;

            // CERTIFICATES RESET
            _context.Certificates.RemoveRange(
                person.Certificates);

            foreach (var certificate in dto.Certificates?
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CertificateName))
                ?? Enumerable.Empty<Certificate>())
            {
                certificate.Id = 0;
                certificate.PersonId = person.Id;

                await _context.Certificates
                    .AddAsync(certificate);
            }

            // NOTES RESET
            _context.PersonNotes.RemoveRange(
                person.PersonNotes);

            foreach (var note in dto.PersonNotes?
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Description))
                ?? Enumerable.Empty<PersonNote>())
            {
                note.Id = 0;
                note.PersonId = person.Id;

                await _context.PersonNotes
                    .AddAsync(note);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Kişi başarıyla güncellendi!";

            return RedirectToAction(nameof(PersonList));
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            var value = await _context.Persons
                .FirstOrDefaultAsync(x => x.Id == id);

            if (value==null)
            {
                TempData["Error"] = "Kişi bulunamadı!";

                return RedirectToAction(nameof(PersonList));
            }

            value.Status = !value.Status;

            _context.Persons.Update(value);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Kişi durumu başaryıla değiştirildi!";

            return RedirectToAction(nameof(PersonList));
        }

        //not ekleme
        public async Task<IActionResult> AddNote(int personId)
        {
            var person=await _context.Persons
                .FirstOrDefaultAsync(x=>x.Id==personId);

            if (person==null)
            {
                return NotFound();
            }

            ViewBag.PersonName = $"{person.Name} {person.Surname}";

            return View(new PersonNote
            {
                PersonId = personId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(PersonNote note)
        {
            if (!ModelState.IsValid)
            {
                return View(note);
            }

            await _context.PersonNotes.AddAsync(note);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Kişi notu eklendi!";

            return RedirectToAction(nameof(PersonDetails), new { id = note.PersonId });
        }

        #region Sertifika ekleme bölümü

        [HttpGet]
        public async Task<IActionResult> AddCertificate(int personId)
        {
            var person = await _context.Persons
                .FirstOrDefaultAsync(x => x.Id == personId);

            if (person==null)
            {
                return NotFound();
            }

            ViewBag.PersonName = $"{person.Name} {person.Surname}";

            return View(new Certificate
            {
                PersonId = personId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCertificate(Certificate certificate)
        {
            if (!ModelState.IsValid)
            {
                return View(certificate);
            }

            await _context.Certificates
                .AddAsync(certificate);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Sertifika eklendi!";

            return RedirectToAction(nameof(PersonDetails), new { id = certificate.Person });
        }

        #endregion
    }
}
