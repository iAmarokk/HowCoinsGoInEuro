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
        Dictionary<string, int> CountrySolution = new Dictionary<string, int>();

        public ModelDiffusion(List<Country> countries)
        {
            Countries = new List<Country>();
            Countries = countries;
            InitArrayOfCity(Countries);
            BuildCityMap(Countries);
            GetNeighborsForCities();
            CheckForConnectionsCounty();
            CountrySolution = new Dictionary<string, int>();
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

        private void BuildCityMap(List<Country> countries)
        {
            foreach (var item in countries)
            {
                for (int i = item.Xl; i <= item.Xh; i++)
                {
                    for (int j = item.Yl; j <= item.Yh; j++)
                    {
                        Cities[i, j] = (new City(item, countries, new Coord(i, j)));
                        item.CitiesInCountry.Add(Cities[i, j]);
                    }
                }
            }
        }

        private void InitArrayOfCity(List<Country> countries)
        {
            int maxX = 0, maxY = 0;
            foreach (var item in countries)
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
                CountrySolution.Add(item.Name, 0);
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

                if (!CountrySolution.ContainsValue(0))
                {
                    done = true;
                }
            }

            PrintSolution();
        }

        private void PrintSolution()
        {
            foreach (KeyValuePair<string, int> keyValue in CountrySolution)
            {
                Console.WriteLine("Country {0} Days {1}", keyValue.Key, keyValue.Value.ToString());
            }
        }

        public void CheckForConnectionsCounty()
        {
            GetNeighborForCountry();
            bool isCheck = СountryHasAnyNeighbors();
            var result = GetNumberConnections();
            var numberGraphEdges = (result / 2);
            if(!(numberGraphEdges >= (Countries.Count() - 1)) || !isCheck)
            {
                throw new ArgumentException("Countries not connect");
            }            
        }

        private bool СountryHasAnyNeighbors()
        {
            bool connect = Countries.All(c => c.NeighborCountry.Count() > 0);                        
            return connect;
        }

        private int GetNumberConnections()
        {
            int numberConnections = 0;
            foreach(var country in Countries)
            {
                foreach(var neighbor in country.NeighborCountry)
                {
                    numberConnections++;
                }
            }
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
                        if (neighbor.Country.Name != country.Name)
                        {
                            country.NeighborCountry.Add(neighbor.Country);
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
            foreach (var item in city.NeighborCities)
            {
                foreach (KeyValuePair<string, int> keyValue in city.CoinsCountry)
                {
                    if (keyValue.Value != 0)
                    {
                        Cities[item.CityCoords.X, item.CityCoords.Y].CoinsForDayTransfer[keyValue.Key] += keyValue.Value / diffusionRate;
                        city.CoinsForDayTransfer[keyValue.Key] -= keyValue.Value / diffusionRate;
                    }
                }
            }
        }

        private void CloseOfDay(City city)
        {
            foreach (var country in Countries)
            {
                city.CoinsCountry[country.Name] += city.CoinsForDayTransfer[country.Name];
                city.CoinsForDayTransfer[country.Name] = 0;
            }
        }

        private void CheckIsReadyCountries(int day)
        {
            foreach(var item in Countries)
            {
                if(!item.Complete)
                {
                    bool citiesReady = item.CitiesInCountry.All(x => x.Complete);
                    if(citiesReady)
                    {
                        item.Complete = true;
                        CountrySolution[item.Name] = day;
                    }
                }
            }
        }

        private void CheckCityIsDone(City city)
        {
            int numberCoinsOfCountry = 0;
            foreach(KeyValuePair<string, int> keyValue in city.CoinsCountry)
            {
                if(keyValue.Value != 0)
                {
                    numberCoinsOfCountry++;
                }
            }
            if(numberCoinsOfCountry == Countries.Count())
            {
                city.Complete = true;
            }
        }
    }
}
