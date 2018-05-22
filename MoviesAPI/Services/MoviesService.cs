using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoviesAPI.DbModels;
using MoviesAPI.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MoviesAPI.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly MoviesContext _moviesContext;

        public MoviesService(MoviesContext moviesContext)
        {
            _moviesContext = moviesContext;
        }

        public List<Movie> GetAll()
        {
            return _moviesContext.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            Movie foundMovie = _moviesContext.Movies
                  .Where(movie => movie.Id == id)
                  .SingleOrDefault();

            return foundMovie;
        }

        public List<Movie> GetByYear(int year)
        {
            List<Movie> foundMovie = _moviesContext.Movies
                  .Where(movie => movie.Year == year)
                  .ToList();

            return foundMovie;
        }

        public List<Movie> GetByTitle(string title)
        {
            List<Movie> foundMovie = _moviesContext.Movies
                  .Where(movie => movie.Title.Contains(title))
                  .ToList();

            return foundMovie;
        }

        public string GetRating(int id)
        {
            var movie = _moviesContext.Movies.Find(id);
            var list = movie.Reviews.ToList();
            int sum = 0;
            int counter = 0;
            if (list != null)
            {
                foreach (Review rew in list)
                {
                    sum += rew.Rate;
                    counter++;
                }
            }
            RatingClass ratingClass = new RatingClass();
            ratingClass.MovieId = id;
            ratingClass.Rating = Math.Round((double)sum / counter, 2);
            var json = JsonConvert.SerializeObject(ratingClass);
            return json;
        }

        public List<Movie> GetMoviesByActorId(int actorId)
        {
            List<Movie> foundMovies = _moviesContext.MovieActors
                 .Where(m => m.ActorId == actorId)
                 .Select(m => m.Movie)
                 .ToList();

            return foundMovies;
        }

        public void AddNewMovie(Movie movie)
        {
            _moviesContext.Movies.Add(movie);
            _moviesContext.SaveChanges();
        }

        public void AddMovieToActor(int movieId, int actorId)
        {
            {
                _moviesContext.MovieActors.Add(new MovieActor { ActorId = actorId, MovieId = movieId });
                _moviesContext.SaveChanges();
            }
        }

        public bool UpdateMovie(Movie movie)
        {
            Movie foundMovie = GetById(movie.Id);

            if (foundMovie == null)
            {
                return false;
            }

            foundMovie.Title = movie.Title;
            foundMovie.Year = movie.Year;

            _moviesContext.SaveChanges();

            return true;
        }

        public void Remove(int movieId)
        {
            Movie movie = GetById(movieId);
            _moviesContext.Movies.Remove(movie);
            _moviesContext.SaveChanges();
        }

        
    }
}
