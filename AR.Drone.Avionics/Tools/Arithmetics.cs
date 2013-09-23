namespace AR.Drone.Avionics.Tools
{
    public static class Arithmetics
    {
        // Returns the giving aValue, making sure it is no lesser than aMin and isn't grater than aMax
        public static float KeepInRange(float aValue, float aMin, float aMax)
        {
            if (aValue < aMin) return aMin;
            if (aValue > aMax) return aMax;
            return aValue;
        }

        // Returns the giving aValue, making sure it is no lesser than aMin and isn't grater than aMax
        // Checks whether aMin is lesser than aMax, if not then fixes it
        public static float KeepInRangeChecked(float aValue, float aMin, float aMax)
        {
            if (aMin > aMax)
            {
                float temp = aMin;
                aMin = aMax;
                aMax = temp;
            }

            return KeepInRange(aValue, aMin, aMax);
        }
    }
}