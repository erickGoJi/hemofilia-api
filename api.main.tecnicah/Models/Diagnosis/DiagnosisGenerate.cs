using System.Collections.ObjectModel;

namespace api.main.tecnicah.Models.Diagnosis;

public class DiagnosisGenerate
{
    public DiagnosisGenerate()
    {
        Top = new HashSet<DiagnosisTopDto>();
    }

    public int Made { get; set; }
    public int Found { get; set; }
    public decimal FoundPercentage { get; set; }
    public int NoResult { get; set; }
    public virtual ICollection<DiagnosisTopDto> Top { get; set; }
}