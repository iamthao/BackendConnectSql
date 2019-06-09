using System;

namespace Framework.Utility
{
    public static class UnitHelper
    {
        public static double MetersToMiles(this double? meters, int digits = 0)
        {
            return meters == null ? 0 : Math.Round(meters.Value * 0.000621371, digits);
        }

        public static double MilesToMeters(this double? miles, int digits = 0)
        {
            return miles == null ? 0 : Math.Round(miles.Value * 1609.34, digits);
        }

        public static double MetersPerSecondTpMph(this double meter, int digits = 0)
        {
            return  Math.Round(meter * 2.23693629, digits);
        }
    }
}