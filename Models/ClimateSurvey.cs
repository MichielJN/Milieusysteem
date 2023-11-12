namespace Milieusysteem.Models
{
    public class ClimateSurvey
    {
        public int Id { get; set; }
        public string SurveyQuestion { get; set; }
        public int TeacherId { get; set; }
        public ICollection<Choice>? SurveyChoices { get; set; } = new List<Choice>();
        public int? ClassId { get; set; }
        public int? SurveyCountId { get; set; }

    }
}
