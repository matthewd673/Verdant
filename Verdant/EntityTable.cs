using System;
using System.Collections.Generic;

namespace Verdant
{
	internal class EntityTable
	{
		private Dictionary<ulong, List<Entity>> table;

		public EntityTable()
		{
			table = new();
		}

		public void Insert(int x, int y, Entity e)
		{
			List<Entity> cell;
			ulong key = Hash(x, y);

			if (table.TryGetValue(key, out cell))
			{
				cell.Add(e);
			}
			else
			{
				table.Add(key, new() { e });
			}
		}

		public bool Remove(int x, int y, Entity e)
		{
			List<Entity> cell;
			if (table.TryGetValue(Hash(x, y), out cell))
			{
				return cell.Remove(e);
			}

			return false;
		}

		public bool GetCell(int x, int y, out List<Entity> cell)
		{
			return table.TryGetValue(Hash(x, y), out cell);
		}

		public bool ContainsCell(int x, int y)
		{
			return table.ContainsKey(Hash(x, y));
		}

		public List<Entity>[] GetCellRange(int x1, int y1, int x2, int y2)
		{
			List<Entity>[] output = new List<Entity>[Math.Abs((x2 - x1 + 1) * (y2 - y1 + 1))];
			int p = 0;
			for (int i = x1; i <= x2; i++)
			{
				for (int j = y1; j <= y2; j++)
				{
					if (!GetCell(i, j, out output[p]))
					{
						output[p] = new List<Entity>();
					}
					p++;
				}
			}
			return output;
		}

		private ulong Hash(int x, int y)
		{
            ulong key = 0x0;
            key = key | (uint)x;
            key = key << 32;
            key = key | (uint)y;
			return key;
        }
	}
}

