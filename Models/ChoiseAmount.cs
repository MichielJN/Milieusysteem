namespace Milieusysteem.Models
{
    public class ChoiceAmount
    {
        public int Id { get; set; }
        public int SurveyCounterId { get; set; }
        public int ChoiceId { get; set; }
        public int ChoiceCount { get; set; } = 0;
    }
}
