using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DbModels;
using MoviesAPI.Interfaces;
using MoviesAPI.Models;
using System.Collections.Generic;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    public class ActorController : Controller
    {
        private IActorsService _actorsService;

        public ActorController(IActorsService actorService)
        {
            _actorsService = actorService;
        }

        /// <summary>
        /// Get all actors
        /// </summary>
        /// <returns>List of actors</returns>
        /// 
        [HttpGet]
        public IActionResult GetAllActors()
        {
            var actors = _actorsService.GetAll();
            return Ok(AutoMapper.Mapper.Map<List<ActorResponse>>(actors));
        }

        /// <summary>
        /// Get list of actors by movieId
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>List of actors</returns>
        [HttpGet("{movieId}/actors")]
        public IActionResult GetActorsByMovie(int movieId)
        {
            var list = _actorsService.GetActorsByMovieId(movieId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<List<ActorResponse>>(list));
        }

        /// <summary>
        /// Get actor by id
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>Actor if found</returns>
        [HttpGet("{actorId}")]
        public IActionResult GetActorById(int actorId)
        {
            Actor actor = _actorsService.GetById(actorId);

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(AutoMapper.Mapper.Map<ActorResponse>(actor));
        }

        /// <summary>
        /// Get actors by surname
        /// </summary>
        /// <param name="surname"></param>
        /// <returns>Actors if found</returns>
        [HttpGet("{surname}/surname")]
        public IActionResult GetActorBySurname(string surname)
        {
            var list = _actorsService.GetBySurname(surname);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<List<ActorResponse>>(list));
        }

        /// <summary>
        /// Add new actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]ActorRequest actor)
        {
            _actorsService.AddNewActor(AutoMapper.Mapper.Map<Actor>(actor));
            return Ok();
        }

        /// <summary>
        /// Add actor to movie
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpPost("{actorId}/{movieId}")]
        public IActionResult AddActorToMovie(int actorId, int movieId)
        {
            try
            {
                _actorsService.AddActorToMovie(actorId, movieId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update actor in repositorium
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]ActorRequest actor)
        {
            if (_actorsService.UpdateActor(AutoMapper.Mapper.Map<Actor>(actor)))
            {
                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove actor from movie
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpDelete("{actorId}/{movieId}")]
        public IActionResult Delete(int actorId, int movieId)
        {
            _actorsService.RemoveActorFromMovie(actorId, movieId);
            return Ok();
        }

        /// <summary>
        /// Remove actor from repositorium
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        [HttpDelete("{actorId}")]
        public IActionResult Delete(int actorId)
        {
            _actorsService.RemoveActor(actorId);
            return Ok();
        }
    }
}
