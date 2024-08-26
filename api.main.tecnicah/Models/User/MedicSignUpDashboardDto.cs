namespace api.main.tecnicah.Models.User;

public class MedicSignUpDashboardDto
{
    public MedicSignUpDashboardDto()
    {
        FidelityLevel = new HashSet<FidelityLevelDto>();
        MedicRequestSupports = new HashSet<MedicRequestSupport>();
    }
    public int ObjectiveSignUp { get; set; }
    public int MedicSignUp { get; set; }
    public int MedicActive { get; set; }
    public int MedicInactive { get; set; }
    public decimal MedicSignUpPercentage { get; set; }
    public ICollection<FidelityLevelDto> FidelityLevel { get; set; }
    public ICollection<MedicRequestSupport> MedicRequestSupports { get; set; }
}