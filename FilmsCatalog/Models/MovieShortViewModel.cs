namespace FilmsCatalog.Models
{
    public class MovieShortViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public int Year { get; set; }

        public User User { get; set; }
    }
}
