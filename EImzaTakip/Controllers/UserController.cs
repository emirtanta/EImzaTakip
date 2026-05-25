using BCrypt.Net;
using EImzaTakip.Data;
using EImzaTakip.Filters;
using EImzaTakip.Models.Dtos.UserDtos;
using EImzaTakip.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace EImzaTakip.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [SessionAuthorize]
        public async Task<IActionResult> UserList()
        {
            var values = await _context.Users
                .Include(x => x.Role)
                .OrderByDescending(x => x.Id)
                .Select(x => new ListUserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    NickName = x.NickName,
                    Email = x.Email,
                    Status=x.Status,
                    RoleName=x.Role.Name
                }).ToListAsync();

            return View(values);
        }

        [SessionAuthorize]
        public async Task<IActionResult> UserDetails(int id)
        {
            var value = await _context.Users
                .Include(x => x.Role)
                .Where(x => x.Id == id)
                .Select(x => new GetUserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    NickName = x.NickName,
                    Email = x.Email,
                    RoleName=x.Role.Name,
                    Status=x.Status
                }).FirstOrDefaultAsync();

            if (value==null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return View(value);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = await _context.Roles
                        .Where(x => x.Status)
                        .OrderBy(x => x.Name)
                        .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            ViewBag.Roles = await _context.Roles
                            .Where(x => x.Status)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            bool emailExist=await _context.Users
                .AnyAsync(x=>x.Email==dto.Email);

            if (emailExist)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi kullanılmaktadır!");

                return View(dto);
            }

            User user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                NickName = dto.NickName,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Status = true,
                RoleId = dto.RoleId
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kullanıcı başarıyla oluşturuldu!";

            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user=await _context.Users
                .Include(x=>x.Role)
                .FirstOrDefaultAsync(x=>x.Email==dto.Email);

            if (user==null)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı!");

                return View(dto);
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!checkPassword)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı");

                return View(dto);
            }

            // SESSION
            HttpContext.Session.SetInt32(
                "UserId",
                user.Id);

            HttpContext.Session.SetString(
                "FullName",
                $"{user.Name} {user.Surname}");

            HttpContext.Session.SetString(
                "RoleName",
                user.Role.Name);

            TempData["Success"] = "Giriş başarılı";

            return RedirectToAction("Index", "Dashboard");
        }


        [SessionAuthorize]
        [HttpGet]
        public async Task<IActionResult> UserUpdate(int id)
        {
            ViewBag.Roles = await _context.Roles
                            .Where(x => x.Status)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

            var value = await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new UserProfileEditDto
                {
                    Name = x.Name,
                    Surname = x.Surname,
                    NickName = x.NickName,
                    Email = x.Email,
                    Status = x.Status,
                    RoleId = x.RoleId
                })
                .FirstOrDefaultAsync();

            if (value == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return View(value);
        }

        [SessionAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserUpdate(int id,UserProfileEditDto dto)
        {
            ViewBag.Roles = await _context.Roles
        .Where(x => x.Status)
        .OrderBy(x => x.Name)
        .ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.NickName = dto.NickName;
            user.Email = dto.Email;
            user.Status = dto.Status;
            user.RoleId = dto.RoleId;

            // ŞİFRE DEĞİŞTİRME
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Kullanıcı güncellendi!";

            return RedirectToAction(nameof(UserList));
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int userId,ChangePasswordUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user==null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            bool checkPassword=BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password);

            if (!checkPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Mevcut şifre yanlış!");

                return View(dto);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Şifre başarıyla değiştirildi!";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    "Email",
                    "Bu e-posta adresine ait kullanıcı bulunamadı!"
                );

                return View(dto);
            }

            // YENİ ŞİFRE OLUŞTUR
            string newPassword =
                Guid.NewGuid().ToString().Substring(0, 8);

            // HASHLE
            user.Password =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            // SMTP AYARLARI
            string host =
                _configuration["EmailSettings:Host"];

            int port =
                Convert.ToInt32(
                    _configuration["EmailSettings:Port"]);

            string senderEmail =
                _configuration["EmailSettings:Email"];

            string senderPassword =
                _configuration["EmailSettings:Password"];

            bool enableSsl =
                Convert.ToBoolean(
                    _configuration["EmailSettings:EnableSsl"]);

            // MAIL OLUŞTUR
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(senderEmail);

            mail.To.Add(user.Email);

            mail.Subject = "Şifre Sıfırlama";

            mail.Body =
                $"Merhaba {user.Name} {user.Surname},\n\n" +
                $"Yeni şifreniz: {newPassword}\n\n" +
                $"Sisteme giriş yaptıktan sonra şifrenizi değiştiriniz.";

            mail.IsBodyHtml = false;

            // SMTP CLIENT
            SmtpClient smtp = new SmtpClient(host, port);

            smtp.Credentials =
                new NetworkCredential(
                    senderEmail,
                    senderPassword);

            smtp.EnableSsl = enableSsl;

            // MAIL GÖNDER
            smtp.Send(mail);

            TempData["Success"] =
                "Yeni şifreniz e-posta adresinize gönderildi.";

            return RedirectToAction(nameof(Login));
        }

        [SessionAuthorize]
        //Profile
        public async Task<IActionResult> Profile(int id)
        {
            var value=await _context.Users
                .Where(x=>x.Id==id)
                .Select(x => new UserProfileDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    NickName = x.NickName,
                    Email = x.Email,
                    RoleName=x.Role.Name
                })
                .FirstOrDefaultAsync();

            if (value == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return View(value);
        }

        [SessionAuthorize]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var value = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (value == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            value.Status = !value.Status;

            _context.Users.Update(value);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Kullanıcı durumu değiştirildi.";

            return RedirectToAction(nameof(UserList));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Success"] =
                "Çıkış yapıldı.";

            return RedirectToAction(nameof(Login));
        }
    }
}
