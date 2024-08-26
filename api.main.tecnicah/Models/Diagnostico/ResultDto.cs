using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace api.main.tecnicah.Models.Diagnostico
{
    public class ResultDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int? OptionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }

    }
}
