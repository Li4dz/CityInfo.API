using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextEntensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {

            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>
            {
                new City
                {
                    Name = "New York",
                    Description = "City Description 1",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "Central Park", Description =  "Description 1"},
                        new PointOfInterest { Name = "Empire State", Description =  "Description 2"}
                    }
                },
                new City
                {
                    Name = "Lima",
                    Description = "City Description 2",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "Convento", Description =  "Description 3"},
                        new PointOfInterest { Name = "Parque", Description =  "Description 4"}
                    }
                },
                new City
                {
                    Name = "Rio",
                    Description = "City Description 3",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "Favelas", Description =  "Description 4"},
                        new PointOfInterest { Name = "Estadio", Description =  "Description 5"}
                    }
                }
            };

            context.AddRange(cities);
            context.SaveChanges();
        }
    }
}
