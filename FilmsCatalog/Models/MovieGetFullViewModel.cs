namespace FilmsCatalog.Models
{
    public class MovieGetFullViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Director { get; set; }

        public string PosterPath { get; set; }

        public User User { get; set; }
    }
}
