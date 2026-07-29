using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Dtos.StatisticDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

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
            var values = await GetReportData(
                startDate,
                endDate,
                certificateType);

            ViewBag.StartDate =
                startDate.ToString("yyyy-MM-dd");

            ViewBag.EndDate =
                endDate.ToString("yyyy-MM-dd");

            ViewBag.CertificateType =
                certificateType;

            return View(values);
        }

        // PDF
        [HttpGet]
        public async Task<IActionResult> ExportPdf(
            DateTime startDate,
            DateTime endDate,
            string? certificateType)
        {
            var values = await GetReportData(
                startDate,
                endDate,
                certificateType);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());

                    page.Margin(25);

                    page.DefaultTextStyle(x =>
                        x.FontSize(9));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Text("E-İmza / Mobil İmza Raporu")
                                .FontSize(18)
                                .Bold();

                            column.Item()
                                .PaddingTop(5)
                                .AlignCenter()
                                .Text(
                                    $"Tarih Aralığı: " +
                                    $"{startDate:dd.MM.yyyy} - " +
                                    $"{endDate:dd.MM.yyyy}");

                            column.Item()
                                .PaddingTop(3)
                                .AlignCenter()
                                .Text(
                                    $"Sertifika Türü: " +
                                    $"{GetCertificateTypeText(certificateType)}");
                        });

                    page.Content()
                        .PaddingVertical(20)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                HeaderCell(
                                    header,
                                    "TC Kimlik No");

                                HeaderCell(
                                    header,
                                    "Ad Soyad");

                                HeaderCell(
                                    header,
                                    "Birim");

                                HeaderCell(
                                    header,
                                    "Sertifika Türü");

                                HeaderCell(
                                    header,
                                    "Başlangıç Tarihi");

                                HeaderCell(
                                    header,
                                    "Bitiş Tarihi");
                            });

                            foreach (var item in values)
                            {
                                BodyCell(
                                    table,
                                    item.IdentityNumber);

                                BodyCell(
                                    table,
                                    item.FullName);

                                BodyCell(
                                    table,
                                    item.DepartmentName);

                                BodyCell(
                                    table,
                                    item.CertificateName);

                                BodyCell(
                                    table,
                                    item.StartDate
                                        .ToString("dd.MM.yyyy"));

                                BodyCell(
                                    table,
                                    item.ExpirationDate
                                        .ToString("dd.MM.yyyy"));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Sayfa ");

                            text.CurrentPageNumber();

                            text.Span(" / ");

                            text.TotalPages();
                        });
                });
            });

            byte[] pdfBytes =
                pdf.GeneratePdf();

            string fileName =
                $"ImzaRaporu_" +
                $"{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(
                pdfBytes,
                "application/pdf",
                fileName);
        }

        // ORTAK RAPOR SORGUSU
        private async Task<List<StatisticReportDto>>
            GetReportData(
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

            return await query
                .Select(x => new StatisticReportDto
                {
                    PersonId = x.PersonId,

                    IdentityNumber =
                        x.Person != null
                            ? x.Person.IdentityNumber
                            : "",

                    FullName =
                        x.Person != null
                            ? x.Person.Name +
                              " " +
                              x.Person.Surname
                            : "",

                    DepartmentName =
                        x.Person != null &&
                        x.Person.Department != null
                            ? x.Person.Department.Name
                            : "",

                    CertificateName =
                        x.CertificateName ?? "",

                    StartDate =
                        x.StartDate,

                    ExpirationDate =
                        x.ExpirationDate
                })
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }

        private static string GetCertificateTypeText(
            string? certificateType)
        {
            if (certificateType == "NES")
            {
                return "NES / E-İmza";
            }

            if (certificateType == "Mobil NES")
            {
                return "Mobil NES / Mobil İmza";
            }

            return "Tümü";
        }

        private static void HeaderCell(
            TableCellDescriptor header,
            string text)
        {
            header.Cell()
                .Background(Colors.Grey.Lighten2)
                .Border(1)
                .Padding(5)
                .Text(text)
                .Bold();
        }

        private static void BodyCell(
            TableDescriptor table,
            string text)
        {
            table.Cell()
                .Border(1)
                .Padding(5)
                .Text(text);
        }
    }
}

