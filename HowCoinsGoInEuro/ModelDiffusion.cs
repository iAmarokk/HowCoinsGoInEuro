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

        private void GetNeighborsForCities()
        {
            for (int i = 0; i < Cities.GetLength(0); i++)
                for (int j = 0; j < Cities.GetLength(1); j++)
                {
                    if (OnMap(i, j) && Cities[i, j] != null)
                        GetNeighborForCity(new Coord(i, j));
                }
        }

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
                        TransferCoins(city);
                    }
                }
                foreach (var country in Countries)
                {
                    foreach (var city in country.CitiesInCountry)
                    {
                        CloseOfDay(city);
                    }
                }

                foreach (var country in Countries)
                {
                    foreach (var city in country.CitiesInCountry)
                    {
                        CheckCityIsDone(city);
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
                            if(!country.NeighborCountry.Contains(Countries[neighbor.Country]))
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

        public void TransferCoins(City city)
        {
            city.TrasferCoins();
        }

        private void CloseOfDay(City city)
        {
            city.CloseOfDay();
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

        private void CheckCityIsDone(City city)
        {
            city.IsDone();
        }
    }
}
