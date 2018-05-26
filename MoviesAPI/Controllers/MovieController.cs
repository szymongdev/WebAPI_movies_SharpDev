using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DbModels;
using MoviesAPI.Interfaces;
using MoviesAPI.Models;
using System.Collections.Generic;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private IMoviesService _moviesService;
        private IReviewsService _reviewsService;

        public MovieController(IMoviesService moviesService, IReviewsService reviewsService)
        {
            _moviesService = moviesService;
            _reviewsService = reviewsService;
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns>List of movies</returns>
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var list = _moviesService.GetAll();
            
            return Ok(AutoMapper.Mapper.Map<List<MovieResponse>>(list));
        }

        /// <summary>
        /// Get movie by id
        /// </summary>
        /// <param name="movieId">movie identifier</param>
        /// <returns>Movie if found</returns>
        [HttpGet("{movieId}")]
        public IActionResult Get(int movieId)
        {
            Movie movie = _moviesService.GetById(movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(AutoMapper.Mapper.Map<MovieResponse>(movie));
        }

        /// <summary>
        /// Get movies by relase year
        /// </summary>
        /// <param name="relaseYear"></param>
        /// <returns>Movies if found</returns>
        [HttpGet("{year}/relaseYear")]
        public IActionResult GetMovieByYear(int year)
        {
            var list = _moviesService.GetByYear(year);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<List<MovieResponse>>(list));
        }

        /// <summary>
        /// Get movie by title
        /// </summary>
        /// <param name="movieTitle"></param>
        /// <returns>Movie if found</returns>
        [HttpGet("{title}/movieTitle")]
        public IActionResult GetByTitle(string title)
        {
            var list = _moviesService.GetByTitle(title);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<List<MovieResponse>>(list));
        }


        /// <summary>
        /// Get movie average rating
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>Rating</returns>
        [HttpGet("{movieId}/rating")]
        public IActionResult GetR(int movieId)
        {
            try
            {
                var rate = _moviesService.GetRating(movieId);
                return Ok(rate);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get reviews by 
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpGet("{movieId}/reviews")]
        public IActionResult GetReviews(int movieId)
        {
            var reviews = _reviewsService.GetByMovieId(movieId);
            return Ok(AutoMapper.Mapper.Map<List<ReviewResponse>>(reviews));
        }

        /// <summary>
        /// Get movies by actor
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>List of movies</returns>
        [HttpGet("{actorId}/movies")]
        public IActionResult GetMoviesByActor(int actorId)
        {
            var list = _moviesService.GetMoviesByActorId(actorId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<List<MovieResponse>>(list));
        }

        /// <summary>
        /// Add new movie to repositorium
        /// </summary>
        /// <param name="movie">new movie</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]MovieRequest movie)
        {
            _moviesService.AddNewMovie(AutoMapper.Mapper.Map<Movie>(movie));

            return Ok();
        }

        /// <summary>
        /// Add movie to actor
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>
        [HttpPost("{movieId}/{actorId}")]
        public IActionResult AddMovieToActor(int movieId, int actorId)
        {
            try
            {
                _moviesService.AddMovieToActor(movieId, actorId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Add top 10 movies from trakt.tv
        /// </summary>
        /// <returns></returns>
        [HttpPost("popularMovies")]
        public IActionResult AddPopularMovies()
        {
            try
            {
                _moviesService.AddPopular();
                return Ok();
            }
            catch
            {
                return NotFound();
            }

        }



        /// <summary>
        /// Update movie in repositorium
        /// </summary>
        /// <param name="movie">updated movie</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]MovieRequest movie)
        {
            if (_moviesService.UpdateMovie(AutoMapper.Mapper.Map<Movie>(movie)))
            {
                return NoContent();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete movie from repositorium
        /// </summary>
        /// <param name="movieId">movie identifier</param>
        /// <returns></returns>
        [HttpDelete("{movieId}")]
        public IActionResult Delete(int movieId)
        {
            try
            {
                _moviesService.Remove(movieId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
