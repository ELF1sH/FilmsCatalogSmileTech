using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class MovieCreateViewModel
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

        [Required(ErrorMessage = "Обязательное заполнение")]
        [Display(Name = "Постер")]
        public IFormFile Poster { get; set; }
    }
}
