using System;

namespace Utilities.Enum
{
    public static class Enum
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Gets a random enum from a Enum Collection
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <returns>Random Enum from specified collection</returns>
        public static T RandomEnumValue<T>()
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(random.Next(v.Length));
        }
    }
}
