using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.main.tecnicah.Models.Diagnosis;

namespace biz.main.tecnicah.Repository.Diagnostico
{
    public interface IQuizRepository : IGenericRepository<Entities.Quiz>
    { 
        public int DiagnosisMade(DateTime start, DateTime end);
        public Task<List<bool>> DiagnosisFound(DateTime start, DateTime end);
        public Task<List<DiagnosisTop>> DiagnosisTop(DateTime start, DateTime end);
    }
}
