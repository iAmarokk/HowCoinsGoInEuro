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
        private const int diffusionRate = 1000;
        public List<int> CoinsCountry { get; set; }
        public List<int> CoinsForDayTransfer { get; set; }
        public int Country { get; set; }
        public Coord CityCoords { get; set; }
        public List<City> NeighborCities { get; set; }
        public bool Complete { get; set; }
        public City(Country country, List<Country> countries, Coord coord)
        {
            NeighborCities = new List<City>();
            CoinsCountry = new List<int>();
            CoinsForDayTransfer = new List<int>();
            Country = countries.IndexOf(country);
            foreach (var item in countries)
            {
                CoinsCountry.Add(0);
                CoinsForDayTransfer.Add(0);
            }
            CityCoords = coord;
            CoinsCountry[Country] = defaultCoins;
        }

        public override string ToString()
        {
            return CityCoords.ToString();
        }

        public void IsDone()
        {
            bool coinsOfAllCountriesInCity = CoinsCountry.All(x => x > 0);
            if (coinsOfAllCountriesInCity)
            {
                Complete = true;
            }
        }
        
        public void TrasferCoins()
        {
            for(int i = 0; i < NeighborCities.Count(); i++)
            {
                for (int j = 0; j < CoinsCountry.Count(); j++)
                {
                    NeighborCities[i].CoinsForDayTransfer[j] += CoinsCountry[j] / diffusionRate;
                    CoinsForDayTransfer[j] -= CoinsCountry[j] / diffusionRate;
                }
            }
        }

        public void CloseOfDay()
        {
            for (int i = 0; i < CoinsCountry.Count(); i++)
            {
                CoinsCountry[i] += CoinsForDayTransfer[i];
                CoinsForDayTransfer[i] = 0;
            }
        }
    }
}
