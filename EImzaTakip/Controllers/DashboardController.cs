using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EImzaTakip.Controllers
{
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // TOPLAM E-İMZA KULLANICI SAYISI
            var eSignatureCount =
                _context.Persons.Count();

            ViewBag.ESignatureCount =
                eSignatureCount;

            // İLK BAŞVURU SAYISI
            var firstRecourseCount =
                _context.Persons
                .Count(x =>
                    x.RecourseType ==
                    RecourseTypeEnum.İlkBavuru);

            ViewBag.FirstRecourseCount =
                firstRecourseCount;

            // YENİLEME SAYISI
            var renovationRecourseCount =
                _context.Persons
                .Count(x =>
                    x.RecourseType ==
                    RecourseTypeEnum.Yenilenme);

            ViewBag.RenovationRecourseCount =
                renovationRecourseCount;

            // DEPARTMANA GÖRE İLK BAŞVURU SAYISI
            var departmentFirstRecourse =
                _context.Persons
                .Include(x => x.Department)
                .Where(x =>
                    x.RecourseType ==
                    RecourseTypeEnum.İlkBavuru)
                .GroupBy(x => x.Department.Name)
                .Select(x => new
                {
                    DepartmentName = x.Key,
                    Count = x.Count()
                })
                .ToList();

            ViewBag.DepartmentFirstRecourse =
                departmentFirstRecourse;

            // DEPARTMANA GÖRE YENİLEME SAYISI
            var departmentRenovationRecourse =
                _context.Persons
                .Include(x => x.Department)
                .Where(x =>
                    x.RecourseType ==
                    RecourseTypeEnum.Yenilenme)
                .GroupBy(x => x.Department.Name)
                .Select(x => new
                {
                    DepartmentName = x.Key,
                    Count = x.Count()
                })
                .ToList();

            ViewBag.DepartmentRenovationRecourse =
                departmentRenovationRecourse;

            // NES Kullanan kişi sayısı
            var totalNESCount =
                _context.Certificates
                .Where(x =>
                    x.CertificateName != null &&
                    EF.Functions.Like(
                        x.CertificateName,
                        "%NES%"))
                .Select(x => x.PersonId)
                .Distinct()
                .Count();

            ViewBag.TotalNESCount =
                totalNESCount;

            // MOBİL NES Kullanan Kişi Sayısı
            var totalMobileNESCount =
                _context.Certificates
                .Where(x =>
                    x.CertificateName != null &&
                    EF.Functions.Like(
                        x.CertificateName,
                        "%Mobil NES%"))
                .Select(x => x.PersonId)
                .Distinct()
                .Count();

            ViewBag.TotalMobileNESCount =
                totalMobileNESCount;

            // BULUNAN YILDAKİ NES SAYISI
            int currentYear =
                DateTime.Now.Year;

            var currentYearNESCount =
                _context.Certificates
                .Count(x =>
                    x.StartDate.Year ==
                    currentYear);

            ViewBag.CurrentYearNESCount =
                currentYearNESCount;

            // BULUNAN AYDAKİ NES SAYISI
            int currentMonth =
                DateTime.Now.Month;

            var currentMonthNESCount =
                _context.Certificates
                .Count(x =>
                    x.StartDate.Year ==
                    currentYear
                    &&
                    x.StartDate.Month ==
                    currentMonth);

            ViewBag.CurrentMonthNESCount =
                currentMonthNESCount;

            // BU AY SÜRESİ DOLACAK SERTİFİKALAR
            var expiringCertificates =
                _context.Certificates
                .Include(x => x.Person)
                .Where(x =>
                    x.ExpirationDate.Year ==
                    currentYear
                    &&
                    x.ExpirationDate.Month ==
                    currentMonth)
                .Select(x => new
                {
                    PersonName =
                        x.Person.Name +
                        " " +
                        x.Person.Surname,

                    IdentityNumber =
                        x.Person.IdentityNumber,

                    CertificateName =
                        x.CertificateName,

                    ExpirationDate =
                        x.ExpirationDate
                })
                .OrderBy(x => x.ExpirationDate)
                .ToList();

            ViewBag.ExpiringCertificates =
                expiringCertificates;

            // BU AY SÜRESİ DOLACAK SERTİFİKA SAYISI
            ViewBag.ExpiringCertificateCount =
                expiringCertificates.Count;

            return View();
        }
    }
}
