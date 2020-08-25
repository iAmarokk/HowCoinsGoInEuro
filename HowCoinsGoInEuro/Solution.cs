using System;

namespace HowCoinsGoInEuro
{
    class Solution : IComparable<Solution>
    {

        public Solution(string country, int days)
        {
			this.Country = country;
			this.Days = days;
        }

        public int Days { get; set; }
        public string Country { get; set; }


        public int CompareTo(Solution other)
        {
            return this.Days.CompareTo(other.Days); 
        }
    }
}
