using System;
using System.Collections.Generic;
using System.Linq;

namespace EuroDiffusion
{
    class ModelDiffusion
    {
        private const int diffusionRate = 1000;
        List<Country> Countries { get; set; }
        private City[,] Cities;
        List<int> CountrySolution = new List<int>();

        public ModelDiffusion(List<Country> countries)
        {
            Countries = new List<Country>();
            Countries = countries;
            InitArrayOfCity();
            BuildCityMap();
            GetNeighborsForCities();
            CheckForConnectionsCountries();

        }

        /// <summary>
        /// get a neighbor for all cities
        /// </summary>
        private void GetNeighborsForCities()
        {
            for (int i = 0; i < Cities.GetLength(0); i++)
                for (int j = 0; j < Cities.GetLength(1); j++)
                {
                    if (OnMap(i, j) && Cities[i, j] != null)
                        GetNeighborForCity(new Coord(i, j));
                }
        }

        /// <summary>
        /// get a neighborhood for the city
        /// </summary>
        /// <param name="coord"></param>
        private void GetNeighborForCity(Coord coord)
        {

            if (OnMap(coord.X + 1, coord.Y))
            {
                if (Cities[coord.X + 1, coord.Y] != null)
                {
                    Cities[coord.X, coord.Y].NeighborCities.Add(Cities[coord.X + 1, coord.Y]);
                }
            }
            if (OnMap(coord.X - 1, coord.Y))
            {
                if (Cities[coord.X - 1, coord.Y] != null)
                {
                    Cities[coord.X, coord.Y].NeighborCities.Add(Cities[coord.X - 1, coord.Y]);
                }
            }
            if (OnMap(coord.X, coord.Y + 1))
            {
                if (Cities[coord.X, coord.Y + 1] != null)
                {
                    Cities[coord.X, coord.Y].NeighborCities.Add(Cities[coord.X, coord.Y + 1]);
                }
            }
            if (OnMap(coord.X, coord.Y - 1))
            {
                if (Cities[coord.X, coord.Y - 1] != null)
                {
                    Cities[coord.X, coord.Y].NeighborCities.Add(Cities[coord.X, coord.Y - 1]);
                }
            }
        }

        /// <summary>
        /// build a map of cities
        /// </summary>
        private void BuildCityMap()
        {
            foreach (var item in Countries)
            {
                for (int i = item.Xl; i <= item.Xh; i++)
                {
                    for (int j = item.Yl; j <= item.Yh; j++)
                    {
                        Cities[i, j] = (new City(item, Countries, new Coord(i, j)));
                        item.CitiesInCountry.Add(Cities[i, j]);
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
            foreach (var item in Countries)
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
            Cities = new City[maxX + 1, maxY + 1];
        }

        public void Solve()
        {
            bool done = false;
            int days = 0;

            foreach (var item in Countries)
            {
                CountrySolution.Add(0);
            }

            while (!done)
            {

                foreach (var country in Countries)
                {
                    foreach (var city in country.CitiesInCountry)
                    {
                        city.TrasferCoins();
                        city.CloseOfDay();
                        city.IsDone();
                    }
                }

                days++;
                CheckIsReadyCountries(days);

                if (CountrySolution.All(x => x > 0))
                {
                    done = true;
                }
            }

            PrintSolution();
        }

        private void PrintSolution()
        {
            for(int i = 0; i < Countries.Count(); i++)
            {
                Console.WriteLine("Country {0} Days {1}", Countries[i], CountrySolution[i]);
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
            GetNeighborForCountry();
            bool isCheck = СountriesHasAnyNeighbors();
            var result = GetNumberConnections();
            var numberGraphEdges = (result / 2);
            if(!(numberGraphEdges >= (Countries.Count() - 1)) || !isCheck)
            {
                throw new ArgumentException("Countries not connect");
            }            
        }

        private bool СountriesHasAnyNeighbors()
        {
            bool connect = Countries.All(c => c.NeighborCountry.Count() > 0);                        
            return connect;
        }

        private int GetNumberConnections()
        {
            int numberConnections = Countries.Sum(c => c.NeighborCountry.Count());
            
            return numberConnections;
        }

        private void GetNeighborForCountry()
        {
            foreach (var country in Countries)
            {
                foreach (var city in country.CitiesInCountry)
                {
                    foreach (var neighbor in city.NeighborCities)
                    {
                        if (neighbor.Country != Countries.IndexOf(country))
                        {
                            if (!country.NeighborCountry.Contains(Countries[neighbor.Country]))
                                country.NeighborCountry.Add(Countries[neighbor.Country]);
                        }
                    }
                }
            }
        }

        private bool OnMap(int i, int j)
        {
            return (i >= 0 && i < Cities.GetLength(0)) &&
                   (j >= 0 && j < Cities.GetLength(1));
        }

        private void CheckIsReadyCountries(int day)
        {
            for (int i = 0; i < Countries.Count(); i++)
            {
                if(!Countries[i].Complete)
                {
                    if(Countries[i].IsDone())
                    {
                        CountrySolution[i] = day;
                    }
                }
            }
        }

    }
}
