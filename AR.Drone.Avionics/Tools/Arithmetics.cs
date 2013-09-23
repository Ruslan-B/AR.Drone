using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Drone.Avionics.Tools
{
    public class Arithmetics
    {
        // Returns the giving aValue, making sure it is no lesser than aMin and isn't grater than aMax
        public static float KeepInRange(float aValue, float aMin, float aMax)
        {
            if (aValue < aMin) return aMin;
            else if (aValue > aMax) return aMax;
            else return aValue;
        }

        // Returns the giving aValue, making sure it is no lesser than aMin and isn't grater than aMax
        // Checks whether aMin is lesser than aMax, if not then fixes it
        public static float KeepInRangeChecked(float aValue, float aMin, float aMax)
        {
            if (aMin > aMax)
            {
                float __temp = aMin;
                aMin = aMax;
                aMax = __temp;
            }

            return KeepInRange(aValue, aMin, aMax);
        }
    }
}
