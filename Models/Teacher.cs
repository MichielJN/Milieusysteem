namespace Milieusysteem.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Class>? Classes { get; set; }
        public ICollection<ClimateSurvey>? ClimateSurveys { get; set; }
    }
}
