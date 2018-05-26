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
            if (list != null)
            {
                foreach (Review rew in list)
                {
                    sum += rew.Rate;
                }
            }
            RatingClass ratingClass = new RatingClass();
            ratingClass.MovieId = id;
            ratingClass.Rating = Math.Round((double)sum / list.Count(), 2);
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
            
            _moviesContext.MovieActors.Add(new MovieActor { ActorId = actorId, MovieId = movieId });
            _moviesContext.SaveChanges();
            
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

        public void AddPopular()
        {
            List<JObject> movies_json_list = GetPopularMoviesAsync().Result;
            foreach (JObject m in movies_json_list)
            {
                Movie movie = new Movie
                {
                    Title = m["title"].ToString(),
                    Year = (int)m["year"]
                };
                Movie foundMovie = _moviesContext.Movies
                  .Where(mov => mov.Title == movie.Title)
                  .Where(mov => mov.Year == movie.Year)
                  .SingleOrDefault();
                if (foundMovie == null)
                {
                    _moviesContext.Movies.Add(movie);
                    _moviesContext.SaveChanges();
                }
            }
        }
        
        public async Task<List<JObject>> GetPopularMoviesAsync()
        {
            var baseAddress = new Uri("https://api.trakt.tv");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "bafdc324cc00f86d72d18eb86c58a5933a950ebeea0ee089641be1c63f40acac");

                using (var response = await httpClient.GetAsync(baseAddress + "/movies/popular"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<JObject> movies_json_list = JsonConvert.DeserializeObject<List<JObject>>(responseData);
                    return movies_json_list;
                }
            }
        }


    }
}
