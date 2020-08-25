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
			this.NeighborCities = new List<City>();
			this.CoinsCountry = new List<int>();
			this.CoinsForDayTransfer = new List<int>();
			this.Country = countries.IndexOf(country);
            foreach (var item in countries)
            {
				this.CoinsCountry.Add(0);
				this.CoinsForDayTransfer.Add(0);
            }
			this.CityCoords = coord;
			this.CoinsCountry[this.Country] = defaultCoins;
        }

        public override string ToString()
        {
            return this.CityCoords.ToString();
        }
        /// <summary>
        /// check eat coins of all countries in the city
        /// </summary>
        public void IsDone()
        {
            if (!this.Complete)
            {
                bool coinsOfAllCountriesInCity = this.CoinsCountry.All(x => x > 0);
                if (coinsOfAllCountriesInCity)
                {
					this.Complete = true;
                }
            }
        }
        /// <summary>
        /// transfer coins nearby cities
        /// </summary>
        public void TrasferCoins()
        {
            for(int i = 0; i < this.NeighborCities.Count(); i++)
            {
                for (int j = 0; j < this.CoinsCountry.Count(); j++)
                {
					this.NeighborCities[i].CoinsForDayTransfer[j] += this.CoinsCountry[j] / diffusionRate;
					this.CoinsForDayTransfer[j] -= this.CoinsCountry[j] / diffusionRate;
                }
            }
        }
        /// <summary>
        /// coin fixing on end of day
        /// </summary>
        public void CloseOfDay()
        {
            for (int i = 0; i < this.CoinsCountry.Count(); i++)
            {
				this.CoinsCountry[i] += this.CoinsForDayTransfer[i];
				this.CoinsForDayTransfer[i] = 0;
            }
        }
    }
}
