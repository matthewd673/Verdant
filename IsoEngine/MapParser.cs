using System;

namespace IsoEngine
{
    public static class MapParser
    {

        /// <summary>
        /// Given the contents of a MapTools map file, returns a 2D array of characters to be further parsed into Entities.
        /// </summary>
        /// <param name="data">The data of the map file.</param>
        /// <returns>A 2D array of the map characters.</returns>
        public static char[,] Parse(string data)
        {
            string[] lines = data.Split('\n');

            string[] firstSplit = lines[0].Split(',');

            int width = Convert.ToInt32(firstSplit[0]);
            int height = Convert.ToInt32(firstSplit[1]);
            char[,] map = new char[width, height];

            int i = 0;
            int j = 0;
            foreach (char c in lines[1].ToCharArray())
            {
                map[i, j] = c;

                i++;
                if (i == width)
                {
                    i = 0;
                    j++;
                }
            }

            return map;
        }

    }
}
