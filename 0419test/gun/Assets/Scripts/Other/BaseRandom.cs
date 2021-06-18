using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public static class BaseRandom
    {
        static Random random = new Random( System.Environment.TickCount );

        public static int Next()
        {
            return random.Next();
        }

        public static int Next( int max )
        {
            return random.Next( max );
        }

        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        public static float Next( float max )
        {
            return (float)(random.NextDouble() * max);
        }

        public static float Next(float min , float max)
        {
            return min + (float)(random.NextDouble() * (max - min ));
        }

        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
