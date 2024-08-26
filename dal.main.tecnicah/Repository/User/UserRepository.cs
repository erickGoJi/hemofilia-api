using biz.main.tecnicah.Models.Email;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Email;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using biz.main.tecnicah.Entities;
using Microsoft.EntityFrameworkCore;
using biz.main.tecnicah.Paged;
using biz.main.tecnicah.Models.Diagnosis;

namespace dal.main.tecnicah.Repository.User
{
    public class UserRepository : GenericRepository<biz.main.tecnicah.Entities.User>, IUserRepository
    {
        private IConfiguration _config;
        private IEmailService _emailservice;

        public UserRepository(db_HemofiliaContext context, IConfiguration config, IEmailService emailService) : base(context)
        {
            _config = config;
            _emailservice = emailService;
        }

        public string BuildToken(biz.main.tecnicah.Entities.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //esta estaba commentada
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                // Issuer = ,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public string SendMail(string emailTo, string body, string subject)
        {
            EmailModel email = new EmailModel();
            email.To = emailTo;
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            return _emailservice.SendEmail(email);
        }

        public async Task AddLogIn(int userId)
        {
            await _context.UserLogIns.AddAsync(new UserLogIn()
            {
                Id = new Guid(),
                EventType = 1,
                UserId = userId,
                CreatedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        public Task<int> MedicInactive(DateTime start, DateTime end)
        {
            return _context.UserLogIns.Where(x => x.CreatedDate ! >= start && x.CreatedDate ! <= end)
                .GroupBy(g => g.UserId)
                .CountAsync();
        }

        public string VerifyEmail(string email)
        {
            var result = "";

            if (_context.Users.SingleOrDefault(x => x.Email.ToLower().Trim() == email.ToLower().Trim()) != null)
            {
                result = "Exist";
            }
            else
            {
                result = "No Exist";
            }

            return result;
        }

        public bool VerifyPassword(string hash, string password)
        {
            return Crypto.VerifyHashedPassword(hash, password);
        }

        public override biz.main.tecnicah.Entities.User Add(biz.main.tecnicah.Entities.User user)
        {
            user.Password = HashPassword(user.Password);
            return base.Add(user);
        }

        public override biz.main.tecnicah.Entities.User Update(biz.main.tecnicah.Entities.User user, object id)
        {
            user.UpdatedDate = DateTime.Now;
            return base.Update(user, id);
        }

        public async Task<PagedList<biz.main.tecnicah.Entities.User>> GetAllUsersPaged(int page, int pageSize)
        {
            page = page == 0 ? 1 : page;

            int totalRows = await _context.Set<biz.main.tecnicah.Entities.User>().CountAsync();
            List<biz.main.tecnicah.Entities.User> result = await _context.Set<biz.main.tecnicah.Entities.User>().OrderByDescending(o => o.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<biz.main.tecnicah.Entities.User>(totalRows, result);
        }

        public async Task<List<FidelityLevel>> GetFidelitiesAsync(DateTime start, DateTime end)
        {
            var fidelities = new List<FidelityLevel>();
            var logIns = _context.UserLogIns.Where(x => x.CreatedDate >= start && x.CreatedDate <= end && x.EventType == 1).GroupBy(x => x.UserId);
            var onceTime = logIns.Count(x=> x.Count() >= 0 && x.Count() <= 1);
            fidelities.Add(new FidelityLevel { Frequency = "0 - 1 vez", HowMany = onceTime });
            fidelities.Add(new FidelityLevel { Frequency = "2 - 4 veces", HowMany = logIns.Count(x => x.Count() >= 2 && x.Count() <= 4) });
            fidelities.Add(new FidelityLevel { Frequency = "5 - 10 veces", HowMany = logIns.Count(x => x.Count() >= 5 && x.Count() <= 10) });
            fidelities.Add(new FidelityLevel { Frequency = "10 - veces o mas", HowMany = logIns.Count(x => x.Count() >= 11) });
            return fidelities;
        }

        public async Task<int> GetMedicActive(DateTime start, DateTime end)
        {
            var logIns = _context.UserLogIns.Where(c => c.CreatedDate >= start && c.CreatedDate <= end)
                .GroupBy(g => g.UserId);
            return logIns.Count();
        }

        public async Task<int> GetMedicInactive(DateTime start, DateTime end)
        {
            var logIns = _context.UserLogIns.Where(c => c.CreatedDate < start || c.CreatedDate > end)
                .GroupBy(g => g.UserId);
            return logIns.Count();
        }
    }

}
