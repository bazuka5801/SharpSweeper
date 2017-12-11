using System;
using System.Collections.Generic;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    internal static class Ranges
    {
        private static Coord m_Size;
        
        
        private static List<Coord> m_AllCoords;

        internal static Coord Size
        {
            get => m_Size;
            set {
                m_Size = value;
                m_AllCoords = new List<Coord>();
                for (int y = 0; y < m_Size.y; y++)
                    for (int x = 0; x < m_Size.x; x++)
                        m_AllCoords.Add(new Coord(x, y));
            }
        }

        internal static List<Coord> GetAllCoords()
        {
            return m_AllCoords;
        }

        internal static bool InRange(Coord coord)
        {
            return coord.x >= 0 && coord.x < m_Size.x &&
                   coord.y >= 0 && coord.y < m_Size.y;
        }

        internal static List<Coord> GetCoordsAround(Coord coord)
        {
            Coord around;
            List<Coord> list = new List<Coord>();
            for (int x = coord.x - 1; x <= coord.x + 1; x++)
            for (int y = coord.y - 1; y <= coord.y + 1; y++)
                if (InRange(around = new Coord(x, y)))
                    if (!around.Equals(coord))
                        list.Add(around);
            return list;
        }
    }
}