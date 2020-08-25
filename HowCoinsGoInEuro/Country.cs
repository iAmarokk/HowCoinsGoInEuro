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
        /// Example - France 1  4  4  6
        /// France Xl Yl Xh Yh
        /// 6|* * * *
        /// 5|* * * *
        /// 4|* * * *
        /// 3|
        /// 2| 
        /// 1|
        ///   1 2 3 4 5 6 
        /// </summary>

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
			this.CitiesInCountry = new List<City>();
			this.NeighborCountry = new List<Country>();
        }
        public override string ToString()
        {
            return this.Name;
        }

        public int CompareTo(Country other)
        {
            return this.Name.CompareTo(other.Name);
        }
        /// <summary>
        /// check eat coins of all countries in all cities
        /// </summary>
        /// <returns></returns>
        public bool IsDone()
        {
            if(!this.Complete)
            {
                bool citiesReady = this.CitiesInCountry.All(x => x.Complete);

                if (citiesReady)
                {
					this.Complete = true;
                }
            }

            return this.Complete;
        }

    }
}
