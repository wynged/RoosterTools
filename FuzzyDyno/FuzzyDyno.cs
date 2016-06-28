using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyString;

namespace FuzzyDyno
{
    /// <summary>
    /// Fuzzy String comparisons enable us to find a string that most closely matches a given string input.
    /// icon from http://cookie-waffle.deviantart.com/
    /// </summary>
    public static class FuzzyStringComparisons
    {
        /// <summary>
        /// Fuzzy string comparison betwee two strings using the JaroWinkler algorithm. 
        /// Returns 1 if the strings are identical.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>A value between 0 and 1</returns>
        public static double CompareTwoStrings(string str1, string str2)
        {
            double value = FuzzyString.ComparisonMetrics.JaroWinklerDistance(str1, str2);
            return value;
        }
    }
}
