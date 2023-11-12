namespace Milieusysteem.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public ICollection<ClimateSurvey>? FinishedSurveys { get; set; } = new List<ClimateSurvey>();
    }
}
