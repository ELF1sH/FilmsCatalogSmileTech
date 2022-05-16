using FilmsCatalog.Models;
using FilmsCatalog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PagedList;

namespace FilmsCatalog.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            return View(_movieService.GetAllMoviesShort().ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMovie(MovieCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _movieService.AddMovie(model, User);
                    return RedirectToAction("Index", "Movie");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
    }
}
