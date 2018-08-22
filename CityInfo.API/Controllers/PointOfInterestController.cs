using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInforepository;

        public PointOfInterestController(
            ILogger<PointOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInforepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Exception sample");

                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

                if(!_cityInforepository.CityExist(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} is not found.");
                    return NotFound();
                }

                var pointOfInterestForCity = _cityInforepository.GetPointsOfInterestsForCity(cityId);

                var pointsOfInterestCityResults = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity);
                

                return Ok(pointsOfInterestCityResults);

                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} is not found.");
                //    return NotFound();
                //}

                //return Ok(city.PointsOfInterest);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception {ex.ToString()}");
                return StatusCode(500, "Log information critical");
            }

            
            
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (_cityInforepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInforepository.GetPointOfInterestForCity(cityId, id);

            if(pointOfInterest is null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(pointOfInterestResult);

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(
            int cityId, [FromBody]PointOfInterestForCreationDto pointOfInterest)
        {
            if(pointOfInterest is null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description.Equals(pointOfInterest.Name))
            {
                ModelState.AddModelError("Description", "Descripción y nombre identicos");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (!_cityInforepository.CityExist(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);

            _cityInforepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if(!_cityInforepository.Save())
            {
                return StatusCode(500, "Problems");
            }

            var createPointOfInterestReturn = Mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = finalPointOfInterest.Id }, createPointOfInterestReturn);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest is null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description.Equals(pointOfInterest.Name))
            {
                ModelState.AddModelError("Description", "Descripción y nombre identicos");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInforepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInforepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity is null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_cityInforepository.Save())
            {
                return StatusCode(500, "Problems");
            }
            
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            if (_cityInforepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInforepository.GetPointOfInterestForCity(cityId, id);

            if(pointOfInterestEntity is null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);


            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pointOfInterestToPatch.Description.Equals(pointOfInterestToPatch.Description))
            {
                ModelState.AddModelError("Description", "Descripción y nombre identicos");
            }

            TryValidateModel(pointOfInterestToPatch);

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_cityInforepository.Save())
            {
                return StatusCode(500, "Problems");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if(!_cityInforepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInforepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity is null)    
            {
                return NotFound();
            }

            _cityInforepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInforepository.Save())
            {
                return StatusCode(500, "Problems");
            }

            _mailService.Send("Point of Interest is deleted", $"Point interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

            return NoContent();

        }

    }
}
