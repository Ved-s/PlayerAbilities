using System;

namespace PlayerAbilities
{
    public static class RaindomExtension
    {
        public static float NextFloat(this Random random, float min, float max) 
        {
            double diff = max - min;
            return (float)(random.NextDouble()*diff+min);
        }
    }
}
