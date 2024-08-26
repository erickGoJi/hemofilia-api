using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Models.Diagnosis;
using biz.main.tecnicah.Paged;
using biz.main.tecnicah.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.main.tecnicah.Repository.User
{
    public interface IUserRepository : IGenericRepository<Entities.User>
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
        string BuildToken(Entities.User user);
        string VerifyEmail(string email);
        string SendMail(string emailTo, string body, string subject);
        Task AddLogIn(int userId);
        Task<int> MedicInactive(DateTime start, DateTime end);
        Task<PagedList<Entities.User>> GetAllUsersPaged(int page, int pageSize);
        Task<List<FidelityLevel>> GetFidelitiesAsync(DateTime start, DateTime end);
        Task<int> GetMedicActive(DateTime start, DateTime end);
        Task<int> GetMedicInactive(DateTime start, DateTime end);
    }
}
