namespace api.main.tecnicah.Models.RequestSupport
{
    public class RequestSupportDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int EventType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class RequestSupportAdd
    {
        public int UserId { get; set; }
        public int EventType { get; set; }
    }

}
