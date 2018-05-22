using System.Collections.Generic;
using System.Threading.Tasks;
using MoviesAPI.DbModels;

namespace MoviesAPI.Interfaces
{
    public interface IMoviesService
    {
        List<Movie> GetAll();

        Movie GetById(int id);

        void AddNewMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        void Remove(int movieId);

        string GetRating(int id);

        List<Movie> GetByTitle(string title);

        List<Movie> GetByYear(int year);

        void AddMovieToActor(int movieId, int actorId);

        List<Movie> GetMoviesByActorId(int actorId);
    }
}
