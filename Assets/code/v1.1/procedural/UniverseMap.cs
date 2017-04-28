using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class UniverseMap
    {
        public static int LocationX = 0;
        public static int LocationY = 0;
        public static int LocationZ = 0;

        public static int GetSeed()
        {
            return (LocationX << 17) ^ (LocationX >> 15) ^ (LocationY << 12) ^ (LocationY << 20) ^ (LocationZ << 24) ^ (LocationZ >> 8);
        }
        public static void ReturnHome()
        {
            LocationX = 0;
            LocationY = 0;
            LocationZ = 0;
        }
        public static void SetDestination(int x, int y, int z)
        {
            LocationX = x;
            LocationY = y;
            LocationZ = z;
        }
    }
}