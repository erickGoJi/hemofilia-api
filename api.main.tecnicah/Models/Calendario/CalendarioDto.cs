using System.ComponentModel.DataAnnotations;

namespace api.main.tecnicah.Models.Calendario
{
    public class CalendarioDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "IdUser is required")]
        public int DayId { get; set; }
        public int ScheduleId { get; set; }
    }
}
