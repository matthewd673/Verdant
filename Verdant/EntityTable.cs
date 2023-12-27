using System;
using System.Collections.Generic;

namespace Verdant;

internal class EntityTable
{
    private Dictionary<ulong, EntityList> table;

    public EntityTable()
    {
        table = new();
    }

    public void Insert(int x, int y, Entity e)
    {
        EntityList cell;
        ulong key = Hash(x, y);

        if (table.TryGetValue(key, out cell))
        {
            cell.Add(e);
        }
        else
        {
            EntityList l = new();
            l.Add(e);
            table.Add(key, l);
        }
    }

    public bool Remove(int x, int y, Entity e)
    {
        EntityList cell;
        if (table.TryGetValue(Hash(x, y), out cell))
        {
            return cell.Remove(e);
        }

        return false;
    }

    public bool GetCell(int x, int y, out EntityList cell)
    {
        return table.TryGetValue(Hash(x, y), out cell);
    }

    public bool ContainsCell(int x, int y)
    {
        return table.ContainsKey(Hash(x, y));
    }

    public IEnumerable<EntityList> GetCellRange(int x1,
                                                int y1,
                                                int x2,
                                                int y2
        )
    {
        // iterate through all cells in range
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                EntityList l;
                if (!GetCell(i, j, out l))
                {
                    continue;
                }
                yield return l;
            }
        }
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

