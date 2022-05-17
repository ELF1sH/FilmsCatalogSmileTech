using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class MovieEditViewModel
    {
        [Required(ErrorMessage = "Обязательное заполнение")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное заполнение")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное заполнение")]
        [Display(Name = "Год выпуска")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Обязательное заполнение")]
        [Display(Name = "Режиссер")]
        public string Director { get; set; }

        [Display(Name = "Обновить постер")]
        public IFormFile Poster { get; set; }

        public string OwnerUsername { get; set; }   // to further checks whether user has access to edit this movie
    }
}
