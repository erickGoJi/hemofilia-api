using api.main.tecnicah.Models.Diagnosis;

namespace api.main.tecnicah.Models.State
{
    public class StateDashboardDto
    {
        public StateDashboardDto()
        {
            MainStates = new HashSet<MainStates>();
        }
        public int States { get; set; }
        public string? StateMostSignUp { get; set; }
        public string StateMinusSignUp { get; set; } = string.Empty;
        public int StatesNoSignUp { get; set; }
        public decimal StatesNoSignUpPercentage { get; set; }
        public ICollection<MainStates> MainStates { get; set; }

    }

    public class MainStates
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
