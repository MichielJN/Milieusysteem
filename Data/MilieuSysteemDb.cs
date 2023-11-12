using Microsoft.EntityFrameworkCore;
using Milieusysteem.Models;

namespace Milieusysteem.Data
{
    public class MilieuSysteemDb : DbContext
    {
        public DbSet<Milieusysteem.Models.Teacher> Teachers { get; set; } = default!;
        public DbSet<Milieusysteem.Models.Class> Classes { get; set; } = default!;
        public DbSet<SurveyCounter> surveyCounters { get; set; }
        public DbSet<ChoiceAmount> ChoiceAmounts { get; set; }
        public DbSet<ClimateSurvey> climateSurveys { get; set; }
        public DbSet<Choice> choices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"Data Source=.;Initial Catalog=MilieuSysteem;Integrated Security=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connection);
        }
        public MilieuSysteemDb(DbContextOptions<MilieuSysteemDb> options)
             : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }
        public DbSet<Milieusysteem.Models.Class> Class { get; set; } = default!;
        
    }
}
