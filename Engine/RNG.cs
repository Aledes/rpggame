using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Engine
{
    public static class RNG
    {
        //ensures that no patterns emerge in RNG.
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int NumberBetween(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            //Using Math.Max and subtracting .000000000001,
            //to ensure 'multiplier' will always be between 0.0 and .9999999999,
            //otherwise, possible for it to be "1", causes problems in rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            //Need to add 1 to the range, allow for rounding done w/ Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }
    }
}
