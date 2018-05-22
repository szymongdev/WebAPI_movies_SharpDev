using System.Collections.Generic;
using System.Linq;
using MoviesAPI.DbModels;
using MoviesAPI.Interfaces;

namespace MoviesAPI.Services
{
    public class ActorsService : IActorsService
    {
        private readonly MoviesContext _moviesContext;

        public ActorsService(MoviesContext moviesContext)
        {
            _moviesContext = moviesContext;
        }

        public List<Actor> GetAll()
        {
            return _moviesContext.Actors.ToList();
        }

        public Actor GetById(int id)
        {
            Actor foundActor = _moviesContext.Actors
                  .Where(m => m.ActorId == id)
                  .SingleOrDefault();

            return foundActor;
        }

        public List<Actor> GetActorsByMovieId(int movieid)
        {
            List<Actor> foundActors = _moviesContext.MovieActors
                  .Where(m => m.MovieId == movieid)
                  .Select(m => m.Actor)
                  .ToList();

            return foundActors;
        }

        public List<Actor> GetBySurname(string surname)
        {
            List<Actor> foundActors = _moviesContext.Actors
                  .Where(m => m.Surname==surname)
                  .ToList();

            return foundActors;
        }

        public void AddNewActor(Actor actor)
        {
            _moviesContext.Actors.Add(actor);
            _moviesContext.SaveChanges();
        }

        public void AddActorToMovie(int actorId, int movieId)
        {
            {
                _moviesContext.MovieActors.Add(new MovieActor { ActorId = actorId, MovieId = movieId});
                _moviesContext.SaveChanges();
            }
        }

        public bool UpdateActor(Actor actor)
        {
            Actor foundActor = GetById(actor.ActorId);

            if (foundActor == null)
            {
                return false;
            }

            foundActor.Name  = actor.Name;
            foundActor.Surname = actor.Surname;
            foundActor.YearOfBirth = actor.YearOfBirth;

            _moviesContext.SaveChanges();

            return true;
        }

        public void RemoveActorFromMovie(int actorId, int movieId)
        {
            MovieActor found = _moviesContext.MovieActors
                  .Where(m => m.MovieId == movieId &&  m.ActorId == actorId)
                  .Single();
            _moviesContext.MovieActors.Remove(found);
            _moviesContext.SaveChanges();
        }

        public void RemoveActor(int actorId)
        {
            Actor actor = GetById(actorId);
            _moviesContext.Actors.Remove(actor);
            _moviesContext.SaveChanges();
        }

    }
}