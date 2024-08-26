using api.main.tecnicah.Models.Diagnostico;
using biz.main.tecnicah.Entities;

namespace api.main.tecnicah.Models.Diagnostico
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Result { get; set; }
        public string TextResult { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual ICollection<ResultDto> Results { get; set; }


    }
}
