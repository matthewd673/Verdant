using System;
using System.Collections.Generic;

namespace Verdant
{
    public class MapParser
    {

        int tileWidth = 1;
        int tileHeight = 1;

        Dictionary<char, Action<int, int>> buildActionDictionary;

        public MapParser()
        {
            buildActionDictionary = new Dictionary<char, Action<int, int>>();
        }
        public MapParser(int tileWidth, int tileHeight)
        {
            buildActionDictionary = new Dictionary<char, Action<int, int>>();
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

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

        public void DefineBuildAction(char tileChar, Action<int, int> buildAction)
        {
            buildActionDictionary.Add(tileChar, buildAction);
        }
        public void RemoveBuildAction(char tileChar)
        {
            buildActionDictionary.Remove(tileChar);
        }
        public void ClearBuildActions()
        {
            buildActionDictionary.Clear();
        }

        public void BuildMap(char[,] mapData)
        {
            for (int i = 0; i < mapData.GetLength(0); i++)
            {
                for (int j = 0; j < mapData.GetLength(1); j++)
                {
                    if (buildActionDictionary.ContainsKey(mapData[i, j]))
                        buildActionDictionary[mapData[i, j]].Invoke(i * tileWidth, j * tileHeight);
                }
            }
        }

    }
}
