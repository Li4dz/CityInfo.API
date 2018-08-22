using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current  { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1,
                    Name = "New York",
                    Description = "City Description 1",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto { Id = 1, Name = "Central Park", Description =  "Description 1"},
                        new PointOfInterestDto { Id = 2, Name = "Empire State", Description =  "Description 2"}
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Lima",
                    Description = "City Description 2",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto { Id = 3, Name = "Convento", Description =  "Description 3"},
                        new PointOfInterestDto { Id = 4, Name = "Parque", Description =  "Description 4"}
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Rio",
                    Description = "City Description 3",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto { Id = 4, Name = "Favelas", Description =  "Description 4"},
                        new PointOfInterestDto { Id = 5, Name = "Estadio", Description =  "Description 5"}
                    }
                }
            };
        }
    }
}
