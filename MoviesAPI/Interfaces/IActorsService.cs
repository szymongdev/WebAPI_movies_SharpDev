using System.Collections.Generic;
using MoviesAPI.DbModels;

namespace MoviesAPI.Interfaces
{
    public interface IActorsService
    {
        List<Actor> GetAll();

        List<Actor> GetActorsByMovieId(int movieid);

        Actor GetById(int id);

        List<Actor> GetBySurname(string surname);

        void AddNewActor(Actor actor);

        void AddActorToMovie(int actorId, int movieId);

        bool UpdateActor(Actor actor);

        void RemoveActorFromMovie(int actorId, int movieId);

        void RemoveActor(int actorId);
    }
}
