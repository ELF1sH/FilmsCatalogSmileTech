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

        [HttpGet]
        [Authorize]
        [Route("EditMovie/{id:int}")]
        public async Task<IActionResult> EditMovie([FromRoute] int id, string returnAction = null, int? page = null)
        {
            try
            {
                var movie = await _movieService.GetMovieToEdit(id);
                if (movie.OwnerUsername != User.Identity.Name)
                    throw new Exception("User doesn't have access to edit not their movie");
                ViewBag.ReturnAction = returnAction;
                ViewBag.Page = page; 
                return View(movie);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("EditMovie/{id:int}")]
        public async Task<IActionResult> EditMovie([FromRoute] int id, MovieEditViewModel model, string returnAction = null, int? page = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.OwnerUsername != User.Identity.Name)
                    throw new Exception("User doesn't have access to edit not their movie");
                await _movieService.EditMovie(id, model);
                if (returnAction != null)
                {
                    return RedirectToAction(returnAction, new { id, page });
                }
                return RedirectToAction("Index", new { page });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Route("Movie/{id:int}")]
        public async Task<IActionResult> GetMovieDetails([FromRoute] int id, int? page = null)
        {
            try
            {
                var model = await _movieService.GetMovieLongViewModel(id);
                ViewBag.Page = page;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
