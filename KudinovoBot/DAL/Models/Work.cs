namespace KudinovoBot.DAL.Models
{
    public class Work
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; }

        public (long, string) Author { get; set; }
    }
}
