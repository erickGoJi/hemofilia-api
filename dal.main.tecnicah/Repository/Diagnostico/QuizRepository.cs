using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.main.tecnicah.Repository.Diagnostico;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using System.Globalization;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Models.Diagnosis;
using Microsoft.EntityFrameworkCore;

namespace dal.main.tecnicah.Repository.Diagnostico
{
    public class QuizRepository : GenericRepository<biz.main.tecnicah.Entities.Quiz>, IQuizRepository
    {
        public QuizRepository(db_HemofiliaContext context) : base(context)
        {
            
        }
        
        public int DiagnosisMade(DateTime start, DateTime end)
        {
            return _context.Quizzes.Count(c => c.CreatedDate >= start && c.CreatedDate <= end);
        }

        public async Task<List<bool>> DiagnosisFound(DateTime start, DateTime end)
        {
            List<bool> results = new List<bool>();
            var keys = await _context.CatClaveDiagnostics.Select(s => new Key()
            {
                chars = s.KeyCode.ToCharArray(),
                Id = s.Id
            }).ToListAsync();
            var quizzes = await _context.Quizzes.Where(x => x.CreatedDate >= start && x.CreatedDate <= end).ToListAsync();
            foreach (var quiz in quizzes)
            {
                results.Add(DiagnosisFoundByQuiz(quiz, keys));
            }
            return results;
        }

        public async Task<List<DiagnosisTop>> DiagnosisTop(DateTime start, DateTime end)
        {
            var keys = await _context.CatClaveDiagnostics.Select(s => new Key()
            {
                chars = s.KeyCode.ToCharArray(),
                Id = s.Id,
                Option = s.Option1
            }).ToListAsync();

            foreach (var key in keys)
            {
                Array.Sort(key.chars);
            }

            var q = _context.Quizzes.Where(c => c.CreatedDate >= start && c.CreatedDate <= end).Select(s => s.Name.ToCharArray())
                .ToList();
            foreach (var c in q)
            {
                Array.Sort(c);
            }
            var quizzes = q.Select(s =>
                new DiagnosisTop()
                {
                    Name = keys.FirstOrDefault(x=> x.chars.SequenceEqual(s) )?.Option,
                    Value = 0
                })
                .GroupBy(g=>g.Name)
                .ToList();
            var quizzesFiltered = quizzes
                .Select(s=>new DiagnosisTop()
                {
                    Name = s.First().Name,
                    Value = s.Count()
                })
                .Where(x => x.Value != 0 && x.Name != null)
                .OrderBy(o=>o.Value)
                .Take(5)
                .ToList();
            return quizzesFiltered;
        }

        private static bool DiagnosisFoundByQuiz(Quiz quiz, List<Key> keys)
        {
            bool res = false;
            var keyValue = quiz.Name.ToCharArray();
            foreach (var key in keys) 
            {
                Array.Sort(key.chars);
                Array.Sort(keyValue);
                if (key.chars.SequenceEqual(keyValue))
                {
                    res = true;
                }
            }

            return res;
        }
        
        private static string DiagnosisNameFoundByQuiz(string quiz, List<Key> keys)
        {
            var res = "";
            var keyValue = quiz.ToCharArray();
            foreach (var key in keys) 
            {
                Array.Sort(key.chars);
                Array.Sort(keyValue);
                if (key.chars.SequenceEqual(keyValue))
                {
                    res = key.Option!;
                }
            }

            return res;
        }
        
    }
}
