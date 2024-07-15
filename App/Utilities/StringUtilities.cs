using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Utilities
{
    public static class StringUtilities
    {
        /// <summary>
        /// Searches for the value in the strings
        /// </summary>
        /// <param name="strings">The enumeration of strings</param>
        /// <param name="value">The value to search for</param>
        /// <returns>Whether the enumeration of string contains value</returns>
        public static bool Contains(IEnumerable<string> strings, string value)
        {
            return strings.Any(s => s.Contains(value));
        }
        /// <summary>
        /// Validates if left string match the right strings
        /// </summary>
        /// <param name="left">The left strings</param>
        /// <param name="right">The right strings</param>
        /// <returns>True if they match, false otherwise</returns>
        public static bool Same(this IEnumerable<string> left, IEnumerable<string> right)
        {
            List<string> leftList = left.ToList();
            List<string> rightList = right.ToList();
            if(leftList.Count != rightList.Count)
            {
                return false;
            }
            for (int index = 0; index < leftList.Count; index++)
            {
                if (leftList[index] != rightList[index])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
