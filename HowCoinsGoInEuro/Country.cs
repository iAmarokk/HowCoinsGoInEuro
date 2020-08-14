using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion
{
    class Country : IComparable<Country>
    {
        /// <summary>
        /// x low point
        /// </summary>
        public int Xl { get; set; }
        /// <summary>
        /// x high point
        /// </summary>
        public int Xh { get; set; }
        /// <summary>
        /// y low point
        /// </summary>
        public int Yl { get; set; }
        /// <summary>
        /// y high point
        /// </summary>
        public int Yh { get; set; }
        public string Name { get; set; }
        public List<Country> NeighborCountry { get; set; }
        public List<City> CitiesInCountry { get; set; }
        public bool Complete { get; set; }
        public Country()
        {
            CitiesInCountry = new List<City>();
            NeighborCountry = new List<Country>();
        }
        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Country other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
