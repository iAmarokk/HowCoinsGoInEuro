using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion
{
    class City
    {
        private const int defaultCoins = 1000000;
        public Dictionary<string, int> CoinsCountry { get; set; }
        public Dictionary<string, int> CoinsForDayTransfer { get; set; }
        public Country Country { get; set; }
        public Coord CityCoords { get; set; }
        public List<City> NeighborCities { get; set; }
        public bool Complete { get; set; }
        public City(Country country, List<Country> countries, Coord coord)
        {
            NeighborCities = new List<City>();
            CoinsCountry = new Dictionary<string, int>(countries.Count());
            CoinsForDayTransfer = new Dictionary<string, int>(countries.Count());
            Country = country;
            foreach (var item in countries)
            {
                CoinsCountry.Add(item.Name,0);
                CoinsForDayTransfer.Add(item.Name, 0);
            }
            CityCoords = coord;
            CoinsCountry[Country.Name] = defaultCoins;
        }

        public override string ToString()
        {
            return Country.Name;
        }

    }
}
