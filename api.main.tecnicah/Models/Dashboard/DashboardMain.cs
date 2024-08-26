using api.main.tecnicah.Models.Diagnosis;
using api.main.tecnicah.Models.State;
using api.main.tecnicah.Models.User;

namespace api.main.tecnicah.Models.Dashboard
{
    public class DashboardMain
    {
        public DashboardMain()
        {
            diagnosisTops = new HashSet<DiagnosisTopDto>();
            topInstitutions = new HashSet<TopInstitution>();
            topStates = new HashSet<MainStates>();
            TopSpecialities = new HashSet<TopSpeciality>();
            FidelityLevel = new HashSet<FidelityLevelDto>();
        }
        public int Goal { get; set; }
        public decimal GoalPercentage { get; set; }
        public int SignUpTotal { get; set; }
        public int SignUpMonth { get; set; }
        public ICollection<DiagnosisTopDto> diagnosisTops { get; set; }
        public ICollection<TopInstitution> topInstitutions { get; set; }
        public ICollection<MainStates> topStates { get; set; }
        public ICollection<TopSpeciality> TopSpecialities { get; set; }
        public ICollection<FidelityLevelDto> FidelityLevel { get; set; }
    }
}
