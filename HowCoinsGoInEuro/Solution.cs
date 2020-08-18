using System;

namespace HowCoinsGoInEuro
{
    class Solution : IComparable<Solution>
    {

        public Solution(string country, int days)
        {
            Country = country;
            Days = days;
        }

        public int Days { get; set; }
        public string Country { get; set; }


        public int CompareTo(Solution other)
        {
            return Days.CompareTo(other.Days); 
        }
    }
}
