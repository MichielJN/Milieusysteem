namespace Milieusysteem.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public int ClimateSurveyId { get; set; }
        public string ChoiceTekst { get; set; }
        public string Advice { get; set; }
        public int? AmountOfVotes { get; set; }
        public List<Choice>? Choices { get; set; } = new List<Choice>();

    }
}
