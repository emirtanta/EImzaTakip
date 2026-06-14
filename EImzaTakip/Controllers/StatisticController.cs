using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Dtos.StatisticDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EImzaTakip.Controllers
{
    [SessionAuthorize]
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SignatureReport()
        {
            ViewBag.StartDate = "";
            ViewBag.EndDate = "";
            ViewBag.CertificateType = "";

            return View(new List<StatisticReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignatureReport(
            DateTime startDate,
            DateTime endDate,
            string? certificateType)
        {
            var query = _context.Certificates
                .Include(x => x.Person)
                .ThenInclude(x => x.Department)
                .Where(x =>
                    x.ExpirationDate.Date >= startDate.Date &&
                    x.ExpirationDate.Date <= endDate.Date);

            if (!string.IsNullOrWhiteSpace(certificateType))
            {
                if (certificateType == "Mobil NES")
                {
                    query = query.Where(x =>
                        x.CertificateName != null &&
                        x.CertificateName.Contains("Mobil NES"));
                }
                else if (certificateType == "NES")
                {
                    query = query.Where(x =>
                        x.CertificateName != null &&
                        x.CertificateName.Contains("NES") &&
                        !x.CertificateName.Contains("Mobil"));
                }
            }

            var values = await query
                .Select(x => new StatisticReportDto
                {
                    PersonId = x.PersonId,

                    IdentityNumber =
                        x.Person != null
                            ? x.Person.IdentityNumber
                            : "",

                    FullName =
                        x.Person != null
                            ? x.Person.Name + " " + x.Person.Surname
                            : "",

                    DepartmentName =
                        x.Person != null && x.Person.Department != null
                            ? x.Person.Department.Name
                            : "",

                    CertificateName =
                        x.CertificateName ?? "",

                    StartDate = x.StartDate,
                    ExpirationDate = x.ExpirationDate
                })
                .OrderBy(x => x.FullName)
                .ToListAsync();

            ViewBag.StartDate =
                startDate.ToString("yyyy-MM-dd");

            ViewBag.EndDate =
                endDate.ToString("yyyy-MM-dd");

            ViewBag.CertificateType =
                certificateType;

            return View(values);
        }
    }
}

