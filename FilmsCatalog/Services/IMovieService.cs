﻿using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IMovieService
    {
        Task AddMovie(MovieCreateViewModel model, ClaimsPrincipal currentUserClaims);
        List<MovieShortViewModel> GetAllMoviesShort();
        Task<MovieEditViewModel> GetMovieToEdit(int id);
        Task EditMovie(int id, MovieEditViewModel model);
        Task<MovieLongViewModel> GetMovieLongViewModel(int id);
    }

    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        private static string[] AllowedPosterExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

        public MovieService(ApplicationDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public async Task AddMovie(MovieCreateViewModel model, ClaimsPrincipal currentUserClaims)
        {
            var user = await _userManager.GetUserAsync(currentUserClaims);
            var path = await CreatePosterFile(model.Poster);

            await _context.Movies.AddAsync(new Movie
            {
                Title = model.Title,
                Description = model.Description,
                Year = (int)model.Year,
                Director = model.Director,
                PosterPath = path,
                User = user
            });
            await _context.SaveChangesAsync();
        }

        private async Task<string> CreatePosterFile(IFormFile poster)
        {
            var extension = Path.GetExtension(poster.FileName);
            if (!AllowedPosterExtensions.Contains(extension))
            {
                throw new ArgumentException("Attached file has not supported extension");
            }
            var path = $"Files/{Guid.NewGuid()}-{poster.FileName}";
            using (var fs = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
            {
                await poster.CopyToAsync(fs);
            }
            return path;
        }

        public List<MovieShortViewModel> GetAllMoviesShort()
        {
            return _context.Movies.Select(movie => new MovieShortViewModel
            {
                Id = movie.Id,
                Title =movie.Title,
                Director = movie.Director,
                Year = movie.Year,
                User = movie.User
            }).ToList();
        }

        public async Task<MovieEditViewModel> GetMovieToEdit(int id)
        {
            var movie = await GetMovie(id);
            return new MovieEditViewModel
            {
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                Director = movie.Director,
                OwnerUsername = movie.User.UserName
            };
        }

        public async Task EditMovie(int id, MovieEditViewModel model)
        {
            var movie = await GetMovie(id);
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Year = (int)model.Year;
            movie.Director = model.Director;
            if (model.Poster != null)
            {
                var path = await CreatePosterFile(model.Poster);
                movie.PosterPath = path;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<MovieLongViewModel> GetMovieLongViewModel(int id)
        {
            var movie = await GetMovie(id);
            return new MovieLongViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                Director = movie.Director,
                PosterPath = movie.PosterPath,
                User = movie.User
            };
        }

        private async Task<Movie> GetMovie(int id)
        {
            var movie = await _context.Movies.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with id {id} does not exist");
            }
            return movie;
        }
    }
}
