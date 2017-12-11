using System.Collections.Generic;

namespace SharpSweeper.CSharp
{
    public static class Random
    {
        private static System.Random random = new System.Random();
        
        public static T GetRandom<T>(this List<T> array)
        {
            if (array == null || (int)array.Count == 0)
            {
                return default(T);
            }
            return array[random.Next(0, (int)array.Count)];
        }
        
        public static T GetRandom<T>(this T[] array)
        {
            if (array == null || (int)array.Length == 0)
            {
                return default(T);
            }
            return array[random.Next(0, (int)array.Length)];
        }
    }
}