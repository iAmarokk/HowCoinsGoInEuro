using System;
using System.Collections.Generic;
using System.Linq;
using HowCoinsGoInEuro;

namespace EuroDiffusion
{
	class ModelDiffusion
    {
        private const int diffusionRate = 1000;
        List<Country> Countries { get; set; }
        private City[,] cities;
		private List<int> countrySolution = new List<int>();

        public ModelDiffusion(List<Country> countries)
        {
			this.Countries = new List<Country>();
			this.Countries = countries;
			this.InitArrayOfCity();
			this.BuildCityMap();
			this.GetNeighborsForCities();
			this.CheckForConnectionsCountries();

        }

        /// <summary>
        /// get a neighbor for all cities
        /// </summary>
        private void GetNeighborsForCities()
        {
            for (int i = 0; i < this.cities.GetLength(0); i++)
            {
                for (int j = 0; j < this.cities.GetLength(1); j++)
                {
                    if (this.OnMap(i, j) && this.cities[i, j] != null)
                    {
						this.GetNeighborForCity(new Coord(i, j));
                    }                        
                }
            }
        }

        /// <summary>
        /// get a neighborhood for the city
        /// </summary>
        /// <param name="coord"></param>
        private void GetNeighborForCity(Coord coord)
        {

            if (this.OnMap(coord.X + 1, coord.Y))
            {
                if (this.cities[coord.X + 1, coord.Y] != null)
                {
					this.cities[coord.X, coord.Y].NeighborCities.Add(this.cities[coord.X + 1, coord.Y]);
                }
            }
            if (this.OnMap(coord.X - 1, coord.Y))
            {
                if (this.cities[coord.X - 1, coord.Y] != null)
                {
					this.cities[coord.X, coord.Y].NeighborCities.Add(this.cities[coord.X - 1, coord.Y]);
                }
            }
            if (this.OnMap(coord.X, coord.Y + 1))
            {
                if (this.cities[coord.X, coord.Y + 1] != null)
                {
					this.cities[coord.X, coord.Y].NeighborCities.Add(this.cities[coord.X, coord.Y + 1]);
                }
            }
            if (this.OnMap(coord.X, coord.Y - 1))
            {
                if (this.cities[coord.X, coord.Y - 1] != null)
                {
					this.cities[coord.X, coord.Y].NeighborCities.Add(this.cities[coord.X, coord.Y - 1]);
                }
            }
        }

        /// <summary>
        /// build a map of cities
        /// </summary>
        private void BuildCityMap()
        {
            foreach (Country item in this.Countries)
            {
                for (int i = item.Xl; i <= item.Xh; i++)
                {
                    for (int j = item.Yl; j <= item.Yh; j++)
                    {
						this.cities[i, j] = (new City(item, this.Countries, new Coord(i, j)));
                        item.CitiesInCountry.Add(this.cities[i, j]);
                    }
                }
            }
        }
        /// <summary>
        /// defining an array of cities
        /// </summary>
        private void InitArrayOfCity()
        {
            int maxX = 0, maxY = 0;
            foreach (Country item in this.Countries)
            {
                if (item.Xh > maxX)
                {
                    maxX = item.Xh;
                }
                if (item.Yh > maxY)
                {
                    maxY = item.Yh;
                }
            }
			this.cities = new City[maxX + 1, maxY + 1];
        }

        /// <summary>
        /// solve solution and console output
        /// </summary>
        public void Solve()
        {
            bool done = false;
            int days = 0;

            foreach (Country item in this.Countries)
            {
				this.countrySolution.Add(0);
            }

            while (!done)
            {

                foreach (Country country in this.Countries)
                {
                    foreach (City city in country.CitiesInCountry)
                    {
                        city.TrasferCoins();
                    }
                }

                foreach (Country country in this.Countries)
                {
                    foreach (City city in country.CitiesInCountry)
                    {
                        city.CloseOfDay();
                        city.IsDone();
                    }
                }

                days++;
				this.CheckIsReadyCountries(days);

                if (this.countrySolution.All(x => x > 0))
                {
                    done = true;
                }
            }

			this.PrintSolution();
        }

        private void PrintSolution()
        {
            List<Solution> solution = new List<Solution>();
            for (int i = 0; i < this.Countries.Count(); i++)
            {
                solution.Add(new Solution(this.Countries[i].Name, this.countrySolution[i]));
            }

            List<Solution> result = solution.OrderBy(n => n.Days).ToList();

            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("Country {0} Days {1}", result[i].Country, result[i].Days);
            }
        }

        /// <summary>
        /// checking whether countries are connected
        /// 1) checking whether the country has a neighbor
        /// 2) counts the number of edges
        /// 3) according to the formula, the minimum number of faces is n - 1
        /// </summary>
        public void CheckForConnectionsCountries()
        {
			this.GetNeighborForCountry();
            bool isCheck = this.СountriesHasAnyNeighbors();
            int result = this.GetNumberConnections();
            int numberGraphEdges = (result / 2);
            if(!(numberGraphEdges >= (this.Countries.Count() - 1)) || !isCheck)
            {
                throw new ArgumentException("Countries not connect");
            }            
        }

        private bool СountriesHasAnyNeighbors()
        {
            bool connect = this.Countries.All(c => c.NeighborCountry.Count() > 0);                        
            return connect;
        }

        private int GetNumberConnections()
        {
            int numberConnections = this.Countries.Sum(c => c.NeighborCountry.Count());
            
            return numberConnections;
        }

        private void GetNeighborForCountry()
        {
            foreach (Country country in this.Countries)
            {
                foreach (City city in country.CitiesInCountry)
                {
                    foreach (City neighbor in city.NeighborCities)
                    {
                        if (neighbor.Country != this.Countries.IndexOf(country))
                        {
                            if (!country.NeighborCountry.Contains(this.Countries[neighbor.Country]))
                            {
                                country.NeighborCountry.Add(this.Countries[neighbor.Country]);
                            }
                                
                        }
                    }
                }
            }
        }

        private bool OnMap(int i, int j)
        {
            return (i >= 0 && i < this.cities.GetLength(0)) &&
                   (j >= 0 && j < this.cities.GetLength(1));
        }

        private void CheckIsReadyCountries(int day)
        {
            for (int i = 0; i < this.Countries.Count(); i++)
            {
                if(!this.Countries[i].Complete)
                {
                    if(this.Countries[i].IsDone())
                    {
						this.countrySolution[i] = day;
                    }
                }
            }
        }

    }
}
