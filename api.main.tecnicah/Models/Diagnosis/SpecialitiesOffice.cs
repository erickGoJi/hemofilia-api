namespace api.main.tecnicah.Models.Diagnosis;

public class SpecialityOffice
{
    public SpecialityOffice()
    {
        InstitutionsCollection = new HashSet<TopInstitution>();
        TopSpecialities = new HashSet<TopSpeciality>();
    }
    public int Institution { get; set; }
    public int Speciality { get; set; }
    public int InstitutionNoSignUp { get; set; }
    public decimal InstitutionNoSignUpPercentage { get; set; }
    public int SpecialityNoSignUp { get; set; }
    public ICollection<TopInstitution> InstitutionsCollection { get; set; }
    public ICollection<TopSpeciality> TopSpecialities { get; set; }
}

public class TopInstitution
{
    public string Name { get; set; }
    public int Value { get; set; }
}
public class TopSpeciality
{
    public string Name { get; set; }
    public int Value { get; set; }
}